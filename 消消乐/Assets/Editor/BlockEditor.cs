using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
// Generate New Block and Modify Block Infomation
/// </summary>
public class BlockEditor : EditorWindow {
	//managed blocks
	private List<GameObject> _blockSprite = new List<GameObject>();

	//show this editor
	[MenuItem("Puzzle/Block Editor")]
	public static void ShowWindow()
	{
		BlockEditor window = (BlockEditor)EditorWindow.GetWindow(typeof(BlockEditor));
	}

	//init blocks
	public BlockEditor(){
		_blockSprite = UpdateBlockPrefab();
	}

	//Find all blocks prefab in project 
	public static UnityEngine.Object[] GetAssetsOfType(System.Type type, string fileExtension)
	{
		List<UnityEngine.Object> tempObjects = new List<UnityEngine.Object>();
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

		int i = 0; int goFileInfoLength = goFileInfo.Length;
		string tempFilePath;
		FileInfo tempGoFileInfo;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
			tempGoFileInfo = goFileInfo[i];
			if (tempGoFileInfo == null)
				continue;

			tempFilePath = tempGoFileInfo.FullName;
			tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");

			tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
			if (tempGO == null)
				continue;
			else if (tempGO.GetType() != type)
				continue;
			tempObjects.Add(tempGO);
		}
		return tempObjects.ToArray();
	}

	// generate new block prefab
	void NewBlockPrefab(){
		int i=0;
		string root = "Assets/Resources/Prefab/";
		string file = root+"NewBlock.prefab";
		
		//check override file name
		while(true){
			if(File.Exists(file) == false)
				break;
			file = root+"NewBlock("+i+").prefab";
			i++;
		}
		GameObject go = new GameObject();
		go.AddComponent<Blocks>();
		go.AddComponent<SpriteRenderer>();
		
		UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(file);
		PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);

		DestroyImmediate(go);
	}

	//Update Blocks prefab file (if modified outside)
	public static List<GameObject> UpdateBlockPrefab(){
		List<GameObject> list = new List<GameObject>();
		
		UnityEngine.Object[] objs = GetAssetsOfType(typeof(GameObject),".prefab");
		GameObject go = null;
		foreach(GameObject obj in objs){
			go = (GameObject)obj;
			if(go.GetComponent<Blocks>() == null)
				continue;

			SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
			if(renderer == null){
				renderer = go.AddComponent<SpriteRenderer>();
			}
			list.Add(go);
		}
		return list;
	}

	//////////////////////////////////////////////////
	////// On GUI
	//////////////////////////////////////////////////
	void OnGUI()
	{
		OnBlocksManage(0);
	}

	//block manage gui function
	Vector2 blockManageScroll = new Vector2(0,0); // for scroll position
	void OnBlocksManage(int id){
		_blockSprite = UpdateBlockPrefab();
		blockManageScroll=EditorGUILayout.BeginScrollView(blockManageScroll);
		for(int i=0;i<_blockSprite.Count;i++){
			GameObject go = _blockSprite[i];
			Blocks blockScript = go.GetComponent<Blocks>();
			SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();

			MonoScript o = MonoScript.FromMonoBehaviour(blockScript);
			MonoScript n = null;
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(i+":",EditorStyles.boldLabel,GUILayout.MaxWidth(20));
				EditorGUILayout.BeginVertical();
					
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.ObjectField("",go,typeof(GameObject), false,GUILayout.MinWidth(0));
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Block Script",EditorStyles.boldLabel,GUILayout.MaxWidth(100));
						n=EditorGUILayout.ObjectField("",o,typeof(MonoScript), false,GUILayout.MinWidth(0)) as MonoScript;
					EditorGUILayout.EndHorizontal();

					blockScript._normal  = EditorGUILayout.ObjectField("Normal",blockScript._normal, typeof(Sprite), false,GUILayout.MinWidth(0)) as Sprite;
					blockScript._pressed = EditorGUILayout.ObjectField("Press",blockScript._pressed, typeof(Sprite), false,GUILayout.MinWidth(0)) as Sprite;
					
					blockScript._destroyParticle  = EditorGUILayout.ObjectField("DestroyParticle",blockScript._destroyParticle, typeof(GameObject), false,GUILayout.MinWidth(0)) as GameObject;
					renderer.sprite = blockScript._normal;
					if(n!=null){
						if(o.GetClass() != n.GetClass()){
							Blocks b = null;
							GameObject _ = new GameObject();
							try{
								 b = _.AddComponent(n.GetClass()) as Blocks;
							}catch (IOException){
							}

							if(b != null){
								DestroyImmediate(blockScript,true);
								go.AddComponent(n.GetClass());
							}else{
								EditorUtility.DisplayDialog("Not a Block Script","it not block script.","ok");
							}

							DestroyImmediate(_);
						}
					}
				/*
				if(GUILayout.Button("Del",GUILayout.MinWidth(30))){
					_blockSprite.RemoveAt(i);
					return ;
				}
				*/
				EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
		if(GUILayout.Button("New")){
			NewBlockPrefab();
			AssetDatabase.Refresh();
		}
		EditorGUILayout.EndScrollView();
	}
}
