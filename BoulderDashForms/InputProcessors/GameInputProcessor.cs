using System;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.Entities;
using ClassLibrary.Entities.Player;
using ClassLibrary.InputProcessors;

namespace BoulderDashForms.InputProcessors {
    public class GameInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Func<Player> getPlayer,
            Action<float> changeVolume
        ) {
            var player = getPlayer();
            switch (key) {
                case Keys.W:
                    player.Move(MoveDirectionEnum.Horizontal, -1);
                    player.Keyboard.W = KeyboardEnum.Enabled;
                    player.LastMove = MoveDirectionExtended.Top;
                    break;
                case Keys.S:
                    player.Move(MoveDirectionEnum.Horizontal, 1);
                    player.Keyboard.S = KeyboardEnum.Enabled;
                    player.LastMove = MoveDirectionExtended.Bot;
                    break;
                case Keys.A:
                    player.Move(MoveDirectionEnum.Vertical, -1);
                    player.PlayerAnimator.Reverse = -1;
                    player.Keyboard.A = KeyboardEnum.Enabled;
                    player.LastMove = MoveDirectionExtended.Left;
                    break;
                case Keys.D:
                    player.Move(MoveDirectionEnum.Vertical, 1);
                    player.PlayerAnimator.Reverse = 1;
                    player.Keyboard.D = KeyboardEnum.Enabled;
                    player.LastMove = MoveDirectionExtended.Right;
                    break;
                case Keys.T:
                    player.Teleport();
                    player.Keyboard.T = KeyboardEnum.Enabled;
                    break;
                case Keys.Space:
                    player.UseEnergyConverter();
                    player.Keyboard.Space = KeyboardEnum.Enabled;
                    break;
                case Keys.Q:
                    player.ConvertNearStonesInDiamonds();
                    player.Keyboard.Q = KeyboardEnum.Enabled;
                    break;
                case Keys.E:
                    player.UseDynamite();
                    player.Keyboard.E = KeyboardEnum.Enabled;
                    break;
                case Keys.R:
                    player.Attack();
                    player.Keyboard.F = KeyboardEnum.Enabled;
                    break;
                case Keys.F:
                    player.Shoot();
                    break;
                case Keys.Add:
                    changeVolume(0.1f);
                    break;
                case Keys.Subtract:
                    changeVolume(-0.1f);
                    break;
                case Keys.Escape:
                    break;
            }
        }

        public void ProcessKeyUp(Keys key, Func<Player> getPlayer, Action<GameStatusEnum> changeGameStatus) {
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
                    player.CheckAdrenalineCombo();
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
                case Keys.F:
                    player.Keyboard.F = KeyboardEnum.Disabled;
                    break;
                case Keys.Escape:
                    changeGameStatus(GameStatusEnum.Menu);
                    break;
            }
        }
    }
}