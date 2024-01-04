using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExCSS;
using log4net;

namespace ArlongStreambot
{
    public partial class SideNav : FlowLayoutPanel
    {
        private static ILog log = LogManager.GetLogger(typeof(SideNav));

        public SideNav()
        {
            base.BackColor = System.Drawing.Color.DarkSlateGray;
            base.Width = 70;
            InitializeComponent();
        }


        public SideNav(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public SideNav WithSideNavItem(ButtomItem sideNavItem)
        {
            sideNavItem.Location = new System.Drawing.Point(12, 12);
            sideNavItem.Padding = new Padding(12);
            Controls.Add(sideNavItem);
            return this;
        }

        public void AddSideNavItem(ButtomItem sideNavItem)
        {
            Controls.Add(sideNavItem);
        }

     
    }
}