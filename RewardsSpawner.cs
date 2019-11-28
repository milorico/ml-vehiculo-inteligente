using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RewardsSpawner : MonoBehaviour
{
	public float range = 100;
	public bool teleport = true;
	public Transform map;
	public GameObject box;
	public float minDistance = 1;
	public float cantidad = 30;

	public bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, minDistance, NavMesh.AllAreas))
			{
				result = hit.position;
				print ("encontre punto");
				return true;
			}

		}
		print ("no encontre punto");
		result = Vector3.zero;
		return false;
	}

    // Start is called before the first frame update
    void Start()
    {
		for (int i = 0; i < 10; i++) {
			CreateTargets ();
			range = range*2;
			minDistance = minDistance*5;
			cantidad = cantidad + 30;
		}
    }
	void CreateTargets(){
		Vector3 point;
		for (int i = 0; i < cantidad; i++) {
			if (RandomPoint (map.position, range, out point)) {
				Instantiate (box, point + Vector3.up, Quaternion.identity);
			}
		}
	}

    // Update is called once per frame
	
}
