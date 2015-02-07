using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public float gameOverY;
	public float acceleration;
	public static float distanceTraveled = 0f;
	public Vector3 jumpVel, boostVel;
	private bool touchingPlatform = false;
	public static int boosts;

	private Vector3 startPosition;
	
	void Start () {
		GameEventManager.GameStart += GameStartRunner;
		GameEventManager.GameOver += GameOverRunner;
		startPosition = transform.localPosition;
		renderer.enabled = false;
		rigidbody.isKinematic = true;
		enabled = false;
	}

	public static void AddBoost()
	{
		boosts++;
		GUIManager.SetBoosts(boosts);
	}
	private void GameStartRunner () {
		boosts = 0;
		GUIManager.SetBoosts(boosts);
		distanceTraveled = 0f;
		GUIManager.SetDistance(distanceTraveled);
		transform.localPosition = startPosition;
		renderer.enabled = true;
		rigidbody.isKinematic = false;
		enabled = true;
	}
	
	private void GameOverRunner () {
		renderer.enabled = false;
		rigidbody.isKinematic = true;
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump")){
			if(touchingPlatform)
			{
			rigidbody.AddForce(jumpVel,ForceMode.VelocityChange);
			touchingPlatform = false;
			}
			else if(boosts > 0)
			{
				rigidbody.AddForce(boostVel,ForceMode.VelocityChange);
				boosts -= 1;
				GUIManager.SetBoosts(boosts);
			}
		}
		GUIManager.SetDistance(distanceTraveled);
		if(transform.localPosition.y < gameOverY){
			GameEventManager.TriggerGameOver();
		}

		distanceTraveled = transform.localPosition.x;
	}

	void FixedUpdate ()
	{
		if(touchingPlatform)
			rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
	}

	void OnCollisionEnter () {
		touchingPlatform = true;
	}
	
	void OnCollisionExit () {
		touchingPlatform = false;
	}
}
