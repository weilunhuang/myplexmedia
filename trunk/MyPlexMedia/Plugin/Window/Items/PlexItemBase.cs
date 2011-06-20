using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex;
using MediaPortal.GUI.Library;
using PlexMediaCenter.Plex.Data.Types;
using MediaPortal.Util;


namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemBase : MenuItem {
                
        public static event OnHasBackgroundEventHandler OnHasBackground;
        public delegate void OnHasBackgroundEventHandler(string imagePath);

        public static event OnItemDetailsUpdatedEventHandler OnItemDetailsUpdated;
        public delegate void OnItemDetailsUpdatedEventHandler(MediaContainer itemDetails);
       
        public MediaContainer ItemMetaData { get; set; }        

        public PlexItemBase(IMenuItem parentItem, string title, Uri path)
            : base(parentItem, title) {
            if (path != null) {
                UriPath = path.AbsoluteUri.Contains("?") ? path : new Uri((path.AbsoluteUri).EndsWith("/") ? path.AbsoluteUri : path.AbsoluteUri + "/");
            }
            OnRetrieveArt += new RetrieveCoverArtHandler(PlexItemBase_OnRetrieveArt);
            ViewItems = new List<IMenuItem>();
            //base.SetIcons(MediaRetrieval.GetArtWork(UriPath.AbsoluteUri));
        }

        void PlexItemBase_OnRetrieveArt(GUIListItem item) {
            Utils.SetDefaultIcons(item);
            OnRetrieveArtwork(item);
            Utils.SetThumbnails(ref item);
        }

        protected virtual void OnRetrieveArtwork(GUIListItem item) {
           
        }

        public virtual void SetMetaData(MediaContainer infoContainer) {
            int year;
            if (int.TryParse(infoContainer.parentYear, out year)) {
                base.Year = year;
            }
            ItemMetaData = infoContainer;
            OnItemDetailsUpdated(ItemMetaData);
        }


        public override void OnClicked(object sender, EventArgs e) {
            if (ChildItems == null || ChildItems.Count < 1) {
                SetChildItems(Navigation.GetCreateSubMenuItems(this, UriPath));
            }            
            Navigation.ShowCurrentMenu(this, LastSelectedChildIndex);
        }    

        public override void OnSelected() {
            if (!String.IsNullOrEmpty(BackgroundImage)) {
                OnHasBackground(BackgroundImage);
            }
            base.OnSelected();
        }


    }
}
