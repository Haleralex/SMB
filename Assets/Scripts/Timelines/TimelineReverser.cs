using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TimelineReverser
{
    public static void ReplaceClipsOnReversedClips(TimelineAsset timelineAsset, AnimationClip[] animationClips){
        int j = 0;
        for (int i = 0; i < timelineAsset.outputTrackCount; i++)
        {
            TrackAsset track = timelineAsset.GetRootTrack(i);

            AnimationTrack animTrack = track as AnimationTrack;

            if (animTrack)
            {
                foreach(var q in animTrack.GetClips()){
                    var animationAsset = q.asset as AnimationPlayableAsset;
                    var startTime = q.start;
                    q.start = timelineAsset.duration - q.duration-startTime;
                    animationAsset.clip = animationClips[j];
                    j++;
                }
            }
        }
    }
}
