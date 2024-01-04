using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ArlongStreambot.audioplayer.soundboard;
using ArlongStreambot.core;
using ExCSS;
using log4net;
using Color = System.Drawing.Color;

namespace ArlongStreambot
{
    public partial class WorkPanel : Panel
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WorkPanel));

        private AudioPlayerCore _audioPlayerCore;


        public WorkPanel()
        {
            _audioPlayerCore = new AudioPlayerCore();
            base.Name = "workpanel";
            base.Location = new System.Drawing.Point(74, 0);
            base.Size = new System.Drawing.Size(ScreenController.minimumSize.Width - 74,
                ScreenController.minimumSize.Height);
  
            Controls.Add(_audioPlayerCore._audioPlayerView.GetInputDevicesLayout());
            Controls.Add(_audioPlayerCore._audioPlayerView.GetOutputDevicesLayout());
            FlowLayoutPanel audioPlayerButtonsLayout = _audioPlayerCore._audioPlayerView.GetAudioPlayerButtonsLayout();
            
            Controls.Add(audioPlayerButtonsLayout);
            InitializeComponent();
        }

        public WorkPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}