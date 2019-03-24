using UnityEngine;

//This component will notify the ScreenShaker to shake with a given power.
//This component should be on a gameobject which has a collider (i.e. boxcollider, spherecollider etc)
//In our case its attached to the level bounderies and the blocks.
public class ScreenShakerNotifier : MonoBehaviour
{
    //The reference to the ScreenShaker component (which should be located on only one camera)
    private ScreenShaker shaker;

    //Start is called one time when the scene has been loaded
    void Start()
    {
        //We know there is only one ShakeScreen script in the scene, so we find and save the reference to it so we can use it when there is a collision.
         shaker = FindObjectOfType(typeof(ScreenShaker)) as ScreenShaker;
    }

    //OnCollisionEnter will only be called when the other collider has a rigidbody, like our ball has
    void OnCollisionEnter(Collision c)
    {
        //Notify the shaker that there is a collision and pass the direction of the collision to it.
        //So if the ball bounces onto the left of the screen, the shaker will shake from the right to left and back
        shaker.Shake(c.relativeVelocity);
    }
}
