using UnityEngine;

public class RandomColor : MonoBehaviour
{
    //Start is called one time when the scene has been loaded
    void Start()
    {
        //Grab the material on this gameobject and set its main color to a random value.
        this.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
}