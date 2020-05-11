using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace BoulderDashForms.FormsDrawers {
    public class FormDrawer {
        protected  Font _menuFont;
        protected  Font _boldFont;
        protected readonly Brush _guiBrush = Brushes.Crimson;
        
        
        
        protected readonly Image _mainSprites;
        protected readonly Image _secondarySprites;
        protected readonly Image _tileset;
        protected readonly Image _icons;
        protected readonly Image _attack;
        protected readonly Image _effects;
        protected readonly Image _hpFull;
        protected readonly Image _hpEmpty;
        protected readonly Image _shield;
        protected readonly Image _energy;
        protected readonly Image _keyboard;

        public FormDrawer() {
            string mainsSpritesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\mainSprites.png");
            string secondarySpritesPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\secondarySprites.png");
            string TilesetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\Tileset.png");
            string iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\icons.png");
            string attackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\attack.png");
            string effectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\fx.png");
            string hpFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart.png");
            string hpEmptyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\heart_empty.png");
            string shieldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\shield.png");
            string energyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\energy.png");
            string keyboardPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sprites\keyboard.png");

            _mainSprites = new Bitmap(mainsSpritesPath);
            _secondarySprites = new Bitmap(secondarySpritesPath);
            _tileset = new Bitmap(TilesetPath);
            _icons = new Bitmap(iconsPath);
            _effects = new Bitmap(effectPath);
            _attack = new Bitmap(attackPath);
            _hpFull = new Bitmap(hpFullPath);
            _hpEmpty = new Bitmap(hpEmptyPath);
            _shield = new Bitmap(shieldPath);
            _energy = new Bitmap(energyPath);
            _keyboard = new Bitmap(keyboardPath);
            
            
            
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