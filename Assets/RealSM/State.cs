using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQStateMachine
{
    public class State
    {
        public List<Test.Transition> _connectedStates = new List<Test.Transition>();
        public Test.ARQTimeline timeline;
        public event Action<State, State> NodeComplete;

        private bool _isStarted = false;
        private bool _isCompleted = false;

        public string _nodeName = " ";

        
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
            foreach(var k in _connectedStates)
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

        }
        protected void OnExit()
        {

        }
    }
}
