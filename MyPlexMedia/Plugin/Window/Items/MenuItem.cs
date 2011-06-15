using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;


namespace MyPlexMedia.Plugin.Window.Items {
    public class MenuItem : GUIListItem, IMenuItem {               

        public static event OnMenuItemSelectedEventHandler OnMenuItemSelected;
        public delegate void OnMenuItemSelectedEventHandler(IMenuItem selectedItem);

        public MenuItem(IMenuItem parent, string title) : base(title) {
            Parent = parent;
            OnItemSelected += new ItemSelectedHandler(MenuItem_OnItemSelected);
            IconImage = Settings.PLEX_ICON_DEFAULT;
            IconImageBig = Settings.PLEX_ICON_DEFAULT;
            ThumbnailImage = Settings.PLEX_ICON_DEFAULT;                   
        }

        void MenuItem_OnItemSelected(GUIListItem item, GUIControl parent) {                        
            OnSelected();
        }

        #region IMenuItem Members

        public string BackgroundImage { get; set; }
        public IMenuItem Parent { get; set; }
        public Uri UriPath { get; set; }
        
        public List<IMenuItem> ChildItems { get; set; }

        public virtual void OnClicked(object sender, EventArgs e) {            
            //TODO: Handle Search Items!!!
            if (ChildItems != null && ChildItems.Count > 0) {
                Navigation.ShowCurrentMenu(this);
            }
        }

        public List<IMenuItem> GetChildItems() {
            return ChildItems;
        }

        public virtual void OnSelected() {
            OnMenuItemSelected(this);
        }       
        
        #endregion  

        public void SetChildItems(List<IMenuItem> childItems) {
            childItems.ForEach(ch => ch.Parent = this);
            if (Parent != null) {
                childItems.Add(new ActionItem(Parent, "Back", Settings.PLEX_ICON_DEFAULT_BACK, () => Navigation.FetchPreviousMenu(this)));
            }
            ChildItems = childItems;
        }
    }
}
