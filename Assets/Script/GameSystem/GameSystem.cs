using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
// Game Main System.
/// </summary>
public class GameSystem : MonoBehaviour {
	//configures
	public GameObject[] _blocks;
	public GameObject[] _match4;
	public GameObject[] _match5;
	public GameObject[] _matchCross;

	public Vector2 _blockSize;

	public bool _isPassBlank = false;
	public bool _updatePause = false;
	public int _maxX=0;
	public int _maxY=0;
	public float _gapX=0;
	public float _gapY=0;
	public int[] _boardshape;
	public bool[] _blockEntry;
	public Sprite _background;

	public GameSystemMessage[] _eventObjects;

	public GameClearCondition _clearCondition;
	public GameOverCondition _overCondition;
	//member variables
	public Blocks[,] _boards;
	private Blocks _touchBlock = null;
	private Blocks _swapTest1 = null;
	private Blocks _swapTest2 = null;
	
	public Vector2 _lastMoveDir = new Vector2();
	public Blocks  _lastMovedBlock1 = null;
	public Blocks  _lastMovedBlock2 = null;
	
	private bool _touched = false;
	private bool _updateHint = false;
	public int _score = 0;

	private bool _isInit = false;

	public int nowScore(){
		return _score;
	}

	//Event Throw!
	void EventThrow_UpdateScore(){
		foreach(GameSystemMessage v in _eventObjects){
			v.UpdateScore(_score);
		}
	}

	void EventThrow_RemoveBlockType(int type){
		foreach(GameSystemMessage v in _eventObjects){
			v.RemoveBlock(type);
		}
	}

	//return stage max-X
	public int maxX(){
		return _maxX;
	}

	//return stage max-Y
	public int maxY(){
		return _maxY;
	}

	//get Blocks real-position
	public Vector2 blockPos(int x,int y){
		float bptux = 100f/(_background.rect.width/_background.bounds.size.x);
		float bptuy = 100f/(_background.rect.height/_background.bounds.size.y);
		float ptu = _background.rect.width / _background.bounds.size.x;
		float bx = (_blockSize.x / ptu)/2f;
		float by = (_blockSize.y / ptu)/2f;
		float gx = _gapX / ptu;
		float gy = _gapY / ptu;
		float mx = bx * x * 2 + gx*x;
		float my = by * y * 2 + gy*y;
		float posX = mx - _background.bounds.size.x/2*bptux + gx+bx;
		float posY = my - _background.bounds.size.y/2*bptuy + gy+by;
		return new Vector2(posX,-posY);
	}

	//Get block board-positin 
	public Vector2 blockIdx(Blocks b){
		for(int x=0;x<_maxX;x++){
			for(int y=0;y<_maxY;y++){
				if(_boards[y,x] == b){
					return new Vector2(x,y);
				}
			}
		}
		return new Vector2(-1,-1);
	}

	//create new block
	public Blocks createBlock(GameObject obj,int x,int y){
		GameObject blck = (GameObject)Instantiate(obj);
		Blocks blocks = blck.GetComponent<Blocks>();
		blocks.init(this);
		blocks.movePos(x,y);

		blck.transform.parent = this.transform;
		blck.transform.localPosition = blockPos(x,y);
		
		Sprite bs = blck.GetComponent<SpriteRenderer>().sprite;
		float ptu = bs.rect.width / bs.bounds.size.x;

		float sx = _blockSize.x/bs.rect.width;///_ptu;
		float sy = _blockSize.y/bs.rect.width;///_ptu;
		blck.transform.localScale = new Vector3(sx,sy,1);	

		return blck.GetComponent<Blocks>();
	}
	//create block randomly in list
	public Blocks createBlock(GameObject[] list,int x,int y){
		int random = UnityEngine.Random.Range(0,list.Length);
		return createBlock(list[random],x,y);
	}

	//get boardshape
	int boardshape(int y,int x){
		return _boardshape[x+y*_maxX];
	}

	//init system!
	public void Init(){
		if(_isInit)
			return ;
		_boards = new Blocks[_maxY,_maxX];
		for(int y =0;y<_maxY;y++){
			for(int x =0;x<_maxX;x++){
				_boards[y,x] = null;
			}
		}
		_isInit = true;
	}

	// awake this gameObject
	void Awake() {
		_eventObjects = FindObjectsOfType(typeof(GameSystemMessage)) as GameSystemMessage[];
		Init();
	}

