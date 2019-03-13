using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
// Puzzle Stage Editor
/// </summary>
public class PuzzleEditor : EditorWindow {
	private int _numX = 8;
	private int _numY = 8;
	private int _sizeX = 80;
	private int _sizeY = 80;
	private int _clearType = 0;
	private int _overType = 0;
	private int _stackType = 0;
	private int _selectTabs =0;
	private int[,] _boards = new int[15,15];
	private bool[] _boardsEnter = new bool[15];

	private int _BlockSelectedIndex = -2;

	//use block list
	private List<GameObject> _useBlock = new List<GameObject>();
	private List<Color> _useBlockColor = new List<Color>();

	//clear types
	private GameObject[] _gameClearBlocksType = new GameObject[1];
	private int[] _gameClearBlocksCount = new int[1];

	private int _gameClearScore = 30000;
	//over types
	private int _gameOverMoves = 30;
	private int _gameOverTime = 90;

	//strings
	private string currentSettings="";
	private string ROOTDIR = Application.dataPath+"/Resources/Stage/";

	private StageTexture _stageTexture = new StageTexture(10,10,"Textures/tile");
	//private GameObject _block = (GameObject)Resources.Load("Prefab/Block");

	//for UI
	private string[] _stackStrs = new string[] {"No Pass","Pass Blank"};
	private string[] _clearStrs = new string[] {"Blocks","Score"};
	private string[] _overStrs = new string[] {"Move","Time"};
	private string[] _toolbarStrs = new string[] {"GameSystem", "Generate"};

	//show this editor
	[MenuItem("Puzzle/Stage Editor")]
	public static void ShowWindow()
	{
		PuzzleEditor window = (PuzzleEditor)EditorWindow.GetWindow(typeof(PuzzleEditor));
	}

	// init and load tmp puzzle
	public PuzzleEditor(){
		for(int i =0;i<_boards.GetLength(0);i++){
			for(int j=0;j<_boards.GetLength(1);j++){
				_boards[i,j]=-1;
			}
		}
		for(int i =0;i<_boardsEnter.Length;i++){
			_boardsEnter[i] = true;
		}

		//load auto save
		string tmpFolder = ROOTDIR+"tmp/";
		if(!Directory.Exists(tmpFolder))
			Directory.CreateDirectory(tmpFolder);
		open(tmpFolder+"meta.puzzle");
	}

	// make unique color 
	Color GetNewBlockColor(){
		int gap = 100;
		while(true){
			int r = UnityEngine.Random.Range(0,255);
			int g = UnityEngine.Random.Range(0,255);
			int b = UnityEngine.Random.Range(0,255);
			if( 255-r < gap || g < gap || g < gap )
				continue;
			if( r < gap || 255-g < gap || g < gap )
				continue;
			bool min = false;

			foreach(Color _c in _useBlockColor){
				if( Math.Abs(_c.r-r) < gap || 
					Math.Abs(_c.g-g) < gap || 
					Math.Abs(_c.b-b) < gap ){
					min = true;
					break;
				}
			}

			if(min == false){
				return new Color((float)r/255f,(float)g/255f,(float)b/255f);
			}
			gap -= 3;
			if(gap < 0)
				gap = 0;
		}
	}

	//Loaded All Blocks prefab and regist "use block list"
	public void UpdateAllBlockUse(){
		List<GameObject> blcks = BlockEditor.UpdateBlockPrefab();
		foreach( GameObject b in blcks ){
			if(_useBlock.Exists(x => x == b) == false){
				_useBlock.Add(b);
				_useBlockColor.Add(GetNewBlockColor());
			}
		}
	}

	//insert int-variable in byte list
	void insertIntByte(List<byte> data,int i){
		byte[] scoreBytes = BitConverter.GetBytes(i);
		if (BitConverter.IsLittleEndian)
			Array.Reverse(scoreBytes);
		data.Add(scoreBytes[0]);
		data.Add(scoreBytes[1]);
		data.Add(scoreBytes[2]);
		data.Add(scoreBytes[3]);
	}

	//bytes to int
	int bytesToInt(byte[] bytes,ref int c){
		byte[] bb = new byte[4];
		bb[0] = bytes[c++];
		bb[1] = bytes[c++];
		bb[2] = bytes[c++];
		bb[3] = bytes[c++];

		if (BitConverter.IsLittleEndian)
			Array.Reverse(bb);
		return BitConverter.ToInt32(bb,0);
	}

