using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineConnector : MonoBehaviour
{
    public ARQTimeline.TimelineDirector timelineDirector;
    public Slider slider;

    public void OnEnable()
    {
        timelineDirector.Played += TimelineDirector_Played;
        timelineDirector.Updated += TimelineDirector_Updated;
    }

    private void TimelineDirector_Updated(ARQTimeline.TimelineDirector obj)
    {
        slider?.SetValueWithoutNotify(obj.ARQTimeline.Time);
    }

    private void TimelineDirector_Played(ARQTimeline.TimelineDirector obj)
    {
        slider.maxValue = obj.ARQTimeline.duration;
        slider.onValueChanged.AddListener((value) => obj.ARQTimeline.Rewind(value));
        obj.ARQTimeline.Rewind(slider.value);
    }

    public void TimelineDirectorPause()
    {
        timelineDirector.Pause();
    }
    public void TimelineDirectorResume()
    {
        timelineDirector.Resume();
    }
}
