using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

	public Booster boosters;
	public Material[] materials;
	public PhysicMaterial[] physicsMaterials;

	public Transform prefab;
	public float recycleOffset;
	public Vector3 startPosition;
	public int numberOfObjects;
	public Vector3 minScale, maxScale, minGap, maxGap;
	public float minY, maxY;
	
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
		Recycle(true);	//Instantiate first platform as speed up
		for(int i = 1; i < numberOfObjects; i++)
		{
			Recycle(false);
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
			Recycle(false);	//Not first so can randomize
		}
	}
	
	private void Recycle(bool first)
	{
		int materialIndex;
		Vector3 scale = new Vector3(
			Random.Range (minScale.x, maxScale.x),
			Random.Range (minScale.y, maxScale.y),
			Random.Range (minScale.z, maxScale.z));
		Transform obj = transQueue.Dequeue();
		obj.localScale = scale;
		if(first)	//First one is always speed up platform
			materialIndex = 2;	
		else
			materialIndex = Random.Range (0, materials.Length);
		obj.renderer.material = materials[materialIndex];
		obj.collider.material = physicsMaterials[materialIndex];
		Vector3 position = nextPosition;
		position.x += scale.x /2f;
		position.y += scale.y /2f;
		if(first)
			boosters.FirstSpawnIfAvailable(position);
		else
			boosters.SpawnIfAvailable(position);
		obj.localPosition = position;
		nextPosition.x += obj.localScale.x;
		transQueue.Enqueue(obj);

		nextPosition += new Vector3(
			Random.Range(minGap.x, maxGap.x) + scale.x,
			Random.Range(minGap.y, maxGap.y),
			Random.Range(minGap.z, maxGap.z));
		
		if(nextPosition.y < minY){
			nextPosition.y = minY + maxGap.y;
		}
		else if(nextPosition.y > maxY){
			nextPosition.y = maxY - maxGap.y;
		}
	}
}