	//swap test two block.
	bool swapBlock(Vector2 p1,Vector2 p2){
		Blocks b1 = _boards[(int)p1.y,(int)p1.x];
		Blocks b2 = _boards[(int)p2.y,(int)p2.x];
		if(b1!=null && b1._canMove == false)
			return false;
		if(b2!=null && b2._canMove == false)
			return false;
		if(b1!=null)
			b1.movePos(p2.x,p2.y);
		if(b2!=null)
			b2.movePos(p1.x,p1.y);
		
		_boards[(int)p2.y,(int)p2.x] = b1;
		_boards[(int)p1.y,(int)p1.x] = b2;
		_updateHint = true;
		return true;
	}

	//swap test two block.
	bool swapBlock(Blocks blck1,Blocks blck2){
		return swapBlock(blck1.pos(),blck2.pos());
	}

	//get block with blocks position.
	Blocks indexBlock(Vector2 pos){
		for(int x=0;x<_maxX;x++){
			for(int y=0;y<_maxY;y++){
				if(_boards[y,x] != null){
					SpriteRenderer spr = _boards[y,x]._sprite;
					Vector2 size = new Vector2(spr.sprite.bounds.size.x * 0.5f,
											   spr.sprite.bounds.size.y * 0.5f);
					float _x = Mathf.Abs(spr.transform.localPosition.x - pos.x);
					float _y = Mathf.Abs(spr.transform.localPosition.y - pos.y);

					if( _x > 0 && _y > 0 && _x < size.x && _y < size.y ){
						return _boards[y,x];
					}
				}
			}
		}
		return null;
	}

	//check Blocks animation.
	bool isBlocksMoveToAnim(){
		for(int x=0;x<_maxX;x++){
			for(int y=0;y<_maxY;y++){
				if(_boards[y,x] != null && _boards[y,x].isAnimation()){
					return true;
				}
			}
		}
		return false;
	}

	//find moveable to down block 
	int findMovableDown(int x,int starty){
		for(int y=starty;y<_maxY;y++){
			if(boardshape(y,x) != 0){
				if(_boards[y,x]){
					return -1;
				}else{
					return y;
				}
			}else if(_isPassBlank == false && boardshape(y,x) == 0){
				return -1;
			}
		}
		return -1;
	}

	//check vertical line is empty 
	bool isEmptyUpSide(int x,int starty){
		for(int y=starty;y>=0;y--){
			if(boardshape(y,x) != 0){
				if(_boards[y,x]){
					if(_boards[y,x]._canMove == false){
						return true;
					}
					return false;
				}
			}else if(_isPassBlank == false && boardshape(y,x) == 0){
				return true;
			}
		}
		return true;
	}

	//destroy all blocks same type
	public void DestroyBlock(int type){
		for(int x=0;x<_maxX;x++){
			for(int y=0;y<_maxY;y++){
				if(_boards[y,x] != null){
					if(_boards[y,x].type() == type){
						DestroyBlock(x,y);
					}
				}
			}
		}
	}

	//destroy block
	public void DestroyBlock(Blocks b){
		DestroyBlock((int)b.pos().x,(int)b.pos().y);
	}

	//destroy block and call destroy event
	public void DestroyBlock(int x,int y){
		Blocks o = _boards[y,x];
		if(o != null){
			_score += 10;
			EventThrow_RemoveBlockType(o.type());

			o.destroyEvent();
			Destroy(o.gameObject);
		}
		_boards[y,x] = null;
	}

	// find block in list<list<block>>
	List<Blocks> findListInBlockList(Blocks blck,List<List<Blocks>> blist,List<Blocks> except=null){
		foreach(List<Blocks> lists in blist){
			if(except == lists){
				continue;
			}
			bool find = false;
			foreach(Blocks b in lists)
				if(b == blck){
					find = true;
					break;
				}

			if(find){
				return lists;
			}
		}
		return null;
	}
	// find list<block> in list<list<block>>
	List<List<Blocks>> findListInBlockListAll(Blocks blck,List<List<Blocks>> blist){
		List<List<Blocks>> ret = new List<List<Blocks>>();
		foreach(List<Blocks> lists in blist){
			foreach(Blocks b in lists){
				if(b == blck){
					ret.Add(lists);
					break;
				}
			}
		}
		return ret;
	}

	//update entry block
	//if emtpy entry, create new block.
	void UpdateEntryBlock(){
		for(int x=0;x<_maxX;x++){
			if(_blockEntry[x]){
				for(int y=0;y<_maxY;y++){
					if(boardshape(y,x) != 0){
						if(_boards[y,x] == null){
							_boards[y,x] = createBlock(_blocks,x,y);
							if(_boards[y,x] != null){
								_updateHint = true;
							}
						}
						break;
					}
				}
			}
		}
	}

