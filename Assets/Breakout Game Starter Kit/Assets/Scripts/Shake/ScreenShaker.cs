using UnityEngine;

//This component needs to be on the main camera and there should be only one instance in the whole scene!
public class ScreenShaker : MonoBehaviour
{
    //Make the amount of bouncyness and drag configurable in the editor
    public float Drag = 6F;
    public float Elasticity = 200F;

    private Vector3 position = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        //Grab and save the current Camera position
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Every frame we calculate the bounce
        //When the _velocity has not been changed by the Shake(Vector3 power) method below,
        //nothing will happen as _velocity will remain zero. And we all know adding zero to something wont make a difference :)

        //Calculate the bounce. Note we do not change the y since we don't want the camera to bounce up and down
        velocity.x -= (position.x * Elasticity) + (velocity.x * Drag);
        velocity.z -= (position.z * Elasticity) + (velocity.z * Drag);

        //By multiplying with Time.deltaTime we ensure the motion is framerate independent
        //If we wouldn't do this the motion could be completely different on different computers
        position.x += velocity.x * Time.deltaTime;
        position.z += velocity.z * Time.deltaTime;

        //Apply the newly calculated position vector so we can actually see it happen in on our display
        transform.position = position;
    }

    //Set the _velocity used in the Update() method.
    //Note that because we add the power vector3 to the _velocity vector3,
    //calling Shake(Vector3 power) i.e. twice, it will bounce with twice as much power 
    public void Shake(Vector3 power)
    {
        velocity.x += power.x;
        velocity.z += power.z;
    }
}