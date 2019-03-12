using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

    private GameObject targetGameObject;
    public Transform target;

    // Use this for initialization
    void Start () {
        targetGameObject = new GameObject();
        this.transform.LookAt(target.transform);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = target.transform.position;
        this.transform.position += target.up * 10;
	}
}
