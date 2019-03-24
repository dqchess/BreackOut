using UnityEngine;
using System.Collections;

/// <summary>
// MATCH-4 Block
// remove one-line.
// if you make this block swaped vertically, this block will destroy vertical line.
// you can horinzontally swaped too.
/// </summary>
public class Match4Blocks : Blocks {
	public GameObject _particle;
	public GameObject _dirArrow;

	private bool _direct = false;

	//return create-type
	public override CREATETYPE CreateType(){
		return Blocks.CREATETYPE.MATCH4;
	}

	//destroy event
	//remove one-line.
	public override void destroyEvent(){
		if( _isDestroy )
			return ;
		base.destroyEvent();
		
		GameObject blck = (GameObject)Instantiate(_particle);
		blck.transform.localPosition = this.transform.localPosition;

		if(_direct){
			for(int y=0;y<_system.maxY();y++){
				if( y != (int)_posInBoard.y ){
					_system.DestroyBlock((int)_posInBoard.x,y);
				}
			}
		}else{
			for(int x=0;x<_system.maxX();x++){
				if( x != (int)_posInBoard.x ){
					_system.DestroyBlock(x,(int)_posInBoard.y);
				}
			}
			blck.transform.Rotate(0,0,90);
		}
	} 

	//init and check vertical or horizontal
	public override void init(GameSystem sys){
		base.init(sys);
		_useType = USETYPE.CLICK;

		Vector2 v = sys._lastMoveDir;
		if(Mathf.Abs(v.y) > 0){
			_direct = true;
			_dirArrow.transform.Rotate(0f,0f,90);
		}
	}

	//Click Event
	public override void Click(){
		_system.DestroyBlock((int)_posInBoard.x,(int)_posInBoard.y);
	}
}
