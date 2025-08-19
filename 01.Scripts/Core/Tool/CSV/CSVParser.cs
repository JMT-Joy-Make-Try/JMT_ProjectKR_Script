using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace JMT.Core.Tool.CSV
{
    public static class CSVParser
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string BASE_LINK = "https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&sheet={1}";

        /// <summary>
        /// 구글 시트에서 CSV를 다운로드하여 2차원 리스트로 반환
        /// </summary>
        public static async Task<List<List<string>>> DownloadCSVAsync(string sheetId, string sheetName)
        {
            string url = string.Format(BASE_LINK, sheetId, Uri.EscapeDataString(sheetName));
            var csvText = await httpClient.GetStringAsync(url).ConfigureAwait(false);
            return ParseCSV(csvText);
        }

        /// <summary>
        /// CSV 문자열을 2차원 리스트로 파싱
        /// </summary>
        public static List<List<string>> ParseCSV(string csvText)
        {
            var rows = new List<List<string>>();
            var lines = csvText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = ParseCSVLine(line);
                rows.Add(cols);
            }

            return rows;
        }

        /// <summary>
        /// CSV의 각 행을 파싱 (쉼표, 따옴표 처리)
        /// </summary>
        private static List<string> ParseCSVLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var value = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        value += '"'; // 이스케이프된 따옴표
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(value);
                    value = string.Empty;
                }
                else
                {
                    value += c;
                }
            }

            result.Add(value);
            return result;
        }

        /// <summary>
        /// CSV 데이터를 객체 리스트로 변환
        /// </summary>
        public static List<T> ToObjects<T>(List<List<string>> csvData) where T : new()
        {
            var result = new List<T>();
            if (csvData.Count < 2) return result;

            var headers = csvData[0];

            for (int i = 1; i < csvData.Count; i++)
            {
                var row = csvData[i];
                var obj = new T();

                if (obj is ICSVSerializable serializable)
                {
                    // 한 줄 전체를 CSV 문자열로 조합해서 FromCSV 호출
                    string csvLine = string.Join(",", row);
                    serializable.FromCSV(csvLine);
                    Debug.Log($"Parsed CSV to object: {obj}");
                    Debug.Log($"CSV Line: {csvLine}");
                    result.Add(obj);
                }
                else
                {
                    var type = typeof(T);

                    for (int c = 0; c < headers.Count && c < row.Count; c++)
                    {
                        var fieldName = headers[c];
                        var field = type.GetField(fieldName);
                        if (field == null) continue;

                        var converted = ConvertValue(row[c], field.FieldType);
                        field.SetValue(obj, converted);
                    }
                    result.Add(obj);
                }
            }

            return result;
        }


        /// <summary>
        /// 문자열을 지정 타입으로 변환
        /// </summary>
        private static object ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (Nullable.GetUnderlyingType(targetType) != null)
                    return null;
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            // ICSVSerializable 지원
            if (typeof(ICSVSerializable).IsAssignableFrom(targetType))
            {
                var instance = (ICSVSerializable)Activator.CreateInstance(targetType);
                instance.FromCSV(value);
                return instance;
            }

            // Enum 처리
            if (targetType.IsEnum)
            {
                if (int.TryParse(value, out var intValue))
                    return Enum.ToObject(targetType, intValue);
                return Enum.Parse(targetType, value, true);
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // bool 처리
            if (underlyingType == typeof(bool))
            {
                if (value == "1") return true;
                if (value == "0") return false;
                return bool.Parse(value);
            }

            // DateTime 처리
            if (underlyingType == typeof(DateTime))
            {
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }

            return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
        }
    }
}
