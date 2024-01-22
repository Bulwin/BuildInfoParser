using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScreenManager : MonoBehaviour
{
    public Button OpenFileButton;
    public Button CompareModeButton;
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
    string PreviousVersion;
    BuildInfo PreviousVersionXml;
    bool isCompareModeActive = false;

    void Start()
    {
        FillMainScreen();
        CompareModeButton.onClick.AddListener(() =>
        {
            TurnCompareMode();
        });
    }

    void FillMainScreen()
    {
        PrepareLoading();
        LoadPreviousData("Show ALL", true);
        XmlLoader xmlLoader = GetComponent<XmlLoader>();
        xmlLoader.XmlLoadedEvent += ParseAndFill;
    }

    void LoadPreviousData(string projectToShow, bool updateDropdownOptions)
    {
        Dictionary<string, AppVersionData> Vdata = new Dictionary<string, AppVersionData>();
        ParseSizeToFloat pstf = new ParseSizeToFloat();
        Debug.Log(projectToShow);
        VersionsDictionary.LoadDictionary();
        ClearGrid(PreviousBuild);
        Vdata = SortXmlsAsc();
        string previousSize = "0 MB";
        foreach (var pd in Vdata)
        {
            if (projectToShow.Equals("Show ALL") || pd.Value.AppName.Equals(projectToShow))
            {

                GameObject PreviousResult = Instantiate(PreviousBuildPrefab);

                PreviousResult.transform.SetParent(PreviousBuild, false);
                PreviousResult.transform.transform.SetSiblingIndex(0);
                PreviousResult.GetComponent<PreviousBuild>().FillPreviousBuildPrefab(pd.Key, pd.Value.AppName, pd.Value.Size);
                PreviousResult.GetComponent<PreviousBuild>().OpenBuild.onClick.AddListener(() =>
                {
                    PreviousVersion = GetPreviousBuildVersion(pd.Key, pd.Value.AppName);
                    OpenBuild(pd.Key, PreviousVersion);
                });
              

                if (pstf.ParseToFloat(pd.Value.Size) > pstf.ParseToFloat(previousSize) && !projectToShow.Equals("Show ALL"))
                {
                    Debug.Log(pd.Value.Size + "  " + previousSize);
                    PreviousResult.GetComponent<PreviousBuild>().SetSizeBack(1);
                }
                else if (pstf.ParseToFloat(pd.Value.Size) < pstf.ParseToFloat(previousSize) && !projectToShow.Equals("Show ALL"))
                {
                    Debug.Log(pd.Value.Size + "  " + previousSize);
                    PreviousResult.GetComponent<PreviousBuild>().SetSizeBack(2);
                }
                previousSize = pd.Value.Size;
            }
        }
        if (updateDropdownOptions.Equals(true))
        {
            DropDownManager(VersionsDictionary.ProjectsString);
        }
    }
    XmlDocument GetXml(string version)
    {
        XmlDocument tmpXML = new XmlDocument();
        XmlSaver xmlSaver = new XmlSaver();
        tmpXML = xmlSaver.LoadXML(version);
        return (tmpXML);
    }
    void OpenBuild(string newVersion, string oldVersion)
    {
        CompareModeButton.gameObject.SetActive(false);
        if (!oldVersion.Equals(null))
            {
                PreviousVersionXml = ParseXML(GetXml(oldVersion));
            }
            ParseAndFill(GetXml(newVersion));
     
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

    void ParseAndFill(XmlDocument xmlDoc)
    {
        ParsedXml = ParseXML(xmlDoc);
        FillData(ParsedXml);

        BuildInfoBlock.SetActive(true);
        OpenText.SetActive(false);
        AssetScrollView.SetActive(true);
        PreviousBuildsScrollView.SetActive(false);
        BackButton.gameObject.SetActive(true);
        GameListDropdown.gameObject.SetActive(false);
    }

    BuildInfo ParseXML(XmlDocument xmlDoc)
    {

        BuildInfo parsedXml;
        XmlParser Xmlprs = new XmlParser();
        parsedXml = Xmlprs.ParseXml(xmlDoc);
        Debug.Log(parsedXml.SizePartsDic.Count);
        return (parsedXml);
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

        for (int i = 0; i < parsedXml.SizePartsDic.Count; i++)
        {
            int sizeCompareStatus = ComparePartWithPreviousBuild(parsedXml.SizePartsDic[i].Name, parsedXml.SizePartsDic[i].Size);
            GameObject assetString = Instantiate(AssetStringPrefab);
            assetString.transform.SetParent(AssetGrid, false);
            assetString.GetComponent<AssetString>().FillAssetStringPrefab(i + 1, parsedXml.SizePartsDic[i], sizeCompareStatus);
        }
    }

    void TurnCompareMode()
    {

         PreviousBuild[] previousBuilds;
         List<string> selectedVersions = new List<string>();

        if (isCompareModeActive.Equals(false))
        {
            isCompareModeActive = true;
            foreach (Transform tr in PreviousBuild)
            {
                tr.gameObject.GetComponent<PreviousBuild>().GoCompareMode();
                tr.gameObject.GetComponent<PreviousBuild>().CompareToogle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        // Если переключатель включен, добавляем версию в список
                        selectedVersions.Add(tr.gameObject.GetComponent<PreviousBuild>().VersionNumber);
                        if (selectedVersions.Count == 2)
                        {
                            // Если выбраны две версии, вызываем функцию
                            PreviousVersion = selectedVersions[1];
                            OpenBuild(selectedVersions[0], selectedVersions[1]);

                            //CompareVersions(selectedVersions[0], selectedVersions[1]);
                        }
                    }
                    else
                    {
                        // Если переключатель выключен, удаляем версию из списка
                        selectedVersions.Remove(tr.gameObject.GetComponent<PreviousBuild>().VersionNumber);
                    }
                });
            }
        }

        else
        {
            foreach (Transform tr in PreviousBuild)
            {
                tr.gameObject.GetComponent<PreviousBuild>().EndCompareMode();
            }
                isCompareModeActive = false;
        }
        
    }

    int ComparePartWithPreviousBuild(string appPart, string size)
    {
        //1 - В старом билде ассет весил больше
        //2 - В старом билде ассет весил столько же
        //3 - старом билде ассет весил меньше
        //4 - старом билде ассета не было
        ParseSizeToFloat prsz = new ParseSizeToFloat();
        string appPartNewBuild = TrimAfterWord(appPart.Split('/').Last(), "android");
        Debug.Log("NewappPart " + appPartNewBuild + " " + size);
        foreach (var ps in PreviousVersionXml.SizePartsDic)
        {
            var appPartName = TrimAfterWord(ps.Value.Name.Split('/').Last(), "android");
            Debug.Log(appPartName + " " + appPartNewBuild + "!");
            if (appPartName.Equals(appPartNewBuild))
            {
                Debug.Log("OldAppPart" + appPartName + " " + ps.Value.Size);
                if (prsz.ParseToFloat(ps.Value.Size) > prsz.ParseToFloat(size))
                {
                    return (1);
                }

                else if (prsz.ParseToFloat(ps.Value.Size) == prsz.ParseToFloat(size))
                {
                    return (2);
                }

                else if (prsz.ParseToFloat(ps.Value.Size) < prsz.ParseToFloat(size))
                {
                    return (3);
                }
            }
        }

        return (4);
    }

    string GetPreviousBuildVersion(string version, string project)
    {
        Dictionary<string, AppVersionData> sortedXml = VersionsDictionary.GetversionDatabyProject(project);
        Debug.Log(sortedXml.Count);
        string previousKey = null;
        foreach (var pair in sortedXml)
        {
            
            Debug.Log(pair.Key);
            if (previousKey != null && pair.Key == version)
            {
                Debug.Log("current version is: " + version + " previous version is: " + previousKey);
                return previousKey;
            }

            else if (previousKey == null && pair.Key == version)
            {
                return version;
            }
            previousKey = pair.Key;
        }
        return null;
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
        CompareModeButton.gameObject.SetActive(true);
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

    Dictionary<string, AppVersionData> SortXmls()
    {
        var sortedDict = VersionsDictionary.versionData.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        return (sortedDict);
    }

    Dictionary<string, AppVersionData> SortXmlsAsc()
    {
        var sortedDict = VersionsDictionary.versionData.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        return (sortedDict);
    }

    public string TrimAfterWord(string input, string word)
    {
        int index = input.IndexOf(word);
        if (index < 0)
        {
            // Слово не найдено, вернуть исходную строку
            return input;
        }
        // Индекс + длина слова, чтобы включить само слово в результирующую строку
        return input.Substring(0, index + word.Length);
    }
}


