using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARQTimeline;

namespace ARQStateMachine
{
    public class State
    {
        public event Action StateEntered;

        public List<Transition> _listTransitions = new List<Transition>();
        public readonly Timeline timeline;
        public event Action<State> NodeComplete;
        public event Action<string> SuccessCheck;

        private bool _isStarted = false;
        public bool _isCompleted = false;

        public bool _isStart = false;

        public readonly string _nodeName = " ";

        
        public State(string nodeName, Timeline timeline, List<Transition> listTransitions, bool isStart)
        {
            _isStart = isStart;
            _nodeName = nodeName;
            this.timeline = timeline;
            _listTransitions = listTransitions;
            timeline.TimelineFinished += OnTimelineFinished;
        }

        public void OnTimelineFinished()
        {
            _isCompleted = true;
            NodeComplete?.Invoke(this);
        }

        public State()
        {
            _isStart = false;
            _nodeName = " ";
            timeline = null;
            _listTransitions = new List<ARQTimeline.Transition>();

        }

        public void Tick()
        {
            if (!_isStarted)
                return;
            
            OnUpdate();
        }


        #region Methods

        public void StartNode()
        {
            _isCompleted = false;
            _isStarted = true;
            OnEnter();
        }

        #endregion

        protected void CheckCondition()
        {
            string next = GoNext();
            if (next == null)
                return;
            SuccessCheck?.Invoke(next);
        }

        internal void UnpackState()
        {
            timeline.UnpackTracks();
        }

        internal void PackState()
        {
            timeline.PackTracks();
        }

        protected string GoNext()
        {
            foreach(var k in _listTransitions)
            {
                if (Input.GetKeyDown(k._keyCode))
                {
                    return k._nameAimState;
                }
            }
            return null;
        }

        protected void OnUpdate()
        {
            CheckCondition();
        }
        protected void OnEnter()
        {
            StateEntered?.Invoke();
        }

        public void OnExit()
        {
            _isCompleted = true;
            _isStarted = false;
        }
    }
}
