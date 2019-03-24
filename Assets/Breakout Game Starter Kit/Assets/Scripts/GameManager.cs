using System.Collections;
using UnityEngine;

//List of all the posible gamestates
public enum GameState
{
    NotStarted,
    Playing,
    Completed,
    Failed
}

//Make sure there is always an AudioSource component on the GameObject where this script is added.
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    //Text element to display certain messages on
    public GUIText FeedbackText;

    //Text to be displayed when entering one of the gamestates
    public string GameNotStartedText;
    public string GameCompletedText;
    public string GameFailedText;

    //Sounds to be played when entering one of the gamestates
    public AudioClip StartSound;
    public AudioClip FailedSound;

    private GameState currentState = GameState.NotStarted;
    //All the blocks found in this level, to keep track of how many are left
    private Block[] allBlocks;
    private Ball[] allBalls;

    // Use this for initialization
    void Start()
    {
        //Find all the blocks in this scene
        allBlocks = FindObjectsOfType(typeof(Block)) as Block[];

        //Find all the balls in this scene
        allBalls = FindObjectsOfType(typeof(Ball)) as Ball[];

        //Prepare the start of the level
        SwitchTo(GameState.NotStarted);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.NotStarted:
                    //Check if the player taps/clicks.
                    if (Input.GetMouseButtonDown(0))    //Note: on mobile this will translate to the first touch/finger so perfectly multiplatform!
                    {
                        for (int i = 0; i < allBalls.Length; i++)
                            allBalls[i].Launch();

                        SwitchTo(GameState.Playing);
                    }
                break;
            case GameState.Playing:
                {
                    bool allBlocksDestroyed = true;

                    //Check if all blocks have been destroyed
                    for (int i = 0; i < allBlocks.Length; i++)
                    {
                        if (!allBlocks[i].BlockIsDestroyed)
                        {
                            allBlocksDestroyed = false;
                            break;
                        }
                    }

                    //Are there no balls left?
                    if (FindObjectOfType(typeof(Ball)) == null)
                        SwitchTo(GameState.Failed);

                    if (allBlocksDestroyed)
                        SwitchTo(GameState.Completed);
                }
                break;
            //Both cases do the same: restart the game
            case GameState.Failed:
            case GameState.Completed:
                //Check if the player taps/clicks.
                if (Input.GetMouseButtonDown(0))    //Note: on mobile this will translate to the first touch/finger so perfectly multiplatform!
                    Restart();
                break;
        }
    }

    //Do the appropriate actions when changing the gamestate
    public void SwitchTo(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            default:
            case GameState.NotStarted:
                DisplayText(GameNotStartedText);
                break;
            case GameState.Playing:
                GetComponent<AudioSource>().PlayOneShot(StartSound);
                DisplayText("");
                break;
            case GameState.Completed:
                GetComponent<AudioSource>().PlayOneShot(StartSound);
                DisplayText(GameCompletedText);
                StartCoroutine(RestartAfter(StartSound.length));
                break;
            case GameState.Failed:
                GetComponent<AudioSource>().PlayOneShot(FailedSound);
                DisplayText(GameFailedText);
                StartCoroutine(RestartAfter(FailedSound.length));
                break;
        }
    }

    //Helper to display some text
    private void DisplayText(string text)
    {
        FeedbackText.text = text;
    }

    //Coroutine which waits and then restarts the level
    //Note: You need to call this method with StartRoutine(RestartAfter(seconds)) else it won't restart
    private IEnumerator RestartAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Restart();
    }

    //Helper to restart the level
    private void Restart()
    {
        Application.LoadLevel(0);
    }
}