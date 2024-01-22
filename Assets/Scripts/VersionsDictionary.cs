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
        Debug.Log("� ����");
        FileStream file = File.Open(Application.dataPath + "/StreamingAssets/SavedResults/Previousxml.bin", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);

        writer.Write(versionData.Count);

        foreach (KeyValuePair<string, AppVersionData> pair in versionData)
        {
            writer.Write(pair.Key); // ���������� ���� (������)
            writer.Write(pair.Value.AppName); // ���������� ��� ����������
            writer.Write(pair.Value.Size); // ���������� ������
        }

        // ��������� ����
        writer.Close();
        file.Close();
    }

    // ����� ��� �������� ������� �� ��������� �����
    public static void LoadDictionary()
    {
        versionData.Clear();
        ProjectsString.Clear();
        // ��������� ���� ��� ������
        FileStream file = File.Open(Application.dataPath + "/StreamingAssets/SavedResults/Previousxml.bin", FileMode.Open);
        BinaryReader reader = new BinaryReader(file);

        // ������ ���������� ��������� � �������
        int count = reader.ReadInt32();
        Debug.Log(count);
        // ������ ������ ������� ������� � ��������� � �������
        for (int i = 0; i < count; i++)
        {
            string key = reader.ReadString(); // ������ ���� (������)
            string appName = reader.ReadString(); // ������ ��� ����������
            string size = reader.ReadString(); // ������ ������

            AppVersionData data = new AppVersionData(appName, size);
            versionData.Add(key, data);
            if (!ProjectsString.Contains(appName))
            {
                ProjectsString.Add(appName);
            }
        }

        // ��������� ����
        reader.Close();
        file.Close();
    }

    // ��������� ������ � �������
    public static void AddData(string version, AppVersionData appdata)
    {
        if (!versionData.ContainsKey(version))
        {
            Debug.Log("��������� � �������");
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
