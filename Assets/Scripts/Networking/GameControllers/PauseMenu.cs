using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GameSetup.GS.pauseMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu != null)
        {
            // Guard for scene switch, when the pause menu momentarily disappears.
            MenuButton();
        }
    }

    void MenuButton()
    {
        if (pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(false);
        }
        else if (!pauseMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
    }
}
