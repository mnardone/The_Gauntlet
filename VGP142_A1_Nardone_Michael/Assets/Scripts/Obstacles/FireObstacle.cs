using UnityEngine;
using System.Collections;

public class FireObstacle : MonoBehaviour {

    GameObject[] fireSpawns;
    public GameObject prefabFlameStrike;

    float fireRate;
    float timeSinceFire;

	// Use this for initialization
	void Start () {

        fireSpawns = GameObject.FindGameObjectsWithTag("Fire Obstacle");
        if (fireSpawns.Length < 1)
            Debug.LogWarning("Fire Obstacle spawns not found");

        //for (int i = 0; i < fireSpawns.Length; ++i)
        //    Debug.Log(fireSpawns[i].name);

        fireRate = 7;
        timeSinceFire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > timeSinceFire + fireRate)
        {
            for (int i = 0; i < fireSpawns.Length; ++i)
            {
                GameObject flamestrike = Instantiate(prefabFlameStrike,
                    fireSpawns[i].transform.position,
                    fireSpawns[i].transform.rotation) as GameObject;
                timeSinceFire = Time.time;
            }
        }
    }
}
