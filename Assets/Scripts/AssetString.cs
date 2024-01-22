using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetString : MonoBehaviour
{
    public TextMeshProUGUI Position;
    public TextMeshProUGUI AssetName;
    public TextMeshProUGUI Size;
    public Image SizeBack;


    public void FillAssetStringPrefab(int assetNumber, SizePart sizePart, int sizeStatus)
    {
        Position.text = assetNumber.ToString();
        var s = sizePart.Name.ToString();
        var last = s.Split('/').Last();
        AssetName.text = last;
        Size.text = sizePart.Size.ToString();
        SetSizeBack(sizeStatus);

    }

    public void SetSizeBack(int sizeChanges)
    {
        if (sizeChanges == 1)
        {
            SizeBack.color = new Color32(0, 155, 0, 255);
        }

        else if(sizeChanges == 2)
        {
            SizeBack.color = new Color32(200, 155, 0, 255);
        }

        else if (sizeChanges == 3)
        {
            SizeBack.color = new Color32(155, 0, 0, 255);
        }

        else if (sizeChanges == 4)
        {
            SizeBack.color = new Color32(0, 150, 200, 255);
        }
    }
}
