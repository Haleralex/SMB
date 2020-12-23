﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQTimeline
{
    public abstract class TimelineTrack
    {
        public readonly List<TimelineClip> _listARQTimelineClips = new List<TimelineClip>();

        public float maxEndTime { 
            get
            {
                float maxTime = 0.0f;
                foreach(TimelineClip timelineClip in _listARQTimelineClips)
                {
                    if (timelineClip._endTime > maxTime)
                    {
                        maxTime = timelineClip._endTime;
                    }
                }
                return maxTime;
            } 
        }

        public virtual void SetPlayer<T>(T player) 
        {

        }

        public T CreateClip<T, P>(float start, float duration, P clipInstance) where T: TimelineClip, new(){
            T addedClip = CreateDefaultClip<T>();
            addedClip.SetInstance(clipInstance);
            addedClip._startTime = start;
            addedClip._endTime = start+duration;
            AddClip(addedClip);
            return addedClip;
        }
        public T CreateDefaultClip<T>() where T: TimelineClip, new()
        {
            T addedClip = new T();
            AddClip(addedClip);
            return addedClip;
        }
        public void AddClip<T>(T clip) where T:TimelineClip {
            _listARQTimelineClips.Add(clip);
        }
        public void DeleteClip<T>(T clip) where T:TimelineClip {
            _listARQTimelineClips.Remove(clip);
        }

        public void BindTimeline(Timeline aRQTimeline)
        {
            aRQTimeline.TimeWasChanged += ARQTimeline_TimeWasChanged;  
        }

        private void ARQTimeline_TimeWasChanged(Timeline timeline, float time)
        {
            if (timeline._isStarted && !timeline._isPaused)
                CheckTime(time);
            else
                Rewind(time);
        }

        public virtual void CheckTime(float time)
        {
            
        }
        public virtual void Rewind(float time)
        {

        }

        public virtual void PackClips()
        {
            
        }

        public virtual void UnpackClips()
        {
            
        }
    }
}
