using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class Abilities : MonoBehaviour
    {
        public GameObject ThirdPersonController;
        public GameObject Camera;
        public GameObject Shield;
        public ThirdPersonCharacter herman;
        private static System.Timers.Timer aTimer;
        public float DashCooldown;
        public float SpeedCooldown;
        public float ShieldCooldown;
        public float SpeedDuration;
        public float DashDuration;
        bool AllowDoubleJump=true;
        bool Toggle=true;
        bool dash;
        bool boost;
        bool shield;
        void JumpAgain()
        {
                herman.m_Rigidbody.velocity = herman.m_Rigidbody.velocity + new Vector3(0, 6, 0);
                herman.m_IsGrounded = false;
                herman.m_Animator.applyRootMotion = false;
                herman.m_GroundCheckDistance = 0.1f;
        }
      

        // Update is called once per frame
        void Update()

        {
            herman = GetComponent<ThirdPersonCharacter>();
            
            //speed boost abilitiy
             boost = Input.GetKeyDown(KeyCode.E);
            if (boost && SpeedCooldown >= 5f)
            {
                SpeedDuration = 3;

            }
            if (SpeedDuration > 0)
            {
                herman.m_MoveSpeedMultiplier = 3f;
                SpeedDuration -= Time.deltaTime;
                SpeedCooldown = 0f;
            }
            else
            {
                //herman.m_MoveSpeedMultiplier = 1.5f;
                SpeedDuration = 0f;
            }

            //dash ability
            dash = Input.GetKeyDown(KeyCode.R);
            if (dash && DashCooldown >= 5f) 
            {
                DashDuration = 0.1f; 
            }
            if (DashDuration > 0)
            {  
                herman.m_MoveSpeedMultiplier = 40; 
                DashDuration -= Time.deltaTime;
                DashCooldown = 0f; 
            }
            else
            {
                //herman.m_MoveSpeedMultiplier = 1.5f;
                DashDuration = 0f;
            }


            
            Toggle = Input.GetKeyDown(KeyCode.T) ? !Toggle : Toggle;
            //Toggle = true;
            bool Jump = Input.GetKeyDown(KeyCode.Space);
            if (Jump && Toggle && AllowDoubleJump && !herman.m_IsGrounded)
            {
               
                
                    JumpAgain();
                    AllowDoubleJump = false;
                
            }
            if (herman.m_IsGrounded)
            {
                AllowDoubleJump = true;
            }
         
            shield = Input.GetKeyDown(KeyCode.Q);
            if (shield && ShieldCooldown>5)
            {
                Shield.SetActive(true);
                ShieldCooldown = 0;
            }
            if(ShieldCooldown>2)
            {
                Shield.SetActive(false);
            }
          
            DashCooldown += Time.deltaTime;
            SpeedCooldown += Time.deltaTime;
            ShieldCooldown += Time.deltaTime;
        }
        // Start is called before the first frame update
        void Start()
        {
            Shield.SetActive(false);
        }
    }
}