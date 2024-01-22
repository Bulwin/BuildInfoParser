using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class VersionsDictionary : MonoBehaviour
{
    public static Dictionary<string, AppVersionData> versionData = new Dictionary<string, AppVersionData>();
    public static List<string> ProjectsString = new List<string>();

    public static void SaveDictionary()
    {
        Debug.Log("я тута");
        FileStream file = File.Open(Application.dataPath + "/StreamingAssets/SavedResults/Previousxml.bin", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);

        writer.Write(versionData.Count);

        foreach (KeyValuePair<string, AppVersionData> pair in versionData)
        {
            writer.Write(pair.Key); // записываем ключ (версию)
            writer.Write(pair.Value.AppName); // записываем им€ приложени€
            writer.Write(pair.Value.Size); // записываем размер
        }

        // закрываем файл
        writer.Close();
        file.Close();
    }

    // метод дл€ загрузки словар€ из бинарного файла
    public static void LoadDictionary()
    {
        versionData.Clear();
        ProjectsString.Clear();
        // открываем файл дл€ чтени€
        FileStream file = File.Open(Application.dataPath + "/StreamingAssets/SavedResults/Previousxml.bin", FileMode.Open);
        BinaryReader reader = new BinaryReader(file);

        // читаем количество элементов в словаре
        int count = reader.ReadInt32();
        Debug.Log(count);
        // читаем каждый элемент словар€ и добавл€ем в словарь
        for (int i = 0; i < count; i++)
        {
            string key = reader.ReadString(); // читаем ключ (версию)
            string appName = reader.ReadString(); // читаем им€ приложени€
            string size = reader.ReadString(); // читаем размер

            AppVersionData data = new AppVersionData(appName, size);
            versionData.Add(key, data);
            if (!ProjectsString.Contains(appName))
            {
                ProjectsString.Add(appName);
            }
        }

        // закрываем файл
        reader.Close();
        file.Close();
    }

    // добавл€ем данные в словарь
    public static void AddData(string version, AppVersionData appdata)
    {
        if (!versionData.ContainsKey(version))
        {
            Debug.Log("ƒобавлено в словарь");
            versionData.Add(version, appdata);
            SaveDictionary();
        }

    }

    public static Dictionary<string, AppVersionData> GetversionDatabyProject(string project)
    {
        Dictionary<string, AppVersionData> projectVersionData = new Dictionary<string, AppVersionData>();
        foreach(var vd in versionData)
        {
        if (vd.Value.AppName.Equals(project))
            {
                projectVersionData.Add(vd.Key,vd.Value);
            }
        }
        return SortXmls(projectVersionData);
    }

    static public Dictionary<string, AppVersionData> SortXmls(Dictionary<string, AppVersionData> versionDataDic)
    {
        var sortedDict = versionDataDic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        return (sortedDict);
    }
}
