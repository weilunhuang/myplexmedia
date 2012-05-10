#region #region Copyright (C) 2005-2011 Team MediaPortal

// 
// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

using System;
using System.Linq;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = System.Action;
using MyPlexMedia.Plugin.Config;
using MyPlexMedia.Plugin.Window.Playback;

namespace MyPlexMedia.Plugin.Window.Dialogs {
    internal static class CommonDialogs {
        #region GUI Helper Methods

        #region Delegates

        public delegate void OnProgressCancelledEventHandler();

        #endregion

        private static GUIDialogProgress DialogProgress;
        //static GuiDialogBufferingProgress dialogBufferingProgress = new GuiDialogBufferingProgress();

        public static string GetKeyBoardInput(string defaultText, string labelText) {
            VirtualKeyboard keyboard =
                (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
            if (keyboard == null) {
                return String.Empty;
            }
            keyboard.Reset();
            keyboard.Label = labelText;
            keyboard.IsSearchKeyboard = true;
            keyboard.Text = defaultText;
            keyboard.DoModal(GUIWindowManager.ActiveWindow);
            return keyboard.IsConfirmed ? keyboard.Text : String.Empty;
        }

        public enum PLUGIN_NOTIFY_WINDOWS {
            WINDOW_DIALOG_AUTO,
            WINDOW_DIALOG_NOTIFY = GUIWindow.Window.WINDOW_DIALOG_NOTIFY,
            WINDOW_DIALOG_OK = GUIWindow.Window.WINDOW_DIALOG_OK,
            WINDOW_DIALOG_TEXT = GUIWindow.Window.WINDOW_DIALOG_TEXT
        }


        public static T ShowSelectionDialog<T>() {
            GUIDialogSelect2 dlgSelect = (GUIDialogSelect2)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_SELECT2);
            dlgSelect.Reset();
            dlgSelect.SetHeading("Selection: " + typeof(T).Name);
            Enum.GetNames(typeof(T)).ToList().ForEach(dlgSelect.Add);
            dlgSelect.DoModal(GUIWindowManager.ActiveWindow);
            try {
                return Enum<T>.Parse(dlgSelect.SelectedLabelText);
            } catch { 
            return default (T);
            }
        }

        public static void ShowNotifyDialog(int timeOut, string header, string text, string icon, PLUGIN_NOTIFY_WINDOWS notifyType = PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_AUTO) {
            try {
                GUIWindow guiWindow = GUIWindowManager.GetWindow((int)notifyType);
                switch (notifyType) {
                    default:
                    case PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_AUTO:
                        if (text.Length <= 60) {
                            ShowNotifyDialog(timeOut, header, text, icon,
                                             PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_NOTIFY);
                        } else {
                            ShowNotifyDialog(timeOut, header, text, icon,
                                             PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_TEXT);
                        }
                        break;
                    case PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_NOTIFY:
                        GUIDialogNotify notifyDialog = (GUIDialogNotify)guiWindow;
                        notifyDialog.Reset();
                        notifyDialog.TimeOut = timeOut;
                        notifyDialog.SetImage(icon);
                        notifyDialog.SetHeading(header);
                        notifyDialog.SetText(text);
                        notifyDialog.DoModal(GUIWindowManager.ActiveWindow);
                        break;
                    case PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_OK:
                        GUIDialogOK okDialog = (GUIDialogOK)guiWindow;
                        okDialog.Reset();
                        okDialog.SetHeading(header);
                        okDialog.SetLine(1, (text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))[0]);
                        okDialog.DoModal(GUIWindowManager.ActiveWindow);
                        break;
                    case PLUGIN_NOTIFY_WINDOWS.WINDOW_DIALOG_TEXT:
                        GUIDialogText textDialog = (GUIDialogText)guiWindow;
                        textDialog.Reset();
                        try {
                            textDialog.SetImage(icon);
                        } catch (Exception e) {

                        }
                        textDialog.SetHeading(header);
                        textDialog.SetText(text);
                        textDialog.DoModal(GUIWindowManager.ActiveWindow);
                        break;
                }
            } catch (Exception ex) {
                Log.Error(ex);
            }
        }


