using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Dialogs {
    class GuiDialogBufferingProgress : GUIDialogProgress {

        [SkinControl(12)]
        protected GUITVProgressControl _bufferingProgress = null;

        public GuiDialogBufferingProgress() {
            GetID = Settings.DIALOG_BUFFERING_WINDOW_ID;
        }

        public override bool Init() {
            return Load(GUIGraphicsContext.Skin + @"\MyPlexMedia.GuiDialogBufferingProgress.xml");
        }

        public override bool OnMessage(GUIMessage message) {
            switch (message.Message) {
                case GUIMessage.MessageType.GUI_MSG_WINDOW_INIT: {
                        base.OnMessage(message);
                        DisplayProgressBar = false;
                    }
                    return true;
            }

            return base.OnMessage(message);
        }

        public void SetBufferingProgress(string headerText, string line1, string line2, string line3,string line4,
                                    int percentageCurrentPosition, int percentageBuffered, int percentageOverall = 100) {
            SetHeading(headerText);
            SetLine(1, line1);
            SetLine(2, line2);
            SetLine(3, line3);
            SetLine(4, line4);
            _bufferingProgress.Percentage1 = percentageCurrentPosition;
            _bufferingProgress.Percentage2 = percentageBuffered;
            _bufferingProgress.Percentage3 = percentageOverall;
        }

        public override void Reset() {
            SetHeading(string.Empty);
            SetLine(1, string.Empty);
            SetLine(2, string.Empty);
            SetLine(3, string.Empty);
            SetLine(4, string.Empty);
            _bufferingProgress.Percentage1 = 0;
            _bufferingProgress.Percentage2 = 0;
            _bufferingProgress.Percentage3 = 0;
        }


    }
}
