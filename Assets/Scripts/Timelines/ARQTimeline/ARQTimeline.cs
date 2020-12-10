using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class ARQTimeline : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Fields
        private List<ARQTimelineTrack> _listARQTimelineTrack = new List<ARQTimelineTrack>();


        public float duration { 
            get 
            {
                float maxDur = 0.0f;
                foreach(ARQTimelineTrack timelineTrack in _listARQTimelineTrack)
                {
                    if (timelineTrack.maxEndTime > maxDur)
                        maxDur = timelineTrack.maxEndTime;
                }
                return maxDur;
            }
        }

        public event Action<ARQTimeline, float> TimeWasChanged;

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
        public T CreateTrack<T>() where T : ARQTimelineTrack
        {
            T addedTrack = ScriptableObject.CreateInstance<T>();
            AddTrack(addedTrack);
            addedTrack.BindTimeline(this);
            return addedTrack;
        }

        private void AddTrack<T>(T addedTrack) where T : ARQTimelineTrack
        {
            _listARQTimelineTrack.Add(addedTrack);
        }
        public bool DeleteTrack(ARQTimelineTrack track)
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
            //_isPaused = true;
            Time = time;
            foreach (ARQTimelineTrack timelineTrack in _listARQTimelineTrack)
            {
                timelineTrack.Rewind(time);
            }
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
        #endregion

    }
}

