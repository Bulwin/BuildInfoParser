using System.Collections.Generic;
using System.Xml;

class XmlParser
{
    public BuildInfo ParseXml(XmlDocument xmlDoc)
    {
        BuildInfo ParsedInfo = new BuildInfo();
    

        // Получение элементов по имени тега
        ParsedInfo.BuildType = xmlDoc.SelectSingleNode("BuildInfo/BuildType").InnerText;
        ParsedInfo.BuildTimeGot = xmlDoc.SelectSingleNode("BuildInfo/BuildTimeGot").InnerText;
        ParsedInfo.ProductName = xmlDoc.SelectSingleNode("BuildInfo/UnityBuildSettings/ProductName").InnerText;
        ParsedInfo.TotalBuildSize = xmlDoc.SelectSingleNode("BuildInfo/TotalBuildSize").InnerText;
        ParsedInfo.StreamingAssetsSize = xmlDoc.SelectSingleNode("BuildInfo/StreamingAssetsSize").InnerText;
        ParsedInfo.BuildVersion = xmlDoc.SelectSingleNode("BuildInfo/UnityBuildSettings/MobileBundleVersion").InnerText;

        // Получение всех элементов с тегом "SizePart"
        XmlNodeList sizeParts = xmlDoc.SelectNodes("BuildInfo/UsedAssets/All/SizePart");
        Dictionary<int, SizePart> sizePartsDic = new Dictionary<int, SizePart>();
        int StringCount = 100;
        if (sizeParts.Count < 100)
        {
            StringCount = sizeParts.Count;
        }

        for (int i=0; i < StringCount; i++) 
        {
            sizePartsDic.Add(i, new SizePart(sizeParts[i].SelectSingleNode("Name").InnerText, sizeParts[i].SelectSingleNode("Size").InnerText));
        }
        ParsedInfo.SizePartsDic = sizePartsDic;
        SaveXml(xmlDoc, ParsedInfo.BuildVersion);
        VersionsDictionary.AddData(ParsedInfo.BuildVersion, new AppVersionData(ParsedInfo.ProductName.ToString(), ParsedInfo.TotalBuildSize.ToString()));
        return (ParsedInfo);
    }

    void SaveXml(XmlDocument xmlDoc, string xmlName)
    {
        XmlSaver xmlSave = new XmlSaver();
        xmlSave.SaveXml(xmlName, xmlDoc);

    }

  

   
}