using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Test
{
    public class ARQTimelineAnimationClip : ARQTimelineClip
    {
        public AnimationClip _animationClip;
        public override void Play(Animation animation, float time)
        {
            animation[_animationClip.name].wrapMode = WrapMode.Once;
            animation[_animationClip.name].blendMode = AnimationBlendMode.Blend;
            animation.CrossFade(_animationClip.name, 1f);
            //animation.Blend(_animationClip.name,1f,0.5f);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 1;
        }

        public override void Rewind(Animation animation, float time)
        {
            animation.Stop();
            animation.Blend(_animationClip.name, 1f, 1f);
            animation[_animationClip.name].time = time - _startTime;
            animation[_animationClip.name].speed = 0;
        }

    }
}