	//bytes to string
	string bytesToString(byte[] bytes,int length,ref int c){
		byte[] newByte = new byte[length];
		Array.Copy(bytes,c,newByte,0,length);

		c += length;

		return ASCIIEncoding.ASCII.GetString(newByte);
	}

	// save *.puzzle file
	byte[] stageDataToByte(){
		List<byte> data = new List<byte>();
		data.Add((byte)_numX);
		data.Add((byte)_numY);
		data.Add((byte)_sizeX);
		data.Add((byte)_sizeY);
		for(int x=0;x< _numX; x++){
			for(int y=0;y< _numY; y++){
				data.Add((byte)(_boards[y,x]+2));
			}
		}
		for(int x=0;x<_numX;x++){
			if(_boardsEnter[x])
				data.Add((byte)1);
			else
				data.Add((byte)2);
		}
		data.Add((byte)_clearType);

		//Use Block List
		data.Add((byte)_useBlock.Count);
		for(int i=0;i<_useBlock.Count;i++){
			string path = AssetDatabase.GetAssetPath(_useBlock[i]);
			insertIntByte(data,path.Length);
			data.AddRange(Encoding.ASCII.GetBytes(path));
		}

		//clear type 1
		data.Add((byte)_gameClearBlocksType.Length);
		for(int i=0;i<_gameClearBlocksType.Length;i++){
			string path = AssetDatabase.GetAssetPath(_gameClearBlocksType[i]);

			insertIntByte(data,path.Length);
			data.AddRange(Encoding.ASCII.GetBytes(path));
			data.Add((byte)_gameClearBlocksCount[i]);
		}

		//clear type 2
		insertIntByte(data,_gameClearScore);

		//gameover
		data.Add((byte)_overType);
		insertIntByte(data,_gameOverMoves);
		insertIntByte(data,_gameOverTime);

		return data.ToArray();
	}

	//load *.puzzle file
	void open(string dist=""){
		string distDir = ROOTDIR;
		string path = "";
		if(dist.Length == 0)
			path = EditorUtility.OpenFilePanel("Open Puzzle",distDir,"puzzle");
		else
			path = dist;

		if (path.Length != 0) {
			int c = 0;
			if(!File.Exists(path))
				return ;
			byte[] bytes = File.ReadAllBytes(path);
			if( bytes.Length > 0 ){
				_numX = (int)bytes[c++];
				_numY = (int)bytes[c++];
				_sizeX = (int)bytes[c++];
				_sizeY = (int)bytes[c++];
				for(int x=0;x<_numX;x++){
					for(int y=0;y<_numY;y++){
						_boards[y,x] = ((int)bytes[c++])-2;
					}
				}
				for(int x=0;x<_numX;x++){
					_boardsEnter[x] = (bytes[c++] == 1);
				}
				_clearType = (int)bytes[c++];

				//Use Block List
				int useBlockCount = (int)bytes[c++];
				_useBlock.Clear();
				_useBlockColor.Clear();
				for(int i=0;i<useBlockCount;i++){
					int length = bytesToInt(bytes,ref c);
					string p = bytesToString(bytes,length,ref c);

					_useBlock.Add(AssetDatabase.LoadAssetAtPath<GameObject>(p));
					_useBlockColor.Add(GetNewBlockColor());
				}

				//clear type 1
				int typeSize = (int)bytes[c++];
				_gameClearBlocksType  = new GameObject[typeSize];
				_gameClearBlocksCount = new int[typeSize];
				for(int i=0;i<typeSize;i++){
					int length = bytesToInt(bytes,ref c);
					string p = bytesToString(bytes,length,ref c);

					_gameClearBlocksType[i] = AssetDatabase.LoadAssetAtPath<GameObject>(p);//(int)bytes[c++];
					_gameClearBlocksCount[i]= (int)bytes[c++];
				}
				//clear type 2
				_gameClearScore = bytesToInt(bytes,ref c);

				//game over 
				_overType = (int)bytes[c++];
				_gameOverMoves = bytesToInt(bytes,ref c);
				_gameOverTime = bytesToInt(bytes,ref c);

				if(dist.Length == 0){
					currentSettings = path.Replace("meta.puzzle","");
				}
			}
		}
	}

