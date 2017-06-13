using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    GameManager gm;

    Canvas instructionScreen;

    Button start;
    Button quit;
    Button instruction;
    Button menu;

	// Use this for initialization
	void Start () {

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!gm)
            Debug.LogWarning("GameManager not found");

        start = GameObject.Find("Start Button").GetComponent<Button>();
        if (!start)
            Debug.LogWarning("Title - Start button not found");

        quit = GameObject.Find("Quit Button").GetComponent<Button>();
        if (!quit)
            Debug.LogWarning("Title - Quit button not found");

        instruction = GameObject.Find("Instruction Button").GetComponent<Button>();
        if (!instruction)
            Debug.LogWarning("Title - Instruction button not found");

        menu = GameObject.Find("Menu Button").GetComponent<Button>();
        if (!menu)
            Debug.LogWarning("Instructions - Menu button not found");

        if (gm)
        {
            if (start)
                start.onClick.AddListener(gm.LoadLevel);

            if (quit)
                quit.onClick.AddListener(gm.QuitGame);

            if (menu)
                menu.onClick.AddListener(toggleInstruction);

            if (instruction)
                instruction.onClick.AddListener(toggleInstruction);
        }

        instructionScreen = GameObject.Find("InstructionScreen").GetComponent<Canvas>();
        instructionScreen.gameObject.SetActive(false);
    }

    void toggleInstruction()
    {
        if (!instructionScreen.gameObject.activeSelf)
            instructionScreen.gameObject.SetActive(true);
        else
            instructionScreen.gameObject.SetActive(false);
    }
}
