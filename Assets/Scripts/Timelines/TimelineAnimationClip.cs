using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARQTimeline
{
    public class TimelineAnimationClip : TimelineClip
    {
        private AnimationClip _animationClip;
        private float _targetWeight = 1f;
        private float _fadeLenght = 0f;
        public float TargetWeight { 
            get { 
                return _targetWeight; 
            } 
            set {
                _targetWeight = Mathf.Clamp(value, 0, 1);
            } 
        }
        public float FadeLenght {
            get
            {
                return _fadeLenght;
            }
            set
            {
                _fadeLenght = Mathf.Clamp(value, 0, 1);
            }
        }
    public AnimationClip AnimationClip 
        { 
            get 
            { 
                return _animationClip; 
            } 
            set 
            { 
                _animationClip = value; 
            } 
        }
        public override void Play<T>(T director, float time)
        {
            Animation animation = director as Animation;
            animation[_animationClip.name].wrapMode = WrapMode.Clamp;
            animation[_animationClip.name].blendMode = AnimationBlendMode.Blend;

            animation.Blend(_animationClip.name, _targetWeight, _fadeLenght);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 1;
        }

        public override void Rewind<T>(T director, float time)
        {
            Animation animation = director as Animation;

            animation[_animationClip.name].wrapMode = WrapMode.Clamp;
            animation[_animationClip.name].blendMode = AnimationBlendMode.Blend;
            if (time >= 0.5f && time <= 1.0f) 
            {
                if (_animationClip.name == "StartToHalf")
                    animation.Blend(_animationClip.name, Mathf.Clamp(2 - 2 * time, 0, 1), 0);
                if (_animationClip.name == "StartToParts")
                    animation.Blend(_animationClip.name, Mathf.Clamp(2*time-1,0,1), 0);
            }
            else if (time <= 0.5f)
            {
                if (_animationClip.name == "StartToHalf")
                    animation.Blend(_animationClip.name, 1, 0f);
            }
            else if (time >= 1.0f && time <=1.5f)
            {
                if (_animationClip.name == "StartToParts")
                    animation.Blend(_animationClip.name, Mathf.Clamp(3- 2*time, 0, 1), 0);
                if (_animationClip.name == "StartToHalf")
                    animation.Blend(_animationClip.name, Mathf.Clamp(time-1, 0, 1), 0);
            }
            else if (time >= 1.5f)
            {
                if (_animationClip.name == "StartToHalf")
                    animation.Blend(_animationClip.name, 1, 0);
            }


            animation[_animationClip.name].speed = 0;
            animation[_animationClip.name].time = time - _startTime;
            animation.Sample();
        }
        public void Rewind<T>(T director, float time, float targetWeight, float fadeLenght)
        {
            Animation animation = director as Animation;

            animation[_animationClip.name].wrapMode = WrapMode.Clamp;
            animation[_animationClip.name].blendMode = AnimationBlendMode.Blend;

            animation.Blend(_animationClip.name, targetWeight, fadeLenght);

            //animation[_animationClip.name].speed = 0;
            animation[_animationClip.name].time = time - _startTime;
            animation.Sample();
        }


            public void RewindAfterPlaying<T>(T director, float time)
        {
            Animation animation = director as Animation;

            animation[_animationClip.name].wrapMode = WrapMode.Clamp;
            animation[_animationClip.name].blendMode = AnimationBlendMode.Blend;
            
            animation.Blend(_animationClip.name, 0, 0);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 0;
            animation.Sample();
        }

        public override void SetInstance<T>(T instance)
        {
            _animationClip = instance as AnimationClip;
        }

    }
}