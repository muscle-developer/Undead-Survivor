using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var text = "Test";
        Debug.Log(text);
    }

    // Update is called once per frame
    void Update()
    {
        // Pc에서 입력값
        if(Input.GetKeyDown(KeyCode.A))  
        {
            // 캐릭터 왼쪽으로 이동
        } 
        
    }
}
