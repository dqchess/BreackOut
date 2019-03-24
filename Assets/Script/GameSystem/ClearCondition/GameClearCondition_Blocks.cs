using UnityEngine;
using System.Collections;

/// <summary>
// GameClearCondition Remove Blocks
/// </summary>
public class GameClearCondition_Blocks : GameClearCondition {
	public GameObject[] _types;
	public int[] _count;

	public Blocks[] _blocks;
	//init, get blocks components
	public void Awake(){
		_blocks = new Blocks[_types.Length];
		for(int i=0;i<_types.Length;i++){
			_blocks[i] = _types[i].GetComponent<Blocks>();
		}
	}

	//check is clear
	public override bool isClear(){
		for(int i=0;i<_types.Length;i++){
			if(_count[i]>0)
				return false;
		}
		return true;
	}

	//return clear-type
	public override GameClearCondition.ClearType clearType(){
		return GameClearCondition.ClearType.REMOVEBLOCKS;
	}

	//Remove Some Block , check type!
	void RemoveBlockType(int type){
		for(int i=0;i<_types.Length;i++){
			if(_blocks[i].type() == type){
				_count[i]--;
				if(_count[i] < 0){
					_count[i] = 0;
				}
				return;
			}
		}
	}
}
