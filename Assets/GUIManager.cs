using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public GUIText boostsText, distanceText, gameOver, instructions, title, welcome;
	private static GUIManager instance;
	
	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStarts;
		GameEventManager.GameOver += GameOverbruh;
		gameOver.enabled = false;
	}

	
	void Update () {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
	
	public static void SetBoosts(int boosts){
		instance.boostsText.text = "Boosts: " + boosts.ToString();
	}
	
	public static void SetDistance(float distance){
		instance.distanceText.text = distance.ToString("f0");
	}

	private void GameStarts()
	{
		welcome.enabled = false;
		gameOver.enabled = false;
		instructions.enabled = false;
		title.enabled= false;
		this.enabled = false;	//Update is disabled
	}
	
	
	private void GameOverbruh()
	{
		welcome.enabled = true;
		gameOver.enabled = true;
		instructions.enabled = true;
		title.enabled= true;
		this.enabled = true;	//Update is enabled
	}
}
