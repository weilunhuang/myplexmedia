using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PlexMediaCenter.Util;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using PlexMediaCenter.Plex.Data.Types;
using MyPlexMedia.Plugin.Config;


namespace MyPlexMedia.Plugin.Window.Items {
    public class PlexItemBase : MenuItem {

        public static event OnHasBackgroundEventHandler OnHasBackground;
        public delegate void OnHasBackgroundEventHandler(string imagePath);

        public PlexItemBase(IMenuItem parentItem, string title, Uri path)
            : base(parentItem, title) {
            if (path != null) {
                UriPath = path.AbsoluteUri.Contains("?") ? path : new Uri((path.AbsoluteUri).EndsWith("/") ? path.AbsoluteUri : path.AbsoluteUri + "/");
            }
            ViewItems = new List<IMenuItem>();
            Utils.SetDefaultIcons(this);
            SetIcon(Settings.PLEX_ICON_DEFAULT);
            SetImage(Settings.PLEX_ARTWORK_DEFAULT);
        }

        protected void SetIcon(string imagePath) {
            IconImage = ThumbnailImage = imagePath;
            RefreshCoverArt();
        }

        protected void SetImage(string imagePath) {
            IconImageBig = imagePath;
            BackgroundImage = imagePath;
            RefreshCoverArt();
        }

        public virtual void SetMetaData(MediaContainer infoContainer) {
            int year;
            if (int.TryParse(infoContainer.parentYear, out year)) {
                base.Year = year;
            }
            base.AlbumInfoTag = infoContainer;
        }


        public override void OnClicked(object sender, EventArgs e) {
            if (ChildItems == null || ChildItems.Count < 1) {
                Navigation.RequestChildItems(UriPath, this);
            } else {
                Navigation.History.Add(Name);
                Navigation.ShowCurrentMenu(this, LastSelectedChildIndex);
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
