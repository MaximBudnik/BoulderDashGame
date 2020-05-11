﻿using System;
using System.Windows.Forms;
using ClassLibrary.InputProcessors;
using ClassLibrary.Entities.Player;

namespace BoulderDashForms {
    public class FormsInputProcessor : InputProcessor  {

        public void ProcessKeyDown(Keys key,Func<Player> getPlayer, Action<int> changeGameStatus) {
            Player player = getPlayer();
            switch (key) {
                case Keys.W:
                    player.Move("vertical", -1);
                    player.IsMoving = true;
                    player.SetAnimation(1);
                    player.Keyboard.W = 6;
                    break;
                case Keys.S:
                    player.Move("vertical", 1);
                    player.SetAnimation(1);
                    player.IsMoving = true;
                    player.Keyboard.S = 6;
                    break;
                case Keys.A:
                    player.Move("horizontal", -1);
                    player.SetAnimation(1);
                    player.IsMoving = true;
                    player.reverse = -1;
                    player.Keyboard.A = 6;
                    break;
                case Keys.D:
                    player.Move("horizontal", 1);
                    player.SetAnimation(1);
                    player.IsMoving = true;
                    player.reverse = 1;
                    player.Keyboard.D = 6;
                    break;
                case Keys.T:
                    player.Teleport();
                    player.Keyboard.T = 6;
                    break;
                case Keys.Space:
                    player.HpInEnergy();
                    player.Keyboard.Space = 6;
                    break;
                case Keys.Q:
                    player.ConvertNearStonesInDiamonds();
                    player.Keyboard.Q = 6;
                    break;
                case Keys.E:
                    player.UseTnt();
                    player.Keyboard.E = 6;
                    break;
                case Keys.R:
                    player.Attack();
                    player.Keyboard.R = 6;
                    break;
                case Keys.Escape:
                    changeGameStatus(0);
                    break;
            }
        }
        
        public void ProcessKeyUp(Keys key,Func<Player> getPlayer) {
            Player player = getPlayer();
            switch (key) {
                case Keys.W:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    player.Keyboard.W = 0;
                    break;
                case Keys.S:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    player.Keyboard.S = 0;
                    break;
                case Keys.A:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    player.Keyboard.A = 0;
                    break;
                case Keys.D:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    player.Keyboard.D = 0;
                    break;
                case Keys.T:
                    player.Keyboard.T = 0;
                    break;
                case Keys.Space:
                    player.Keyboard.Space = 0;
                    break;
                case Keys.Q:
                    player.Keyboard.Q = 0;
                    break;
                case Keys.E:
                    player.Keyboard.E = 0;
                    break;
                case Keys.R:
                    player.Keyboard.R = 0;
                    break;
                case Keys.Escape:
                    break;
            }
        }
        
       
        
        
        
    }
}