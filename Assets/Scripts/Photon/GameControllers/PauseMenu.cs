using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private PhotonView PV;

    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        pauseMenu = GameSetup.GS.pauseMenu;
    }

    // Update is called once per frame
    void Update()
    {
        MenuButton();
    }

    void MenuButton()
    {
        if (pauseMenu.active && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(false);
        }
        else if (!pauseMenu.active && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
    }
}
