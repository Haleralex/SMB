using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Test
{
    public class ARQTimelineAnimationClip : ARQTimelineClip
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
        public override void Play(Animation animation, float time)
        {
            animation[_animationClip.name].wrapMode = WrapMode.Clamp;
            animation[_animationClip.name].blendMode = AnimationBlendMode.Blend;
            animation.Blend(_animationClip.name);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 1;
        }

        public override void Rewind(Animation animation, float time)
        {
            animation.Stop();
            animation.Blend(_animationClip.name);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 0;
        }

    }
}