using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInput
{
    public class PlayerControl
    {
        //agregado variable player
        public Player player;


        //cambio de Input a KeyCode

        public KeyCode usePotion = KeyCode.E;
        KeyCode interactKey = KeyCode.F;
        //public int attack = 0;
        //public int block = 1;
        public KeyCode kick = KeyCode.V;
        public KeyCode sprint = KeyCode.LeftShift;
        public KeyCode pickUp = KeyCode.LeftControl;



        public PlayerControl(Player newPlayer)
        {
            player = newPlayer;
        }

        public void FakeUpdate()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
                player._playerMovemente(horizontal, vertical);

            #region Debug Set Weapon
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    player.SetWeapon(Weapon.WeaponType.Sword);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    player.SetWeapon(Weapon.WeaponType.SwordAndShield);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    player.SetWeapon(Weapon.WeaponType.GreatSword);
            //} 
            #endregion            

            if (Input.GetKeyDown(usePotion))
            {               
                player.UsePotion();
            }
            if (Input.GetMouseButtonDown(0) && !player.outOfBreath)
            {
                player._myAttack();
                Debug.Log($"<color=red> Player Attack </color>");
            }
            if (Input.GetMouseButtonDown(1))
            {
                //player._myBlock();
                Debug.Log($"<color=blue> Player Guard Up </color>");
            }
            if (Input.GetMouseButtonUp(1))
            {
                //player.EndBlock()
                Debug.Log($"<color=blue> Player Guard Down </color>");
            }
            if (Input.GetKeyDown(kick))
            {
                player.Kick();
                //Debug.Log($"<color=orange> Player Kick </color>");
            }
            if (Input.GetKeyDown(sprint))
            {
                player.RunningState(true);
                Debug.Log($"<color=purple> Player Running </color>");
            }
            if (Input.GetKeyUp(sprint))
            {
                player.RunningState(false);
                Debug.Log($"<color=purple> Player Walking </color>");
            }            
            if (Input.GetKeyUp(pickUp))
            {
                player.CheckPickUp();
            }
            if (Input.GetKeyDown(interactKey))
            {
                player.TryInteract();
            }
        }
    }
}

