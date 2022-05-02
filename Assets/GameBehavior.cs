using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{
    [SerializeField] AudioClip[] _clips;
    private int clipIndex;
    public bool showWinScreen = false;
    public bool showLossScreen = false;
    public string labelText = "Collect all the intel";
    public int maxItems = 36;
    private int _itemsCollected = 0;
    public Player player;
    public string disguised = "No";
    public string boosted = "No";
    public Stack<string> lootStack = new Stack<string>();
    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;
    private string _state;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            Debug.LogFormat("Items: {0}", _itemsCollected);
            if (_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                showWinScreen = true;
                Time.timeScale = 0f;
            }
            else
            {
                labelText = "Intel found, " + (maxItems - _itemsCollected) + " more to go.";
            }
        }
    }

    private int _playerHP = 3;
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            if (_playerHP <= 0)
            {
                labelText = "You want another life with that?";
                showLossScreen = true;
                Time.timeScale = 0;
                clipIndex = 0;
                AudioClip clip = _clips[clipIndex];
                GetComponent<AudioSource>().PlayOneShot(clip);
            }
            else
            {
                labelText = "Ouch... that's gotta hurt.";
            }
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }

    private int _playerAmmo = 20;
    public int ammo
    {
        get { return _playerAmmo; }
        set
        {
            _playerAmmo = value;
            Debug.LogFormat("Ammo: {0}", _playerAmmo);
        }
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        _state.FancyDebug();
        debug(_state);
        LogWithDelegate(debug);
        GameObject player = GameObject.Find("Player");
        Player playerBehavior = player.GetComponent<Player>();
        playerBehavior.playerJump += HandlePlayerJump;
        Debug.Log(_state);
    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public static void Print(string newText)
    {
        Debug.Log(newText);
    }

    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating the debug task...");
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health:" + _playerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Ammo:" + _playerAmmo);
        GUI.Box(new Rect(20, 80, 150, 25), "Items Collected: " + _itemsCollected);
        GUI.Box(new Rect(20, 110, 150, 25), "Disguised: " + disguised);
        GUI.Box(new Rect(20, 140, 150, 25), "Boosted: " + boosted);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
        if (showWinScreen)
        {
           WinLevel();
        }
        if (showLossScreen)
        {
            LoseLevel();
        }
    }
    void WinLevel()
    {
        SceneManager.LoadScene("GameWin");
    }
    void LoseLevel()
    {
        SceneManager.LoadScene("GameLose");
    }
}
