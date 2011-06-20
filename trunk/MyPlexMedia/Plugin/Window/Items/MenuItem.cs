using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Config;
using MediaPortal.Util;


namespace MyPlexMedia.Plugin.Window.Items {
    public class MenuItem : GUIListItem, IMenuItem {               

        public static event OnMenuItemSelectedEventHandler OnMenuItemSelected;
        public delegate void OnMenuItemSelectedEventHandler(IMenuItem selectedItem);
        public List<IMenuItem> ViewItems { get; set; }
        public string Name { get { return Label; } }

        public MenuItem(IMenuItem parent, string title) : base(title) {
            Parent = parent;
            OnItemSelected += new ItemSelectedHandler(MenuItem_OnItemSelected);
            OnRetrieveArt += new RetrieveCoverArtHandler(MenuItem_OnRetrieveArt);             
        }

        void MenuItem_OnRetrieveArt(GUIListItem item) {
            Utils.SetDefaultIcons(item);
            Utils.SetThumbnails(ref item);           
            OnRetrieveArtwork(item);
        }

        void MenuItem_OnItemSelected(GUIListItem item, GUIControl parent) {            
            OnSelected();
        }

        #region IMenuItem Members

        public string BackgroundImage { get; set; }
        public IMenuItem Parent { get; set; }
        public Uri UriPath { get; set; }
        public int LastSelectedChildIndex { get; set; }

        public List<IMenuItem> ChildItems { get; set; }
        
        public List<IMenuItem> GetChildItems() {
            return ChildItems;
        }

        public virtual void OnClicked(object sender, EventArgs e) {
            if (ChildItems != null && ChildItems.Count > 0) {
                Navigation.ShowCurrentMenu(this, 0);
            }
        }

        public virtual void OnRetrieveArtwork(GUIListItem item) {
            item.IconImage = Settings.PLEX_ICON_DEFAULT;
            item.IconImageBig = Settings.PLEX_ICON_DEFAULT;
            item.ThumbnailImage = Settings.PLEX_ICON_DEFAULT;    
        }


        public virtual void OnSelected() {
            OnMenuItemSelected(this);
        }       
        
        #endregion  

        public void SetChildItems(List<IMenuItem> childItems) {
            childItems.ForEach(ch => ch.Parent = this);           
            ChildItems = childItems;
        }

        public GUIFacadeControl.Layout PreferredLayout { get; set; }

     
    }
}
