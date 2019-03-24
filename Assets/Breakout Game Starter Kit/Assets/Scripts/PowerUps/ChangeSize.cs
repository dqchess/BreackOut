using UnityEngine;

//Notice that we inherit from PowerUpBase, it contains the general behaviour of a powerup
//Here we just implement the specific power up logic
public class ChangeSize : PowerUpBase
{
    //How much units should the paddle change when this powerup is picked up?
    //Can also be negative to shrink the paddle!
    public Vector3 SizeIncrease = Vector3.zero;

    //Notice how we override we the OnPickup method of the base class  
    protected override void OnPickup()
    {
        //Call the default behaviour of the base class frist
        base.OnPickup();

        //Then do the powerup specific behaviour, changing the size in this case
        Paddle p = FindObjectOfType(typeof(Paddle)) as Paddle;
        p.transform.localScale += SizeIncrease;
    }
}