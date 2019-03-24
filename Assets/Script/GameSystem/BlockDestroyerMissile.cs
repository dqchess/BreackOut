using UnityEngine;
using System;
using System.Collections;

/// <summary>
// Block Destroyer!
// 5match-block use this missile
/// </summary>
public class BlockDestroyerMissile : MonoBehaviour {
	public GameSystem _system;
	private Blocks _target;

	//init missile
	public void Init(GameSystem sys,Blocks block){
		_system = sys;
		_target = block;
	}
	
	//update. 
	//going to some block.
	//destroy block
	void Update () {
		Vector3 moveTo = new Vector3();
		try{
			int _x = (int)_target._posInBoard.x;
			int _y = (int)_target._posInBoard.y;
			if(_system._boards[_y,_x] != _target){
				Destroy(this.gameObject);
				return ;
			}
			moveTo = _target.transform.localPosition;
		}catch (Exception e){
			Destroy(this.gameObject);
			return ;
		}


		float x = (this.transform.localPosition.x - moveTo.x) * 0.1f;
		float y = (this.transform.localPosition.y - moveTo.y) * 0.1f; 

		this.transform.localPosition = new Vector3 (this.transform.localPosition.x - x,
		                                            this.transform.localPosition.y - y);

		if ( Math.Abs(x) < 0.005 && Math.Abs(y) < 0.005){
			_system.DestroyBlock(_target);
			Destroy(this.gameObject);
		}

	}
}
