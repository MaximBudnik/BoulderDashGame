using System;
using System.Windows.Forms;
using ClassLibrary.Entities.Player;
using ClassLibrary.InputProcessors;

namespace BoulderDashForms.InputProcessors {
    public class GameInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Func<Player> getPlayer, Action<int> changeGameStatus,
            Action<float> changeVolume,
            Action<string> playSound
        ) {
            var player = getPlayer();
            switch (key) {
                case Keys.W:
                    playSound("walk");
                    player.Move("vertical", -1);
                    player.Keyboard.W = KeyboardEnum.Enabled;
                    break;
                case Keys.S:
                    playSound("walk");
                    player.Move("vertical", 1);
                    player.Keyboard.S = KeyboardEnum.Enabled;
                    break;
                case Keys.A:
                    playSound("walk");
                    player.Move("horizontal", -1);
                    player.PlayerAnimator.Reverse = -1;
                    player.Keyboard.A = KeyboardEnum.Enabled;
                    break;
                case Keys.D:
                    playSound("walk");
                    player.Move("horizontal", 1);
                    player.PlayerAnimator.Reverse = 1;
                    player.Keyboard.D = KeyboardEnum.Enabled;
                    break;
                case Keys.T:
                    playSound("teleport");
                    player.Teleport();
                    player.Keyboard.T = KeyboardEnum.Enabled;
                    break;
                case Keys.Space:
                    player.HpInEnergy();
                    player.Keyboard.Space = KeyboardEnum.Enabled;
                    break;
                case Keys.Q:
                    playSound("converter");
                    player.ConvertNearStonesInDiamonds();
                    player.Keyboard.Q = KeyboardEnum.Enabled;
                    break;
                case Keys.E:
                    playSound("bomb");
                    player.UseTnt();
                    player.Keyboard.E = KeyboardEnum.Enabled;
                    break;
                case Keys.R:
                    playSound("attack");
                    player.Attack();
                    player.Keyboard.R = KeyboardEnum.Enabled;
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
                    player.Keyboard.W = KeyboardEnum.Disabled;
                    break;
                case Keys.S:
                    player.SetAnimation(0);
                    player.Keyboard.S = KeyboardEnum.Disabled;
                    break;
                case Keys.A:
                    player.SetAnimation(0);
                    player.Keyboard.A = KeyboardEnum.Disabled;
                    break;
                case Keys.D:
                    player.SetAnimation(0);
                    player.Keyboard.D = KeyboardEnum.Disabled;
                    break;
                case Keys.T:
                    player.Keyboard.T = KeyboardEnum.Disabled;
                    break;
                case Keys.Space:
                    player.Keyboard.Space = KeyboardEnum.Disabled;
                    break;
                case Keys.Q:
                    player.Keyboard.Q = KeyboardEnum.Disabled;
                    break;
                case Keys.E:
                    player.Keyboard.E = KeyboardEnum.Disabled;
                    break;
                case Keys.R:
                    player.Keyboard.R = KeyboardEnum.Disabled;
                    break;
                case Keys.Escape:
                    break;
            }
        }
    }
}