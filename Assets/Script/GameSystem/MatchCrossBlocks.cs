using UnityEngine;
using System.Collections;

/// <summary>
// Match-Cross Block
/// </summary>
public class MatchCrossBlocks : Blocks {
	public GameObject _particle;

	//return create-type
	public override CREATETYPE CreateType(){
		return Blocks.CREATETYPE.MATCHCROSS;
	}

	//destroy cross line when destroy
	public override void destroyEvent(){
		if( _isDestroy )
			return ;
		base.destroyEvent();
		
		GameObject effect = null;
		
		effect = (GameObject)Instantiate(_particle);
		effect.transform.localPosition = this.transform.localPosition;
		effect.transform.Rotate(0,0,0);

		effect = (GameObject)Instantiate(_particle);
		effect.transform.localPosition = this.transform.localPosition;
		effect.transform.Rotate(0,0,90);

		for(int y=0;y<_system.maxY();y++){
			if( y != (int)_posInBoard.y ){
				_system.DestroyBlock((int)_posInBoard.x,y);
			}
		}
		for(int x=0;x<_system.maxX();x++){
			if( x != (int)_posInBoard.x ){
				_system.DestroyBlock(x,(int)_posInBoard.y);
			}
		}
	} 

	//init block
	public override void init(GameSystem sys){
		base.init(sys);
		_useType = USETYPE.CLICK;
	}

	//click event
	public override void Click(){
		_system.DestroyBlock((int)_posInBoard.x,(int)_posInBoard.y);
	}
}
