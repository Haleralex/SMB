using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARQTimeline
{
    public interface ISerializationCallbackReceiver 
    {
        void OnAfterDeserialize();
        
        void OnBeforeSerialize();
    }
}