	//check can move down
	int checkMoveDownLeft(int x,int y){
		if(y >= 0 && _boards[y,x]){
			if(_boards[y,x]._canMove == false)
				return -1;
			if(x>0 && isEmptyUpSide(x-1,y) && _boards[y+1,x-1]==null){
				return findMovableDown(x-1,y+1);
			}
		}
		return -1;
	}

	//check can move down
	int checkMoveDownRight(int x,int y){
		if(y >= 0 && _boards[y,x]){
			if(_boards[y,x]._canMove == false)
				return -1;
			if(x<_maxX-1 && isEmptyUpSide(x+1,y) && _boards[y+1,x+1]==null){
				return findMovableDown(x+1,y+1);
			}
		}
		return -1;
	}

	//swap test 
	void testSwap(Blocks[,] boards,int x,int y,int x2,int y2){
		if(boards[y,x] == null || boards[y2,x2] == null)
			return ;
		Blocks b = boards[y,x];
		boards[y,x]=boards[y2,x2];
		boards[y2,x2] = b;
	}

	//remove Distinct value in list
	public void GetDistinctValues<T>(List<T> list){
		T[] array = list.ToArray();
		list.Clear();
		for (int i = 0; i < array.Length; i++){
			if (list.Contains(array[i]))
				continue;
			list.Add(array[i]);
		}
	}

	//shake board
	//if no more matching block, call this function
	public void ShakeBoard(){
		int _x = 0;
		int _y = 0;
		for(int x=0;x<_maxX;x++){
			for(int y=0;y<_maxY;y++){
				if(_boards[_y,_x] != null && _boards[y,x] != null){
					if(UnityEngine.Random.Range(0,2) == 0){
						swapBlock(_boards[_y,_x],_boards[y,x]);
					}
				}
				_x = x;
				_y = y;
			}
		}
	}

	// call when block destroy, and notice near blocks 
	void noticeDestroyToNearBlock(Blocks b){
		int x = (int)b._posInBoard.x;
		int y = (int)b._posInBoard.y;
		if(y>0&&_boards[y-1,x]) _boards[y-1,x].nearBlockDestroyed();
		if(y<_maxY-1&&_boards[y+1,x]) _boards[y+1,x].nearBlockDestroyed();
		if(x>0&&_boards[y,x-1]) _boards[y,x-1].nearBlockDestroyed();
		if(x<_maxX-1&&_boards[y,x+1]) _boards[y,x+1].nearBlockDestroyed();
	}

	//get zero-index list
	List<Blocks> extractMatch(List<List<Blocks>>[] arr){
		for(int i=0;i<arr.Length;i++){
			if(arr[i].Count>0){
				return arr[i][0];
			}
		}
		return null;
	}

