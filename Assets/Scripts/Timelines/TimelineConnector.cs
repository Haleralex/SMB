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
            slider.onValueChanged.AddListener((value) => timelineDirector.ARQTimeline.Rewind(value));
        }
        public void OnDisable()
        {
            timelineDirector.Played -= TimelineDirector_Played;
            timelineDirector.Updated -= TimelineDirector_Updated;
        }
        private void TimelineDirector_Updated(TimelineDirector obj)
        {
            slider?.SetValueWithoutNotify(obj.ARQTimeline.Time);
        }

        private void TimelineDirector_Played(TimelineDirector obj)
        {
            slider.value = 0;
            slider.maxValue = obj.ARQTimeline.Duration;
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
        public recording rec;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {

                
                StartCoroutine(Check());
                // rec.StartSex();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                StartCoroutine(CheckR());
            }
        }

        IEnumerator Check()
        {
            for(float i=0; i < 2f; i+=0.01f)
            {
                slider.value = i;
                yield return new WaitForSeconds(0.01f);
            }
        }
        IEnumerator CheckR()
        {
            for (float i = 2f; i >0.0f; i -= 0.01f)
            {
                slider.value = i;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}