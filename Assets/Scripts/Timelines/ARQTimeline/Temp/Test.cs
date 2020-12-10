using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        AnimationClip animationClip;
        [SerializeField]
        AnimationClip idleClip;
        [SerializeField]
        Animation animation;

        private void Start()
        {
            idleClip.legacy = true;
            animationClip.legacy = true;
            animationClip.wrapMode = WrapMode.Once;
            idleClip.wrapMode = WrapMode.Once;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                animation.Blend(idleClip.name);
                animation.Blend(animationClip.name);
            }
            
        }

    }
}

