using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyPlexMedia.Plugin.Window.Items {
    class ActionItem : MenuItem {
                
        Action Action { get; set; }

        public ActionItem(IMenuItem parentItem,string title, string specialIcon, Action actionDelegate)
            : base(parentItem, title) {
            Action = actionDelegate;
            base.IconImage = specialIcon;
            base.IconImageBig = specialIcon;
            base.ThumbnailImage = specialIcon;
        }      

        public override void OnClicked(object sender, EventArgs e) {
            Action();
        }
    }
}
