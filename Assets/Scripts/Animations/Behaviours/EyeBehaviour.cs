using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EyeBehaviour : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    PlayableDirector playableDirector;
    [SerializeField]
    List<TimelineAsset> lis = new List<TimelineAsset>();
    private void OnEnable(){
        SceneLinkedSMB<EyeBehaviour>.Initialise(anim,this);
    }
    private void Awake(){
        anim = GetComponent<Animator>();
    }

    public void ReturnToBeginning(){
        anim.ResetTrigger("Half");
        anim.ResetTrigger("HalfParts");
        anim.SetTrigger("Obratno");
    }
    public void StartState(string nameState,int id){
        anim.ResetTrigger("Half");
        anim.ResetTrigger("HalfParts");
        anim.SetTrigger(nameState);
        playableDirector.playableAsset = lis[id];
        playableDirector.Play();
    }
}
