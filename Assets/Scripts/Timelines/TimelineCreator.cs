using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace ARQTimeline
{
    public class TimelineCreator : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> _rightPartList = new List<GameObject>();
        public PlayableDirector playableDirector;
        public Text tx;

        public void CreateTimelineFromAnimClip(Animator animator, GameObject gameObject)
        {
            TimelineAsset instance = ScriptableObject.CreateInstance<TimelineAsset>();
            Dictionary<GameObject, Queue<KindaKey>> dict = new Dictionary<GameObject, Queue<KindaKey>>();
            int i = 0;
            foreach (var k in _rightPartList)
            {
                i++;
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
            
            playableDirector.playableAsset = instance;
            playableDirector.Play();

                      
        }
        

    }
}