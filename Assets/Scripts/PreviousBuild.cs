using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviousBuild : MonoBehaviour
{
    public TextMeshProUGUI Version;
    public TextMeshProUGUI AppName;
    public TextMeshProUGUI Size;
    public Button OpenBuild;
    public Image SizeBack;


    public void FillPreviousBuildPrefab(string version, string appname, string size)
    {
        Version.text = version;
        AppName.text = appname;
        Size.text = size;
    }

    public void SetSizeBack(int sizeChanges)
    {
        if (sizeChanges == 2 )
        {
            SizeBack.color = new Color32(0, 155, 0, 255);
        }

        else if (sizeChanges == 1)
        {
            SizeBack.color = new Color32(155, 0, 0, 255);
        }
    }
}
