using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARQTimeline{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private Animation animation;
        [SerializeField]
        private AnimationClip animationClip1;
        [SerializeField]
        private AnimationClip animationClip2;
        private Timeline Create1Timeline()
        {
            var _arqTimeline = new Timeline();
            animationClip1.legacy = true;
            animationClip2.legacy = true;
            animation.AddClip(animationClip1, animationClip1.name);
            animation.AddClip(animationClip2, animationClip2.name);
            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<TimelineAnimationTrack>();


            arqTimelineAnimationTrack.Animation = animation;
            var timelineClip = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip>(2, 1);
            var timelineClipExtra = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip>(2.5f, 1);
            timelineClip.AnimationClip = animationClip1;
            timelineClipExtra.AnimationClip = animationClip2;
            return _arqTimeline;
        }
        [SerializeField]
        private AudioSource audioSource1;
        [SerializeField]
        private AudioClip audioClip1;
        [SerializeField]
        private AudioSource audioSource2;
        [SerializeField]
        private AudioClip audioClip2;
        [SerializeField]
        private Animation animation2;
        [SerializeField]
        private AnimationClip animationClip11;
        [SerializeField]
        private AnimationClip animationClip12;
        private Timeline Create2Timeline()
        {
            var _arqTimeline = new Timeline();
            animationClip11.legacy = true;
            animationClip12.legacy = true;
            animation2.AddClip(animationClip11, animationClip11.name);
            animation2.AddClip(animationClip12, animationClip12.name);

            var arqTimelineAudioTrack = _arqTimeline.CreateTrack<TimelineAudioTrack>();
            var arqAudioClip1 = arqTimelineAudioTrack.CreateClip<TimelineAudioClip>(1, 1);
            arqAudioClip1.AudioSource = audioSource1;
            arqAudioClip1.AudioClip = audioClip1;
            var arqAudioClip2 = arqTimelineAudioTrack.CreateClip<TimelineAudioClip>(2, 1);
            arqAudioClip2.AudioSource = audioSource2;
            arqAudioClip2.AudioClip = audioClip2;

            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<TimelineAnimationTrack>();
            arqTimelineAnimationTrack.Animation = animation2;
            var timelineClip2 = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip>(1, 21.25f);
            var timelineClipIdle = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip>(1, 4f);
            timelineClip2.AnimationClip = animationClip11;
            timelineClipIdle.AnimationClip = animationClip12;
            return _arqTimeline;
        }


        [SerializeField]
        private ARQStateMachine.StateMachine _stateMachine;
        public void CreateTestStates()
        {
            _stateMachine.CreateState("Half", Create1Timeline(), new List<Transition>() { new Transition(KeyCode.A, "HalfParts") }, true);
            
            _stateMachine.CreateState("HalfParts", Create2Timeline(), new List<Transition>() { new Transition(KeyCode.B, "Half") }, false);

        }

        public void TestStartStateMachine()
        {
            _stateMachine.StartMachine();
        }
    }
}

