using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ARQTimeline
{
    public class TimelineConnector : MonoBehaviour
    {
        public TimelineDirector timelineDirector;
        public Slider slider;

        public void OnEnable()
        {
            timelineDirector.Played += TimelineDirector_Played;
            timelineDirector.Updated += TimelineDirector_Updated;
        }

        private void TimelineDirector_Updated(TimelineDirector obj)
        {
            slider?.SetValueWithoutNotify(obj.ARQTimeline.Time);
        }

        private void TimelineDirector_Played(TimelineDirector obj)
        {
            slider.value = 0;
            slider.maxValue = obj.ARQTimeline.Duration;
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
}