using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Continue : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	private void OnClick() {
        SceneManager.LoadScene("Demo");
        Time.timeScale = 1;
    }
}
