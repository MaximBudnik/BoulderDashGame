using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace BoulderDashForms.FormsDrawers {
    public class FormDrawer {
        protected PrivateFontCollection fontCollection;
        protected FontFamily family1;
        protected FontFamily family2;
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
            GC.KeepAlive(MainSprites);
            SecondarySprites = new Bitmap(secondarySpritesPath);
            GC.KeepAlive(SecondarySprites);
            Icons = new Bitmap(iconsPath);
            GC.KeepAlive(Icons);
            Tileset = new Bitmap(TilesetPath);
            GC.KeepAlive(Tileset);
            Effects = new Bitmap(effectPath);
            GC.KeepAlive(Effects);
            Attack = new Bitmap(attackPath);
            GC.KeepAlive(Attack);
            HpFull = new Bitmap(hpFullPath);
            GC.KeepAlive(HpFull);
            HpEmpty = new Bitmap(hpEmptyPath);
            GC.KeepAlive(HpEmpty);
            Shield = new Bitmap(shieldPath);
            GC.KeepAlive(Shield);
            Energy = new Bitmap(energyPath);
            GC.KeepAlive(Energy);
            Keyboard = new Bitmap(keyboardPath);
            GC.KeepAlive(Keyboard);



             fontCollection = new PrivateFontCollection();
            GC.KeepAlive(fontCollection);
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\monogram.ttf"));
            fontCollection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Fonts\ThaleahFat.ttf"));
            family1 = fontCollection.Families[0];
            family2 = fontCollection.Families[1];
            GC.KeepAlive(family1);
            GC.KeepAlive(family2);
            _menuFont = new Font(family1, 22);
            _boldFont = new Font(family2, 22);
            GC.KeepAlive(_menuFont);
            GC.KeepAlive(_boldFont);
        }
    }
}