	//check match3 or more or cross 
	List<List<Blocks>>[] match3Check(Blocks[,] board=null){
		if(board == null){
			board = _boards;
		}

		List<List<Blocks>> destroyNormalBlocks = new List<List<Blocks>>();
		List<List<Blocks>> destroyCrossBlocks = new List<List<Blocks>>();
		//check horizontal 
		for(int y=0;y<_maxY;y++){
			List<Blocks> dest = new List<Blocks>();
			for(int x=0;x<_maxX+1;x++){
				if( (x<_maxX && board[y,x]!=null) && 
					(dest.Count == 0 || dest[0].type() == board[y,x].type()) && 
					(board[y,x].UseType() == Blocks.USETYPE.MATCH3) ){
					dest.Add(board[y,x]);
				}else{
					if(dest.Count >= 3){
						destroyNormalBlocks.Add(dest);
						dest = new List<Blocks>();
					}else if(x<_maxX){
						dest.Clear();
						if(board[y,x]!=null){
							dest.Add(board[y,x]);
						}
					}
				}
			}
		}

		//check vertical
		for(int x=0;x<_maxX;x++){
			List<Blocks> dest = new List<Blocks>();
			for(int y=0;y<_maxY+1;y++){
				if( (y<_maxY) && 
					(board[y,x]!=null) && 
					(dest.Count==0 || dest[0].type()==board[y,x].type()) && 
					(board[y,x].UseType() == Blocks.USETYPE.MATCH3) ){
					dest.Add(board[y,x]);
				}else{
					if(dest.Count >= 3){
						destroyNormalBlocks.Add(dest);
						dest = new List<Blocks>();
					}else if(y<_maxY){
						dest.Clear();
						if(board[y,x]!=null){
							dest.Add(board[y,x]);
						}
					}
				}
			}
		}

		//check cross match
		List<int> removeList = new List<int>();
		for (int i = destroyNormalBlocks.Count - 1; i >= 0; i--){
			if(removeList.IndexOf(i) >= 0)
				continue;

			List<Blocks> lim = destroyNormalBlocks[i];
			foreach(Blocks b in lim){
				List<Blocks> tmp = findListInBlockList(b,destroyNormalBlocks,lim);
				if(tmp!=null){
					int index = destroyNormalBlocks.IndexOf(tmp);
					if(removeList.IndexOf(index) == -1 ){
						removeList.Add(i);
						removeList.Add(index);

						lim.Remove(b);
						lim.AddRange(tmp);

						destroyCrossBlocks.Add(lim);
						break;
					}else{
						tmp=null;
					}
				}
				if(tmp==null){
					tmp = findListInBlockList(b,destroyCrossBlocks);
					if(tmp!=null){
						removeList.Add(i);
						lim.Remove(b);

						tmp.AddRange(lim);
						break;
					} 
				}
			}
		}

		removeList.Sort();
		removeList.Reverse();
		foreach(int c in removeList){
			destroyNormalBlocks.RemoveAt(c);
		}

		// check linked cross
		for (int i = destroyCrossBlocks.Count - 1; i >= 0; i--){
			List<Blocks> lim = destroyCrossBlocks[i];
			List<List<Blocks>> overlap = new List<List<Blocks>>();
			foreach(Blocks b in lim){
				foreach(List<Blocks> _ in findListInBlockListAll(b,destroyCrossBlocks)){
					if(_ != lim && overlap.IndexOf(_) == -1){
						overlap.Add(_);
					}
				}
			}
			foreach(List<Blocks> _ in overlap){
				lim.AddRange(_);
				destroyCrossBlocks.Remove(_);
				i=destroyCrossBlocks.Count;
			}
		}

		// Distinct array
		for (int i = destroyCrossBlocks.Count - 1; i >= 0; i--){
			GetDistinctValues<Blocks>(destroyCrossBlocks[i]);
		}

		return new List<List<Blocks>>[2]{destroyNormalBlocks,destroyCrossBlocks};
	}

	//Update block moving down
	void UpdateDropdown(){
		//if(isBlocksMoveToAnim()) return ;

		bool left = true;
		bool right = true;
		for(int x=0;x<_maxX;x++){
			for(int y=_maxY-2;y>=0;y--){
				if(boardshape(y,x) != 0){
					if(_boards[y,x]){
						//아래로 가기
						int destY = -1;
						if(_boards[y+1,x] == null && (destY = findMovableDown(x,y+1)) != -1){
							swapBlock(new Vector2(x,destY),_boards[y,x].pos());
							continue ;
						}
						//왼쪽으로 가기
						if(left && checkMoveDownLeft(x,y-1) == -1 && (destY=checkMoveDownLeft(x,y))!= -1 ){
							swapBlock(new Vector2(x-1,destY),_boards[y,x].pos());
							left = false;
							continue;
						}
						//오른쪽으로 가기
						if(right && checkMoveDownRight(x,y-1) == -1 && (destY=checkMoveDownRight(x,y))!= -1 ){
							swapBlock(new Vector2(x+1,destY),_boards[y,x].pos());
							right = false;
							continue;
						}
					}
				}
			}
		}
	}

	//Update Mouse Input
	void UpdateInput(){
		if(isBlocksMoveToAnim()) return ;

		if (Input.GetMouseButton(0)){ // touch start or mouse clicked
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Blocks b = indexBlock(pos);
			if(_touched == false){
				if(b){
					_lastMoveDir.Set(0,0);
					_lastMovedBlock1 = null;
					_lastMovedBlock2 = null;
					
					b.mousePress();
					_touchBlock = b;
					_touched = true;
				}
			}else{
				if(_touchBlock != null){
					if(b==null){
						//_touchBlock.mouseRelease();
						//_touchBlock.mouseRelease();
						//_touchBlock = null;
					}else{
						if( b != _touchBlock ){
							Vector2 idx1 = blockIdx(b);
							Vector2 idx2 = blockIdx(_touchBlock);

							if(idx1.x != -1 && idx2.x != -1){
								if(Mathf.Abs(idx1.x-idx2.x)+Mathf.Abs(idx1.y-idx2.y) == 1){
									if(swapBlock(b,_touchBlock)){
										_swapTest1 = b;
										_swapTest2 = _touchBlock;

										_lastMoveDir.Set(idx1.x-idx2.x,idx1.y-idx2.y);
										_lastMovedBlock1 = b;
										_lastMovedBlock2 = _touchBlock;

										Vector2 w = new Vector2(_touchBlock.pos().x,_touchBlock.pos().y);
										b.swapEvent(_touchBlock);
										if(_boards[(int)w.y,(int)w.x] != null){
											_touchBlock.swapEvent(b);
										}
									}
								}
							}

							_touchBlock.mouseRelease();
							_touchBlock = null;
						}						
					}
				}
			}
		}else{
			if(_touched){
				if( _touchBlock ){
					_touchBlock.mouseRelease();
					_touchBlock.Click();
					_touchBlock = null;
				}
				_touched = false;
			}
		}
	}

