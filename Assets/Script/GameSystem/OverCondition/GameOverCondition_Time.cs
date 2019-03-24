using UnityEngine;
using System.Collections;

/// <summary>
// GameOverCondition Time
/// </summary>
public class GameOverCondition_Time : GameOverCondition {
	public float _time = 0; 

	//check game over
	public override bool isOver(){
		return _time <= 0;
	}

	public override void message(string type){}

	//return over-type
	public override GameOverCondition.OverType overType(){
		return GameOverCondition.OverType.TIME;
	}

	//decrease time every frame
	void Update(){
		_time -= Time.deltaTime;
	}
}
