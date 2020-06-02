using System;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.Entities.Player;
using ClassLibrary.InputProcessors;
using ClassLibrary.SoundPlayer;

namespace BoulderDashForms.InputProcessors {
    public class GameInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Func<Player> getPlayer, Action<GameStatusEnum> changeGameStatus,
            Action<float> changeVolume,
            Action<SoundFilesEnum> playSound
        ) {
            var player = getPlayer();
            switch (key) {
                case Keys.W:
                    playSound(SoundFilesEnum.WalkSound);
                    player.Move("vertical", -1);
                    player.Keyboard.W = KeyboardEnum.Enabled;
                    break;
                case Keys.S:
                    playSound(SoundFilesEnum.WalkSound);
                    player.Move("vertical", 1);
                    player.Keyboard.S = KeyboardEnum.Enabled;
                    break;
                case Keys.A:
                    playSound(SoundFilesEnum.WalkSound);
                    player.Move("horizontal", -1);
                    player.PlayerAnimator.Reverse = -1;
                    player.Keyboard.A = KeyboardEnum.Enabled;
                    break;
                case Keys.D:
                    playSound(SoundFilesEnum.WalkSound);
                    player.Move("horizontal", 1);
                    player.PlayerAnimator.Reverse = 1;
                    player.Keyboard.D = KeyboardEnum.Enabled;
                    break;
                case Keys.T:
                    playSound(SoundFilesEnum.TeleportSound);
                    player.Teleport();
                    player.Keyboard.T = KeyboardEnum.Enabled;
                    break;
                case Keys.Space:
                    player.HpInEnergy();
                    player.Keyboard.Space = KeyboardEnum.Enabled;
                    break;
                case Keys.Q:
                    playSound(SoundFilesEnum.ConverterSound);
                    player.ConvertNearStonesInDiamonds();
                    player.Keyboard.Q = KeyboardEnum.Enabled;
                    break;
                case Keys.E:
                    playSound(SoundFilesEnum.BombSound);
                    player.UseDynamite();
                    player.Keyboard.E = KeyboardEnum.Enabled;
                    break;
                case Keys.R:
                    playSound(SoundFilesEnum.AttackSound);
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
                    playSound(SoundFilesEnum.MenuAcceptSound);
                    changeGameStatus(GameStatusEnum.Menu);
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