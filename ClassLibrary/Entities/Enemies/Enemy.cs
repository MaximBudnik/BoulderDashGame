namespace ClassLibrary.Entities.Enemies {
    public class Enemy : Movable {
        protected int _damage;

        protected void EnemyDefaultDamage() {
            int playerPosX = GameEngine.GameLogic.Player.PositionX;
            int playerPosY = GameEngine.GameLogic.Player.PositionY;
            bool one = ((playerPosX + 1 == PositionX) && (playerPosY == PositionY));
            bool two = ((playerPosX - 1 == PositionX) && (playerPosY == PositionY));
            bool three = ((playerPosX == PositionX) && (playerPosY + 1 == PositionY));
            bool four = ((playerPosX == PositionX) && (playerPosY - 1 == PositionY));
            if (one || two || three || four) {
                DealDamage(GameEngine.GameLogic.Player, _damage);
            }
        }

        protected void DealDamage(Player player, int value) {
            player.Hp -= value;
            GameEngine.GameLogic.UpdatePlayerInterface();
        }
    }
}