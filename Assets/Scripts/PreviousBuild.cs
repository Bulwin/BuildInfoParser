using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviousBuild : MonoBehaviour
{
    public TextMeshProUGUI Version;
    public TextMeshProUGUI AppName;
    public TextMeshProUGUI Size;
    public Button OpenBuild;


    public void FillPreviousBuildPrefab(string version, string appname, string size)
    {
        Version.text = version;
        AppName.text = appname;
        Size.text = size;
    }
}
