using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.UserInterface.Controls;
using MediaPortal.GUI;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MediaPortal.Dialogs;
using MediaPortal.Configuration;
using System.Threading;
using System.Collections;
using MyPlexMedia.Plugin.Window.Items;
using MyPlexMedia.Plugin.Config;

namespace MyPlexMedia.Plugin.Window.Dialogs {

    internal static class CommonDialogs {

        #region GUI Helper Methods

        
        public static string GetKeyBoardInput(string defaultText, string labelText) {
        VirtualKeyboard keyboard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
            if (keyboard == null) {
                return String.Empty;
            }
            keyboard.Reset();
            keyboard.Label = labelText;
            keyboard.IsSearchKeyboard = true;
            keyboard.Text = defaultText;
            keyboard.DoModal(GUIWindowManager.ActiveWindow);
            if (keyboard.IsConfirmed) {
                return keyboard.Text;
            } else {
                return String.Empty;
            }
        }

        public static void ShowNotifyDialog(int timeOut, string headerText, string notifyMessage) {
            GUIDialogNotify dialogMailNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
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
        /// Displays a yes/no dialog with custom labels for the buttons
        /// This method may become obsolete in the future if media portal adds more dialogs
        /// </summary>
        /// <returns>True if yes was clicked, False if no was clicked</returns>
        /// This has been taken (stolen really) from the wonderful MovingPictures Plugin -Anthrax.
        public static bool ShowCustomYesNo(string heading, string lines, string yesLabel, string noLabel, bool defaultYes) {
            GUIDialogYesNo dialog = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
            try {
                dialog.Reset();
                dialog.SetHeading(heading);
                string[] linesArray = lines.Split(new string[] { "\\n" }, StringSplitOptions.None);
                if (linesArray.Length > 0)
                    dialog.SetLine(1, linesArray[0]);
                if (linesArray.Length > 1)
                    dialog.SetLine(2, linesArray[1]);
                if (linesArray.Length > 2)
                    dialog.SetLine(3, linesArray[2]);
                if (linesArray.Length > 3)
                    dialog.SetLine(4, linesArray[3]);
                dialog.SetDefaultToYes(defaultYes);

                foreach (var item in dialog.Children) {
                    if (item is GUIButtonControl) {
                        GUIButtonControl btn = (GUIButtonControl)item;
                        if (btn.GetID == 11 && !String.IsNullOrEmpty(yesLabel)) // Yes button
                            btn.Label = yesLabel;
                        else if (btn.GetID == 10 && !String.IsNullOrEmpty(noLabel)) // No button
                            btn.Label = noLabel;
                    }
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
        public delegate void OnProgressCancelledEventHandler();


        static GUIDialogProgress DialogProgress;
        public static void ShowProgressDialog(string headerText, string currentItem, int progressPercentage) {
            DialogProgress = (GUIDialogProgress)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            if (!DialogProgress.IsVisible && !DialogProgress.IsCanceled) {
                if (progressPercentage < 100) {
                    DialogProgress.Reset();
                    DialogProgress.SetHeading(headerText);
                    DialogProgress.SetLine(1, "Currently Fetching:");
                    DialogProgress.Percentage = 0;
                    DialogProgress.DisplayProgressBar = true;
                    DialogProgress.ShowWaitCursor = true;
                    DialogProgress.IsVisible = true;
                    DialogProgress.DoModal(GUIWindowManager.ActiveWindow);
                    if (DialogProgress.IsCanceled) {
                        HideProgressDialog();
                        OnProgressCancelled();
                    }
                }
            }
            DialogProgress.SetPercentage(progressPercentage);
            DialogProgress.SetLine(2, currentItem);
            DialogProgress.SetLine(3, String.Format("({0} % completed)", progressPercentage));
            DialogProgress.Progress();
        }

        public static void HideProgressDialog() {
            DialogProgress = (GUIDialogProgress)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            DialogProgress.IsVisible = false;
            DialogProgress.ShowWaitCursor = false;
            DialogProgress.Close();
        }

        public static void ShowWaitCursor() {
            if (GUIGraphicsContext.form.InvokeRequired) {
                GUIGraphicsContext.form.Invoke(new System.Action(ShowWaitCursor));
            }

            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
            GUIWindowManager.Process();
        }

        public static void HideWaitCursor() {
            if (GUIGraphicsContext.form.InvokeRequired) {
                GUIGraphicsContext.form.Invoke(new System.Action(HideWaitCursor));
            }
            GUIWaitCursor.Hide();
        }

        #endregion

    }
}
