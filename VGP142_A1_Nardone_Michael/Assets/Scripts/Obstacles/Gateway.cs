using UnityEngine;
using System.Collections;

public class Gateway : MonoBehaviour {

    public Transform door;
    public bool open;

	// Use this for initialization
	void Start () {

        door = GetComponent<Transform>();
        if (!door)
            Debug.Log(name + " transform not found.");

        open = false;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (open)
        {
            if (door.transform.localEulerAngles.y < 120)
            {
                door.transform.Rotate(0, 1, 0);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        // if player has key
        if (other.gameObject.tag == "Player")
        {
            if (GameManager.instance.keys > 0)
            {
                open = true;
                GameManager.instance.keys--;
            }
        }
    }
}
