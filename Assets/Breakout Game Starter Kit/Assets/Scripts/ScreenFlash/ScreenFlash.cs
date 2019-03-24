using UnityEngine;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    //Make the flash duration configurable in the editor.
    public float FlashDuration = 0.05F;
    //And the flash color to
    public Color FlashColor = Color.white;

    private Color originalColor;

    // Use this for initialization
    void Start()
    {
        //Save the original color so we can always go back
        originalColor = Camera.main.backgroundColor;
    }

    public void Flash()
    {
        //First we stop any running coroutine (we might hit two blocks in a very short time)
        StopAllCoroutines();

        //Start the flashing trick
        StartCoroutine(DoFlash());   
    }

    //Note: Methods which IEnumerators are coroutines and need to be called with StartCoroutine()
    //Coroutines have the advantage of being able to wait in the middle of the method.
    private IEnumerator DoFlash()
    {
        //Set camera clear screen to the flash color...
        Camera.main.backgroundColor = FlashColor;
        //...wait a moment...
        yield return new WaitForSeconds(FlashDuration);
        //...and set it back
        Camera.main.backgroundColor = originalColor;

        //Wait a bit
        yield return new WaitForSeconds(FlashDuration);

        //Do exactly the same again        
        Camera.main.backgroundColor = FlashColor;
        yield return new WaitForSeconds(FlashDuration);
        Camera.main.backgroundColor = originalColor;
    }
}