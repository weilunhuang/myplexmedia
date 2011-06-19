using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PlexMediaCenter.Util;
using PlexMediaCenter.Plex;
using MediaPortal.GUI.Library;
using PlexMediaCenter.Plex.Data.Types;


namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemBase : MenuItem {

        public static event OnPreferredLayoutEventHandler OnPreferredLayout;
        public delegate void OnPreferredLayoutEventHandler(GUIFacadeControl.Layout preferredLayout);

        public static event OnHasBackgroundEventHandler OnHasBackground;
        public delegate void OnHasBackgroundEventHandler(string imagePath);

        public static event OnItemDetailsUpdatedEventHandler OnItemDetailsUpdated;
        public delegate void OnItemDetailsUpdatedEventHandler(MediaContainer itemDetails);

        public GUIFacadeControl.Layout PreferredLayout { get; set; }
        public MediaContainer ItemDetails { get; set; }

        public PlexItemBase(IMenuItem parentItem, string title, Uri path)
            : base(parentItem, title) {
            if (path != null) {
                UriPath = path.AbsoluteUri.Contains("?") ? path : new Uri((path.AbsoluteUri).EndsWith("/") ? path.AbsoluteUri : path.AbsoluteUri + "/");
            }
            //base.SetIcons(MediaRetrieval.GetArtWork(UriPath.AbsoluteUri));
        }

        public virtual void SetItemInfos(MediaContainer infoContainer) {
            int year;
            if (int.TryParse(infoContainer.parentYear, out year)) {
                base.Year = year;
            }
            ItemDetails = infoContainer;
            OnItemDetailsUpdated(ItemDetails);
        }


        public override void OnClicked(object sender, EventArgs e) {
            if (ChildItems == null || ChildItems.Count < 1) {
                SetChildItems(Navigation.GetSubMenuItems(this, UriPath));
            }
            OnPreferredLayout(PreferredLayout);
            Navigation.ShowCurrentMenu(this, 0);
        }

        public void SetPreferredLayout() {
            OnPreferredLayout(PreferredLayout);
            if (!String.IsNullOrEmpty(BackgroundImage)) {
                OnHasBackground(BackgroundImage);
            }
        }

        public override void OnSelected() {
            if (!String.IsNullOrEmpty(BackgroundImage)) {
                OnHasBackground(BackgroundImage);
            }
            base.OnSelected();
        }


    }
}
