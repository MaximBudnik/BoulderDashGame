using System;
using System.Windows.Forms;
using ClassLibrary.InputProcessors;

namespace BoulderDashForms.InputProcessors {
    public class MenuInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Action<int> changeCurrentMenuAction,
            Action performCurrentMenuAction,
            Action nullRightBlockWidth,
            Action changeActiveAction,
            bool isBlockActive,
            Action<int> changeSubAction,
            Action<int> performSubAction,
            bool isNameEntered,
            Action changeIsNameEntered,
            Action<string> addCharToName,
            Action<float> changeVolume,
            Action<string> playSound
        ) {
            if (key == Keys.Enter || key == Keys.Escape) playSound("menuAccept");
            else playSound("menuClick");
            if (isNameEntered) {
                if (key == Keys.Enter) {
                    changeIsNameEntered();
                    return;
                }
                var kc = new KeysConverter();
                var keyChar = kc.ConvertToString(key);
                addCharToName(keyChar);
                return;
            }
            switch (key) {
                case Keys.W:
                    if (isBlockActive == false) {
                        changeCurrentMenuAction(-1);
                        nullRightBlockWidth();
                    }
                    else {
                        changeSubAction(-1);
                    }
                    break;
                case Keys.S:
                    if (isBlockActive == false) {
                        changeCurrentMenuAction(1);
                        nullRightBlockWidth();
                    }
                    else {
                        changeSubAction(1);
                    }
                    break;
                case Keys.A:
                    if (isBlockActive == false) { }
                    else {
                        performSubAction(-1);
                    }
                    break;
                case Keys.D:
                    if (isBlockActive == false) { }
                    else {
                        performSubAction(1);
                    }
                    break;
                case Keys.Enter:
                    if (isBlockActive == false)
                        performCurrentMenuAction();
                    else
                        performSubAction(0);
                    break;
                case Keys.Add:
                    changeVolume(0.1f);
                    break;
                case Keys.Subtract:
                    changeVolume(-0.1f);
                    break;
                case Keys.Escape:
                    changeActiveAction();
                    break;
            }
        }
    }
}