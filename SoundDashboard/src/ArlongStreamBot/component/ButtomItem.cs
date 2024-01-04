using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ArlongStreambot.core;
using log4net;
using SkiaSharp;
using Button = System.Windows.Forms.Button;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace ArlongStreambot
{
    public partial class ButtomItem : Button
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ButtomItem));

        public ButtomItem()
        {
            InitializeComponent();
        }

        public ButtomItem(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public ButtomItem(string text, Panel form)
        {
            base.Enabled = true;
            base.Text = text;
            base.Height = 64;
            base.Width = 64;
            base.Padding = new Padding();
            base.Name = form.Name + "Button";
            base.Click += (sender, args) => { form.Show(); };
            try
            {
                WithSvg(text.ToLower());
            }
            catch 
            {
                
                base.BackgroundImage = ResourceHandler.LoadImage("default.png");
            }

            base.BackgroundImageLayout = ImageLayout.Zoom;
            InitializeComponent();
        }

        public ButtomItem WithSvg(string svgName)
        {
            base.BackgroundImage = ResourceHandler.LoadSvg($"{svgName}.svg").First();
            base.BackgroundImageLayout = ImageLayout.Zoom;
            return this;
        }

    }
}