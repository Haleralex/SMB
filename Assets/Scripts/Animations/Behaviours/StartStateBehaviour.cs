using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace Test
{
    public class StartStateBehaviour : MonoBehaviour
    {
        //is needed to playing ARQTimelines
        [SerializeField]
        private ARQTimelineDirector _arqTimelineDirector;

        //is needed to QueueMod, queue of states in State Machine
        private Queue<ARQTimeline> _queueARQTimelines = new Queue<ARQTimeline>();

        //is needed to select playing mod
        public bool QueueMod = false;

        //is needed to match between state name and ARQTimeline


        private Dictionary<string, ARQTimeline> _stateTimelines = new Dictionary<string, ARQTimeline>();


        private void OnEnable()
        {
            _stateTimelines.Add("Half", Create1Timeline());
            _stateTimelines.Add("HalfParts", Create2Timeline());
        }

        public void StartState(string nameState)
        {
            if (QueueMod)
            {
                if (!_arqTimelineDirector.ARQTimeline || !_arqTimelineDirector.ARQTimeline._isStarted)
                {
                    _arqTimelineDirector.ARQTimeline = _stateTimelines[nameState];
                    _arqTimelineDirector.Play();
                }
                else
                {
                    _queueARQTimelines.Enqueue(_stateTimelines[nameState]);
                    _arqTimelineDirector.Finished = SimplePlay;
                }
            }
            else
            {
                _arqTimelineDirector.Stop();
                _arqTimelineDirector.ARQTimeline = _stateTimelines[nameState];
                _arqTimelineDirector.Play();
            }
        }
        void SimplePlay()
        {
            if (_queueARQTimelines.Count > 0)
            {
                _arqTimelineDirector.ARQTimeline = _queueARQTimelines.Dequeue();
                _arqTimelineDirector.Play();
            }
        }

        #region test 
        [SerializeField]
        private Animation animation;
        [SerializeField]
        private AnimationClip animationClip1;
        [SerializeField]
        private AnimationClip animationClip2;
        private ARQTimeline Create1Timeline()
        {
            var _arqTimeline = ScriptableObject.CreateInstance<ARQTimeline>();
            animationClip1.legacy = true;
            animationClip2.legacy = true;
            animation.AddClip(animationClip1, animationClip1.name);
            animation.AddClip(animationClip2, animationClip2.name);
            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<ARQTimelineAnimationTrack>();
            arqTimelineAnimationTrack.Animation = animation;
            var timelineClip = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(2, 1);
            var timelineClipExtra = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(2.5f, 1);
            timelineClip.AnimationClip = animationClip1;
            timelineClipExtra.AnimationClip = animationClip2;
            return _arqTimeline;
        }

        [SerializeField]
        private Animation animation2;
        [SerializeField]
        private AnimationClip animationClip11;
        [SerializeField]
        private AnimationClip animationClip12;
        private ARQTimeline Create2Timeline()
        {
            var _arqTimeline = ScriptableObject.CreateInstance<ARQTimeline>();
            animationClip11.legacy = true;
            animationClip12.legacy = true;
            animation2.AddClip(animationClip11, animationClip11.name);
            animation2.AddClip(animationClip12, animationClip12.name);
            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<ARQTimelineAnimationTrack>();
            arqTimelineAnimationTrack.Animation = animation2;
            var timelineClip2 = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(1, 21.25f);
            var timelineClipIdle = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(1, 4f);
            timelineClip2.AnimationClip = animationClip11;
            timelineClipIdle.AnimationClip = animationClip12;
            return _arqTimeline;
        }
        #endregion
    
    }
}