using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
            foreach (TimelineClip arqTimelineClip in _listARQTimelineClips)
            {
                if (time >= arqTimelineClip._startTime && time <= arqTimelineClip._endTime)
                {
                    (arqTimelineClip as TimelineAnimationClip).Rewind(_animation, time);
                    arqTimelineClip.wasStarted = false;
                }
                else if (time < arqTimelineClip._startTime)
                {
                    _animation[(arqTimelineClip as TimelineAnimationClip).AnimationClip.name].speed = 0;
                    arqTimelineClip.wasStarted = false; 
                }
                else if (time > arqTimelineClip._endTime)
                {
                    (arqTimelineClip as TimelineAnimationClip).RewindAfterPlaying(_animation, arqTimelineClip._endTime);
                    arqTimelineClip.wasStarted = true;
                }
            }
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