using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQTimeline
{
    public abstract class TimelineClip
    {
        public float _startTime = 0.0f;

        public float _endTime = 1.0f;

        public bool wasStarted = false;

        public float _duration  {
            get{
                return _endTime-_startTime;
                } 
            set{
                _endTime = _startTime+value;
            }
        }

        public Object asset { get; set; }

        public virtual void Play<T>(T director, float time)
        {
            
        }
        public virtual void Rewind<T>(T director, float time)
        {
            
        }
    }
}
