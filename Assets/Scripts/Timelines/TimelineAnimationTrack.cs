using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQTimeline
{
    public class TimelineAnimationTrack : TimelineTrack
    {
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
                if (time < arqTimelineClip._startTime)
                {
                    arqTimelineClip.wasStarted = false;
                }
            }
            if (!wasRewind)
            {
                TimelineClip lastClip = _listARQTimelineClips[0];
                foreach(TimelineClip arqTimelineClip in _listARQTimelineClips)
                {
                    if(time > arqTimelineClip._endTime && arqTimelineClip._endTime> lastClip._endTime)
                    {
                        lastClip = arqTimelineClip;
                    }
                }
                if (time > _listARQTimelineClips[0]._endTime)
                {
                    (lastClip as TimelineAnimationClip).Rewind(_animation, lastClip._endTime);
                    lastClip.wasStarted = false;
                }
                else
                {
                    (_listARQTimelineClips[0] as TimelineAnimationClip).Rewind(_animation, _listARQTimelineClips[0]._startTime);
                    _listARQTimelineClips[0].wasStarted = false;
                }
            }
        }
    }
}