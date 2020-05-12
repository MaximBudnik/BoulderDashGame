using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace BoulderDashForms.FormsDrawers {
    public class FormDrawer {
        protected Font _menuFont;
        protected Font _boldFont;
        protected readonly Brush _guiBrush = Brushes.Crimson;

        protected Image MainSprites;
        protected Image SecondarySprites;
        protected Image Tileset;
        protected Image Icons;
        protected Image Attack;
        protected Image Effects;
        protected Image HpFull;
        protected Image HpEmpty;
        protected Image Shield;
        protected Image Energy;
        protected Image Keyboard;

        protected string mainsSpritesPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");

        protected string secondarySpritesPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");

        protected string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
        protected string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");
        protected string attackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\attack.png");
        protected string effectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\fx.png");
        protected string hpFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart.png");
        protected string hpEmptyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart_empty.png");
        protected string shieldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\shield.png");
        protected string energyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\energy.png");
        protected string keyboardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\keyboard.png");

        protected FormDrawer() {
            MainSprites = new Bitmap(mainsSpritesPath);
            SecondarySprites = new Bitmap(secondarySpritesPath);
            Icons = new Bitmap(iconsPath);
            Tileset = new Bitmap(TilesetPath);
            Icons = new Bitmap(iconsPath);
            Effects = new Bitmap(effectPath);
            Attack = new Bitmap(attackPath);
            HpFull = new Bitmap(hpFullPath);
            HpEmpty = new Bitmap(hpEmptyPath);
            Shield = new Bitmap(shieldPath);
            Energy = new Bitmap(energyPath);
            Keyboard = new Bitmap(keyboardPath);


            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\monogram.ttf"));
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\ThaleahFat.ttf"));
            FontFamily family1 = fontCollection.Families[0];
            FontFamily family2 = fontCollection.Families[1];
            _menuFont = new Font(family1, 22);
            _boldFont = new Font(family2, 22);
        }
    }
}