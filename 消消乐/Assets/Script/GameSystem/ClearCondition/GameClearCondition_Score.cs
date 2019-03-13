using UnityEngine;
using System.Collections;

/// <summary>
// GameClearCondition Score!
/// </summary>
public class GameClearCondition_Score : GameClearCondition {
	public int _targetScore = 0;
	//check is clear??
	public override bool isClear(){
		if(_system.nowScore() > _targetScore){
			return true;
		}else{
			return false;
		}
	}
	//return clear-type
	public override GameClearCondition.ClearType clearType(){
		return GameClearCondition.ClearType.SCORE;
	}
}
