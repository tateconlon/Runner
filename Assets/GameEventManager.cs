using UnityEngine;
using System.Collections;

public class GameEventManager : MonoBehaviour {

	public delegate void GameEvent();

	public static event GameEvent GameStart, GameOver;

	public static void TriggerGameStart(){
		if(GameStart != null){
			Debug.Log("Trigger Start");
			GameStart();
		}
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}
	
	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
		}
	}

}