	// save current configure
	bool save(string dist=""){
		if(_stageTexture.tex()){
			string distDir = ROOTDIR;
			string path = "";
			if(dist.Length == 0 )
				path = EditorUtility.OpenFolderPanel("save puzzle",distDir,"");
			else
				path = dist;
			if (path.Length != 0) {
				string tileImg = path+"/tile.png";
				string stageInf= path+"/meta.puzzle";

				if(dist.Length==0){
					if(File.Exists(stageInf)){
						if(!EditorUtility.DisplayDialog("Puzzle file already here.","overwrite this file?","overwrite","cancel")){
							return false;
						}
					}
				}

				File.WriteAllBytes(tileImg,_stageTexture.bytes());
				File.WriteAllBytes(stageInf,stageDataToByte());

				if(dist.Length == 0){
					currentSettings = path+"/";
				}
				return true;
			}
		}
		return false;
	}

	// apply current configure to current scene
	void apply(){
		if(UnityEditor.EditorApplication.isPlaying){
			if(EditorUtility.DisplayDialog("Off Play Mode.","turn off playmode first.","ok"))
				UnityEditor.EditorApplication.isPlaying = false;
			return ;
		}
		if(currentSettings.Length == 0){
			if(!EditorUtility.DisplayDialog("Save first.","you need save this settings","yes","cancel"))
				return ;
			if(!save())
				return ;
		}
		
		GameSystem gs = (GameSystem)FindObjectOfType(typeof(GameSystem));
		if (gs){
			if(!EditorUtility.DisplayDialog("PuzzleSystem Already Apply Current Scene.","do you want replace settings?","replace","cancel"))
				return ;
			DestroyImmediate(gs.gameObject);
		}
		
		GameClearCondition _gc = (GameClearCondition)FindObjectOfType(typeof(GameClearCondition));
		GameOverCondition _go = (GameOverCondition)FindObjectOfType(typeof(GameOverCondition));
		if(_gc) DestroyImmediate(_gc.gameObject);
		if(_go) DestroyImmediate(_go.gameObject);

		save(currentSettings);
		AssetDatabase.Refresh();

		string absolute = currentSettings.Replace(ROOTDIR,"Stage/");
		Sprite backTex = Resources.Load<Sprite>(absolute+"tile");

		List<GameObject> blcks = _useBlock;
		List<GameObject> normal = new List<GameObject>();
		List<GameObject> match4 = new List<GameObject>();
		List<GameObject> match5 = new List<GameObject>();
		List<GameObject> cross  = new List<GameObject>();
		foreach(GameObject g in blcks){
			Blocks.CREATETYPE type = g.GetComponent<Blocks>().CreateType();
			switch(type){
				case Blocks.CREATETYPE.NORMAL: normal.Add(g); break;
				case Blocks.CREATETYPE.MATCH4: match4.Add(g); break;
				case Blocks.CREATETYPE.MATCH5: match5.Add(g); break;
				case Blocks.CREATETYPE.MATCHCROSS: cross.Add(g); break;
			}
		}

		GameObject go = new GameObject("PuzzleSystem");

		float sx = 100f/(backTex.rect.width/backTex.bounds.size.x);
		float sy = 100f/(backTex.rect.height/backTex.bounds.size.y);

		GameObject obj = new GameObject("PuzzleBackground");
		obj.transform.parent = go.transform;
		obj.transform.localPosition = new Vector3(0,0,1);
		obj.transform.localScale = new Vector3(sx,sy,1);
		SpriteRenderer backSpr = obj.AddComponent<SpriteRenderer>();
		backSpr.sprite = backTex;

		gs = go.AddComponent<GameSystem>();
		gs._maxX = _numX;
		gs._maxY = _numY;
		gs._gapX = _stageTexture.cornorX;
		gs._gapY = _stageTexture.cornorY;
		gs._blocks = normal.ToArray();
		gs._match4 = match4.ToArray();
		gs._match5 = match5.ToArray();
		gs._matchCross = cross.ToArray();
		gs._boardshape = new int[_numX*_numY];
		gs._blockEntry = new bool[_numX];
		gs._blockSize = new Vector2(_sizeX,_sizeY);
		gs._background = backSpr.sprite;
		gs._isPassBlank = _stackType==1 ;
		///////////////////////////////////////
		// Clear Condition!
		///////////////////////////////////////
		GameObject gc = new GameObject("GameClearCondition");
		GameClearCondition clearCondition = null;
		if(_clearType == 0){
			clearCondition = gc.AddComponent<GameClearCondition_Blocks>();
			GameClearCondition_Blocks blocks = (GameClearCondition_Blocks)clearCondition;

			blocks._types = new GameObject[_gameClearBlocksType.Length];//_gameClearBlocksType;
			blocks._count = new int[_gameClearBlocksCount.Length];//_gameClearBlocksCount;
			Array.Copy(_gameClearBlocksType,0,blocks._types,0,_gameClearBlocksType.Length);
			Array.Copy(_gameClearBlocksCount,0,blocks._count,0,_gameClearBlocksCount.Length);
		}else if(_clearType == 1){
			clearCondition = gc.AddComponent<GameClearCondition_Score>();
			GameClearCondition_Score score = (GameClearCondition_Score)clearCondition;
			
			score._targetScore = _gameClearScore;
		}
		gc.AddComponent<GameMessage>();
		gs._clearCondition = gc.GetComponent<GameClearCondition>();
		gs._clearCondition._system = gs;

		///////////////////////////////////////
		// Over Condition
		///////////////////////////////////////
		GameObject goc = new GameObject("GameOverCondition");
		GameOverCondition overCondition = null;
		if(_overType == 0){
			overCondition = goc.AddComponent<GameOverCondition_Move>();
			GameOverCondition_Move move = (GameOverCondition_Move)overCondition;
			move._move = _gameOverMoves;
		}else if(_overType == 1){
			overCondition = goc.AddComponent<GameOverCondition_Time>();
			GameOverCondition_Time time = (GameOverCondition_Time)overCondition;
			time._time = (float)_gameOverTime;
		}
		goc.AddComponent<GameMessage>();
		gs._overCondition = goc.GetComponent<GameOverCondition>();
		gs._overCondition._system = gs;

		//board shape
		for(int x = 0;x<_numX;x++){
			gs._blockEntry[x] = _boardsEnter[x];
			for(int y = 0;y<_numY;y++){
				gs._boardshape[x+y*_numX] = _boards[y,x] != -2 ? 1 : 0;
			}
		}

		//board prepare block!
		for(int x = 0;x<_numX;x++){
			for(int y = 0;y<_numY;y++){
				if(_boards[y,x] >= 0){
					Blocks block = gs.createBlock(_useBlock[_boards[y,x]],x,y);
				}
			}
		}
	}

