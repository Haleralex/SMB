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
            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<TimelineAnimationTrack, Animation>(animation);

            var timelineClip = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip, AnimationClip>(2, 1, animationClip1);
            var timelineClipExtra = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip, AnimationClip>(2.5f, 1, animationClip2);
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

            var arqTimelineAudioTrack = _arqTimeline.CreateTrack<TimelineAudioTrack, GameObject>(gameObject); //не так

            var arqAudioClip1 = arqTimelineAudioTrack.CreateClip<TimelineAudioClip, AudioSource>(1, 1, audioSource1);
            arqAudioClip1.AudioClip = audioClip1;
            var arqAudioClip2 = arqTimelineAudioTrack.CreateClip<TimelineAudioClip, AudioSource>(2, 1, audioSource2);
            arqAudioClip2.AudioClip = audioClip2;

            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<TimelineAnimationTrack,Animation>(animation2);
            var timelineClip2 = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip, AnimationClip>(1, 21.25f, animationClip11);
            var timelineClipIdle = arqTimelineAnimationTrack.CreateClip<TimelineAnimationClip, AnimationClip>(1, 4f, animationClip12);
            return _arqTimeline;
        }


        [SerializeField]
        private ARQStateMachine.StateMachine _stateMachine;
        public void CreateTestStates()
        {
            _stateMachine.CreateState("Half", Create1Timeline(), new List<Transition>() { new Transition(KeyCode.A, "HalfParts") }, true);
            
            _stateMachine.CreateState("HalfParts", Create2Timeline(), new List<Transition>() { new Transition(KeyCode.B, "Half") }, false);

        }

        /* аттрибуты state 
         * 1)имя
         * 2)таймлайн
         * 3)пути из state (transition)
         * 4)является ли state стартовым
         * 
         * аттрибуты таймлайна 
         * 1)имя
         * 2)длительность
         * 3)список трэков
         * 4)мод (очередь или обычная)
         * 
         * атрибуты трэка(абстрактного)
         * 1)список клипов
         * 
         * атрибуты клипа(абстарктного)
         * 1)старт тайм
         * 2)энд тайм
         * 3)длительность
         * 
         * аттрибуты трэка(анимации)
         * 1)animation
         * 2)последняя активное время трэка (типо последний момент у трэка)
         * 
         * аттрибуты трэка(аудио)
         * 1)audioSource
         * 
         */



        public void TestStartStateMachine()
        {
            _stateMachine.StartMachine();
        }
    }
}

