using System;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.Entities;
using ClassLibrary.InputProcessors;
using ClassLibrary.LevelRedactor;
using ClassLibrary.SoundPlayer;

namespace BoulderDashForms.InputProcessors {
    public class LevelRedactorInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Action<GameStatusEnum> changeGameStatus,
            Action<MoveDirectionEnum, int> moveAnchor, Action<float> changeVolume,
            Action<SoundFilesEnum> playSound, Action placeBlock, Action<int> changeTool, Action saveLevel,
            Action<int> changeBrushSize, Action<RedactorBrushes> changeBrush, Action fillAll, Action collapseHelp) {
            switch (key) {
                case Keys.W:
                    moveAnchor(MoveDirectionEnum.Vertical, -1);
                    break;
                case Keys.A:
                    moveAnchor(MoveDirectionEnum.Horizontal, -1);
                    break;
                case Keys.S:
                    moveAnchor(MoveDirectionEnum.Vertical, 1);
                    break;
                case Keys.D:
                    moveAnchor(MoveDirectionEnum.Horizontal, 1);
                    break;
                case Keys.Oemplus:
                    changeBrushSize(1);
                    break;
                case Keys.OemMinus:
                    changeBrushSize(-1);
                    break;

                case Keys.Up:
                    playSound(SoundFilesEnum.MenuClickSound);
                    changeTool(-1);
                    break;

                case Keys.Down:
                    playSound(SoundFilesEnum.MenuClickSound);
                    changeTool(1);
                    break;

                case Keys.R:
                    changeBrush(RedactorBrushes.Default);
                    break;

                case Keys.E:
                    changeBrush(RedactorBrushes.Empty);
                    break;

                case Keys.F:
                    fillAll();
                    break;

                case Keys.H:
                    collapseHelp();
                    break;

                case Keys.Space:
                    placeBlock();
                    break;

                case Keys.Enter:
                    playSound(SoundFilesEnum.MenuAcceptSound);
                    saveLevel();
                    break;
                case Keys.Add:
                    changeVolume(0.1f);
                    break;
                case Keys.Subtract:
                    changeVolume(-0.1f);
                    break;
                case Keys.Escape:
                    playSound(SoundFilesEnum.MenuAcceptSound);
                    changeGameStatus(GameStatusEnum.Menu);
                    break;
            }
        }
    }
}