using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContentDisplayListing
{
    private List<IContentDisplayInfo> list = new List<IContentDisplayInfo>();

    public ContentDisplayListing(params IContentDisplayInfo[] values)
    {
        list.AddRange(values);
    }

    public void Add(IContentDisplayInfo info)
    {
        list.Add(info);
    }

    public void Remove(IContentDisplayInfo info)
    {
        list.Remove(info);
    }

    public List<IContentDisplayInfo> Infos => list;
}
