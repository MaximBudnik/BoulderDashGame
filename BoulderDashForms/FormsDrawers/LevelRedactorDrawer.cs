using System.Drawing;
using ClassLibrary;

namespace BoulderDashForms.FormsDrawers {
    public class LevelRedactorDrawer : FormDrawer {
        public void DrawRedactor(Graphics graphics, GameEngine gameEngine, int width, int height) {
            graphics.DrawString($"Size X: {gameEngine.LevelRedactor.NewCustomLevel.SizeX}",
                MenuFont, RedBrush, 520, height/6+120);        }
    }
}