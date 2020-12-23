using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ARQTimeline;
using System;

namespace ARQStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private TimelineDirector _arqTimelineDirector;

        private List<State> _listAllStates = new List<State>();

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

        public void CreateState(string stateName, Timeline timeline = null, List<Transition> listTransitions = null, bool isStart = false)
        {
            while(_listAllStates.Where(a => a._nodeName == stateName).ToList().Count > 0) // чтобы с одним именем не было
            {
                stateName += "1";
            }
            State state = new State(stateName, timeline, listTransitions, isStart);
            state.StateEntered += () => { PackState(state); StartState(state);  };
            state.SuccessCheck += FindNextState;
            _listAllStates.Add(state);
            if (isStart)
            {
                _startState = state;
                _startState.NodeComplete += OnNodeComplete;
            }
        }

        private void PackState(State state)
        {
            state.PackState();
        }
        private void UnpackState(State state)
        {
            state.UnpackState();
        }

        private void FindNextState(string next)
        {
            State nextState = _listAllStates.Where(a => a._nodeName == next).FirstOrDefault();

            if (_currentState._isCompleted)
            {
                _currentState.OnExit();
                _currentState.NodeComplete -= OnNodeComplete;
                nextState.NodeComplete += OnNodeComplete;
                _currentState = nextState;
                _currentState.StartNode();
            }
            else
            {
                if (QueueMod)
                {
                    _arqTimelineDirector.Finished = () =>
                    {
                        _currentState.OnExit();
                        _currentState.NodeComplete -= OnNodeComplete;
                        nextState.NodeComplete += OnNodeComplete;
                        _currentState = nextState;
                        _currentState.StartNode();
                    };
                }
                else
                {
                    _currentState.OnTimelineFinished();
                    _currentState.OnExit();
                    _currentState.NodeComplete -= OnNodeComplete;
                    nextState.NodeComplete += OnNodeComplete;
                    _currentState = nextState;
                    _currentState.StartNode();
                }
            }
        }
        private void NodeComplete(State completed)
        {
            completed.NodeComplete -= OnNodeComplete;
        }
        private void OnNodeComplete(State completed)
        {
            NodeComplete(completed);
        }

        public void StartState(State state)
        {
            Timeline timeline = state.timeline;

            if (timeline == null)
                return;

            _arqTimelineDirector.Stop();
            _arqTimelineDirector.ARQTimeline = timeline;
            _arqTimelineDirector.Play();
        }
    }
}