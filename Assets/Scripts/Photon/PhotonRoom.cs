using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    //public bool isGameLoaded;
    public int currentScene;
    public int multiplayScene;

    //Player info
    //Player[] photonPlayers;
    //public int playersInRoom;
    //public int myNumberInRoom;

    //public int playersInGame;

    //Delayed start
    //private bool readyToCount;
    //private bool readyToStart;
    //public float startingTime;
    //private float lessThanMaxPlayers;
    //private float atMaxPlayers;
    //private float timeToStart;

    private void Awake()
    {
        //set up singleton
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        //PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start()
    {
        //set private variables
        PV = GetComponent<PhotonView>();
        //readyToCount = false;
        //readyToStart = false;
        //lessThanMaxPlayers = startingTime;
        //atMaxPlayers = 6;
        //timeToStart = startingTime;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if(currentScene == multiplayScene)
        {
            //isGameLoaded = true;
            //for delay start game
            //if9MultiplayerSetting.mulitplayerSetting.delayStart)
            //{
            //  PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            //}
            //for non delay start game
            //else
            {
                CreatePlayer();
            }

        }
    }

    void Update()
    {
        //for delay start only, count down to start
        //if(Multiplayer.Setting.mulitplayerSetting.delayStart)
        //{
        //  if(playersInRoom == 1)
        //  {
        //      RestartTimer();
        //  }
        //  if(!isGameLoaded)
        //  {
        //      if(readyToStart)
        //      {
        //          atMaxPlayers -= Time.deltaTime;
        //          lessThanMaxPlayers = atMaxPlayers;
        //          timeToStart = atMaxPlayers;
        //      }
        //      else if(readyToCount)
        //      {
        //          lessThanMaxPlayers -= Time.deltaTime;
        //          timeToStart = lessThanMaxPlayers;
        //      }
        //      Debug.Log("Display time to start to the players " + timeToStart);
        //      if(timeToStart<= 0)
        //      {
        //          StartGame();
        //      }
        //  }
        //}
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");
        //photonPlayers = PhotonNetwork.PlayerList;
        //playersInRoom = photonPlayers.Length;
        //myNumberInRoom = playersInRoom;
        //PhotonNetwork.NickName = myNumberInRoom.ToString();


        {
            StartGame();
        }
    }

    void StartGame()
    {
        //loads the multiplayer scene for all players
        //isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        //if(MultiplayerSetting.multiplayerSetting.delayStart)
        //{
        //  PhotonNetwork.CurrentRoom.IsOpen = false;
        //}
        PhotonNetwork.LoadLevel(multiplayScene);
    }

    //[PunRPC]
    private void CreatePlayer()
    {
        //creates player network controller but not player character
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }

}
