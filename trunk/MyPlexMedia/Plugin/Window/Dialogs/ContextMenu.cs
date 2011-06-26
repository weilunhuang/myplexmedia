using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using MyPlexMedia.Plugin.Window.Items;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Dialogs{
    public static class ContextMenu {

        private static List<IMenuItem> CurrentMenuItems { get; set; }
       
        internal static IDialogbox contextMenu = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);      
        internal static void ShowContextMenu(string headerLabel, List<IMenuItem> listCurrentContextMenuItems) {
            if (contextMenu == null) {
                return;
            } else if(CurrentMenuItems == null) { 
                return;
            }
            if (listCurrentContextMenuItems == null || listCurrentContextMenuItems.Count <= 0) {
                listCurrentContextMenuItems = CurrentMenuItems;
            }
            CurrentMenuItems = listCurrentContextMenuItems;
            contextMenu.SetHeading(headerLabel ?? "Current Context Menu:");              
            listCurrentContextMenuItems.ForEach(item => contextMenu.Add(item.Name));
            contextMenu.DoModal(GUIWindowManager.ActiveWindow);
            if (contextMenu.SelectedId > 0) {                
                listCurrentContextMenuItems[contextMenu.SelectedId - 1].OnClicked(null, null);
            }
        }                      
    }
}
