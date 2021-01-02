using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQTimeline
{
    public class TimelineAnimationTrack : TimelineTrack
    {
        private List<string> _listNames = new List<string>();

        private Animation _animation;
        public Animation Animation
        {
            get
            {
                return _animation;
            }
            set
            {
                _animation = value;
            }
        }

        public override void CheckTime(float time)
        {
            foreach(TimelineClip arqTimelineClip in _listARQTimelineClips){
                if(arqTimelineClip._startTime<=time && arqTimelineClip._endTime >= time && !arqTimelineClip.wasStarted)
                {
                    (arqTimelineClip as TimelineAnimationClip).Play(_animation, time);
                    arqTimelineClip.wasStarted = true;
                }
            }
        }
        public override void Rewind(float time)
        {
            var wasRewind = false;
            foreach (TimelineClip arqTimelineClip in _listARQTimelineClips)
            {
                if (time >= arqTimelineClip._startTime && time <= arqTimelineClip._endTime)
                {
                    (arqTimelineClip as TimelineAnimationClip).Rewind(_animation, time);
                    arqTimelineClip.wasStarted = false;
                    wasRewind = true;
                }
                else if (time < arqTimelineClip._startTime)
                {
                    _animation[(arqTimelineClip as TimelineAnimationClip).AnimationClip.name].speed = 0;
                    arqTimelineClip.wasStarted = false;
                }
                else if (time > arqTimelineClip._endTime)
                {
                    _animation[(arqTimelineClip as TimelineAnimationClip).AnimationClip.name].speed = 0;
                    //(arqTimelineClip as TimelineAnimationClip).Rewind(_animation, arqTimelineClip._endTime);
                    arqTimelineClip.wasStarted = true;
                    wasRewind = true;
                }
            }
            /*if (!wasRewind)
            {
                TimelineClip lastClip = _listARQTimelineClips[0];
                foreach(TimelineClip arqTimelineClip in _listARQTimelineClips)
                {
                    if(time > arqTimelineClip._endTime && arqTimelineClip._endTime> lastClip._endTime)
                    {
                        _animation[(arqTimelineClip as TimelineAnimationClip).AnimationClip.name].speed = 0;
                        arqTimelineClip.wasStarted = true;
                        lastClip = arqTimelineClip;
                    }
                }
                if (time > lastClip._endTime)
                {
                    (lastClip as TimelineAnimationClip).Rewind(_animation, lastClip._endTime);
                    lastClip.wasStarted = true;
                }
                else
                {
                    (_listARQTimelineClips[0] as TimelineAnimationClip).Rewind(_animation, _listARQTimelineClips[0]._startTime);
                    _animation[(_listARQTimelineClips[0] as TimelineAnimationClip).AnimationClip.name].speed = 0;
                    _listARQTimelineClips[0].wasStarted = false;
                }
            }*/
        }
        public override void SetPlayer<T>(T director)
        {
            _animation = director as Animation;
        }

        public override void PackClips()
        {
            foreach(var k in _listARQTimelineClips)
            {
                (k as TimelineAnimationClip).AnimationClip.legacy = true;
                _animation.AddClip((k as TimelineAnimationClip).AnimationClip, (k as TimelineAnimationClip).AnimationClip.name);
            }
        }
        
        public override void UnpackClips()
        {
            _listNames.Clear();
            foreach(AnimationState k in _animation)
                _listNames.Add(_animation[k.name].name);
            foreach(var k in _listNames)
                _animation.RemoveClip(k);
        }
    }
}