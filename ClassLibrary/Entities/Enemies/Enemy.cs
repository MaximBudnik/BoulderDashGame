namespace ClassLibrary.Entities {
    public class Enemy : Movable {

        protected int _damage;

        protected void DealDamage(Player player, int value) {
            player.Hp -= value;
        }


    }
}