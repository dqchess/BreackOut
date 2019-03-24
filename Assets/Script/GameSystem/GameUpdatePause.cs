using UnityEngine;
using System.Collections;

/// <summary>
// Game Update Pause class
// ex) game system will pause when particle live. and resume when particle all removed.
/// </summary>
public class GameUpdatePause : MonoBehaviour {
	private static int LiveCount = 0;
	private GameSystem _gameSystem = null;
	
	//set current game system.
	void SetGameSystem(GameSystem sys){
		_gameSystem = sys;
		if(LiveCount > 0){
			_gameSystem._updatePause = true;
		}
	}

	//increase live-count
	void Awake() {
		LiveCount ++;
	}
	
	//decrease live-count. and resume when live-cont is zero 
	void OnDestroy() {
		LiveCount --;
		if(LiveCount <= 0){
			if(_gameSystem != null){
				_gameSystem._updatePause = false;
			}
			LiveCount = 0;
		}
	}
}
