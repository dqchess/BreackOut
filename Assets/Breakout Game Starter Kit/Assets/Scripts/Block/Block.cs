using UnityEngine;
using System.Collections;

//Make sure there is always an Animation component on the GameObject where this script is added.
[RequireComponent(typeof(Animation))]
public class Block : MonoBehaviour
{
    //Should the block fall down or just disappear? Its configurable in the editor
    public bool FallDown = false;

    [HideInInspector]   //Do not let the user see this value but it does needs to be accesible from other scripts
    public bool BlockIsDestroyed = false;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (FallDown && velocity != Vector3.zero)
        {
            //Multiplying the velocity with Time.deltaTime before adding it to the current position makes sure
            //the motion is framerate independent
            transform.position += velocity * Time.deltaTime;
        }
    }

    //When the block is out of sight of the camera, we mark it as destroyed so the GameManager know our current score
    void OnBecameInvisible()
    {
        BlockIsDestroyed = true;
    }

    //By letting OnCollisionExit return IEnumerator we have the
    //ability to wait for a given time. In this example it waits for the "Woggle" animation to be finished
    //after that the block will fall down or just dissappear, depending on the valueof FallDown.
    private IEnumerator OnCollisionExit(Collision c)
    {
        GetComponent<Collider>().enabled = false;

        //Play the Woggle animation
        GetComponent<Animation>().Play();

        //Wait here for the length of the Woggle animation 
        yield return new WaitForSeconds(GetComponent<Animation>()["Woggle"].length);

        //Animation Woggle has finished, now decide what to do, falldown or just disappear
        if (FallDown)
        {
            //Falldown to the direction the ball hit it, with a random speed and plus a little downwards "gravity"
            velocity = (c.relativeVelocity * Random.Range(1, 2.0F)) + new Vector3(0, 0, -30);
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}