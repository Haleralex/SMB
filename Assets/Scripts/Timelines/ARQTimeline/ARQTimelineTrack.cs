using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public abstract class ARQTimelineTrack : ScriptableObject, ISerializationCallbackReceiver
    {
        public readonly List<ARQTimelineClip> _listARQTimelineClips = new List<ARQTimelineClip>();

        public float maxEndTime { 
            get
            {
                float maxTime = 0.0f;
                foreach(ARQTimelineClip timelineClip in _listARQTimelineClips)
                {
                    if (timelineClip._endTime > maxTime)
                    {
                        maxTime = timelineClip._endTime;
                    }
                }
                return maxTime;
            } 
        }

        public T CreateClip<T>(float start, float duration) where T: ARQTimelineClip{
            T addedClip = CreateDefaultClip<T>();
            addedClip._startTime = start;
            addedClip._duration = duration;
            AddClip(addedClip);
            return addedClip;
        }
        public T CreateDefaultClip<T>() where T: ARQTimelineClip{
            T addedClip = ScriptableObject.CreateInstance<T>();
            AddClip(addedClip);
            return addedClip;
        }
        public void AddClip<T>(T clip) where T:ARQTimelineClip {
            _listARQTimelineClips.Add(clip);
        }
        public void DeleteClip<T>(T clip) where T:ARQTimelineClip {
            _listARQTimelineClips.Remove(clip);
        }

        public void BindTimeline(ARQTimeline aRQTimeline)
        {
            aRQTimeline.TimeWasChanged += ARQTimeline_TimeWasChanged;  
        }

        private void ARQTimeline_TimeWasChanged(ARQTimeline timeline, float time)
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
        public void OnAfterDeserialize()
        {
            
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}
