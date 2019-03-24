using UnityEngine;

//Notice that we inherit from PowerUpBase, it contains the general behaviour of a powerup
//Here we just implement the specific power up logic
public class ExtraBall : PowerUpBase
{
    //Link to the BallPrefab which gets instantiated when the powerup is picked up
    public GameObject BallPrefab;

    //Notice how we override we the OnPickup method of the base class  
    protected override void OnPickup()
    {
        //Call the default behaviour of the base class frist
        base.OnPickup();

        //Create a new ball and launch it
        GameObject ball = Instantiate(BallPrefab, transform.position, Quaternion.identity) as GameObject;
        ball.GetComponent<Ball>().Launch();
    }
}