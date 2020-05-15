using System;
using System.Windows.Forms;
using ClassLibrary.Entities.Player;
using ClassLibrary.InputProcessors;

namespace BoulderDashForms.InputProcessors {
    public class ResultsInputProcessor: InputProcessor {
        
        public void ProcessKeyDown(Keys key, Action<int> changeGameStatus, Action<float> changeVolume,
            Action<string> playSound,
            Action<int> performSubAction
            ) {
            switch (key) {
                case Keys.Enter:
                    playSound("menuAccept");
                    performSubAction(0);
                    break;
                case Keys.Add:
                    changeVolume(0.1f);
                    break;
                case Keys.Subtract:
                    changeVolume(-0.1f);
                    break;
                case Keys.Escape:
                    playSound("menuAccept");
                    changeGameStatus(0);
                    break;
            }
        }
        
    }
}