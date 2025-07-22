using System.Collections.Generic;
using UnityEngine;
using System;

public static class DomainReloadManager
{
    private static List<WeakReference<IDomainReloadable>> weakReferences = new List<WeakReference<IDomainReloadable>>();

    public static void RegisterDomainReloadable(IDomainReloadable domainReloadable) => weakReferences.Add(new WeakReference<IDomainReloadable>(domainReloadable));

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InvokeClear()
    {
        for (int i = weakReferences.Count - 1; i >= 0; i--)
        {
            if (weakReferences[i].TryGetTarget(out IDomainReloadable domainReloadable))
                domainReloadable.OnDomainRefresh();
            else
                weakReferences.RemoveAt(i);
        }
    }
}
