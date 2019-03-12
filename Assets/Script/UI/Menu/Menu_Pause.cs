using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Pause : MonoBehaviour {

    bool pause = false;

    void Start() {
    }

    void OnGUI()
    {
        if (pause == false)
        {
            if (GUI.Button(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, 70,30), "暂停"))
            {
                print("用户单击了按钮");
                Time.timeScale = 0;
                
                pause = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width * 0.02f, Screen.height * 0.02f, 70, 30), "继续"))
            {
                print("用户单击了按钮");
                Time.timeScale = 1;
                pause = false;
            }
        }
    }

}
