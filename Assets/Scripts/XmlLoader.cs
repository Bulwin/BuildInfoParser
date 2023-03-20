using System.Xml;
using UnityEngine;
using SmartDLL;
using System.IO;

public class XmlLoader : MonoBehaviour
{
    public XmlDocument XmlDoc = new XmlDocument();
    public delegate void XmlLoadedHandler(XmlDocument xmlDoc);
    public event XmlLoadedHandler XmlLoadedEvent;
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();
    private bool readText = false;
    private string lastSelectedDirectory;

    void Update()
    {
        if (fileExplorer.resultOK && readText)
        {
            ReadText(fileExplorer.fileName);
            PlayerPrefs.SetString("LastSelectedDirectory", lastSelectedDirectory);
            readText = false;
        }
    }


    public void ShowExplorer()
    {
        lastSelectedDirectory = PlayerPrefs.GetString("LastSelectedDirectory", "");
        string initialDir = string.IsNullOrEmpty(lastSelectedDirectory) ? @"C:\" : lastSelectedDirectory;
        bool restoreDir = true;
        string title = "Show all xnl (.xml)";
        string defExt = "xml";
        string filter = "xml files (*.xml)|*.xml";


        fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
        lastSelectedDirectory = Path.GetDirectoryName(fileExplorer.fileName);
        readText = true;
    }



    void ReadText(string path)
    {
       
        string myXmlDoc = File.ReadAllText(path);
        XmlDoc.LoadXml(myXmlDoc);
        XmlLoadedEvent?.Invoke(XmlDoc);
        
    }
}
