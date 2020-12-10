using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class TimelineDirector : MonoBehaviour
    {
        private static readonly WaitForEndOfFrame _frameWait = new WaitForEndOfFrame();

        public event Action<TimelineDirector> Stopped;
        public event Action<TimelineDirector> Played;
        public event Action<TimelineDirector> Paused;


        private ARQTimeline _arqTimeline;

        [SerializeField]
        private Slider slider;
        [SerializeField]
        AnimationClip animationClip;
        [SerializeField]
        Animation anim;
        [SerializeField]
        AnimationClip animationClip2;
        [SerializeField]
        AnimationClip animationClipExtra;
        [SerializeField]
        AnimationClip animationClipIdle;
        [SerializeField]
        Animation anim2;

        public void Play() {
            if (!_arqTimeline._isStarted)
            {
                StartCoroutine(PlayCoroutine());
                Played?.Invoke(this);
            }
            else
            {
                Resume();
            }
        }

        public void Pause(){
            _arqTimeline._isPaused = true;
            _arqTimeline.Rewind(_arqTimeline.Time);
            Paused?.Invoke(this);
        }

        public void Resume(){
            _arqTimeline._isPaused = false;
        }

        public void Stop(){
            StopCoroutine(PlayCoroutine());
            _arqTimeline._isStarted = false;
            Stopped?.Invoke(this);
        }

        private IEnumerator PlayCoroutine()
        {
            _arqTimeline._isStarted = true;
            _arqTimeline.Rewind(slider.value);
            while (_arqTimeline.Time< _arqTimeline.duration || _arqTimeline._isPaused)
            {
                if (!_arqTimeline._isPaused)
                {
                    _arqTimeline.Time += Time.deltaTime;
                    slider.SetValueWithoutNotify(_arqTimeline.Time);
                }
                yield return _frameWait;
            }
            _arqTimeline._isStarted = false;
        }

        private void Start()
        {
            CreateTestARQTimeline();
        }

        private void CreateTestARQTimeline()
        {
            _arqTimeline = ScriptableObject.CreateInstance<ARQTimeline>();


            animationClip.legacy = true;
            animationClip2.legacy = true;
            animationClipExtra.legacy = true;
            animationClipIdle.legacy = true;

            anim.AddClip(animationClip, animationClip.name);
            anim.AddClip(animationClipExtra, animationClipExtra.name);
            anim2.AddClip(animationClip2, animationClip2.name);
            anim2.AddClip(animationClipIdle, animationClipIdle.name);



            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<ARQTimelineAnimationTrack>();
            var arqTimelineAnimationTrack2 = _arqTimeline.CreateTrack<ARQTimelineAnimationTrack>();

            arqTimelineAnimationTrack._animation = anim;
            arqTimelineAnimationTrack2._animation = anim2;


            var timelineClip = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(2, 1);
            var timelineClipExtra = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(3f, 1);
            var timelineClip2 = arqTimelineAnimationTrack2.CreateClip<ARQTimelineAnimationClip>(1, 21.25f);
            var timelineClipIdle = arqTimelineAnimationTrack2.CreateClip<ARQTimelineAnimationClip>(1, 4f);


            timelineClip._animationClip = animationClip;
            timelineClip2._animationClip = animationClip2;
            timelineClipExtra._animationClip = animationClipExtra;
            timelineClipIdle._animationClip = animationClipIdle;

            slider.maxValue = _arqTimeline.duration;
            slider.onValueChanged.AddListener((value) => _arqTimeline.Rewind(value));
        }

        class test : IEnumerator
        {
            public object Current => value;

            int value = 0;

            public bool MoveNext()
            {
                value++;
                print(value);
                return value < 5;
            }

            public void Reset()
            {
                value = 0;
            }
        }

        private IEnumerator TestCor()
        {
            return new test();
        }
    }
}