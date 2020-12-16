using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Test
{
    public class ARQTimelineDirector : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        private static readonly WaitForEndOfFrame _frameWait = new WaitForEndOfFrame();
        public event Action<ARQTimelineDirector> Stopped;
        public event Action<ARQTimelineDirector> Played;
        public event Action<ARQTimelineDirector> Paused;
        public Action Finished = delegate { };
        private ARQTimeline _arqTimeline;
        public ARQTimeline ARQTimeline { 
            get 
            {
                return _arqTimeline;
            } 
            set 
            {
                StopAllCoroutines();
                _arqTimeline = value;
                _arqTimeline._isStarted = false;
            } 
        }

        public void Play() {
            if (!_arqTimeline._isStarted)
            {
                StartCoroutine(PlayCoroutine());
                Played?.Invoke(this);
            }
            else
            {
                Debug.Log("HHHHHHHHHHHH");
                Resume();
            }
        }

        public void Pause(){
            _arqTimeline._isPaused = true;
            _arqTimeline.Rewind(_arqTimeline.Time);
            Paused?.Invoke(this);
        }

        public void Resume(){
            _arqTimeline._isPaused = false;
        }

        public void Stop(){
            StopAllCoroutines();
            if (_arqTimeline)
            {
                _arqTimeline._isStarted = false;
                _arqTimeline.Rewind(_arqTimeline.Time);
            }
            Stopped?.Invoke(this);
        }

        private IEnumerator PlayCoroutine()
        {
            slider.value = 0;
            slider.maxValue = _arqTimeline.duration;
            slider.onValueChanged.AddListener((value) => _arqTimeline.Rewind(value));
            _arqTimeline._isStarted = true;
            _arqTimeline.Rewind(slider.value);
            while (_arqTimeline.Time< _arqTimeline.duration || _arqTimeline._isPaused)
            {
                if (!_arqTimeline._isPaused)
                {
                    _arqTimeline.Time += Time.deltaTime;
                    
                    slider?.SetValueWithoutNotify(_arqTimeline.Time);
                }
                yield return _frameWait;
            }
            _arqTimeline._isStarted = false;
            Finished?.Invoke();
        }
    }
}