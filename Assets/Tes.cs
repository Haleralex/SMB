using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tes : MonoBehaviour
{
    [SerializeField]
    Animation anim;
    public float k = 0.0f;
    public float k2 = 0.0f;
    public float testWait = 0.5f;
    public bool a1 = false;
    public bool a2 = false;

    private void Start()
    {
        //Invoke(nameof(test1), 2);
    }
    private void Update()
    {
        
        
    }

    public void test1()
    {
        if(a1) anim.Blend("StartToHalf",k);
        Invoke(nameof(test2), testWait);
    }
    public void test2()
    {
        
        if (a2) anim.Blend("StartToParts",k2);
    }
}
