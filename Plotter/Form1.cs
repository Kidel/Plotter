using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

namespace Plotter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Utilities.ResetAllControls(this);
        }

        private void output(string o)
        {
            this.labelOutput.Text = o;
        }

        private void outputSoft(string o)
        {
            if(this.labelOutput.Text == "")
                this.labelOutput.Text = o;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            output("");
            this.plot1.Model = generatePlot();
            outputSoft("Successo: grafo generato. Clicca col tasto destro sul grafo per salvarlo.");
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            output("");
            Utilities.ResetAllControls(this);
        }

        private PlotModel generatePlot()
        {
            var myModel = new PlotModel { Title = this.textBoxTitle.Text };

            myModel.PlotAreaBackground = OxyColor.FromRgb(255,255,255);

            string[] stringSeparators = new string[] { "," };
            string[] pointsXStr = this.textBoxX.Text.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            string[] pointsYStr = this.textBoxY.Text.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            float[] pointsX = new float[pointsXStr.Length];
            float[] pointsY = new float[pointsYStr.Length];
            if (pointsX.Length != pointsY.Length)
            {
                output("Errore: gli assi X e Y hanno un numero di valori diverso tra loro");
                return null;
            }

            try
            {
                for (int i = 0; i < pointsXStr.Length; i++)
                {
                    pointsX[i] = Convert.ToSingle(pointsXStr[i], System.Globalization.CultureInfo.InvariantCulture);
                    pointsY[i] = Convert.ToSingle(pointsYStr[i], System.Globalization.CultureInfo.InvariantCulture);
                    //System.Diagnostics.Debug.WriteLine(pointsXStr[i] + " " + pointsX[i]);
                }

            }
            catch (FormatException)
            {
                output("Errore: uno dei valori inseriti sull'asse x o y non è un numero");
                return null;
            }
            catch (OverflowException)
            {
                output("Errore: uno dei valori inseriti sull'asse x o y è troppo grande");
                return null;
            }

            float[] ranges = new float[4];
            try
            {
                ranges = new float[] {
                    Single.Parse(this.textBoxMinX.Text),
                    Single.Parse(this.textBoxMaxX.Text),
                    Single.Parse(this.textBoxMinY.Text),
                    Single.Parse(this.textBoxMaxY.Text)
                };

                myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = ranges[0], Maximum = ranges[1] });
                myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = ranges[2], Maximum = ranges[3] });
            }
            catch (FormatException)
            {
                output("Nota: uno dei valori inseriti nei range non è un numero, verranno usati automaticamente gli estremi");
            }
            catch (OverflowException)
            {
                output("Nota: uno dei valori inseriti nei range è troppo grande, verranno usati automaticamente gli estremi");
            }

            if(pointsX.Length == 0) output("Nota: nessun valore da disegnare");

            var lineSerie = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                Color = Utilities.MakeColor(this.comboBoxColor.Text),
                MarkerStroke = Utilities.MakeColor(this.comboBoxColor.Text),
                MarkerType = Utilities.MakeMarkerType(this.comboBoxDecoration.Text),
                MarkerFill = Utilities.MakeColor(this.comboBoxColor.Text),
                CanTrackerInterpolatePoints = false,
                Smooth = Utilities.MakeSmoothLine(this.comboBoxLine.Text),
            };

            for (int i = 0; i < pointsX.Length; i++)
            {
                lineSerie.Points.Add(new DataPoint(pointsX[i], pointsY[i]));
            }

            myModel.Series.Add(lineSerie);

            return myModel;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.plot1.Model == null)
            {
                output("Errore: non posso salvare un grafico vuoto");
                return;
            }

            var pngExporter = new PngExporter { Width = this.plot1.Width, Height = this.plot1.Height, Background = OxyColors.White };

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Png Image|*.png";
            saveFileDialog1.Title = "Save a PNG File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                pngExporter.ExportToFile(this.plot1.Model, saveFileDialog1.FileName);
                output("Successo: grafo salvato correttamente in " + saveFileDialog1.FileName);
            }
            else
                output("Errore: il nome del file non può essere nullo");
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.plot1.Model == null)
            {
                output("Errore: non posso copiare un grafico vuoto");
                return;
            }
            var pngExporter = new PngExporter { Width = this.plot1.Width, Height = this.plot1.Height, Background = OxyColors.White };
            var bitmap = pngExporter.ExportToBitmap(this.plot1.Model);
            Clipboard.SetImage(bitmap);
            output("Successo: grafo copiato correttamente. Incollalo in un'applicazione supportata (Word, Paint, ecc)");
        }
    }
}
