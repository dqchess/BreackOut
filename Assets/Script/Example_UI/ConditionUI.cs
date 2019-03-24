using UnityEngine;
using System.Collections;

/// <summary>
// Condition UI
/// </summary>
public class ConditionUI : MonoBehaviour {
	//result window
	public GameObject window = null;

	//finish game
	public void Finish(){
		if(window != null){
			window.transform.localScale = new Vector3(1.2f,1.2f,1);
		}

	}
	
	//SendMessage all displayed object
	public static void BroadcastAll(string fun, System.Object msg) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.SendMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	//Make Text GUI GameObject
	public TextMesh MakeText(string strText,Vector2 pos,int size,Color c,TextAnchor anchor=TextAnchor.MiddleCenter){
		TextMesh t = (TextMesh)FindObjectOfType(typeof(TextMesh));

		GameObject obj = new GameObject();
		obj.transform.parent = this.transform;
		obj.transform.localPosition = pos;
		obj.transform.localScale = new Vector3(
			obj.transform.localScale.x/10.0f,
			obj.transform.localScale.y/10.0f,1
		);

		TextMesh text = obj.AddComponent<TextMesh>();
		text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		text.text = strText;
		text.fontSize = size;
		text.anchor = anchor;

		obj.GetComponent<Renderer>().material = t.GetComponent<Renderer>().material;
		obj.GetComponent<Renderer>().material.color = c;

		return text;
	}

	//Make Sprite GUI GameObject
	public GameObject MakeSprite(Sprite sprite,Vector2 pos){
		GameObject obj = new GameObject();
		obj.transform.parent = this.transform;
		obj.transform.localPosition = pos;
		obj.transform.localScale = new Vector3(0.3f,0.4f,1);
		obj.AddComponent<SpriteRenderer>().sprite = sprite;
		return obj;
	}
}
