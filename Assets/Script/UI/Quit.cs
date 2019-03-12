using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour {

	// Use this for initialization

    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
