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

namespace MyPlexMedia.Plugin.Window.Dialogs {
    internal static class CommonDialogs {
        #region GUI Helper Methods

        #region Delegates

        public delegate void OnProgressCancelledEventHandler();

        #endregion

        private static GUIDialogProgress DialogProgress;

        public static string GetKeyBoardInput(string defaultText, string labelText) {
            VirtualKeyboard keyboard =
                (VirtualKeyboard) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
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

        public static void ShowNotifyDialog(int timeOut, string headerText, string notifyMessage) {
            GUIDialogNotify dialogMailNotify =
                (GUIDialogNotify) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
            try {
                dialogMailNotify.Reset();
                dialogMailNotify.TimeOut = timeOut;
                dialogMailNotify.SetImage(GUIGraphicsContext.Skin + @"\Media\hover_MyPlexMedia.png");
                dialogMailNotify.SetHeading(headerText);
                dialogMailNotify.SetText(notifyMessage);
                dialogMailNotify.DoModal(GUIWindowManager.ActiveWindow);
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
                (GUIDialogYesNo) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_DIALOG_YES_NO);
            try {
                dialog.Reset();
                dialog.SetHeading(heading);
                string[] linesArray = lines.Split(new[] {"\\n"}, StringSplitOptions.None);
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

        public static void ShowProgressDialog(int progressPercentage, string headerText = "", string currentItem = "",
                                              bool doModal = true) {
            if (GUIGraphicsContext.form.InvokeRequired) {
                ShowProgressDialogCallback callback = ShowProgressDialog;
                GUIGraphicsContext.form.Invoke(callback,
                                               new object[] {progressPercentage, headerText, currentItem, doModal});
                return;
            }
            DialogProgress =
                (GUIDialogProgress) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            if (!DialogProgress.IsVisible && !DialogProgress.IsCanceled) {
                if (progressPercentage < 100) {
                    //DialogProgress.Reset();
                    if (!String.IsNullOrEmpty(headerText)) {
                        DialogProgress.SetHeading(headerText);
                    }
                    DialogProgress.SetLine(1, "Currently Fetching:");
                    DialogProgress.DisplayProgressBar = true;
                    DialogProgress.ShowWaitCursor = true;
                    if (doModal) {
                        DialogProgress.DisableCancel(false);
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
            }

            DialogProgress.Percentage = progressPercentage;
            if (!String.IsNullOrEmpty(currentItem)) {
                DialogProgress.SetLine(2, currentItem);
            }
            DialogProgress.SetLine(3, String.Format("({0} % completed)", progressPercentage));
            DialogProgress.Progress();
            DialogProgress.ProcessDoModal();
            DialogProgress.UpdateVisibility();
        }

        public static void HideProgressDialog() {
            DialogProgress =
                (GUIDialogProgress) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            if (DialogProgress.IsVisible) {
                DialogProgress.Close();
            }
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
            GUIWindowManager.Process();
        }

        #region Nested type: ShowProgressDialogCallback

        private delegate void ShowProgressDialogCallback(
            int progressPercentage, string headerText, string currentItem, bool doModal);

        #endregion

        #region Nested type: ShowWaitCursorCallback

        private delegate void ShowWaitCursorCallback();

        #endregion

        #endregion
    }
}