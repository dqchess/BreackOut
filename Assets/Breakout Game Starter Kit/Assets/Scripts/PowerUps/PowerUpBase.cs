using UnityEngine;
using System.Collections;

//Make sure there is always an rigidbody,boxcollider and audiosource component on the GameObject where this script is added.
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(AudioSource))]
public class PowerUpBase : MonoBehaviour    //This is the base class for all powerup. When creating new powerups, derive from this
{
    public float DropSpeed = 5; //How fast does it drop?
    public AudioClip Sound; //Sound played when the powerup is picked up

    // Use this for initialization
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, -DropSpeed) * Time.deltaTime;
    }

    //Monobehaviour method, notice the IEnumerator which tells unity this is a coroutine
    IEnumerator OnTriggerEnter(Collider other)
    {
        //Only interact with the paddle
        if (other.name == "Paddle")
        {
            //Notify the derived powerups that its being picked up
            OnPickup();

            //Prevent furthur collisions
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;

            //Play audio and wait, without the wait the sound would be cutoff by the destroy
            GetComponent<AudioSource>().PlayOneShot(Sound);
            yield return new WaitForSeconds(Sound.length);

            Destroy(this.gameObject);
        }
    }

    //Every powerup which derives from this class should implement this method!
    //Protected means this method is private and only visible to derived classes
    protected virtual void OnPickup()
    {
     
    }
}