using System;
using System.Windows.Forms;
using ClassLibrary.InputProcessors;

namespace BoulderDashForms.InputProcessors {
    public class MenuInputProcessor : InputProcessor {
        public void ProcessKeyDown(Keys key, Action<int> changeCurrentMenuAction,
            Action performCurrentMenuAction,
            Action nullRightBlockWidth,
            Action changeActiveAction
            ) {
            switch (key) {
                case Keys.W:
                    changeCurrentMenuAction(-1);
                    nullRightBlockWidth();
                    break;
                case Keys.S:
                    changeCurrentMenuAction(1);
                    nullRightBlockWidth();
                    break;
                case Keys.Enter:
                    performCurrentMenuAction();
                    break;
                case Keys.Escape:
                    changeActiveAction();
                    break;
            }
        }
    }
}