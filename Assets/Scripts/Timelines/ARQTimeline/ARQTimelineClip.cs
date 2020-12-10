using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public abstract class ARQTimelineClip : ScriptableObject, ISerializationCallbackReceiver
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

        public virtual void Play(Animation animation, float time)
        {
            
        }
        public virtual void Rewind(Animation animation, float time)
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
