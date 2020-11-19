using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineCreator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _rightPartList = new List<GameObject>();
    public PlayableDirector playableDirector;
    public void CreateTimelineFromAnimClip(AnimationClip animationClip, Animator animator, GameObject gameObject)
    {
        TimelineAsset instance = ScriptableObject.CreateInstance<TimelineAsset>();
        animationClip = ClipExtension.CreateAnimationClipParts(_rightPartList);
        var secondAnimClip = ClipExtension.CreateAnimationClipHalf(_rightPartList);

        List<AnimCondition> k1 = ClipExtension.GetStartAndEndPos(animationClip);
        List<AnimCondition> k2 = ClipExtension.GetStartAndEndPos(secondAnimClip);

        AnimationClip result = ClipExtension.CreateAnimationClipFromConditions(k1,k2);
        if (result != null)
        {
            var newTrack = instance.CreateTrack<AnimationTrack>(null, name);

            var animPlayableAsset = (AnimationPlayableAsset)newTrack.CreateClip(result).asset;
            animPlayableAsset.position = gameObject.transform.position;
            animPlayableAsset.rotation = gameObject.transform.rotation;

            playableDirector.SetGenericBinding(newTrack, animator);
        }
        playableDirector.playableAsset = instance;

        playableDirector.Play();
    }
    public static TrackAsset CreateTrack(TimelineAsset asset, Type type, TrackAsset parent = null, string name = null)
    {
        if (asset == null)
            return null;

        var track = asset.CreateTrack(type, parent, name);
        if (track != null)
        {
            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
        }

        return track;
    }

}
