using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AvatarCombat : NetworkBehaviour
{
    private AvatarSetup _avatarSetup;
    public Transform RayOrigin;

    public Text HealthDisplay;


    // Start is called before the first frame update
    void Start()
    {
        _avatarSetup = GetComponent<AvatarSetup>();
        HealthDisplay = GameSetup.GS.healthDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            CmdShoot();
        }
        
        HealthDisplay.text = _avatarSetup.PlayerHealth.ToString();
    }

    [Command]
    void CmdShoot()
    {
        RaycastHit hit;
            if (Physics.Raycast(RayOrigin.position, RayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
            {
                Debug.DrawRay(RayOrigin.position, RayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("hit");
                if(hit.transform.tag == "Avatar")
                {
                    hit.transform.gameObject.GetComponent<AvatarSetup>().PlayerHealth -= _avatarSetup.PlayerDamage;
                }
            }
            else
            {
                Debug.DrawRay(RayOrigin.position, RayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("no hit");
            }
    }
}
