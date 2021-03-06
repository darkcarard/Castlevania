﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameControl : MonoBehaviour {

	[SerializeField] private GameObject[] enemies;
	[SerializeField] private float spawnWait;
	[SerializeField] private float waveWait;
	[SerializeField] private float initWait;
	[SerializeField] private Text scoreText;
	private int score;
	[SerializeField] private Text lifeText;
	private int life;
	[SerializeField] private Text ammoText;
	private int ammo;
	[SerializeField] private Text gameOverText;
	private float restartDelay = 5f;
	private float restartTimer;
	private bool died;

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
		Pause ();
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

	public int GetLife(){
		return this.life;
	}

	void UpdateLife(){
		lifeText.text = "Vida: " + life;
	}

	public void SetAmmo(int ammo){
		this.ammo += ammo;
		UpdateAmmo ();
	}

	public int GetAmmo(){
		return this.ammo;
	}

	void UpdateAmmo(){
		ammoText.text = "Munición: " + ammo;
	}

	void Pause(){
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Time.timeScale == 1) {
				GetComponent<AudioSource> ().Stop ();
				Time.timeScale = 0;
			} else {
				GetComponent<AudioSource> ().Play ();
				Time.timeScale = 1;
			}
		}
	}

	void GameOver(){
		if (life == 0) {
			gameOverText.text = "GAME OVER!";
			restartTimer += Time.deltaTime;
			if (restartTimer >= restartDelay) {
				DeleteAll ();
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}
	}

	public void DeleteAll(){
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
			Destroy(o);
		}
	}

	public bool GetDied(){
		return died;
	}

	public void SetDied(bool died){
		this.died = died;
	}
}
