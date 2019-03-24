using UnityEngine;
using System.Collections;

/// <summary>
// OverConditionUI
// inherite ConditionUI
/// </summary>
public class OverConditionUI : ConditionUI {
	//over condition
	GameOverCondition _overCondition;
	GameOverCondition.OverType type;

	TextMesh _text = null;
	bool fin = false;
	// Use this for initialization
	void Awake () {
		_overCondition = (GameOverCondition)FindObjectOfType(typeof(GameOverCondition));
		type = _overCondition.overType();
		string title = "";
		if(type == GameOverCondition.OverType.MOVE){
			title = "Move";
		}else if(type == GameOverCondition.OverType.TIME){
			title = "Time";
		}
		MakeText(title,new Vector2(0,0.2f),40,Color.black);
		_text=MakeText("0",new Vector2(0,-0.2f),30,Color.black);
	}


	// Update is called once per frame
	void Update () {
		if(fin)
			return ;
		if(_overCondition.isOver()){
			Finish();
			fin = true;
		}
		if(type == GameOverCondition.OverType.MOVE){
			GameOverCondition_Move move = (GameOverCondition_Move)_overCondition;
			_text.text = move._move.ToString();
		}else if(type == GameOverCondition.OverType.TIME){
			GameOverCondition_Time time = (GameOverCondition_Time)_overCondition;
			_text.text = ((int)time._time).ToString();
		}
	}
}
