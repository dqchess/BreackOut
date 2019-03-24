using UnityEngine;
using System.Collections;

/// <summary>
// GameOverCondition Move
/// </summary>
public class GameOverCondition_Move : GameOverCondition {
	public int _move;
	
	//check over condition
	public override bool isOver(){
		return _move < 0;
	}

	//recv message
	public override void message(string type){
		if(type == "move"){
			_move--;
		}
	}
	//return over-type
	public override GameOverCondition.OverType overType(){
		return GameOverCondition.OverType.MOVE;
	}
}
