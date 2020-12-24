using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace ARQTimeline
{
    public class TimelineDirector : MonoBehaviour
    {
        private static readonly WaitForEndOfFrame _frameWait = new WaitForEndOfFrame();
        public event Action<TimelineDirector> Stopped;
        public event Action<TimelineDirector> Played;
        public event Action<TimelineDirector> Paused;
        public event Action<TimelineDirector> Updated;
        public Action Finished = delegate { };
        private Timeline _arqTimeline;
        public Timeline ARQTimeline { 
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
                
            }
            else
            {
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
            if (_arqTimeline != null)
            {
                _arqTimeline._isStarted = false;
                _arqTimeline.Rewind(_arqTimeline.Time);
            }
            Stopped?.Invoke(this);
        }

        private IEnumerator PlayCoroutine()
        {

            Played?.Invoke(this);

            //slider.value = 0;
            
            _arqTimeline._isStarted = true;
            while (_arqTimeline.Time< _arqTimeline.duration || _arqTimeline._isPaused)
            {
                if (!_arqTimeline._isPaused)
                {
                    _arqTimeline.Time += Time.deltaTime;
                    Updated?.Invoke(this);
                }
                yield return _frameWait;
            }
            _arqTimeline._isStarted = false;
            Finished?.Invoke();
            _arqTimeline.FinishTimeline();
        }
    }
}