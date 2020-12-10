using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
public class AnimationBehaviour : MonoBehaviour
{
	public Animation anim;
	AnimationClip animationClip;
	public Text tx;

	void Start()
	{
		anim = GetComponent<Animation>();
		// define animation curve
		AnimationCurve translateX = AnimationCurve.Linear(0.0f, 0.0f, 2.0f, 2.0f);
		animationClip = new AnimationClip();
		// set animation clip to be legacy
		//animationClip = false;
		animationClip.SetCurve("", typeof(Transform), "localPosition.x", translateX);
		tx.text = animationClip.length.ToString();
		//anim.AddClip(animationClip, "test");
		//anim.Play("test");
	}
}