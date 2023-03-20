using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Button OpenFileButton;
    public Button BackButton;
    public TextMeshProUGUI GameName;
    public TextMeshProUGUI Version;
    public TextMeshProUGUI Platform;
    public TextMeshProUGUI Date;
    public TextMeshProUGUI BuildSize;
    public TextMeshProUGUI StreamingAssetsSize;
    public GameObject OpenText;
    public Transform AssetGrid;
    public Transform PreviousBuild;
    public GameObject AssetStringPrefab;
    public GameObject PreviousBuildPrefab;
    public GameObject AssetScrollView;
    public GameObject PreviousBuildsScrollView;
    public GameObject BuildInfoBlock;
    public TMP_Dropdown GameListDropdown;
    public XmlDocument tmpXML;
    private BuildInfo ParsedXml;

    void Start()
    {
        FillMainScreen();
    }

    void FillMainScreen()
    {
        PrepareLoading();
        LoadPreviousData("Show ALL", true);
        XmlLoader xmlLoader = GetComponent<XmlLoader>();
        xmlLoader.XmlLoadedEvent += ParseXML;
    }

    void LoadPreviousData(string projectToShow, bool updateDropdownOptions)
    {
        Debug.Log(projectToShow);
        VersionsDictionary.LoadDictionary();
        ClearGrid(PreviousBuild);
        foreach (var pd in VersionsDictionary.versionData)
        {
            if (projectToShow.Equals("Show ALL") || pd.Value.AppName.Equals(projectToShow))
            {
                GameObject PreviousResult = Instantiate(PreviousBuildPrefab);
                PreviousResult.transform.SetParent(PreviousBuild, false);
                PreviousResult.GetComponent<PreviousBuild>().FillPreviousBuildPrefab(pd.Key, pd.Value.AppName, pd.Value.Size);
                PreviousResult.GetComponent<PreviousBuild>().OpenBuild.onClick.AddListener(() =>
                {
                    ParseXML(GetXml(pd.Key));
                });
            }
        }
        if (updateDropdownOptions.Equals(true))
        {
            DropDownManager(VersionsDictionary.ProjectsString);
        }

        XmlDocument GetXml(string version)
        {
            XmlDocument tmpXML = new XmlDocument();
            XmlSaver xmlSaver = new XmlSaver();
            tmpXML = xmlSaver.LoadXML(version);
            return (tmpXML);
        }
    }

    void PrepareLoading()
    {
        XmlLoader xmlLoader = GetComponent<XmlLoader>();
        OpenFileButton.onClick.AddListener(xmlLoader.ShowExplorer);
        //OpenFileButton.onClick.AddListener(HideDropdown);
        BackButton.onClick.AddListener(ShowMainScreen);

    }

    public void HideDropdown()
    {
        GameListDropdown.gameObject.SetActive(false);
    }

    void ParseXML(XmlDocument xmlDoc)
    {
        XmlParser Xmlprs = new XmlParser();
        ParsedXml = Xmlprs.ParseXml(xmlDoc);
        FillData(ParsedXml);
       
        BuildInfoBlock.SetActive(true);
        OpenText.SetActive(false);
        AssetScrollView.SetActive(true);
        PreviousBuildsScrollView.SetActive(false);
        BackButton.gameObject.SetActive(true);
        GameListDropdown.gameObject.SetActive(false);
    }

    void FillData(BuildInfo parsedXml)
    {
        GameName.text = parsedXml.ProductName;
        Platform.text = parsedXml.BuildType;
        Date.text = parsedXml.BuildTimeGot;
        BuildSize.text = parsedXml.TotalBuildSize;
        Version.text = parsedXml.BuildVersion;
        StreamingAssetsSize.text = parsedXml.StreamingAssetsSize;
        ClearGrid(AssetGrid);

        for (int i=0; i < parsedXml.SizePartsDic.Count; i++)
        {
            GameObject assetString = Instantiate(AssetStringPrefab);
            assetString.transform.SetParent(AssetGrid, false);
            assetString.GetComponent<AssetString>().FillAssetStringPrefab(i+1, parsedXml.SizePartsDic[i]);
        }
    }

    void ClearGrid(Transform grid)
    {
        foreach (Transform trans in grid)
        {
            GameObject.Destroy(trans.gameObject);
        }
    }


    void ShowMainScreen()
    {
        BuildInfoBlock.SetActive(false);
        OpenText.SetActive(true);
        AssetScrollView.SetActive(false);
        PreviousBuildsScrollView.SetActive(true);
        BackButton.gameObject.SetActive(false);
        GameListDropdown.gameObject.SetActive(true);
    }

    void DropDownManager(List<string> options)
    {
      {
            GameListDropdown.ClearOptions();
            options.Insert(0, "Show ALL");
            GameListDropdown.AddOptions(options);
            GameListDropdown.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(GameListDropdown);
            });
        }

    }

    private void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        Debug.Log("FSDFS");
        LoadPreviousData(dropdown.captionText.text, false);
    }
}


