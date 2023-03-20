using System;
using System.Collections.Generic;
using System.Xml;

class BuildInfo
{
    public string BuildType { get; set; }
    public string BuildTimeGot { get; set; }
    public string ProductName { get; set; }

    public string BuildVersion { get; set; }
    public string TotalBuildSize { get; set; }
    public string StreamingAssetsSize { get; set; }
    public Dictionary<int, SizePart> SizePartsDic { get; set; }
}


public class SizePart
{
    public string Name { get; set; }
    public string Size { get; set; }
    public SizePart(string name, string size)
    {
        Name = name;
        Size = size;
    }
   
}

