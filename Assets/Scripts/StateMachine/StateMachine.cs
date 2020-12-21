using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ARQTimeline;
namespace ARQStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private TimelineDirector _arqTimelineDirector;

        private List<State> _listAllStates = new List<State>();

        private Queue<Timeline> _queueARQTimelines = new Queue<Timeline>();

        private State _currentState;

        private State _startState;

        public bool QueueMod = false;

        public void StartMachine()
        {
            _currentState = _startState;
            _currentState.StartNode();
        }

        private void Update()
        {
            _currentState?.Tick();
        }

        public void CreateState(string stateName, ARQTimeline.Timeline timeline = null, List<Transition> listTransitions = null, bool isStart = false)
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
            Timeline timeline = state.timeline;

            if (timeline == null)
                return;

            if (QueueMod)
            {
                if (_arqTimelineDirector.ARQTimeline == null || !_arqTimelineDirector.ARQTimeline._isStarted)
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

    }
}