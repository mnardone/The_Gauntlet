using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    static GameManager _instance = null;

    public GameObject playerPrefab;

    bool pause;
    Canvas pauseScreen;

    RectTransform healthBar;
    RectTransform manaBar;
    Text keysText;

    int _health;
    int _mana;
    int _keys;

	// Use this for initialization
	void Start () {

        if (_instance)
            DestroyImmediate(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        pause = false;

	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel()
    {
        Time.timeScale = 1;     // If this was reached via Pause menu, this must be reset to 1
        SceneManager.LoadScene(1);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadVictory()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Debug.LogWarning("Quitting game.");
        Application.Quit();
    }

    public void spawnPlayer()
    {
        Transform spawnPoint = GameObject.Find("Spawn").GetComponent<Transform>();
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            // Disable Pause Canvas
            pauseScreen = GameObject.Find("Pause Canvas").GetComponent<Canvas>();
            pauseScreen.gameObject.SetActive(false);
            // Reference HUD elements - Health Bar, Mana Bar, Keys
            healthBar = GameObject.Find("HealthBarForeground").GetComponent<RectTransform>();
            if (!healthBar)
                Debug.LogWarning("Health Bar RectTransform not found");

            manaBar = GameObject.Find("ManaBarForeground").GetComponent<RectTransform>();
            if (!manaBar)
                Debug.LogWarning("Mana Bar RectTransform not found");

            keysText = GameObject.Find("KeysText").GetComponent<Text>();
            if (!keysText)
                Debug.LogWarning("Keys Text not found");

            spawnPlayer();

            health = 100;
            mana = 100;
            keys = 0;
        }
    }

    private void PauseGame()
    {
        pause = !pause;

        if(pause)
        {
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseScreen.gameObject.SetActive(false);
        }
    }

    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public int health
    {
        get { return _health; }
        set { _health = value;
            if (_health > 100)
            {
                _health = 100; 
            }
            updateHealthBar(); }
    }

    public int mana
    {
        get { return _mana; }
        set { _mana = value; updateManaBar(); }
    }

    public int keys
    {
        get { return _keys; }
        set { _keys = value; updateKeysText(); }
    }

    void updateHealthBar()
    {
        healthBar.sizeDelta = new Vector2(health * 4, healthBar.sizeDelta.y);
    }

    void updateManaBar()
    {
        manaBar.sizeDelta = new Vector2(mana * 4, manaBar.sizeDelta.y);
    }

    void updateKeysText()
    {
        keysText.text = "" + keys;
    }
}
