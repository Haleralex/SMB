using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQStateMachine
{
    public class State
    {
        public event Action StateEntered;
        public event Action StateExited;

        public List<Test.Transition> _listTransitions = new List<Test.Transition>();
        public Test.ARQTimeline timeline;
        public event Action<State, string> NodeComplete;

        private bool _isStarted = false;
        private bool _isCompleted = false;

        public bool _isStart = false;

        public readonly string _nodeName = " ";

        
        public State(string nodeName, Test.ARQTimeline timeline, List<Test.Transition> listTransitions, bool isStart)
        {
            _isStart = isStart;
            _nodeName = nodeName;
            this.timeline = timeline;
            _listTransitions = listTransitions;
        }

        public State()
        {
            _isStart = false;
            _nodeName = " ";
            timeline = null;
            _listTransitions = new List<Test.Transition>();
        }

        public void Tick()
        {
            if (!_isStarted || _isCompleted)
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
            _isCompleted = true;
            _isStarted = false;
            NodeComplete?.Invoke(this, next);
            OnExit();
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
        protected void OnExit()
        {
            StateExited?.Invoke();
        }
    }
}
