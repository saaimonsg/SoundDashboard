using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ArlongStreambot.audioplayer.soundboard;
using ArlongStreambot.core;
using log4net;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace ArlongStreambot
{
    public partial class MainFormView : Form
    {
        private ILog _log = LogManager.GetLogger(typeof(MainFormView));
        private WorkPanel _workPanel;

        public MainFormView()
        {
            //TODO resizable childs
            Size = ScreenController.defaultSize;
            ClientSize = ScreenController.defaultSize;
            base.MinimumSize = ScreenController.minimumSize;
            base.MaximumSize = ScreenController.maximumSize;
            ControlBox = true;
            _workPanel = new WorkPanel();
            
            Controls.Add(_workPanel);

            InitializeComponent();
        }
    }
}