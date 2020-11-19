using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
    [SerializeField]
    PlayableDirector playableDirector;

    [SerializeField]
    Slider slider;
    private void Update(){
        slider.SetValueWithoutNotify((float)playableDirector.time);
    }

    private void OnEnable(){
        slider.onValueChanged.AddListener(RewindTimeline);
        slider.maxValue = (float)playableDirector.duration;
    }
    public void RewindTimeline(float time){
        playableDirector.Rewind(time);
    }
    public void PauseTimeline(){
        var time = (float)playableDirector.time;
        playableDirector.Stop();
        playableDirector.Rewind(time);
    }
    public void PlayTimeline(){
        playableDirector.Play();
    }
    private void OnDisable(){
         slider.onValueChanged.RemoveListener(RewindTimeline);
    }
    
}
