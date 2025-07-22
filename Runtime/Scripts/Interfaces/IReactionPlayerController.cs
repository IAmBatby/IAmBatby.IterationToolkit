using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactionPlayerController
{

}

public interface IReactionPlayerController<P,R> : IReactionPlayerController where P : IReactionPreset<R>
{
    public void ApplyPreset(P preset);
    public void Play(P preset);
    public void Play(R reactionAsset);
    public void Stop();
}
