using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactionPreset<R>
{
    public R ReactionAsset { get; }
}
