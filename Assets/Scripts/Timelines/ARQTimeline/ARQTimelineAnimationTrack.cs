using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class ARQTimelineAnimationTrack : ARQTimelineTrack
    {        
        public  Animation _animation = new Animation();
        
        public override void CheckTime(float time)
        {
            foreach(ARQTimelineClip arqTimelineClip in _listARQTimelineClips){
                if(arqTimelineClip._startTime<=time && arqTimelineClip._endTime >= time)
                {
                    (arqTimelineClip as ARQTimelineAnimationClip).Play(_animation,time);
                    arqTimelineClip.wasStarted = true;
                }
                
            }
        }
        public override void Rewind(float time)
        {
            var wasRewind = false;
            
            foreach (ARQTimelineClip arqTimelineClip in _listARQTimelineClips)
            {
                if (time >= arqTimelineClip._startTime && time <= arqTimelineClip._endTime)
                {
                    arqTimelineClip.Rewind(_animation, time);
                    wasRewind = true;
                }
                if (time < arqTimelineClip._startTime)
                {
                    arqTimelineClip.wasStarted = false;
                }
            }
            if (!wasRewind)
            {
                ARQTimelineClip lastClip = _listARQTimelineClips[0];
                foreach(ARQTimelineClip arqTimelineClip in _listARQTimelineClips)
                {
                    if(time > arqTimelineClip._endTime && arqTimelineClip._endTime> lastClip._endTime)
                    {
                        lastClip = arqTimelineClip;
                    }
                }
                if (time > _listARQTimelineClips[0]._endTime)
                {
                    lastClip.Rewind(_animation, lastClip._endTime);
                }
                else
                {
                    _listARQTimelineClips[0].Rewind(_animation, _listARQTimelineClips[0]._startTime);
                }
            }
        }
    }
}