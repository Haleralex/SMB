using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System.Linq;
using UnityEngine.Playables;
using System;
using UnityEditor;
using System.Reflection;
using UnityEditor.SceneManagement;

public class TimelineHelper
{
    public static AnimationClip[] GetAllClipsFromTimeline(TimelineAsset timelineAsset)
    {
        List<AnimationClip> lst = new List<AnimationClip>();

        for (int i = 0; i < timelineAsset.outputTrackCount; i++)
        {
            TrackAsset track = timelineAsset.GetRootTrack(i);

            AnimationTrack animTrack = track as AnimationTrack;

            if (animTrack)
            {
                animTrack.GetClips().ToList<TimelineClip>().Select(a => a.animationClip).ToList<AnimationClip>().ForEach(e => lst.Add(e));
            }
        }
        return lst.ToArray<AnimationClip>();
    }
    public static TimelineAsset CreateCopyTimeline(TimelineAsset original, PlayableDirector directorInstance)
    {
        var clone = UnityEngine.Object.Instantiate(original);
        Undo.RegisterCreatedObjectUndo(clone, "Create clip");
        if (clone == null || (clone as IPlayableAsset) == null)
        {
            throw new InvalidCastException("could not cast instantiated object into IPlayableAsset");
        }

        if (directorInstance != null)
        {
            var originalObject = new SerializedObject(original);
            var cloneObject = new SerializedObject(clone);
            SerializedProperty prop = originalObject.GetIterator();
            if (prop.Next(true))
            {
                do { cloneObject.CopyFromSerializedProperty(prop); } while (prop.Next(false));
            }
            cloneObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(directorInstance);

            var exposedRefs = clone.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(ExposedReference<>)).ToList();
            foreach (FieldInfo fi in exposedRefs)
            {
                var exposedRefInstance = fi.GetValue(clone);
                var exposedNameProperty = exposedRefInstance.GetType().GetField("exposedName");
                if (exposedNameProperty != null)
                {
                    var exposedNameValue = (PropertyName)exposedNameProperty.GetValue(exposedRefInstance);

                    bool isValid = false;
                    var originalReference = directorInstance.GetReferenceValue(exposedNameValue, out isValid);

                    if (isValid)
                    {
                        var newPropertyName = new PropertyName(GUID.Generate().ToString());
                        directorInstance.SetReferenceValue(newPropertyName, originalReference);
                        exposedNameProperty.SetValue(exposedRefInstance, newPropertyName);

                        if (!EditorApplication.isPlaying)
                            EditorSceneManager.MarkSceneDirty(directorInstance.gameObject.scene);
                    }
                }
                fi.SetValue(clone, exposedRefInstance);
            }
        }
        return clone as TimelineAsset;
    }
}
