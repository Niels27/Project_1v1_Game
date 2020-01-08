using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject battleButton;
    public GameObject cancelButton;
    public GameObject optionsButton;
    public GameObject backButton;
    public GameObject optionsMenu;

    public void Start()
    {
        battleButton.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle Button was clicked");

        (NetworkManager.singleton as GameLiftRoomNetworkManager).StartClientOnDemand();

        battleButton.SetActive(false);
        cancelButton.SetActive(true);
    }
    public void OnOptionsButtonClicked()
    {
        optionsMenu.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        optionsMenu.SetActive(false);
    }

    public void OnClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Button was clicked");

        cancelButton.SetActive(false);
        battleButton.SetActive(true);
    }
}
