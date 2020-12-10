using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    /*public class Animation : MonoBehaviour
    {
        public AnimationClip animationClip;// {get; set;}
        public Text tx;

        private void Start(){
            var posxKeys = new Keyframe[3];
            posxKeys[0] = new Keyframe(0.0f, 0.0f);
            posxKeys[1] = new Keyframe(4.0f, 1.0f);
            posxKeys[2] = new Keyframe(8.0f, 0.0f);
            var curve = new AnimationCurve(posxKeys);
            curve.Evaluate(1);
            AnimationClip clip = new AnimationClip
            {
                legacy = false
            };
            clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
            clip.EnsureQuaternionContinuity();
            
        }

        public static Animation ConvertStringToAnimation(string stringAnimation){
            
            
            
            return null;
        }

        public string ConvertAnimationToString(){
            string result = "";
            
            return result;
        }
    }*/


    //animation = curve curve curve = (keyframe, keyframe, keyframe) (keyframe, keyframe, keyframe) (keyframe, keyframe, keyframe) 
    public struct KindaKey
    {
        public Vector3 pos;
        public float timeBeforeTheKey;
    }

    public struct AnimCondition
    {
        public string path;
        public string propertyName;
        public Keyframe startKeyFrame;
        public Keyframe endKeyFrame;
    }
}

