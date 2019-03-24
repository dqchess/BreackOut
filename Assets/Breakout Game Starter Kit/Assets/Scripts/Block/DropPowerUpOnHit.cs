using UnityEngine;

//Make sure there is always a BoxCollider component on the GameObject where this script is added.
[RequireComponent(typeof(BoxCollider))]
public class DropPowerUpOnHit : MonoBehaviour
{
    //Every powerup needs to be derived/inherit from PowerUpBase to ensure consistent behaviour
    public PowerUpBase PowerUpPrefab;

    //OnCollision create the powerup
    void OnCollisionEnter(Collision c)
    {
        GameObject.Instantiate(PowerUpPrefab, this.transform.position, Quaternion.identity);
    }
}