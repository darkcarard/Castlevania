using UnityEngine;
using System.Collections;


public class ThrowLoot : MonoBehaviour{

	[SerializeField] private GameObject[] loot;
	private bool isQuitting;

	void OnApplicationQuit(){
		isQuitting = true;
	}

	void OnDestroy(){
		if (!isQuitting){
			Instantiate (GetLoot (),new Vector3(transform.position.x,GameConfig.Y_MIN,0f),transform.rotation);
		}
	}

	GameObject GetLoot(){
		int index = Random.Range (0, loot.Length);
		return loot [index];
	}
}

