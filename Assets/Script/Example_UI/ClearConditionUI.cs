using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
// ClearCondition UI
// inherite ConditionUI
/// </summary>
public class ClearConditionUI : ConditionUI {
	//conditions
	GameClearCondition _clearCondition;
	GameClearCondition.ClearType type;
	
	//3 star
	public GameObject[] star;
	List<TextMesh> _text = new List<TextMesh>();

	bool fin = false;
	// Use this for initialization
	void Awake() {
		_clearCondition = (GameClearCondition)FindObjectOfType(typeof(GameClearCondition));
		type = _clearCondition.clearType();
		if(type == GameClearCondition.ClearType.SCORE){
			MakeText("Score",new Vector2(0,0.2f),40,Color.black);
			_text.Add(MakeText("",new Vector2(0,-0.2f),30,Color.black));
		}else if(type == GameClearCondition.ClearType.REMOVEBLOCKS){
			GameClearCondition_Blocks blocks = (GameClearCondition_Blocks)_clearCondition;
			for(int i=0;i<blocks._types.Length;i++){
				GameObject b = blocks._types[i];
				Sprite spr = b.GetComponent<SpriteRenderer>().sprite;
				int count = blocks._count[i];

				_text.Add(MakeText("30",new Vector2(-0.17f+0.53f*(i%2),0.2f-0.4f*(i/2)),30,Color.black));
				MakeSprite(spr,new Vector2(-0.4f+0.53f*(i%2),0.2f-0.4f*(i/2)));
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(fin)
			return ;
		if(_clearCondition.isClear()){
			Finish();
			fin = true;
		}
		if(type == GameClearCondition.ClearType.SCORE){
			GameClearCondition_Score clear = (GameClearCondition_Score)_clearCondition;
			int now = clear._system.nowScore();
			int score = clear._targetScore;
			for(int i=0;i<((float)now/(float)score)*3-1;i++){
				star[i].transform.localScale = new Vector2(0.82f,0.9f);
			}
			_text[0].text = now+"/"+score;
		}else if(type == GameClearCondition.ClearType.REMOVEBLOCKS){
			GameClearCondition_Blocks blocks = (GameClearCondition_Blocks)_clearCondition;
			for(int i =0;i<_text.Count;i++){
				_text[i].text = blocks._count[i].ToString();
			}
		}
	}
}
