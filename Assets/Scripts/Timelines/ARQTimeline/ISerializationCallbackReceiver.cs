using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public interface ISerializationCallbackReceiver 
    {
        void OnAfterDeserialize();
        
        void OnBeforeSerialize();
    }
}