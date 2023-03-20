using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
    public class AppVersionData
    {
    public string AppName;
    public string Size;

    public AppVersionData(string appName, string size)
    {
        AppName = appName;
        Size = size;
    }
    }

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
    public static void AddData(string version, AppVersionData appdata )
    {
        if (!versionData.ContainsKey(version))
        {
            Debug.Log("��������� � �������");
            versionData.Add(version, appdata);
            SaveDictionary();
        }
       
    }

    // ��������� ���������� � �������� �������
}
