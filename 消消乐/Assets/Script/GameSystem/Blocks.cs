using UnityEngine;
using System;
using System.Collections;

/// <summary>
// Block class 
/// </summary>
public class Blocks : MonoBehaviour {
	//create type
	public enum CREATETYPE{
		NORMAL,
		MATCH4,
		MATCH5,
		MATCHCROSS,
		NONE
	};
	//use type
	public enum USETYPE{
		MATCH3,
		CLICK,
		TURN, // not use currently
		NONE
	}
	public GameObject _destroyParticle = null;
	public SpriteRenderer _sprite;

	public int _type;
	public bool _canMove = true;
	protected USETYPE _useType;

	public Sprite _normal;
	public Sprite _pressed;

	public Vector2 _posInBoard = new Vector2(-1,-1);
	public GameSystem _system;

	protected Vector2 _moveTo = new Vector2(0,0);
	protected bool _isAnimation = false;
	protected float _speed = 0.3f;

	protected bool _isDestroy = false;

	//return create-type
	public virtual CREATETYPE CreateType(){
		return CREATETYPE.NORMAL;
	}

	//rturn use-type
	public USETYPE UseType(){
		return _useType;
	}

	// Use this for initialization
	void Awake(){
		if(_system != null){
			if(_posInBoard.x != -1 && _posInBoard.y != -1){
				_system.Init();
				_system._boards[(int)_posInBoard.y,(int)_posInBoard.x] = this;

				init(_system);
			}
		}
	}

	//init block
	public virtual void init(GameSystem sys){
		_useType = USETYPE.MATCH3;
		_system = sys;
		_sprite = gameObject.GetComponent<SpriteRenderer>();

		_sprite.sprite = _normal;
	}

	//call when block destroy
	public virtual void destroyEvent(){
		if( _isDestroy )
			return ;
		_isDestroy = true;
		if(_destroyParticle){
			GameObject ps = (GameObject)Instantiate(_destroyParticle);
			ps.SendMessage("SetGameSystem",_system);
			ps.transform.localPosition = this.transform.localPosition;
		}
	}

	//return block-type;
	//used checking match block
	public int type(){
		return _type;
	}

	//check is now transform-tweening
	public bool isAnimation(){
		return _isAnimation;
	}

	//go to x,y (moving)
	public void movePos(float x,float y){
		movePos((int)x,(int)y);
	}

	//go to x,y (moving)
	public void movePos(int x,int y){
		_moveTo = _system.blockPos(x,y);
		_posInBoard = new Vector2(x,y);
		_isAnimation = true;
	}
	
	// current position in stage
	public Vector2 pos(){
		return _posInBoard;
	}

	//mouse press
	public void mousePress(){
		_sprite.sprite = _pressed;
	}
	//mouse release
	public void mouseRelease(){
		_sprite.sprite = _normal;
	}

	// Update is called once per frame
	void Update(){
		if (_isAnimation == false){
			return;
		}

		float x = (this.transform.localPosition.x - _moveTo.x) * _speed;
		float y = (this.transform.localPosition.y - _moveTo.y) * _speed; 

		this.transform.localPosition = new Vector3 (this.transform.localPosition.x - x,
		                                            this.transform.localPosition.y - y);

		if ( Math.Abs(x) < 0.005 && Math.Abs(y) < 0.005)
			_isAnimation = false;
	}

	//call when this block swaped
	public virtual void swapEvent(Blocks target){}
	//call when destroy near block
	public virtual void nearBlockDestroyed(){}
	//call when this block press
	public virtual void Click(){}
}
