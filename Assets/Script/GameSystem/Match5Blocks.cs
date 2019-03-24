using UnityEngine;
using System.Collections;

/// <summary>
// Match-5 Block
/// </summary>
public class Match5Blocks : Blocks {
	public GameObject _effect = null;
	private int _destroyType = 0;

	//return create-type
	public override CREATETYPE CreateType(){
		return Blocks.CREATETYPE.MATCH5;
	}

	//init block
	public override void init(GameSystem sys){
		base.init(sys);
		_useType = USETYPE.CLICK;
	}

	//destroy all some type when destroy
	public override void destroyEvent(){
		if( _isDestroy )
			return ;
		base.destroyEvent();
		
		if(_effect == null)
			return;

		for(int x=0;x<_system.maxX();x++){
			for(int y=0;y<_system.maxY();y++){
				if(_system._boards[y,x] != null){
					if(_system._boards[y,x].type() == _destroyType ){
						GameObject effect = (GameObject)Instantiate(_effect);
						effect.SendMessage("SetGameSystem",_system);
						effect.transform.localPosition = this.transform.localPosition;
						effect.GetComponent<BlockDestroyerMissile>().Init(_system,_system._boards[y,x]);
					}
				}
			}
		}
	}

	//store some type when swaped
	public override void swapEvent(Blocks target){
		_destroyType = target.type();
		_system.DestroyBlock((int)_posInBoard.x,(int)_posInBoard.y);
	}

}
