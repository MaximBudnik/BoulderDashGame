using System;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.InputProcessors;
using ClassLibrary.SoundPlayer;

namespace BoulderDashForms.InputProcessors {
    public class ResultsInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Action<GameStatusEnum> changeGameStatus, Action<float> changeVolume,
            Action<SoundFilesEnum> playSound,
            Action<int> performSubAction
        ) {
            switch (key) {
                case Keys.Enter:
                    playSound(SoundFilesEnum.MenuAcceptSound);
                    performSubAction(0);
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