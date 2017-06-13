using UnityEngine;
using System.Collections;

public class HealthGlobe : MonoBehaviour {

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.instance.health += 20;
        }
    }
}
