using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace BoulderDashForms.FormsDrawers {
    public class FormDrawer {
        private readonly string _attackPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\attack.png");

        private readonly string _effectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\fx.png");

        private readonly string _energyPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\energy.png");

        private readonly string _hpEmptyPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart_empty.png");

        private readonly string _hpFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart.png");
        private readonly string _iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");

        private readonly string _keyboardPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\keyboard.png");

        private readonly string _mainsSpritesPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");

        private readonly string _secondarySpritesPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");

        private readonly string _shieldPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\shield.png");

        private readonly string _tilesetPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");

        protected readonly Image Attack;
        protected readonly Image Effects;
        protected readonly Image Energy;
        protected readonly Brush GuiBrush = Brushes.Crimson;
        protected readonly Image HpEmpty;
        protected readonly Image HpFull;
        protected readonly Image Icons;
        protected readonly Image Keyboard;
        protected readonly Image MainSprites;
        protected readonly Image SecondarySprites;
        protected readonly Image Shield;
        protected readonly Image TileSet;
        protected Font BoldFont;
        protected FontFamily Family1;
        protected FontFamily Family2;
        protected PrivateFontCollection FontCollection;
        protected Font MenuFont;
        protected FormDrawer() {
            MainSprites = new Bitmap(_mainsSpritesPath);
            SecondarySprites = new Bitmap(_secondarySpritesPath);
            Icons = new Bitmap(_iconsPath);
            TileSet = new Bitmap(_tilesetPath);
            Effects = new Bitmap(_effectPath);
            Attack = new Bitmap(_attackPath);
            HpFull = new Bitmap(_hpFullPath);
            HpEmpty = new Bitmap(_hpEmptyPath);
            Shield = new Bitmap(_shieldPath);
            Energy = new Bitmap(_energyPath);
            Keyboard = new Bitmap(_keyboardPath);
            FontCollection = new PrivateFontCollection();
            FontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\monogram.ttf"));
            FontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\ThaleahFat.ttf"));
            Family1 = FontCollection.Families[0];
            Family2 = FontCollection.Families[1];
            MenuFont = new Font(Family1, 22);
            BoldFont = new Font(Family2, 22);
        }
    }
}