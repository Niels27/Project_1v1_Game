using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AvatarSetup : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetCharacter))]
    public int CharacterValue;

    public GameObject Character;

    public GameObject GameOver;

    [SyncVar]
    public int PlayerHealth;

    [SyncVar]
    public int PlayerDamage;

    public Camera Camera;
    public AudioListener AL;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(Camera);
            Destroy(AL);
        }
        GameOver = GameSetup.GS.gameOver;
    }

    void Update()
    {
        if (isLocalPlayer && PlayerHealth <= 0)
        {
            GameOver.SetActive(true);
        }
    }

    public void SetCharacter(int characterValue)
    {
        Character = Instantiate(PlayerInfo.PI.allCharacters[characterValue], transform.position, transform.rotation, transform);
    }
}