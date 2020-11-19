using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Playables;

public class RecordTransformHierarchy : MonoBehaviour
{
    private AnimationClip clip;
    bool _isRecording = false;
    private GameObjectRecorder m_Recorder;

    public TimelineCreator timelineCreator;
    

    void LateUpdate()
    {
        if(!_isRecording) return;

        if (clip == null)
            return;

        m_Recorder.TakeSnapshot(Time.deltaTime);
    }

    public void StopRecording(){
        _isRecording = false;
        if (clip == null){
            return;
        }

        if (m_Recorder.isRecording)
        {
            m_Recorder.SaveToClip(clip);
        }
    }
    public void StartRecording(){
        clip = new AnimationClip();
        clip.name = "RecordedTest";
        m_Recorder = new GameObjectRecorder(gameObject);
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
        _isRecording = true;
    }

    public Animator animator;   
    public void DoSmt(){
        timelineCreator.CreateTimelineFromAnimClip(clip,this.GetComponent<Animator>(),gameObject);
    }
}