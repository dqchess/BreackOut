using UnityEngine;
using System.Collections;

/// <summary>
// Destroy Self class.
// used particle animation ended
/// </summary>
public class DestroySelf : MonoBehaviour {
	private ParticleSystem ps = null;
	
	void Awake(){
		ps = GetComponent<ParticleSystem>();
	}
	void destroySelf(){
		Destroy(this.gameObject);
	}

	void Update(){
		if(ps == null)
			return ;

		if(ps.IsAlive()==false)
			Destroy(this.gameObject);
	}
}