	T[] ArrayIntRemoveAt<T>(T[] a,int index){
		List<T> _list = new List<T>(a);
		_list.RemoveAt(index);
		return _list.ToArray();
	}

	//////////////////////////////////////////////////
	////// On GUI
	//////////////////////////////////////////////////
	void OnGUI()
	{
		_selectTabs = GUILayout.Toolbar(_selectTabs, _toolbarStrs);
		OnLoadUI(0);
		EditorGUILayout.Space();

		if(_selectTabs == 0 ){
			OnBoardSize(0);
			EditorGUILayout.BeginHorizontal();
				OnBoardWindow(0);
				OnGameRole(0);
			EditorGUILayout.EndHorizontal();
		}else if(_selectTabs == 1){
			if(GUILayout.Button("Apply Current Scene")){
				save(ROOTDIR+"tmp/");
				apply();
			}
			if(_stageTexture.tex() == null){
				_stageTexture.stageTextureUpdate(new Vector2(_sizeX,_sizeY),
												 new Vector2(_numX,_numY),
												 _boards);
			}
			float sx = _numX*_sizeX/2;
			float sy = _numY*_sizeY/2;
			float x = position.width/2-sx/2;
			float y = 100;
			EditorGUI.DrawPreviewTexture(new Rect(x,y,sx,sy),_stageTexture.tex());
		}
	}

