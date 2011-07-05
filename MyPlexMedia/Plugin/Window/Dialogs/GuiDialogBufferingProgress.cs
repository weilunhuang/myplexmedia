using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Dialogs {
    class GuiDialogBufferingProgress : GUIDialogProgress {

        
        protected GUITVProgressControl _bufferingProgress;

        public GuiDialogBufferingProgress() {
            GetID = Settings.DIALOG_BUFFERING_WINDOW_ID;
            _bufferingProgress = new GUITVProgressControl((int)Window.WINDOW_DIALOG_PROGRESS, GetID, 360, 429, 542, 20, "", "", "", "", "", "", "", "osd_progress_mid_red.png", "osd_progress_mid.png", "osd_progress_mid_orange.png", "");
        }

        public override bool Init() {
          bool test = Load(GUIGraphicsContext.Skin + @"\MyPlexMedia.GuiDialogBufferingProgress.xml");
          
          return test;
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

        public override void AllocResources() {
            base.AllocResources();
            
            _bufferingProgress.AllocResources();
        }

        public override void Render(float timePassed) {
            base.Render(timePassed);
            _bufferingProgress.Render(timePassed);
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
