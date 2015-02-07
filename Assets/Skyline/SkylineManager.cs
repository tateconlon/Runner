using UnityEngine;
using System.Collections.Generic;

public class SkylineManager : MonoBehaviour {

	public Transform prefab;
	public float recycleOffset;
	public Vector3 startPosition;
	public int numberOfObjects;
	public Vector3 minScale, maxScale;

	private Queue<Transform> transQueue;
	private Vector3 nextPosition;
	// Use this for initialization
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		transQueue = new Queue<Transform>(numberOfObjects);
		for (int i = 0; i < numberOfObjects; i++) {
			transQueue.Enqueue((Transform)Instantiate(prefab,
			                                          new Vector3(0f, 0f, -100f), Quaternion.identity));
		}
		enabled = false;
	}
	
	private void GameStart()
	{
		nextPosition = startPosition;
		for(int i = 0; i < numberOfObjects; i++)
		{
			Recycle();
		}
		enabled = true;
	}
	
	private void GameOver()
	{
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(transQueue.Peek().localPosition.x + recycleOffset < Runner.distanceTraveled)
		{
			Recycle();
		}
	}

	private void Recycle()
	{
		Vector3 scale = new Vector3(
			Random.Range (minScale.x, maxScale.x),
			Random.Range (minScale.y, maxScale.y),
			Random.Range (minScale.z, maxScale.z));
		Transform obj = transQueue.Dequeue();
		obj.localScale = scale;
		Vector3 position = nextPosition;
		position.x += scale.x /2f;
		position.y += scale.y /2f;
		obj.localPosition = position;
		nextPosition.x += obj.localScale.x;
		transQueue.Enqueue(obj);
	}
}
