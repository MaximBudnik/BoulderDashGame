using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Candy : Rock {
        private readonly int moveLength = 4;
        public Candy(Func<Level> getLevel, int i, int j, MoveDirectionEnum direction, int value,
            Action<int> changePlayerHp) : base(i, j, getLevel, changePlayerHp) {
            EntityEnumType = GameEntitiesEnum.Candy;
            PerformAction(direction, value);
            CanMove = false;
            Damage = 2;
            Hp = 15;
        }
        private void PerformAction(MoveDirectionEnum direction, int value) {
            var task = new Task(() => {
                for (var i = 0; i < moveLength; i++) {
                    Move(direction, value);
                    Thread.Sleep(100);
                }
            });
            task.Start();
        }
    }
}