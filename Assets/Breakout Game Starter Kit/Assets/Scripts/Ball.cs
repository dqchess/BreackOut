using UnityEngine;

public class Ball : MonoBehaviour
{
    //Make the min and max speed to be configurable in the editor.
    public float MinimumSpeed = 25;
    public float MaximumSpeed = 30;

    //To prevent the ball from keep bouncing horizontally we enforce a minimum vertical movement
    public float MinimumVerticalMovement = 0.1F;

    //Don't move the ball unless you tell it to
    private bool hasBeenLaunched = false;

    //Start is called one time when the scene has been loaded
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenLaunched)
        {
            //Get current speed and direction
            Vector3 direction = GetComponent<Rigidbody>().velocity;
            float speed = direction.magnitude;
            direction.Normalize();

            //Make sure the ball never goes straight horizotal else it could never come down to the paddle.
            if (direction.z > -MinimumVerticalMovement && direction.z < MinimumVerticalMovement)
            {
                //Adjust the y, make sure it keeps going into the direction it was going (up or down)
                direction.z = direction.z < 0 ? -MinimumVerticalMovement : MinimumVerticalMovement;

                //Adjust the x also as x + y = 1
                direction.x = direction.x < 0 ? -1 + MinimumVerticalMovement : 1 - MinimumVerticalMovement;
                
                //Apply it back to the ball
                GetComponent<Rigidbody>().velocity = direction * speed;   
            }

            if (speed < MinimumSpeed || speed > MaximumSpeed)
            {
                //Limit the speed so it always above min en below max
                speed = Mathf.Clamp(speed, MinimumSpeed, MaximumSpeed);

                //Apply the limit
                //Note that we don't use * Time.deltaTime here since we set the velocity once, not every frame.
                GetComponent<Rigidbody>().velocity = direction * speed;   
            }
        }
    }

    //When the bottom of the field it hit destroy the ball. 
    //Note: that the bottom collider is marked as a Trigger, else it would bounce back, now it goes just through the collider.
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Bottom")
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch()
    {
        //Create a random vector but make sure it always point "up" (z axis in this case) else it could be launched straight down
        Vector3 randomDirection = new Vector3(Random.Range(-1.0F, 1.0F), 0, Mathf.Abs(Random.value));

        //Make sure we start at the minimum speed limit
        randomDirection = randomDirection.normalized * MinimumSpeed;

        //Apply it to the rigidbody so it keeps moving into that direction (untill it hits a block or wall ofcourse)
        GetComponent<Rigidbody>().velocity = randomDirection;

        hasBeenLaunched = true;
    }
}
