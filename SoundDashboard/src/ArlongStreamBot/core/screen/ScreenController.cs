using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ArlongStreambot.core
{
    /**
     * Controlador para la pantalla principal
     */
    public class ScreenController
    {
        private static int Height => 600 - 39;

        private static int Width => 800 - 16;

        public static Size minimumSize = new Size(Width, Height);

        public static Size maximumSize = new Size(Width, Height);
        public static Size defaultSize = new Size(Width, Height);
    }
}