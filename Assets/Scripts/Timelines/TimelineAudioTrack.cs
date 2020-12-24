using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARQTimeline
{
    public class TimelineAudioTrack : TimelineTrack
    {
       public override void CheckTime(float time)
       {
            foreach (TimelineClip arqTimelineClip in _listARQTimelineClips)
            {
                if (arqTimelineClip._startTime <= time && arqTimelineClip._endTime >= time && !arqTimelineClip.wasStarted)
                {
                    (arqTimelineClip as TimelineAudioClip).Play(time);
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
                    (arqTimelineClip as TimelineAudioClip).Rewind(time);
                    arqTimelineClip.wasStarted = false;
                    wasRewind = true;
                }
                if (time < arqTimelineClip._startTime)
                {
                    arqTimelineClip.wasStarted = false;
                    (arqTimelineClip as TimelineAudioClip).AudioSource.Stop();
                }
                if (time > arqTimelineClip._endTime)
                {
                    (arqTimelineClip as TimelineAudioClip).AudioSource.Stop();
                    arqTimelineClip.wasStarted = true;
                }
            }

            if (!wasRewind)
            {
                TimelineClip lastClip = _listARQTimelineClips[0];
                foreach (TimelineClip arqTimelineClip in _listARQTimelineClips)
                {
                    if (time > arqTimelineClip._endTime && arqTimelineClip._endTime > lastClip._endTime)
                    {
                        lastClip = arqTimelineClip;
                    }
                }
                if (time > _listARQTimelineClips[0]._endTime)
                {
                    (lastClip as TimelineAudioClip).AudioSource.Stop();
                    lastClip.wasStarted = false;
                }
                else
                {
                    (_listARQTimelineClips[0] as TimelineAudioClip).Rewind(_listARQTimelineClips[0]._startTime);
                    _listARQTimelineClips[0].wasStarted = false;
                }
            }
        }
        public override void SetPlayer<T>(T director)
        {
        
        }
    }
}
