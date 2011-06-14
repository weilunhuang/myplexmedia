using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MyPlexMedia.Plugin.Window.Items {
    public interface IMenuItem {
        IMenuItem Parent { get; set; }
        Uri UriPath { get; set; }     
        string BackgroundImage { get; } 
        List<IMenuItem> ChildItems { get;}
        void OnClicked(object sender, EventArgs e);        
        void OnSelected();
    }
}
