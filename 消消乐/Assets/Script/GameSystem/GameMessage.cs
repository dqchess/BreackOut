using UnityEngine;
using System;
using System.Collections;

/// <summary>
// GameMessage Class
// emitted GameSystem.
/// </summary>
public class GameMessage : GameSystemMessage {
	public override void MoveBlock(){}
	public override void UpdateScore(int score){}
	public override void RemoveBlock(int type){
		this.SendMessage("RemoveBlockType",type,SendMessageOptions.DontRequireReceiver);
	}
}
