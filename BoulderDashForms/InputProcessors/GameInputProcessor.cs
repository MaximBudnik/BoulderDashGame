using System;
using System.Windows.Forms;
using ClassLibrary.Entities.Player;
using ClassLibrary.InputProcessors;

namespace BoulderDashForms.InputProcessors {
    public class GameInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Func<Player> getPlayer, Action<int> changeGameStatus, Action<float> changeVolume,
            Action<string> playSound
            ) {
            var player = getPlayer();
            switch (key) {
                case Keys.W:
                    playSound("walk");
                    player.Move("vertical", -1);
                    player.SetAnimation(1);
                    player.Keyboard.W = 6;
                    break;
                case Keys.S:
                    playSound("walk");
                    player.Move("vertical", 1);
                    player.SetAnimation(1);
                    player.Keyboard.S = 6;
                    break;
                case Keys.A:
                    playSound("walk");
                    player.Move("horizontal", -1);
                    player.SetAnimation(1);
                    player.PlayerAnimator.Reverse = -1;
                    player.Keyboard.A = 6;
                    break;
                case Keys.D:
                    playSound("walk");
                    player.Move("horizontal", 1);
                    player.SetAnimation(1);
                    player.PlayerAnimator.Reverse = 1;
                    player.Keyboard.D = 6;
                    break;
                case Keys.T:
                    playSound("teleport");
                    player.Teleport();
                    player.Keyboard.T = 6;
                    break;
                case Keys.Space:
                    player.HpInEnergy();
                    player.Keyboard.Space = 6;
                    break;
                case Keys.Q:
                    playSound("converter");
                    player.ConvertNearStonesInDiamonds();
                    player.Keyboard.Q = 6;
                    break;
                case Keys.E:
                    playSound("bomb");
                    player.UseTnt();
                    player.Keyboard.E = 6;
                    break;
                case Keys.R:
                    playSound("attack");
                    player.Attack();
                    player.Keyboard.R = 6;
                    break;
                case Keys.Add:
                    changeVolume(0.1f);
                    break;
                case Keys.Subtract:
                    changeVolume(-0.1f);
                    break;
                case Keys.Escape:
                    playSound("menuAccept");
                    changeGameStatus(0);
                    break;
            }
        }

        public void ProcessKeyUp(Keys key, Func<Player> getPlayer) {
            var player = getPlayer();
            switch (key) {
                case Keys.W:
                    player.SetAnimation(0);
                    player.Keyboard.W = 0;
                    break;
                case Keys.S:
                    player.SetAnimation(0);
                    player.Keyboard.S = 0;
                    break;
                case Keys.A:
                    player.SetAnimation(0);
                    player.Keyboard.A = 0;
                    break;
                case Keys.D:
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