	bool _showBlockType = true;
	bool _showClearType = true;
	bool _showOverType = true;
	// Game Role GUI
	void OnGameRole(int id){
		EditorGUILayout.BeginVertical();
			_showBlockType = EditorGUILayout.Foldout(_showBlockType, "Block Stack Type",EditorStyles.boldLabel);
			
			if(_showBlockType) {
				_stackType = GUILayout.SelectionGrid(_stackType,_stackStrs,2);
			}

			GUILayout.Space(10);

			_showClearType = EditorGUILayout.Foldout(_showClearType, "GameClear Condition",EditorStyles.boldLabel);
			if(_showClearType) {
				_clearType = GUILayout.SelectionGrid(_clearType,_clearStrs,_clearStrs.Length);

				int ct = _clearType;

				EditorGUILayout.BeginToggleGroup("Blocks Required", ct==0);
					for(int i=0;i<_gameClearBlocksType.Length; i++){
						GameObject obj = _gameClearBlocksType[i];
						int count = _gameClearBlocksCount[i];
						EditorGUILayout.BeginHorizontal();
							//EditorGUILayout.LabelField("Block Type ",GUILayout.MinWidth(0));
							obj=(GameObject)EditorGUILayout.ObjectField("",obj,typeof(GameObject), false,GUILayout.MinWidth(0));
							EditorGUILayout.LabelField("Count",GUILayout.MinWidth(0));
							count=EditorGUILayout.IntField(count,GUILayout.MinWidth(0));
							if(GUILayout.Button("Del")){
								_gameClearBlocksType  = ArrayIntRemoveAt<GameObject>(_gameClearBlocksType,i);
								_gameClearBlocksCount = ArrayIntRemoveAt<int>(_gameClearBlocksCount,i);
								return;
							}
						EditorGUILayout.EndHorizontal();
						_gameClearBlocksType[i] = obj;
						_gameClearBlocksCount[i] = count;
					}

					if(GUILayout.Button("Add")){
						Array.Resize(ref _gameClearBlocksType, _gameClearBlocksType.Length+1 );
						Array.Resize(ref _gameClearBlocksCount, _gameClearBlocksCount.Length+1 );
					}
				EditorGUILayout.EndToggleGroup();

				EditorGUILayout.BeginToggleGroup("Score Required", ct==1);
					_gameClearScore = EditorGUILayout.IntField("Score",_gameClearScore,GUILayout.MinWidth(0));
				EditorGUILayout.EndToggleGroup();
			}
			
			GUILayout.Space(10);
			
			_showOverType = EditorGUILayout.Foldout(_showClearType, "GameOver Condition",EditorStyles.boldLabel);
			if(_showOverType) {
				_overType = GUILayout.SelectionGrid(_overType,_overStrs,_overStrs.Length);
				
				int ct = _overType;
				
				EditorGUILayout.BeginToggleGroup("Move Limite", ct==0);
					_gameOverMoves = EditorGUILayout.IntField("Moves",_gameOverMoves,GUILayout.MinWidth(0));
				EditorGUILayout.EndToggleGroup();

				EditorGUILayout.BeginToggleGroup("Time Limite", ct==1);
					_gameOverTime  = EditorGUILayout.IntField("Sec",_gameOverTime,GUILayout.MinWidth(0));
				EditorGUILayout.EndToggleGroup();
			}
		EditorGUILayout.EndVertical();
	}

