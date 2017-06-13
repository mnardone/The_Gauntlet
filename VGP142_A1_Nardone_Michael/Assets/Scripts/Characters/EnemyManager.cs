using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour {

    public GameObject enemyPatrolPrefab;

    public GameObject[] patrolPath;

    public GameObject[] patrolPathNodes;

	// Use this for initialization
	void Start () {

        patrolPath = GameObject.FindGameObjectsWithTag("Patrol Path");

        for (int i = 0; i < patrolPath.Length; ++i)
        {
            // instantiate enemy patrols
            GameObject enemy = Instantiate(enemyPatrolPrefab,
                patrolPath[i].transform.position,
                patrolPath[i].transform.rotation) as GameObject;
            // pass patrol path info
            patrolPathNodes = GameObject.FindGameObjectsWithTag("Patrol " + patrolPath[i].name[16]);    // PathNode_Patrol01 --> Patrol 1 tag
            enemy.GetComponent<EnemyPatrol>().path = patrolPathNodes;
        }
	}
}
