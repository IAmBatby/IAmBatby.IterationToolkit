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

    public ContentDisplayGroup(bool combine, params IContentDisplayInfo[] infos)
    {
        if (combine)
            list.Add(new ContentDisplayListing(infos));
        else
            foreach (IContentDisplayInfo info in infos)
                list.Add(new ContentDisplayListing(info));
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
