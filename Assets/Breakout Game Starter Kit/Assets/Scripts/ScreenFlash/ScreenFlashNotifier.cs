using UnityEngine;

public class ScreenFlashNotifier : MonoBehaviour
{
    //OnCollisionEnter will only be called when the other collider has a rigidbody, like our ball has
    private void OnCollisionEnter(Collision c)
    {
        //We know there is only one ScreenFlash script in the scene so its safe to assume the one we get is the one we want
        ScreenFlash flasher = FindObjectOfType(typeof(ScreenFlash)) as ScreenFlash;

        //Start the flash
        flasher.Flash();
    }
}