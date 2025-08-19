using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace JMT.UISystem.Dialogue
{
    public class DialogueModel
    {
        private readonly string address = "https://docs.google.com/spreadsheets/d/1xCZg0hWYwizw9icb2s_h_DEXt3QW4e5Gj2vogHvF2j0";
        private readonly long sheetID = 0;

        public async Task<string> LoadDataAsync(string range)
        {
            // TSV 파일로 변환
            var TSVdata = GetTSVAddress(address, range, sheetID);
            UnityWebRequest www = UnityWebRequest.Get(TSVdata);

            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                return www.downloadHandler.text;

            return string.Empty;
        }

        public Queue<string> SettingDialogueData(string data)
        {

            Queue<string> result = new();
            string[] splitEnterData = data.Split('\n');

            foreach (string str in splitEnterData)
            {
                result.Enqueue(str);
            }
            return result;
        }

        private string GetTSVAddress(string address, string range, long sheetID)
            => $"{address}/export?format=tsv&range={range}&gid={sheetID}";
    }
}
