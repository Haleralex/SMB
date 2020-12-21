using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARQTimeline
{
    public class TimelineAnimationClip : TimelineClip
    {
        private AnimationClip _animationClip;
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
            animation.Blend(_animationClip.name);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 1;
        }

        public override void Rewind<T>(T director, float time)
        {
            Animation animation = director as Animation;
            animation.Stop();
            animation.Blend(_animationClip.name);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 0;
        }

        public override void SetInstance<T>(T instance)
        {
            _animationClip = instance as AnimationClip;
        }

    }
}