using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Test;
namespace ARQStateMachine
{
    //что то напоминает да?))))))
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private ARQTimelineDirector _arqTimelineDirector;

        private List<State> _listAllStates = new List<State>();

        private Queue<ARQTimeline> _queueARQTimelines = new Queue<ARQTimeline>();

        private State _currentState;

        private State _startState;

        public bool QueueMod = false;

        private void Start()
        {
            StartMachine();
        }

        private void StartMachine()
        {
            //test
            CreateState("Half", Create1Timeline(), new List<Transition>() { new Transition(KeyCode.A, "HalfParts") }, true);
            //test
            CreateState("HalfParts", Create2Timeline(), new List<Transition>() { new Transition(KeyCode.B, "Half") }, false);
            
            _currentState = _startState;
            _currentState.StartNode();
        }

        private void Update()
        {
            _currentState?.Tick();
        }

        public void CreateState(string stateName, ARQTimeline timeline, List<Transition> listTransitions, bool isStart = false)
        {
            while(_listAllStates.Where(a => a._nodeName == stateName).ToList().Count > 0) // чтобы с одним именем не было
            {
                stateName += "1";
            }
            State state = new State(stateName, timeline, listTransitions, isStart);
            state.StateEntered += () => StartState(state);
            _listAllStates.Add(state);
            if (isStart)
            {
                _startState = state;
                _startState.NodeComplete += OnNodeComplete;
            }
        }

        private void OnNodeComplete(State completed, string next)
        {
            completed.NodeComplete -= OnNodeComplete;
            State nextState = _listAllStates.Where(a => a._nodeName == next).FirstOrDefault();
            nextState.NodeComplete += OnNodeComplete;
            _currentState = nextState;
            _currentState.StartNode();
        }

        public void StartState(State state)
        {
            ARQTimeline timeline = state.timeline;
            if (QueueMod)
            {
                if (!_arqTimelineDirector.ARQTimeline || !_arqTimelineDirector.ARQTimeline._isStarted)
                {
                    _arqTimelineDirector.ARQTimeline = timeline;
                    _arqTimelineDirector.Play();
                }
                else
                {
                    _queueARQTimelines.Enqueue(timeline);
                    _arqTimelineDirector.Finished = PlayFromQueue;
                }
            }
            else
            {
                _arqTimelineDirector.Stop();
                _arqTimelineDirector.ARQTimeline = timeline;
                _arqTimelineDirector.Play();
            }
        }

        void PlayFromQueue()
        {
            if (_queueARQTimelines.Count > 0)
            {
                _arqTimelineDirector.ARQTimeline = _queueARQTimelines.Dequeue();
                _arqTimelineDirector.Play();
            }
        }

        // тут создаётся два таймлайна йопта
        #region test 
        [SerializeField]
        private Animation animation;
        [SerializeField]
        private AnimationClip animationClip1;
        [SerializeField]
        private AnimationClip animationClip2;
        private ARQTimeline Create1Timeline()
        {
            var _arqTimeline = ScriptableObject.CreateInstance<ARQTimeline>();
            animationClip1.legacy = true;
            animationClip2.legacy = true;
            animation.AddClip(animationClip1, animationClip1.name);
            animation.AddClip(animationClip2, animationClip2.name);
            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<ARQTimelineAnimationTrack>();
            arqTimelineAnimationTrack.Animation = animation;
            var timelineClip = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(2, 1);
            var timelineClipExtra = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(2.5f, 1);
            timelineClip.AnimationClip = animationClip1;
            timelineClipExtra.AnimationClip = animationClip2;
            return _arqTimeline;
        }

        [SerializeField]
        private Animation animation2;
        [SerializeField]
        private AnimationClip animationClip11;
        [SerializeField]
        private AnimationClip animationClip12;
        private ARQTimeline Create2Timeline()
        {
            var _arqTimeline = ScriptableObject.CreateInstance<ARQTimeline>();
            animationClip11.legacy = true;
            animationClip12.legacy = true;
            animation2.AddClip(animationClip11, animationClip11.name);
            animation2.AddClip(animationClip12, animationClip12.name);
            var arqTimelineAnimationTrack = _arqTimeline.CreateTrack<ARQTimelineAnimationTrack>();
            arqTimelineAnimationTrack.Animation = animation2;
            var timelineClip2 = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(1, 21.25f);
            var timelineClipIdle = arqTimelineAnimationTrack.CreateClip<ARQTimelineAnimationClip>(1, 4f);
            timelineClip2.AnimationClip = animationClip11;
            timelineClipIdle.AnimationClip = animationClip12;
            return _arqTimeline;
        }
        #endregion 
    }
}