using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	[SerializeField]
	private Text scoreText;
	private int score;
	[SerializeField]
	private Text lifeText;
	private int life;
	[SerializeField]
	private Text ammoText;
	private int ammo;
	[SerializeField]
	private Text gameOverText;
	private float restartDelay = 5f;
	private float restartTimer;

	void Start (){
		StartCoroutine (SpawnWaves ());
		score = 0;
		life = 10;
		ammo = 0;
		UpdateAmmo ();
		UpdateLife ();
		UpdateScore ();
	}

	void Update(){
		GameOver ();
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

	public void SetScore(int score){
		this.score += score;
		UpdateScore ();
	}

	void UpdateScore(){
		scoreText.text = "Puntaje: " + score;
	}

	public void SetLife(int life){
		this.life += life;
		UpdateLife ();
	}

	void UpdateLife(){
		lifeText.text = "Vida: " + life;
	}

	public void SetAmmo(int ammo){
		this.ammo += ammo;
		UpdateAmmo ();
	}

	void UpdateAmmo(){
		ammoText.text = "Munición: " + ammo;
	}

	void GameOver(){
		if (life == 0) {
			gameOverText.text = "GAME OVER";
			restartTimer += Time.deltaTime;
			if (restartTimer >= restartDelay) {
				DeleteAll ();
				//Application.LoadLevel (Application.loadedLevel);
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}
	}

	public void DeleteAll(){
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
			Destroy(o);
		}
	}
}
