using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private List<State> _listAllStates = new List<State>();

        private State _currentState;

        private void Start()
        {
            foreach (State state in _listAllStates)
            {
                state.NodeComplete += OnNodeComplete;
                state.StartNode();
            }
        }

        private void Update()
        {
            _currentState?.Tick();
        }

        private void OnNodeComplete(State completed, State next)
        {
            completed.NodeComplete -= OnNodeComplete;
            next.NodeComplete += OnNodeComplete;
            _listAllStates.Remove(completed);
            _listAllStates.Add(next);
            next.StartNode();
        }
    }
}