	//update match-3
	void Update3Match(){
		if(isBlocksMoveToAnim()) return ;

		List<List<Blocks>>[] arr = match3Check();
		List<List<Blocks>> destroyNormalBlocks = arr[0];
		List<List<Blocks>> destroyCrossBlocks = arr[1];
		
		if(_swapTest1 != null){
			if(destroyNormalBlocks.Count == 0 && destroyCrossBlocks.Count == 0){
				swapBlock(_swapTest1,_swapTest2);
			}else{
				//decrease move
				_overCondition.SendMessage("message","move");
			}
			_swapTest1 = null;
			_swapTest2 = null;
		}

		int count=0;
		int type=0;
		foreach(List<Blocks> l in destroyNormalBlocks){
			Vector2 p = l[0].pos();
			foreach(Blocks b in l){
				if(_lastMovedBlock1 == b || _lastMovedBlock2 == b)
					p = b.pos();
				count++;
				type = b.type();
				noticeDestroyToNearBlock(b);
				DestroyBlock(b);
			}

			if(	l.Count >=4 ){
				Blocks blck = null;
				if(l.Count==4 && _match4.Length>0) blck = createBlock(_match4,(int)p.x,(int)p.y);
				if(l.Count==5 && _match5.Length>0) blck = createBlock(_match5,(int)p.x,(int)p.y);
				if(blck != null){
					_boards[(int)p.y,(int)p.x] = blck;
				}
			}
		}

		foreach(List<Blocks> l in destroyCrossBlocks){
			Vector2 c = l[0]._posInBoard;
			foreach(Blocks b in l){
				if(_lastMovedBlock1 == b || _lastMovedBlock2 == b)
					c = b._posInBoard;
				type = b.type();
				noticeDestroyToNearBlock(b);
				DestroyBlock(b);
				count++;
			}
			if(_matchCross.Length>0){
				Blocks blck = createBlock(_matchCross,(int)c.x,(int)c.y);
				_boards[(int)c.y,(int)c.x] = blck;
			}
		}

		if(count>0){
			_score += count*5;
			EventThrow_UpdateScore();
		}

	}

	// check exists match hint
	void UpdateHint(){
		if(_updateHint){
			_updateHint = false;

			int[] dx = new int[]{-1,1,0,0};
			int[] dy = new int[]{0,0,-1,1};
			Blocks[,] tester = new Blocks[_boards.GetLength(0),_boards.GetLength(1)];//_boards.Select(a => a.ToArray()).ToArray();
			Array.Copy(_boards,tester,_boards.Length);
			for(int x=0;x<_maxX;x++){
				for(int y=0;y<_maxY;y++){
					List<Blocks> bList;
					if(tester[y,x] != null){
						for(int i=0;i<4;i++){
							if(y+dy[i]>0 && x+dx[i]>0 && y+dy[i]<_maxY && x+dx[i]<_maxX){
								testSwap(tester,x+dx[i],y+dy[i],x,y);
								bList = extractMatch(match3Check(tester));
								testSwap(tester,x+dx[i],y+dy[i],x,y);
								if(bList!=null && bList.Count > 0){
									return ;
								}
							}
						}
					}
				}
			}
			ShakeBoard();
		}
	}

	// Update is called once per frame
	void Update () {
		if(_updatePause){
			return ;
		}
		if(_clearCondition.isClear()){
			return ;
		}
		if(_overCondition.isOver()){
			return ;
		}
		UpdateHint();
		UpdateEntryBlock();
		UpdateDropdown();
		UpdateInput();
		Update3Match();
	}
}
