using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Window.Dialogs {
    class GuiDialogBufferingProgress : GUIDialogProgress {



        public override bool Init() {
            return Load(GUIGraphicsContext.Skin + @"\MyPlexMedia.GuiDialogBufferingProgress.xml");
        }


    }
}
