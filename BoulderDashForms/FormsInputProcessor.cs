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
                    player.SetAnimation(1);
                    break;
                case Keys.S:
                    player.Move("vertical", 1);
                    player.SetAnimation(1);
                    player.IsMoving = true;
                    break;
                case Keys.A:
                    player.Move("horizontal", -1);
                    player.SetAnimation(1);
                    player.IsMoving = true;
                    player.reverse = -1;
                    break;
                case Keys.D:
                    player.Move("horizontal", 1);
                    player.SetAnimation(1);
                    player.IsMoving = true;
                    player.reverse = 1;
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
        
        public void ProcessKeyUp(Keys key,Func<Player> getPlayer) {
            Player player = getPlayer();
            switch (key) {
                case Keys.W:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    break;
                case Keys.S:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    break;
                case Keys.A:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    break;
                case Keys.D:
                    player.IsMoving = false;
                    player.SetAnimation(0);
                    break;
            }
        }
        
       
        
        
        
    }
}