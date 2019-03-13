using UnityEngine;
using System.Collections;

/// <summary>
// Cream Block
/// </summary>
public class CreamBlock : Blocks {
	//create type is none
	public override CREATETYPE CreateType(){
		return Blocks.CREATETYPE.NONE;
	}

	//override "near block destroyed event function"
	public override void nearBlockDestroyed(){
		_system.DestroyBlock((int)_posInBoard.x,(int)_posInBoard.y);
	}

	//block init
	public override void init(GameSystem sys){
		base.init(sys);
		_useType = USETYPE.NONE;
		_canMove = false;
	}
}
