using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
    void OnGUI()
    {

            if (GUI.Button(new Rect(Screen.width * 0.9f, Screen.height * 0.03f,70, 30), "Quit"))
            {
                Application.Quit();
                Debug.Log("退出游戏");            }
        }
    
        // Update is called once per frame
        void Update () {
		
	}
}
