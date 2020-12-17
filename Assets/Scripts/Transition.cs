using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Test
{
    [Serializable]
    public class Transition 
    {
        public readonly KeyCode _keyCode;
        public readonly string _nameAimState;

        public Transition(KeyCode keyCode, string nameAimState)
        {
            _keyCode = keyCode;
            _nameAimState = nameAimState;
        }
    }
}