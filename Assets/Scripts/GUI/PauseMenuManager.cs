using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {

    public GameObject DarkenScreen;
    public Text Resume;
    public Text Restart;
    public Text Quit;
    
    int menuItems = 3;
    int menuCounter = 0;

    Color Selected = new Color(1f, 1f, 1f, 1f);
    Color Grayout = new Color(0.5f, 0.5f, 0.5f, 1f);

    void OnEnable() {
        DarkenScreen.SetActive(true);
        menuCounter = 0;
        ActivateText();
        GrayOutMenuItems();
        Time.timeScale = 0f;
    }

    void Update() {
        InputDevice device = InputManager.ActiveDevice;

        if (device.Action1.WasReleased) {
            // Select was pressed
            switch (menuCounter) {
                case 0:
                    // Resume
                    gameObject.SetActive(false);
                    break;
                case 1:
                    // Restart - Just reload this level
                    GameManager.Instance.LoadState(GameManager.Instance.ActiveState);
                    break;
                case 2:
                    GameManager.Instance.LoadState("main_menu");
                    // Quit to main menu
                    break;
                default:
                    Debug.LogError("Error on pause menu - Out of menu select range");
                    break;
            }
        }
        if (device.Action2.WasReleased) {
            // Resume
            gameObject.SetActive(false);
        }

        if (device.Direction.Up.WasPressed) {
            menuCounter--;
            if (menuCounter < 0) {
                menuCounter = menuItems;
            }
        } else if (device.Direction.Down.WasPressed) {
            menuCounter++;
            if (menuCounter >= menuItems) {
                menuCounter = 0;
            }
        }

        switch (menuCounter) {
            case 0:
                // Resume
                GrayOutMenuItems();
                Resume.color = Selected;
                break;
            case 1:
                // Restart
                GrayOutMenuItems();
                Restart.color = Selected;
                break;
            case 2:
                // Quit
                GrayOutMenuItems();
                Quit.color = Selected;
                break;
            default:
                Debug.LogError("Error on pause menu - Out of menu select range - Movement");
                break;
        }
	}

    void OnDisable() {
        DarkenScreen.SetActive(false);
        DisableText();
        UnpausePlayers();
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>().UnPauseMatch();
    }

    void GrayOutMenuItems() {
        Resume.color = Grayout;
        Restart.color = Grayout;
        Quit.color = Grayout;
    }

    void ActivateText() {
        Resume.transform.gameObject.SetActive(true);
        Restart.transform.gameObject.SetActive(true);
        Quit.transform.gameObject.SetActive(true);
    }

    void DisableText() {
        Resume.transform.gameObject.SetActive(false);
        Restart.transform.gameObject.SetActive(false);
        Quit.transform.gameObject.SetActive(false);
    }

    void UnpausePlayers() {
        GameObject[] tempPlayerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in tempPlayerList) {
            if (g.GetComponent<PlayerManager>().playerState == PlayerManager.PlayerState.Paused) {
                g.GetComponent<PlayerManager>().playerState = PlayerManager.PlayerState.Alive;
            }
        }
    }
}
