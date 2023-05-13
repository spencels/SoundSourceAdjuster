using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;

namespace SoundSourceAdjuster
{
    /// <summary>
    /// Manages the tray icon and its logic.
    /// </summary>
    public class NotifyIconController
    {
        NotifyIcon notifyIcon = null;
        AudioDevicesService audioDevices = new AudioDevicesService();

        private NotifyIconController() { }

        /// <summary>
        /// Creates and initializes and instance of the class.
        /// </summary>
        public static NotifyIconController Create(AudioDevicesService audioDevicesService)
        {
            var controller = new NotifyIconController();
            controller.audioDevices = audioDevicesService;
            var exitMenuItem = new MenuItem()
            {
                Text = "Exit"
            };
            exitMenuItem.Click += new EventHandler(controller.Exit);

            controller.notifyIcon = new NotifyIcon()
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Visible = true,
                Text = "Audio Switcher",
                Icon = new System.Drawing.Icon("NotifyIconController.ico"),
            };
            controller.notifyIcon.Click += new EventHandler(controller.OnClick);

            // Context menu must be populated before first click, or it doesn't show anything.
            controller.PopulateContextMenu(controller.notifyIcon.ContextMenuStrip);
            return controller;
        }

        /// <summary>
        /// Handles Click event.
        /// </summary>
        private void OnClick(object sender, EventArgs e)
        {
            Console.WriteLine("OnClick");
            PopulateContextMenu(notifyIcon.ContextMenuStrip);
        }

        private void PopulateContextMenu(ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Clear();

            var exit = new ToolStripMenuItem("Exit");
            exit.Click += new EventHandler(Exit);

            contextMenu.Items.AddRange(
                audioDevices.EnumerateAudioDevices()
                .Where(x => x.State == DeviceState.Active)
                .Select(x =>
                {
                    var item = new ToolStripMenuItem(x.FullName)
                    {
                        Checked = x.IsDefaultDevice,
                        Tag = x,
                    };
                    item.Click += new EventHandler(
                        (sender, e) =>
                        {
                            x.SetAsDefault();
                            x.SetAsDefaultCommunications();
                        });
                    return item;
                })
                .ToArray());
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                exit,
            });
        }

        private void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
