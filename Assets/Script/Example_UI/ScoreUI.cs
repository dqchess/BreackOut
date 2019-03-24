using UnityEngine;
using System.Collections;

/// <summary>
// ScoreUI
/// </summary>
public class ScoreUI : GameSystemMessage {
	GUIText _text = null;
	// Awake GameObject
	void Awake () {
		_text = gameObject.GetComponent<GUIText>();
	}
	
	public override void MoveBlock(){}
	public override void RemoveBlock(int type){}
	//update score UI
	public override void UpdateScore(int score){
		_text.text = score.ToString();
	}
}
