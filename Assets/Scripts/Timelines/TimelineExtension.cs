using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class TimelineExtension 
{
    private static readonly WaitForEndOfFrame _frameWait = new WaitForEndOfFrame();

    public static void ReversePlay(this UnityEngine.Playables.PlayableDirector timeline, MonoBehaviour l){
        l.StartCoroutine(Reverse(timeline));
    }
    public static void Rewind(this UnityEngine.Playables.PlayableDirector timeline, float time){
        timeline.time = time;
        timeline.Evaluate();
        if(timeline.state == PlayState.Paused){
            timeline.Stop();
        }
        timeline.time = time;
    }
    private static IEnumerator Reverse(UnityEngine.Playables.PlayableDirector timeline){
        UnityEngine.Playables.DirectorUpdateMode defaultUpdateMode = timeline.timeUpdateMode;

        if(timeline.time.ApproxEquals(timeline.duration) || timeline.time.ApproxEquals(0)){
            timeline.time = timeline.duration;
        }
        timeline.Evaluate();

        yield return _frameWait;

        float dt = (float)timeline.duration;
        while(dt > 0){
            dt -= Time.deltaTime / (float)timeline.duration;
            timeline.time = Mathf.Max(dt,0);
            timeline.Evaluate();

            yield return _frameWait;
        }

        timeline.time = 0;
        timeline.Evaluate();
        timeline.timeUpdateMode = defaultUpdateMode;
        timeline.Stop();
    }

    public static bool ApproxEquals(this double num, float other){
        return Mathf.Approximately((float)num, other);
    }
    public static bool ApproxEquals(this double num, double other){
        return Mathf.Approximately((float)num, (float)other);
    }
}
