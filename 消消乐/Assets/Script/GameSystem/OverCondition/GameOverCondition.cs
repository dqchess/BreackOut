using UnityEngine;
using System.Collections;

/// <summary>
// GameOverCondition
/// </summary>
public abstract class GameOverCondition : MonoBehaviour {
	//over type
	public enum OverType{
		MOVE,
		TIME
	};
	public GameSystem _system;
	public abstract bool isOver();
	public abstract void message(string type);
	public abstract OverType overType();
}
