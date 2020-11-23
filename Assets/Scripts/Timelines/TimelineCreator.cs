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

    public AnimationClip anim1;
    public AnimationClip anim2;

    public void CreateTimelineFromAnimClip(Animator animator, GameObject gameObject)
    {
        TimelineAsset instance = ScriptableObject.CreateInstance<TimelineAsset>();
        Dictionary<GameObject, Queue<KindaKey>> dict = new Dictionary<GameObject, Queue<KindaKey>>();
        foreach (var k in _rightPartList)
        {
            Queue<KindaKey> que = new Queue<KindaKey>();
            que.Enqueue(new KindaKey() { pos = k.transform.localPosition, timeBeforeTheKey = 0.0f });
            que.Enqueue(new KindaKey()
            {
                pos = new Vector3(k.transform.localPosition.x, k.transform.localPosition.y,
                k.transform.localPosition.z + 0.05f),
                timeBeforeTheKey = 1.0f
            });
            que.Enqueue(new KindaKey()
            {
                pos = new Vector3(k.transform.localPosition.x, k.transform.localPosition.y,
                k.transform.localPosition.z),
                timeBeforeTheKey = 2.0f
            });
            dict.Add(k, que);
        }
        AnimationClip clip = ClipExtension.CreateFromDictionary(dict);
        if (clip != null)
        {
            var newTrack = instance.CreateTrack<AnimationTrack>(null, name);
            var animPlayableAsset1 = (AnimationPlayableAsset)newTrack.CreateClip(clip).asset;
            animPlayableAsset1.position = gameObject.transform.position;
            animPlayableAsset1.rotation = gameObject.transform.rotation;
            playableDirector.SetGenericBinding(newTrack, animator);
        }
        /*AnimationClip animClip1 = ClipExtension.CreateAnimationClipHalf(_rightPartList);
        AnimationClip animClip2 = ClipExtension.CreateAnimationClipParts(_rightPartList);

        List<AnimCondition> k1 = ClipExtension.GetStartAndEndPos(animClip1);
        List<AnimCondition> k2 = ClipExtension.GetStartAndEndPos(animClip2);

        AnimationClip result = ClipExtension.CreateAnimationClipFromConditions(k1,k2);
        if (result != null)
        {
            var newTrack = instance.CreateTrack<AnimationTrack>(null, name);
            var animPlayableAsset1 = (AnimationPlayableAsset)newTrack.CreateClip(animClip1).asset;
            animPlayableAsset1.position = gameObject.transform.position;
            animPlayableAsset1.rotation = gameObject.transform.rotation;
            var animPlayableAsset = (AnimationPlayableAsset)newTrack.CreateClip(result).asset;
            animPlayableAsset.position = gameObject.transform.position;
            animPlayableAsset.rotation = gameObject.transform.rotation;
            var animPlayableAsset2 = (AnimationPlayableAsset)newTrack.CreateClip(animClip2.Reverse()).asset;
            animPlayableAsset2.position = gameObject.transform.position;
            animPlayableAsset2.rotation = gameObject.transform.rotation;
            playableDirector.SetGenericBinding(newTrack, animator);
        }*/
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
