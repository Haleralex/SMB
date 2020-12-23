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
            
        }


        public virtual void Play<T>(T player, float time)
        {
            
        }
        public virtual void Play(float time)
        {

        }
        public virtual void Rewind<T>(T player, float time)
        {
            
        }
        public virtual void Rewind(float time)
        {

        }
        public virtual void SetInstance<T>(T clipInstance)
        {

        }
    }
}
