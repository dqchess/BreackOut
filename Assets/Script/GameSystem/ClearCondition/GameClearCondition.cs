using UnityEngine;
using System.Collections;

/// <summary>
// GameClearCondition Interface
/// </summary>
public abstract class GameClearCondition : MonoBehaviour {
	//Game Clear Type
	public enum ClearType{
		SCORE,
		REMOVEBLOCKS
	}
	public GameSystem _system;
	public abstract bool isClear();
	public abstract ClearType clearType();
}