        /// <summary>
        ///   Displays a yes/no dialog with custom labels for the buttons
        ///   This method may become obsolete in the future if media portal adds more dialogs
        /// </summary>
        /// <returns>True if yes was clicked, False if no was clicked</returns>
        /// This has been taken (stolen really) from the wonderful MovingPictures Plugin -Anthrax.
        public static bool ShowCustomYesNo(string heading, string lines, string yesLabel, string noLabel,
                                           bool defaultYes) {
            GUIDialogYesNo dialog =
                (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
            try {
                dialog.Reset();
                dialog.SetHeading(heading);
                string[] linesArray = lines.Split(new[] { "\\n" }, StringSplitOptions.None);
                if (linesArray.Length > 0) dialog.SetLine(1, linesArray[0]);
                if (linesArray.Length > 1) dialog.SetLine(2, linesArray[1]);
                if (linesArray.Length > 2) dialog.SetLine(3, linesArray[2]);
                if (linesArray.Length > 3) dialog.SetLine(4, linesArray[3]);
                dialog.SetDefaultToYes(defaultYes);

                foreach (var btn in dialog.Children.OfType<GUIButtonControl>()) {
                    if (btn.GetID == 11 && !String.IsNullOrEmpty(yesLabel)) // Yes button
                        btn.Label = yesLabel;
                    else if (btn.GetID == 10 && !String.IsNullOrEmpty(noLabel)) // No button
                        btn.Label = noLabel;
                }
                dialog.DoModal(GUIWindowManager.ActiveWindow);
                return dialog.IsConfirmed;
            } finally {
                // set the standard yes/no dialog back to it's original state (yes/no buttons)
                if (dialog != null) {
                    dialog.ClearAll();
                }
            }
        }

        public static event OnProgressCancelledEventHandler OnProgressCancelled;

        private delegate void ShowProgressDialogCallback(int progressPercentage, string headerText, string line1, string line2, string line3,
                                              bool doModal);


        public static void ShowProgressDialog(int progressPercentage, string headerText, string line1 = "", string line2 = "", string line3 = "",
                                              bool doModal = false) {
            if (GUIGraphicsContext.form.InvokeRequired) {
                ShowProgressDialogCallback callback = ShowProgressDialog;
                GUIGraphicsContext.form.Invoke(callback,
                                               new object[] { progressPercentage, headerText, line1, line2, line3, doModal });
                return;
            }
            DialogProgress = (GUIDialogProgress)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            if (progressPercentage <= 100) {
                if (!DialogProgress.IsVisible && !DialogProgress.IsCanceled) {
                    DialogProgress.Reset();
                    DialogProgress.IsVisible = true;
                    if (!String.IsNullOrEmpty(headerText)) {
                        DialogProgress.SetHeading(headerText);
                    }
                    DialogProgress.DisplayProgressBar = true;
                    DialogProgress.ShowWaitCursor = true;
                    if (doModal) {
                        DialogProgress.DisableCancel(false);
                        DialogProgress.Percentage = progressPercentage;
                        DialogProgress.SetLine(1, line1);
                        DialogProgress.SetLine(2, line2);
                        DialogProgress.SetLine(3, line3);
                        DialogProgress.DoModal(GUIWindowManager.ActiveWindow);                        
                        if (DialogProgress.IsCanceled) {
                            HideProgressDialog();
                            OnProgressCancelled();
                        }
                    } else {
                        DialogProgress.DisableCancel(true);
                        DialogProgress.StartModal(GUIWindowManager.ActiveWindow);
                    }
                }
                DialogProgress.Percentage = progressPercentage;
                DialogProgress.SetLine(1, line1);
                DialogProgress.SetLine(2, line2);
                DialogProgress.SetLine(3, line3);
            }
        }

        public static void HideProgressDialog() {
            DialogProgress =
                (GUIDialogProgress)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            DialogProgress.IsVisible = false;
            DialogProgress.Close();
            HideWaitCursor();
            GUIWindowManager.Process();
        }

        public static void ShowWaitCursor() {
            if (GUIGraphicsContext.form.InvokeRequired) {
                ShowWaitCursorCallback callback = ShowWaitCursor;
                GUIGraphicsContext.form.Invoke(callback);
                return;
            }
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
        }

        public static void HideWaitCursor() {
            if (GUIGraphicsContext.form.InvokeRequired) {
                GUIGraphicsContext.form.Invoke(new Action(HideWaitCursor));
                return;
            }
            GUIWaitCursor.Hide();
        }



        #region Nested type: ShowWaitCursorCallback

        private delegate void ShowWaitCursorCallback();

        #endregion

        #endregion

        
    }
}