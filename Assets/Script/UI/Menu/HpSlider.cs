using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour {
    private Slider hpslider;
    private RectTransform recttrans;

    public float cuvalue;//当前血量
    public float maxvalue;

    // Use this for initialization
    void Start()
    {
        hpslider = this.GetComponent<Slider>();
        recttrans = GetComponent<RectTransform>();
        maxvalue = PlayerData.HP;
        Init();
    }

    void Init()
    {
        hpslider.maxValue = maxvalue;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Camera.main.transform.rotation;
        cuvalue = PlayerData.HP;
        hpslider.value = cuvalue;
    }
}
