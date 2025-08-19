using System;
using JMT.Core.Tool.CSV;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    private async void Start()
    {
        var data = await CSVParser.DownloadCSVAsync("18fAX0fuy1abKziMrrzwkMpXfftr9GP4mRTSrQ6NV7FU", "시트1");
        var objects = CSVParser.ToObjects<Test>(data);

        foreach (var obj in objects)
        {
            Debug.Log($"{obj.name}, {obj.age}, {obj.classNum}");
        }
    }
}

public class Test : ICSVSerializable
{
    public string name;
    public int age;
    public int classNum;
    public void FromCSV(string csvValue)
    {
        var values = csvValue.Split(',');
        if (values.Length != 3) throw new ArgumentException("Invalid CSV format");

        name = values[0];
        age = int.Parse(values[1]);
        classNum = int.Parse(values[2]);
    }

    public string ToCSV()
    {
        return $"{name},{age},{classNum}";
    }
}