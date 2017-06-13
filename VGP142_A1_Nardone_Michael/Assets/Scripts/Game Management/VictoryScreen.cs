using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour {

    GameManager gm;

    Button menu;

    // Use this for initialization
    void Start()
    {

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!gm)
            Debug.LogWarning("GameManager not found");

        menu = GameObject.Find("Menu Button").GetComponent<Button>();
        if (!menu)
            Debug.LogWarning("Victory - Menu button not found");

        if (gm)
            if (menu)
                menu.onClick.AddListener(gm.LoadTitle);
    }
}