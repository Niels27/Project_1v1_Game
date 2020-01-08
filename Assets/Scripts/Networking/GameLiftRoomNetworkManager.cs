using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Aws.GameLift.Server;
using Aws.GameLift;
using Mirror;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLiftRoomNetworkManager : NetworkRoomManager
{
    [Header("AWS GameLift")]
    public string GameServiceUrl;

    [HideInInspector]
    public static string PlayerSessionId = "";

    public class PlayerSessionInformation
    {
        public string PlayerSessionId;
        public string IpAddress;
        public string Port;
    }

    public class CreateCharacterMessage : MessageBase
    {
        public int ChosenCharacter;
    }

    public override void Start()
    {
        if (isHeadless && startOnHeadless)
        {
            StartServer();
        }
    }

    public void StartClientOnDemand()
    {
        print("Looking for match..");

        var request = WebRequest.Create(GameServiceUrl + "/getMatch/" + System.Guid.NewGuid());
        request.Method = "GET";

        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        if (response?.StatusCode != HttpStatusCode.OK)
        {
            print("Failed to fetch match details.");
        }

        PlayerSessionInformation playerSessionInformation;

        using (var responseReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        {
            string responseBody = responseReader.ReadToEnd();

            playerSessionInformation = JsonUtility.FromJson<PlayerSessionInformation>(responseBody);
        }

        PlayerSessionId = playerSessionInformation.PlayerSessionId;

        var serverUri = new Uri("tcp4://" + playerSessionInformation.IpAddress + ":" + playerSessionInformation.Port);

        StartClient(serverUri);
    }

    public override void OnApplicationQuit()
    {
        GameLiftServerAPI.ProcessEnding();
        GameLiftServerAPI.Destroy();

        base.OnApplicationQuit();
    }

    // Start is called before the first frame update
    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();

        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);

        var initSDKOutcome = GameLiftServerAPI.InitSDK();
        if (initSDKOutcome.Success)
        {
            ProcessParameters processParameters = new ProcessParameters(
                (gameSession) =>
                {
                    GameLiftServerAPI.ActivateGameSession();
                },
                () =>
                {
                    Application.Quit();
                },
                () =>
                {
                    return true;
                },
                7777,
                new LogParameters(new List<string>())
            );

            var processReadyOutcome = GameLiftServerAPI.ProcessReady(processParameters);
            if (processReadyOutcome.Success)
            {
                print("Connected to AWS GameLift.");
            }
            else
            {
                print("Failed to connect to AWS GameLift: " + processReadyOutcome.Error.ToString());
            }
        }
        else
        {
            print("InitSDK failed: " + initSDKOutcome.Error.ToString());
        }
    }

    // On player disconnect (server-side)
    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        base.OnRoomServerDisconnect(conn);

        string gameLiftPlayerSessionId = (conn.authenticationData as GameLiftAuthenticator.GameLiftAuthenticationData)?.GameLiftPlayerSessionId;
        
        GameLiftServerAPI.RemovePlayerSession(gameLiftPlayerSessionId);

        // We don't want the server to stop when a player has been denied.
        if (conn.isAuthenticated && numPlayers < 2)
        {
            Application.Quit();
        }
    }

    // On player connect (client-side)
    public override void OnRoomClientConnect(NetworkConnection conn)
    {
        base.OnRoomClientConnect(conn);

        var msg = new CreateCharacterMessage()
        {
            ChosenCharacter = PlayerInfo.PI.mySelectedCharacter
        };

        conn.Send(msg);
    }

    // On scene change (client)
    public override void OnRoomClientSceneChanged(NetworkConnection conn)
    {
        base.OnRoomClientSceneChanged(conn);

        if (conn.identity != null &&  conn.identity.isLocalPlayer)
        {
            GameObject clientObject = conn.identity.gameObject;

            clientObject.GetComponent<PauseMenu>().pauseMenu = GameSetup.GS.pauseMenu;
            clientObject.GetComponent<AvatarCombat>().HealthDisplay = GameSetup.GS.healthDisplay;
        }
    }

    public void OnCreateCharacter(NetworkConnection conn, CreateCharacterMessage msg)
    {
        int spawnPointInd = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        Transform spawnPoint = GameSetup.GS.spawnPoints[spawnPointInd];

        GameObject playerObject = Instantiate(Resources.Load(Path.Combine("Prefabs", "Player"), typeof(GameObject)),
            spawnPoint.position, spawnPoint.rotation) as GameObject;
        AvatarSetup playerSetup = playerObject.GetComponent<AvatarSetup>();

        playerSetup.CharacterValue = msg.ChosenCharacter;

        NetworkServer.AddPlayerForConnection(conn, playerObject);
    }
}
