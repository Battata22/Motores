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
        public int attack = 0;
        public int block = 1;
        public KeyCode kick = KeyCode.V;
        public KeyCode sprint = KeyCode.LeftShift;



        public PlayerControl(Player newPlayer)
        {
            player = newPlayer;
        }

        public void FakeUpdate()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
                player.Movement(horizontal, vertical);

            if (Input.GetKeyDown(usePotion))
            {
                Debug.Log($"<color=green> Player Use Potion </color>");
                player.UsePotion();
            }
            if (Input.GetMouseButtonDown(0))
            {
                //player.Attack();
                Debug.Log($"<color=red> Player Attack </color>");
            }
            if (Input.GetMouseButtonDown(1))
            {
                //player.Block()
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
                Debug.Log($"<color=orange> Player Kick </color>");
            }
            if (Input.GetKeyDown(sprint))
            {
                player.running = true;
                Debug.Log($"<color=purple> Player Running </color>");
            }
            if (Input.GetKeyUp(sprint))
            {
                player.running = false;
                Debug.Log($"<color=purple> Player Walking </color>");
            }
        }
    }
}

