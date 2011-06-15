using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Window {
    class GuiFacadeControlEx : GUIFacadeControl{

        public GuiFacadeControlEx(int dwParentId, int dwControlId) : 
            base(dwParentId, dwControlId) { 
            
        }

        public GuiFacadeControlEx(int dwParentId)
            : base(dwParentId) {            
        }

         
    }
}
