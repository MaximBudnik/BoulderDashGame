using System;
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
                    SetAnimation(player, 1);
                    break;
                case Keys.S:
                    player.Move("vertical", 1);
                    SetAnimation(player, 1);
                    player.IsMoving = true;
                    break;
                case Keys.A:
                    player.Move("horizontal", -1);
                    SetAnimation(player, 1);
                    player.IsMoving = true;
                    break;
                case Keys.D:
                    player.Move("horizontal", 1);
                    SetAnimation(player, 1);
                    player.IsMoving = true;
                    break;
                case Keys.T:
                    player.Teleport();
                    break;
                case Keys.Space:
                    player.HpInEnergy();
                    break;
                case Keys.Q:
                    player.ConvertNearStonesInDiamonds();
                    break;
                case Keys.E:
                    player.UseTnt();
                    break;
                case Keys.R:
                    player.Attack();
                    break;
                case Keys.Escape:
                    changeGameStatus(0);
                    break;
            }
        }
        
        public void ProcessKeyUp(Keys key,Func<Player> getPlayer, Action<int> changeGameStatus) {
            Player player = getPlayer();
            switch (key) {
                case Keys.W:
                    player.IsMoving = false;
                    break;
                case Keys.S:
                    player.Move("vertical", 1);
                    player.IsMoving = false;
                    break;
                case Keys.A:
                    player.Move("horizontal", -1);
                    player.IsMoving = false;
                    break;
                case Keys.D:
                    player.Move("horizontal", 1);
                    player.IsMoving = false;
                    break;
            }
        }
        
        private void SetAnimation(Player player, int currentAnimation) {
            player.currentAnimation = currentAnimation;
            switch (currentAnimation) {
                case 0:
                    player.framesLimit = player.idleFrames;
                    break;
                case 1:
                    player.framesLimit = player.moveFrames;
                    break;
                case 2:
                    player.framesLimit = player.actionFrames;
                    break;
            }
        }
        
        
        
    }
}