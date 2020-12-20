using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARQTimeline
{
    public class TimelineAudioClip : TimelineClip
    {
        private AudioSource _audioSource;
        public AudioSource AudioSource
        {
            get
            {
                return _audioSource;
            }
            set
            {
                _audioSource = value;
            }
        }

        private AudioClip _audioClip;
        public AudioClip AudioClip
        {
            get
            {
                return _audioClip;
            }
            set
            {
                _audioClip = value;
            }
        }
        public override void Play<T>(T director, float time)
        {

            _audioSource.clip = _audioClip;
            _audioSource.time = time - _startTime;
            _audioSource.Play();
            /*audioSource.clip
            audioSource.Play()
            audioSource[_audioClip.name].wrapMode = WrapMode.Clamp;
            audioSource[_animationClip.name].blendMode = AnimationBlendMode.Blend;
            audioSource.Blend(_animationClip.name);
            audioSource[_animationClip.name].time = time - _startTime;
            audioSource[_animationClip.name].speed = 1;*/
        }

        public override void Rewind<T>(T director, float time)
        {
            _audioSource.Stop();
            _audioSource.clip = _audioClip;
            _audioSource.time = time - _startTime;
            /*audioSource.Stop();
            audioSource.Blend(_animationClip.name);
            audioSource[_animationClip.name].time = time - _startTime;
            audioSource[_animationClip.name].speed = 0;*/
        }
    }
}
