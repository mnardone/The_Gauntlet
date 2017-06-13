using UnityEngine;
using UnityEngine.UI;
public class Pause : MonoBehaviour {

    GameManager gm;

    Button restart;
    Button menu;

	// Use this for initialization
	void Start () {

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!gm)
            Debug.LogWarning("GameManager not found");

        restart = GameObject.Find("Restart Button").GetComponent<Button>();
        if (!restart)
            Debug.LogWarning("Pause - Restart button not found");

        menu = GameObject.Find("Menu Button").GetComponent<Button>();
        if (!menu)
            Debug.LogWarning("Pause - Menu button not found");

        if (gm)
        {
            if (restart)
                restart.onClick.AddListener(gm.LoadLevel);

            if (menu)
                menu.onClick.AddListener(gm.LoadTitle);
        }
	}
}
