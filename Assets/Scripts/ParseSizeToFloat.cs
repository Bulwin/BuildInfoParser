using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ParseSizeToFloat : MonoBehaviour
{
    //public float ParseToFloat(string size)
    //{
    //    float parsedSize = float.Parse(size.Replace(" MB", "").Replace(".", ","));
    //    Debug.Log(parsedSize);
    //    return (parsedSize);
    //}

    public float ParseToFloat(string fileSizeString)
    {
        float fileSize = 0;
        if (string.IsNullOrEmpty(fileSizeString))
        {
            return fileSize;
        }
        fileSizeString = fileSizeString.ToUpper();
        if (fileSizeString.EndsWith("KB"))
        {
            fileSize = float.Parse(fileSizeString.Replace("KB", "").Replace(".", ","));
            fileSize = fileSize / 1024;
        }
        else if (fileSizeString.EndsWith("MB"))
        {
            fileSize = float.Parse(fileSizeString.Replace("MB", "").Replace(".", ","));
        }
        else if (fileSizeString.EndsWith("GB"))
        {
            fileSize = float.Parse(fileSizeString.Replace("GB", "").Replace(".", ","));
            fileSize = fileSize * 1024;
        }
        else if (fileSizeString.EndsWith("TB"))
        {
            fileSize = float.Parse(fileSizeString.Replace("TB", "").Replace(".", ","));
            fileSize = fileSize * 1024 * 1024;
        }
        return fileSize;
    }
}
