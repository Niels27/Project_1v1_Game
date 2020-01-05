using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    RoomInfo[] rooms;


    public GameObject batttleButton;
    public GameObject cancelButton;
    public GameObject optionsButton;
    public GameObject backButton;
    public GameObject optionsMenu;

    private void Awake()
    {
        lobby = this; //Creates the singleton, lives within the Main Menu Scene.
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server.
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected tot the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        batttleButton.SetActive(true); // Player is now connected to servers, enables battlebutton to allow join a game.
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle Button was clicked");
        PhotonNetwork.JoinRandomRoom();
        batttleButton.SetActive(false);
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

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Button was clicked");
        cancelButton.SetActive(false);
        batttleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
