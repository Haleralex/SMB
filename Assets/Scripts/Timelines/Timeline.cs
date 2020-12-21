using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARQTimeline
{
    public class Timeline
    {
        #region Fields
        private List<TimelineTrack> _listARQTimelineTrack = new List<TimelineTrack>();
        

        public float duration { 
            get 
            {
                float maxDur = 0.0f;
                foreach(TimelineTrack timelineTrack in _listARQTimelineTrack)
                {
                    if (timelineTrack.maxEndTime > maxDur)
                        maxDur = timelineTrack.maxEndTime;
                }
                return maxDur;
            }
        }

        public event Action<Timeline, float> TimeWasChanged;

        public bool _isPaused = false;
        public bool _isStarted = false;

        private float time = 0;
        public float Time
        {
            get { return time; }
            set
            {
                TimeWasChanged(this, time); 
                time = value;
            }
        }
        #endregion



        #region Methods
        public T CreateTrack<T,P>(P director) where T : TimelineTrack, new()
        {
            T addedTrack = new T();
            AddTrack(addedTrack);
            addedTrack.SetDirector(director);
            addedTrack.BindTimeline(this);
            return addedTrack;
        }
        private void AddTrack<T>(T addedTrack) where T : TimelineTrack
        {
            _listARQTimelineTrack.Add(addedTrack);
        }
        public bool DeleteTrack(TimelineTrack track)
        {
            if (_listARQTimelineTrack.Contains(track))
            {
                _listARQTimelineTrack.Remove(track);
                return true;
            }
            return false;
        }

        public void Rewind(float time)
        {
            Time = time;
            foreach (TimelineTrack timelineTrack in _listARQTimelineTrack)
            {
                timelineTrack.Rewind(time);
            }
        }

        #endregion

    }
}

