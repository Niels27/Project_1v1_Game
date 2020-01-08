using Aws.GameLift.Server;
using Mirror;
using UnityEngine;

public class GameLiftAuthenticator : NetworkAuthenticator
{
    public class AuthRequestMessage : MessageBase
    {
        public string GameLiftPlayerSessionId;
    }

    public class AuthResponseMessage : MessageBase
    {
        public byte Code;
        public string Message;
    }

    public class GameLiftAuthenticationData
    {
        public string GameLiftPlayerSessionId;
    }

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
    }

    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
    }

    public override void OnServerAuthenticate(NetworkConnection conn)
    {
        // Wait for AuthRequestMessage from client.
    }

    public override void OnClientAuthenticate(NetworkConnection conn)
    {
        AuthRequestMessage authRequestMessage = new AuthRequestMessage
        {
            GameLiftPlayerSessionId = GameLiftRoomNetworkManager.PlayerSessionId
        };

        NetworkClient.Send(authRequestMessage);
    }

    public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
    {
        Debug.LogFormat("Authentication Request: {0}", msg.GameLiftPlayerSessionId);

        var acceptPlayerSessionOutcome = GameLiftServerAPI.AcceptPlayerSession(msg.GameLiftPlayerSessionId);

        if (acceptPlayerSessionOutcome.Success)
        {
            AuthResponseMessage authResponseMessage = new AuthResponseMessage()
            {
                Code = 100,
                Message = "Success"
            };
            conn.authenticationData = new GameLiftAuthenticationData()
            {
                GameLiftPlayerSessionId = msg.GameLiftPlayerSessionId
            };

            conn.Send(authResponseMessage);

            // Let Mirror know authentication was successful.
            base.OnServerAuthenticated.Invoke(conn);
        }
        else
        {
            AuthResponseMessage authResponseMessage = new AuthResponseMessage()
            {
                Code = 200,
                Message = "Invalid Player Session Id"
            };

            conn.Send(authResponseMessage);

            conn.isAuthenticated = false;

            Invoke(nameof(conn.Disconnect), 1);
        }
    }

    public void OnAuthResponseMessage(NetworkConnection conn, AuthResponseMessage msg)
    {
        if (msg.Code == 100)
        {
            Debug.LogFormat("Authentication Response: {0}", msg.Message);

            base.OnClientAuthenticated.Invoke(conn);
        }
        else
        {
            Debug.LogErrorFormat("Authentication Response: {0}", msg.Message);

            conn.isAuthenticated = false;

            conn.Disconnect();
        }
    }
}
