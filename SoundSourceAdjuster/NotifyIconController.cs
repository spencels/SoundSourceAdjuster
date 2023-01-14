using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;

namespace SoundSourceAdjuster
{
    public class NotifyIconController
    {
        NotifyIcon notifyIcon = null;
        AudioDevicesService audioDevices = new AudioDevicesService();

        private NotifyIconController() { }

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
            controller.notifyIcon.Click += new EventHandler(controller.ContextMenuPopup);

            return controller;
        }

        private void ContextMenuPopup(object sender, EventArgs e)
        {
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
                    var item = new ToolStripMenuItem(x.Name)
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
