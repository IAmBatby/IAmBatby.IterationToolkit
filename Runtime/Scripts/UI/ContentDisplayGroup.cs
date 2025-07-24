using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ContentDisplayGroup
{
    private List<ContentDisplayListing> list = new List<ContentDisplayListing>();

    public ContentDisplayGroup(params ContentDisplayListing[] values)
    {
        list.AddRange(values);
    }

    public void Add(ContentDisplayListing info)
    {
        list.Add(info);
    }

    public void Remove(ContentDisplayListing info)
    {
        list.Remove(info);
    }

    public List<ContentDisplayListing> Listings => list;

    public List<IContentDisplayInfo> Infos => list.SelectMany(l => l.Infos).ToList();
}
