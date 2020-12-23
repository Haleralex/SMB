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
        public override void Play(float time)
        {
            _audioSource.clip = _audioClip;
            _audioSource.time = time - _startTime;
            _audioSource.Play();
        }

        public override void Rewind(float time)
        {
            _audioSource.clip = _audioClip;
            if(time - _startTime > _endTime)
                _audioSource.time = _duration;
            else
                _audioSource.time = time - _startTime;
            _audioSource.Pause();
        }

        public override void SetInstance<T>(T instance)
        {
            _audioSource = instance as AudioSource;
        }
    }
}