	// Load-Save GUI
	void OnLoadUI(int id){
		if(currentSettings.Length == 0){
			EditorGUILayout.LabelField("New Puzzle Settings (need save)",EditorStyles.boldLabel);
		}else{
			EditorGUILayout.LabelField(currentSettings);
		}
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Load Puzzle")){
			open();
		}
		if(GUILayout.Button("Save Puzzle")){
			save();
		}
		EditorGUILayout.EndHorizontal();
	}
	
	// Board Size GUI
	void OnBoardSize(int id){
		int prevSizeX = _sizeX;
		int prevSizeY = _sizeY;
		int prevNumX = _numX;
		int prevNumY = _numY;

		EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Block Nums X,Y",GUILayout.Width(100));
				_numX = EditorGUILayout.IntField(_numX,GUILayout.MinWidth(0));
				_numY = EditorGUILayout.IntField(_numY,GUILayout.MinWidth(0));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Block Size X,Y",GUILayout.Width(100));
				_sizeX = EditorGUILayout.IntField(_sizeX,GUILayout.MinWidth(0));
				_sizeY = EditorGUILayout.IntField(_sizeY,GUILayout.MinWidth(0));
			EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();

		if(_numX < 0) _numX = 0;
		if(_numY < 0) _numY = 0;
		if(_numY > _boards.GetLength(0)) _numY = _boards.GetLength(0);
		if(_numX > _boards.GetLength(1)) _numX = _boards.GetLength(1);

		if(prevSizeX != _sizeX || prevSizeY != _sizeY || prevNumX != _numX || prevNumY != _numY)
			_stageTexture.reset();
	}

	Vector2 useBlockScroll = new Vector2(0,0); //for scroll
	// Map Design And "Use Block List" GUI
	void OnBoardWindow(int id)
	{
		bool dirty = false;
		Color colored = GUI.color;
		EditorGUILayout.BeginVertical();
			EditorGUILayout.Foldout(_showBlockType, "Map Design",EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			for(int x=0;x<_numX;x++){
				if(_boardsEnter[x]){
					if(GUILayout.Button("↓",GUILayout.Width(20))){
						_boardsEnter[x] = false;
						dirty = true;
					}
				}else{
					if(GUILayout.Button("X",GUILayout.Width(20))){
						_boardsEnter[x] = true;
						dirty = true;
					}
				}
				
			}
			EditorGUILayout.EndHorizontal();

			for(int y=0;y<_numY;y++){
				EditorGUILayout.BeginHorizontal();
				for(int x=0;x<_numX;x++){
					if(_boards[y,x] >= _useBlockColor.Count ){
						//Debug.Log(_boards[y,x]);
						//Debug.Log(_useBlockColor.Count);
						_boards[y,x] = -1;
					}
					if(_boards[y,x] == -2)
						GUI.color = Color.red;
					else if(_boards[y,x] == -1)
						GUI.color = Color.green;
					else{
						GUI.color = _useBlockColor[_boards[y,x]];
					}
					int display = _boards[y,x]+2;
					if(GUILayout.Button(display.ToString(),GUILayout.Width(20))){
						_boards[y,x] = _BlockSelectedIndex;
						dirty = true;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			GUI.color = colored;
			EditorGUILayout.Foldout(_showBlockType, "Use Block List",EditorStyles.boldLabel);
			useBlockScroll=EditorGUILayout.BeginScrollView(useBlockScroll);
				EditorGUILayout.BeginHorizontal();
					GUI.color = Color.red;
					GUILayout.TextField("Blank");
					GUI.color = colored;
					if(_BlockSelectedIndex == -2 )
						GUI.color = Color.green;
					if(GUILayout.Button("Sel",GUILayout.Width(40))){
						_BlockSelectedIndex = -2;
					}
					GUI.color = colored;
					GUILayout.Button("Del",GUILayout.Width(40));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
					GUI.color = Color.green;
					GUILayout.TextField("Space");
					GUI.color = colored;
					if(_BlockSelectedIndex == -1 )
						GUI.color = Color.green;
					if(GUILayout.Button("Sel",GUILayout.Width(40))){
						_BlockSelectedIndex = -1;
					}
					GUI.color = colored;
					GUILayout.Button("Del",GUILayout.Width(40));
				EditorGUILayout.EndHorizontal();
				for(int i =0;i<_useBlock.Count;i++){
					GUI.color = _useBlockColor[i];
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.ObjectField("",_useBlock[i],typeof(GameObject), false, GUILayout.MinWidth(0));
						GUI.color = colored;
						if(_BlockSelectedIndex == i )
							GUI.color = Color.green;
						if(GUILayout.Button("Sel",GUILayout.Width(40))){
							_BlockSelectedIndex = i;
						}
						GUI.color = colored;
						if(GUILayout.Button("Del",GUILayout.Width(40))){
							_useBlockColor.RemoveAt(i);
							_useBlock.RemoveAt(i);
							return ;
						}
					EditorGUILayout.EndHorizontal();
				}
				GUI.color = colored;
				if(GUILayout.Button("Use All Blocks")){
					UpdateAllBlockUse();
				}
			EditorGUILayout.EndScrollView();

		EditorGUILayout.EndVertical();

		if(dirty){
			_stageTexture.reset();
		}
	}
}
