namespace ClassLibrary.Entities.Enemies {
    public class Enemy : Movable {

        protected int _damage;

        protected void EnemyDefaultDamage() {
            int playerPosX = GameEngine.gameLogic.Player.positionX;
            int playerPosY = GameEngine.gameLogic.Player.positionY;
            bool one = ((playerPosX+1 == positionX)&&(playerPosY==positionY));
            bool two = ((playerPosX-1 == positionX)&&(playerPosY==positionY));
            bool three = ((playerPosX == positionX)&&(playerPosY+1==positionY));
            bool four = ((playerPosX == positionX)&&(playerPosY-1==positionY));
            if (one || two || three || four) {
                DealDamage(GameEngine.gameLogic.Player, _damage);
            }
        }

        protected void DealDamage(Player player, int value) {
            player.Hp -= value;
            GameEngine.gameLogic.updatePlayerInterface();
        }


    }
}