using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class recording : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartSex()
    {
        //StartCoroutine(zapic());
    }

    IEnumerator zapic()
    {
        string folder = @"D:\";
        string fileName = "CSharpCornerAuthors2.txt";
        string fullPath = folder + fileName;
        
        string readText = File.ReadAllText(fullPath);
        string authors = "";
        for (float i = 0; i < 1.5f; i+=0.01f)
        {
            authors += transform.position.ToString("#.####") + "    " + i + '\n';
            yield return new WaitForSeconds(0.01f);
        }
            File.WriteAllText(fullPath, authors);
        Debug.Log("11111111111111111111111111111111111");
    }
    
}
