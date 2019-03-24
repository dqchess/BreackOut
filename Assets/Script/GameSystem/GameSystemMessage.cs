using UnityEngine;
using System.Collections;


/// <summary>
// GameMessage interface.
/// </summary>
public abstract class GameSystemMessage : MonoBehaviour {
	public abstract void MoveBlock();
	public abstract void UpdateScore(int score);
	public abstract void RemoveBlock(int type);
}
