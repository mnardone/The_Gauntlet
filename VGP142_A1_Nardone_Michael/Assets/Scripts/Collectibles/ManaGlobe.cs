using UnityEngine;
using System.Collections;

public class ManaGlobe : MonoBehaviour {

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.instance.mana += 50;
        }
    }
}
