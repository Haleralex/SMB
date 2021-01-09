using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
            TimelineAnimationClip clipToRewind1 = null;
            TimelineAnimationClip clipToRewind2 = null;
        public override void Rewind(float time)
        {
            foreach (TimelineClip arqTimelineClip in _listARQTimelineClips)
            {
                if (time < arqTimelineClip._startTime)
                {
                    _animation[(arqTimelineClip as TimelineAnimationClip).AnimationClip.name].speed = 0;
                    //(arqTimelineClip as TimelineAnimationClip).Rewind(_animation, arqTimelineClip._startTime, 0);
                    arqTimelineClip.wasStarted = false;
                }
                else if (time > arqTimelineClip._endTime)
                {
                    (arqTimelineClip as TimelineAnimationClip).RewindAfterPlaying(_animation, arqTimelineClip._endTime);
                    arqTimelineClip.wasStarted = true;
                }
                else if (time >= arqTimelineClip._startTime && time <= arqTimelineClip._endTime)
                {
                    if (clipToRewind1 == null)
                    {
                        clipToRewind1 = (arqTimelineClip as TimelineAnimationClip);
                    }
                    else
                    {
                        if (clipToRewind2 == null)
                        {
                            clipToRewind2 = (arqTimelineClip as TimelineAnimationClip);
                        }
                    }
                    arqTimelineClip.wasStarted = false;
                }
            }

            if (clipToRewind1 == null)
                return;
            if (clipToRewind2 == null)
            {
                clipToRewind1.Rewind(_animation, time, 1);
            }
            else
            {
                AllocationWeights(clipToRewind1, clipToRewind2, time);
            }
            clipToRewind1 = null;
            clipToRewind2 = null;
        }

        private void AllocationWeights(TimelineAnimationClip a, TimelineAnimationClip b, float time)
        {
            float weight2 = (time - b._startTime) / (a._endTime - b._startTime);
            float weight1 = 1 - weight2;
            a.Rewind(_animation, time,weight1);
            b.Rewind(_animation, time,weight2);
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
            MakeConnections();
        }
        
        public override void UnpackClips()
        {
            _listNames.Clear();
            foreach(AnimationState k in _animation)
                _listNames.Add(_animation[k.name].name);
            foreach(var k in _listNames)
                _animation.RemoveClip(k);
        }

        private void MakeConnections()
        {
            List<TimelineAnimationClip> sortedNumbers = _listARQTimelineClips.OrderBy(i => i._startTime).Select(k=>k as TimelineAnimationClip).ToList();
            _listARQTimelineClips = sortedNumbers.Select(a => a as TimelineClip).ToList(); ;
            if (sortedNumbers.Count >= 3) {
                for (int i = 0; i < sortedNumbers.Count-2; i++)
                {
                    if (sortedNumbers[i]._endTime> sortedNumbers[i+2]._startTime || sortedNumbers[i]._endTime > sortedNumbers[i + 1]._endTime)
                    {
                        //error
                    }
                }
            }
            if (sortedNumbers.Count < 2)
            {
                sortedNumbers[0].FadeLenght = 0;
            }
            else
            {
                if(sortedNumbers[1]._startTime> sortedNumbers[0].AnimationClip.length)
                    sortedNumbers[0].FadeLenght = 0;
                else
                    sortedNumbers[0].FadeLenght = sortedNumbers[1]._startTime;
            }
            for (int i =1; i< sortedNumbers.Count; i++)
            {
                float raznica = sortedNumbers[i - 1]._endTime - sortedNumbers[i]._startTime;
                if (raznica>0.0f)
                {
                    sortedNumbers[i].FadeLenght = raznica;
                }
                else
                {
                    sortedNumbers[i].FadeLenght = 0;
                }
            }
        }
    }
}