using UnityEngine;
using System.Collections;

public class EnemyTurret : MonoBehaviour {

    float projectileDamage;
    float projectileForce;
    float projectileFireRate;
    float timeSinceFire;
    Transform projectileSpawn;

    public GameObject projectile;

	// Use this for initialization
	void Start () {

        projectileDamage = 10;
        projectileForce = 50;
        projectileFireRate = 8;
        timeSinceFire = 0;

        projectileSpawn = this.transform.FindChild("ProjectileSpawn").gameObject.transform;
        if (!projectileSpawn)
            Debug.Log(this.name + " did not find projectile spawn transform.");
	
	}

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            //Debug.Log("Player is in firing zone");
            Fire(c.gameObject.transform.position);
        }
    }

    private void Fire(Vector3 target)
    {
        if (Time.time > projectileFireRate + timeSinceFire)
        {
            projectileSpawn.LookAt(target);
            GameObject fireball = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation) as GameObject;
            fireball.GetComponentInChildren<Rigidbody>().velocity = Vector3.forward * projectileForce;
            timeSinceFire = Time.time;
        }
    }
}
