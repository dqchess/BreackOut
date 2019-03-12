using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Introduction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

    }
    void OnClick()
    {
        SceneManager.LoadScene("Introduction");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
