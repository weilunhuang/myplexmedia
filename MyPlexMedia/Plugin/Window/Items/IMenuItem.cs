using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MediaPortal.GUI.Library;

namespace MyPlexMedia.Plugin.Window.Items {
    public interface IMenuItem {
        IMenuItem Parent { get; set; }
        GUIFacadeControl.Layout PreferredLayout { get; set; }
        string Name { get; }
        List<IMenuItem> ViewItems { get; set; }
        int LastSelectedChildIndex { get; set; }
        Uri UriPath { get; set; }     
        string BackgroundImage { get; } 
        List<IMenuItem> ChildItems { get;}
        void OnClicked(object sender, EventArgs e);        
        void OnSelected();
    }
}
