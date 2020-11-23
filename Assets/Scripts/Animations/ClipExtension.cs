using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ClipExtension
{
    public static AnimationClipCurveData[] GetAllCurves(this AnimationClip clip)
    {
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
        AnimationClipCurveData[] dataArray = new AnimationClipCurveData[curveBindings.Length];
        for (int i = 0; i < dataArray.Length; i++)
        {
            dataArray[i] = new AnimationClipCurveData(curveBindings[i]);
            dataArray[i].curve = AnimationUtility.GetEditorCurve(clip, curveBindings[i]);
        }
        return dataArray;
    }
    static AnimationClip CloneAnimationClip(AnimationClip clip)
    {
        if (clip == null)
            return null;

        var newClip = UnityEngine.Object.Instantiate(clip);
        newClip.name = clip.name + "reversed";

        return newClip;
    }
    public static AnimationClip Reverse(this AnimationClip animationClip)
    {
        var clip = CloneAnimationClip(animationClip);
        if (clip == null)
            return null;
        float clipLength = clip.length;

        var curves = GetAllCurves(clip);
        clip.ClearCurves();
        foreach (AnimationClipCurveData curve in curves)
        {
            var keys = curve.curve.keys;
            int keyCount = keys.Length;
            var postWrapmode = curve.curve.postWrapMode;
            curve.curve.postWrapMode = curve.curve.preWrapMode;
            curve.curve.preWrapMode = postWrapmode;
            for (int i = 0; i < keyCount; i++)
            {
                Keyframe K = keys[i];
                K.time = clipLength - K.time;
                var tmp = -K.inTangent;
                K.inTangent = -K.outTangent;
                K.outTangent = tmp;
                keys[i] = K;
            }
            curve.curve.keys = keys;
            clip.SetCurve(curve.path, curve.type, curve.propertyName, curve.curve);
        }
        var events = AnimationUtility.GetAnimationEvents(clip);
        if (events.Length > 0)
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].time = clipLength - events[i].time;
            }
            AnimationUtility.SetAnimationEvents(clip, events);
        }
        return clip;
    }
    public static AnimationClip CreateAnimationClip()
    {
        var clip = new AnimationClip();
        AnimationCurve curve = AnimationCurve.Linear(0, 0, 2, 2);
        AnimationCurve curve2 = AnimationCurve.Linear(0, 0, 2, 2);
        curve.AddKey(new Keyframe(1, 10f));
        Keyframe keyNew = new Keyframe(5, 10f);
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        clip.SetCurve("kik", typeof(Transform), "localPosition.x", curve2);
        return clip;
    }


    public static AnimationClip CreateAnimationClipParts(List<GameObject> leftPartList)
    {
        var clip = new AnimationClip();        
        var rand = new System.Random();
        foreach(var go in leftPartList){
            string path = GetGameObjectPath(go);
            AnimationCurve curve = AnimationCurve.Linear(0, go.transform.localPosition.z, 1, go.transform.localPosition.z+0.05f);
            float y = rand.Next(-5,5) * 0.01f;
            AnimationCurve curve1 = AnimationCurve.Linear(0,go.transform.localPosition.z, 1, go.transform.localPosition.y + y);
            AnimationCurve curve2 = AnimationCurve.Linear(0,go.transform.localPosition.x, 1, go.transform.localPosition.x);
            clip.SetCurve(path, typeof(Transform), "localPosition.y", curve1);
            clip.SetCurve(path, typeof(Transform), "localPosition.z", curve);
            clip.SetCurve(path, typeof(Transform), "localPosition.x", curve2);
        }
        return clip;
    }

    public static AnimationClip CreateAnimationClipHalf(List<GameObject> leftPartList)
    {
        var clip = new AnimationClip();
        clip.name = "kalkmsdf";
        foreach(var go in leftPartList){
            string path = GetGameObjectPath(go);
            AnimationCurve curve = AnimationCurve.Linear(0, go.transform.localPosition.z, 1,go.transform.localPosition.z+0.05f);
            AnimationCurve curve2 = AnimationCurve.Linear(0, go.transform.localPosition.x, 1,go.transform.localPosition.x);
            clip.SetCurve(path, typeof(Transform), "localPosition.z", curve);
            clip.SetCurve(path, typeof(Transform), "localPosition.x", curve2);
        }
        return clip;
    }

    public static AnimationClip CreateAnimationClipFromConditions(List<AnimCondition> first, List<AnimCondition> second)
    {
        var clip = new AnimationClip();        
        foreach(var e in first){
            var path = e.path;
            var propertyName = e.propertyName;
            var startKeyframe = e.endKeyFrame;
            var endKeyFrame = second.Find(a => a.path == path && a.propertyName == propertyName).endKeyFrame;
            AnimationCurve curve = AnimationCurve.Linear(0, startKeyframe.value, 1, endKeyFrame.value);
            clip.SetCurve(path, typeof(Transform), propertyName, curve);
        }
        return clip;
    }


    public static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name; /*"/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        path.Remove(0,15);*/
        return path;
    }


    public static List<AnimCondition> GetStartAndEndPos(this AnimationClip animationClip){
        List<AnimCondition> result = new List<AnimCondition>();
        foreach(AnimationClipCurveData k in animationClip.GetAllCurves()){
            var startKeyFrame = k.curve[0];
            var endKeyFrame = k.curve[k.curve.length-1];
            var path = k.path;
            var propertyName = k.propertyName;
            var res = new AnimCondition()
            {
                path = path,
                propertyName = propertyName,
                startKeyFrame = startKeyFrame,
                endKeyFrame = endKeyFrame
            };
            result.Add(res);
        }
        return result;
    }
    private static GameObject FindReferencedGameobject(string childName, GameObject parent){
        var i = parent.name + ("\\") +  childName;
        return GameObject.Find(i);
    }

    public static AnimationClip CreateFromDictionary(Dictionary<GameObject,Queue<KindaKey>> arrayKindaKeys){
        var clip = new AnimationClip();
        foreach(GameObject k in arrayKindaKeys.Keys){
            var t = arrayKindaKeys[k].ToArray();
            AnimationCurve curvex = AnimationCurve.Linear(t[0].timeBeforeTheKey, t[0].pos.x, t[1].timeBeforeTheKey,t[1].pos.x);
            AnimationCurve curvey = AnimationCurve.Linear(t[0].timeBeforeTheKey, t[0].pos.y, t[1].timeBeforeTheKey,t[1].pos.y);
            AnimationCurve curvez = AnimationCurve.Linear(t[0].timeBeforeTheKey, t[0].pos.z, t[1].timeBeforeTheKey,t[1].pos.z);
            var time = t[0].timeBeforeTheKey + t[1].timeBeforeTheKey;
            for(int i = 2; i<t.Length; i++){
                time+=t[i].timeBeforeTheKey;
                curvex.AddKey(time,t[i].pos.x);
                curvey.AddKey(time,t[i].pos.y);
                curvez.AddKey(time,t[i].pos.z);
            }
            clip.SetCurve(GetGameObjectPath(k), typeof(Transform), "localPosition.x", curvex);
            clip.SetCurve(GetGameObjectPath(k), typeof(Transform), "localPosition.y", curvey);
            clip.SetCurve(GetGameObjectPath(k), typeof(Transform), "localPosition.z", curvez);
        }
        return clip;
    }
}

public struct KindaKey{
    public Vector3 pos;
    public float timeBeforeTheKey;
}

public struct AnimCondition{
    public string path;
    public string propertyName;
    public Keyframe startKeyFrame;
    public Keyframe endKeyFrame;
}