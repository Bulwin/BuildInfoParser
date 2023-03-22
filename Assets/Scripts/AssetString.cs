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


    public void FillAssetStringPrefab(int assetNumber, SizePart sizePart)
    {
        Position.text = assetNumber.ToString();
        var s = sizePart.Name.ToString();
        var last = s.Split('/').Last();
        AssetName.text = last;
        Size.text = sizePart.Size.ToString();
    }
}
