using UnityEngine;
using System.Collections;


public class GameControl : MonoBehaviour {

	[SerializeField]
	private GameObject[] enemies;
	[SerializeField]
	private float spawnWait;
	[SerializeField]
	private float waveWait;
	[SerializeField]
	private float initWait;

	void Start (){
		StartCoroutine (SpawnWaves ());
	}

	IEnumerator SpawnWaves (){
		yield return new WaitForSeconds (initWait);
		while (true) {
			for (int i = 0; i < enemies.Length; i++) {
				float x = Random.Range (GameConfig.X_MIN, GameConfig.X_MAX);
				float y = GameConfig.Y_MIN;//Random.Range (GameConfig.Y_MIN, GameConfig.Y_MAX);
				float z = 0f;
				Vector3 spawnPosition = new Vector3 (x, y, z);
				Instantiate (enemies [i], spawnPosition, transform.rotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

}
