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
        public Action DirectorFinished = delegate { };
        
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
                _arqTimeline.IsStarted = false;
            } 
        }

        public void Play() {
            if (!_arqTimeline.IsStarted)
            {
                StartCoroutine(PlayCoroutine());
                
            }
            else
            {
                Resume();
            }
        }

        public void Pause(){
            _arqTimeline.IsPaused = true;
            _arqTimeline.Rewind(_arqTimeline.Time);
            Paused?.Invoke(this);
        }

        public void Resume(){
            _arqTimeline.IsPaused = false;
        }

        public void Stop(){
            StopAllCoroutines();
            if (_arqTimeline != null)
            {
                _arqTimeline.IsStarted = false;
                _arqTimeline.Rewind(_arqTimeline.Time);
            }
            Stopped?.Invoke(this);
        }

        private IEnumerator PlayCoroutine()
        {

            Played?.Invoke(this);

            _arqTimeline.IsStarted = true;
            while (_arqTimeline.Time< _arqTimeline.Duration || _arqTimeline.IsPaused)
            {
                if (!_arqTimeline.IsPaused)
                {
                    _arqTimeline.Time += Time.deltaTime;
                    Updated?.Invoke(this);
                }
                yield return _frameWait;
            }
            _arqTimeline.IsStarted = false;
            DirectorFinished?.Invoke();
            _arqTimeline.TimelineIsFinished();
        }
    }
}