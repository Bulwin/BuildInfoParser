using System.IO;
using System.Xml;
using UnityEngine;

public class XmlSaver
{
    string filePath = Application.dataPath + "/StreamingAssets/Xml/";
    public XmlDocument XmlDoc = new XmlDocument();
    public void SaveXml(string XmlName, XmlDocument XmlDoc)
    {
        string fullfilePath = filePath + XmlName + ".xml";

        if (File.Exists(filePath))
        {
            Debug.Log("File already exists");
        }
        else
        {
            XmlDoc.Save(fullfilePath);
        }


    }
    public XmlDocument LoadXML(string XmlName)
    {
        string myXmlDoc = File.ReadAllText(filePath + XmlName + ".xml");
        XmlDoc.LoadXml(myXmlDoc);
        return (XmlDoc);
    }
}