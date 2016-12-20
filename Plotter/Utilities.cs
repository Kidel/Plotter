using System.Windows.Forms;

namespace Plotter
{
    internal class Utilities
    {
        public static void ResetAllControls(Control form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    textBox.Text = null;
                }

                if (control is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)control;
                    if (comboBox.Items.Count > 0)
                        comboBox.SelectedIndex = 0;
                }

                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.Checked = false;
                }

                if (control is ListBox)
                {
                    ListBox listBox = (ListBox)control;
                    listBox.ClearSelected();
                }
            }
        }

        public static OxyPlot.OxyColor MakeColor(string color)
        {
            if (color == "Blu")
                return OxyPlot.OxyColor.FromRgb(66, 134, 244);
            else if (color == "Arancione")
                return OxyPlot.OxyColor.FromRgb(244, 176, 66);
            else if (color == "Rosso")
                return OxyPlot.OxyColor.FromRgb(244, 66, 66);
            else if (color == "Verde")
                return OxyPlot.OxyColors.Green;
            else //Nero
                return OxyPlot.OxyColors.Black;
        }

        public static OxyPlot.MarkerType MakeMarkerType(string markerType)
        {
            if (markerType == "Cerchi")
                return OxyPlot.MarkerType.Circle;
            else if (markerType == "Quadrati")
                return OxyPlot.MarkerType.Square;
            else if (markerType == "Triangoli")
                return OxyPlot.MarkerType.Triangle;
            else //Nessuna
                return OxyPlot.MarkerType.None;
        } 

        public static bool MakeSmoothLine(string type)
        {
            return type == "Curva";
        }
    }
}