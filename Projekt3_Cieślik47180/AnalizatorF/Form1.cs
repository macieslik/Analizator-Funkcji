using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AnalizatorF {
    public partial class mcAnalizator_Funkcji_Fx : Form {
        public mcAnalizator_Funkcji_Fx() {
            InitializeComponent();
            mcCmbStylLiniiWykresu.SelectedIndex = 4;
        }
        private void Form1_Load(object sender, EventArgs e) {
        }
        public static float mcObliczenieFx(float mcX, float mcEps) {
            float mcA = 1.0F;
            float mcSuma = 0.0F;
            int mcK = 0;
            do {
                mcSuma += mcA;
                mcK++;
                mcA *= (-mcX / mcK);
            } while (Math.Abs(mcA) > mcEps);
            return mcSuma;
        }
  

        private void mcBtnObliczWartoscFunkcji_Click(object sender, EventArgs e) {
            float mcEps, mcX; 
            if (string.IsNullOrEmpty(mcTxtX.Text)) {
                mcErrorProvider.SetError(mcTxtX, "ERROR: musisz podać wartość zmiennej niezależnej X");
                return;
            }
            else
                mcErrorProvider.Dispose();
            if (!float.TryParse(mcTxtX.Text, out mcX)) {
                mcErrorProvider.SetError(mcTxtX, "ERROR: wystąpił niedozwolony znak w zapisie wartości X");
                return;
            }
            else
                mcErrorProvider.Dispose();
            if (string.IsNullOrEmpty(mcTxtEps.Text)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: musisz podać dokładność obliczeń Eps");
                return;
            }
            else
                mcErrorProvider.Dispose();
            if (!float.TryParse(mcTxtEps.Text, out mcEps)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: wystąpił niedozwolony znak w zapisie wartości Eps");
                return;
            }
            else
                mcErrorProvider.Dispose();
            if ((mcEps <= 0.0F) || (mcEps >= 1.0F)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: dokładność obliczeń Eps musi spełniać warunek wejściowy" +
                    ": 0.0 < Eps < 1.0");
                return;
            }
            else
                mcErrorProvider.Dispose();
            float mcObliczonaWartoscFx;
            mcObliczonaWartoscFx = mcObliczenieFx(mcX, mcEps);
            mcTxtWartoscFx.Text = Math.Round(mcObliczonaWartoscFx, 3, MidpointRounding.AwayFromZero).ToString();
            mcTxtWartoscFx.Visible = true;
            mcLblWartoscFunkcji.Visible = true;
            mcBtnObliczWartoscFunkcji.Enabled = false;
            mcTxtX.Enabled = false;
            mcTxtEps.Enabled = false;
            mcTxtXd.Enabled = false;
            mcTxtXg.Enabled = false;
            mcTxtH.Enabled = false;
        }
        public bool mcPobierzDaneWyjsciowe(out float mcEps, out float mcXd, out float mcXg, out float mcH) {
            mcEps = 0.0F;
            mcXd = 0.0F;
            mcXg = 0.0F;
            mcH = 0.0F;
            if (string.IsNullOrEmpty(mcTxtEps.Text)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: musisz podać dokładność obliczeń Eps");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (!float.TryParse(mcTxtEps.Text, out mcEps)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: wystąpił niedozwolony znak w zapisie wartości Eps");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if ((mcEps <= 0.0F) || (mcEps >= 1.0F)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: dokładność obliczeń Eps musi spełniać warunek wejściowy" +
                    ": 0.0 < Eps < 1.0");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (string.IsNullOrEmpty(mcTxtXd.Text)) {
                mcErrorProvider.SetError(mcTxtXd, "ERROR: musisz podać wartość Xd (dolnej granicy przedziału wartości X)");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (!float.TryParse(mcTxtXd.Text, out mcXd)) {
                mcErrorProvider.SetError(mcTxtXd, "ERROR: wystąpił niedozwolony znak w zapisie wartości Xd");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (string.IsNullOrEmpty(mcTxtXg.Text)) {
                mcErrorProvider.SetError(mcTxtXg, "ERROR: musisz podać wartość Xg (górnej granicy przedziału wartości X)");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (!float.TryParse(mcTxtXg.Text, out mcXg)) {
                mcErrorProvider.SetError(mcTxtXg, "ERROR: wystąpił niedozwolony znak w zapisie wartości Xg");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (mcXd > mcXg) {
                mcErrorProvider.SetError(mcTxtXg, "ERROR: dolna granica przedziału wartości Xd " +
                    "nie może być większa od górnej granicy przedziału wartości Xg");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (string.IsNullOrEmpty(mcTxtH.Text)) {
                mcErrorProvider.SetError(mcTxtH, "ERROR: musisz podać wartość przyrostu H (krok zmian X)");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if (!float.TryParse(mcTxtH.Text, out mcH)) {
                mcErrorProvider.SetError(mcTxtH, "ERROR: wystąpił niedozwolony znak w zapisie wartości H");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            if ((mcH <= 0.0F) || (mcH >= 1.0F)) {
                mcErrorProvider.SetError(mcTxtEps, "ERROR: przyrost H (krok zmian zmiennej X) musi " +
                    "spełniać warunek wejściowy: 0.0 < H < 1.0");
                return false;
            }
            else
                mcErrorProvider.Dispose();
            return true;
        }
        public void mcTablicowanieWartosciFunkcji(
            ref float[ , ] mcTWF,
            float mcEps, float mcXd, float mcXg, float mcH) {
            float mcX;
            int mcI;
            for (mcX = mcXd, mcI = 0; mcX <= mcXg; mcI++, mcX = mcX + mcH) {
                mcTWF[mcI, 0] = mcX;
                mcTWF[mcI, 1] = mcObliczenieFx(mcX, mcEps);
            }
        }
        private void mcBtnTabelarycznaWizualizacjaFunkcji_Click(object sender, EventArgs e) {
            float mcEps, mcXd, mcXg, mcH;
            if (!mcPobierzDaneWyjsciowe(out mcEps, out mcXd, out mcXg, out mcH))
                return;
            int mcN = (int)((mcXg - mcXd) / mcH) + 1;
            float[,] mcTabelaWartościFunkcji = new float[mcN + 1, 2];
            mcTablicowanieWartosciFunkcji(ref mcTabelaWartościFunkcji, mcEps, mcXd, mcXg, mcH);
            mcPgGrafikaPowitalna.Visible = false;
            mcDgvTabelaFx.Visible = true;
            mcChWykres.Visible = false;
            mcLblWartoscFunkcji.Visible = false;
            mcTxtWartoscFx.Visible = false;
            mcDgvTabelaFx.Rows.Clear();
            mcDgvTabelaFx.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            mcDgvTabelaFx.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int mcI = 0; mcI < mcTabelaWartościFunkcji.GetLength(0); mcI++) {
                mcDgvTabelaFx.Rows.Add();
                mcDgvTabelaFx.Rows[mcI].Cells[0].Value = String.Format("{0:f2}", mcTabelaWartościFunkcji[mcI, 0]);
                mcDgvTabelaFx.Rows[mcI].Cells[1].Value = String.Format("{0:f3}", mcTabelaWartościFunkcji[mcI, 1]);
            }
            mcBtnTabelarycznaWizualizacjaFunkcji.Enabled = false;
            stylCzcionkiToolStripMenuItem.Enabled = true;
            mcTxtX.Enabled = false;
            mcTxtEps.Enabled = false;
            mcTxtXd.Enabled = false;
            mcTxtXg.Enabled = false;
            mcTxtH.Enabled = false;
        }
        private void mcBtnGraficznaWizualizacjaFunkji_Click(object sender, EventArgs e) {
            float mcEps, mcXd, mcXg, mcH;
            if (!mcPobierzDaneWyjsciowe(out mcEps, out mcXd, out mcXg, out mcH))
                return;
            int mcN = (int)((mcXg - mcXd) / mcH) + 1;
            float[,] mcTabelaWartościFunkcji = new float[mcN + 1, 2];
            mcTablicowanieWartosciFunkcji(ref mcTabelaWartościFunkcji, mcEps, mcXd, mcXg, mcH);
            mcPgGrafikaPowitalna.Visible = false;
            mcDgvTabelaFx.Visible = false;
            mcChWykres.Visible = true;
            mcLblWartoscFunkcji.Visible = false;
            mcTxtWartoscFx.Visible = false;
            mcChWykres.Titles.Add("Wykres zmian wartości funkcji");
            mcChWykres.Legends.FindByName("Legend1").Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            mcChWykres.BackColor = Color.Beige;
            mcChWykres.ChartAreas[0].AxisX.Title = "Wartość X";
            mcChWykres.ChartAreas[0].AxisY.Title = "Wartość F(X)";
            mcChWykres.ChartAreas[0].AxisX.LabelStyle.Format = "{0:f2}";
            mcChWykres.ChartAreas[0].AxisY.LabelStyle.Format = "{0:f2}";
            mcChWykres.Series.Clear();
            mcChWykres.Series.Add("Seria 0");
            mcChWykres.Series[0].XValueMember = "X";
            mcChWykres.Series[0].YValueMembers = "Y";
            mcChWykres.Series[0].IsVisibleInLegend = true;
            mcChWykres.Series[0].Name = "Wartość funkcji F(X)";
            mcChWykres.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            mcChWykres.Series[0].Color = Color.Black;
            mcChWykres.Series[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            mcChWykres.Series[0].BorderWidth = 1;
            for (int mcI = 0; mcI < mcTabelaWartościFunkcji.GetLength(0); mcI++) 
                mcChWykres.Series[0].Points.AddXY(mcTabelaWartościFunkcji[mcI, 0], mcTabelaWartościFunkcji[mcI, 1]);
            mcBtnGraficznaWizualizacjaFunkcji.Enabled = false;
            mcBtnWybierzKolorLinii.Enabled = true;
            mcBtnWybierzKolorTla.Enabled = true;
            mcTrackBarGruboscLinii.Enabled = true;
            mcTxtGruboscLinii.ReadOnly = false;
            mcCmbStylLiniiWykresu.Enabled = true;
            mcRdbOsieUkladuWspolrzednychBezOpisu.Enabled = true;
            mcRdbOsieUkladuWspolrzednychZOpisem.Enabled = true;
            kolorTłaWykresuToolStripMenuItem.Enabled = true;
            kolorLiniiWykresuToolStripMenuItem.Enabled = true;
            styleLiniiToolStripMenuItem.Enabled = true;
            stylCzcionkiToolStripMenuItem.Enabled = false;
            grubośćLiniiToolStripMenuItem.Enabled = true;
            typWykresuToolStripMenuItem.Enabled = true;
            mcTxtX.Enabled = false;
            mcTxtEps.Enabled = false;
            mcTxtXd.Enabled = false;
            mcTxtXg.Enabled = false;
            mcTxtH.Enabled = false;
        }
        private void mcBtnKolorLinii_Click(object sender, EventArgs e) {
            if (mcColorDialog.ShowDialog() == DialogResult.OK) {
                mcChWykres.Series[0].Color = mcColorDialog.Color;
            }
            mcTxtWybranyKolorLinii.BackColor = mcChWykres.Series[0].Color;
        }
        private void mcBtnWybierzKolorTla_Click(object sender, EventArgs e) {
            if (mcColorDialog.ShowDialog() == DialogResult.OK) {
                mcChWykres.ChartAreas[0].BackColor = mcColorDialog.Color;
            }
            mcTxtWybranyKolorTla.BackColor = mcChWykres.ChartAreas[0].BackColor;
        }
        private void mcTrackBarGruboscLinii_Scroll(object sender, EventArgs e) {
            mcTxtGruboscLinii.Text = mcTrackBarGruboscLinii.Value.ToString();
            mcChWykres.Series[0].BorderWidth = mcTrackBarGruboscLinii.Value;
        }
        private void kolorTłaWykresuToolStripMenuItem_Click(object sender, EventArgs e) {
            if (mcColorDialog.ShowDialog() == DialogResult.OK) {
                mcChWykres.ChartAreas[0].BackColor = mcColorDialog.Color;
            }
            mcTxtWybranyKolorTla.BackColor = mcChWykres.ChartAreas[0].BackColor;
        }
        private void kolorLiniiWykresuToolStripMenuItem_Click(object sender, EventArgs e) {
            if (mcColorDialog.ShowDialog() == DialogResult.OK) {
                mcChWykres.Series[0].Color = mcColorDialog.Color;
            }
            mcTxtWybranyKolorLinii.BackColor = mcChWykres.Series[0].Color;
        }
        private void kropkowaToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
        }
        private void kreskowaToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
        }
        private void kreskowokropkowaToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
        }
        private void kreskowokropkowokropkowaToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
        }
        private void ciągłaToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 1;
            mcTrackBarGruboscLinii.Value = 1;
            mcTxtGruboscLinii.Text = "1";
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 2;
            mcTrackBarGruboscLinii.Value = 2;
            mcTxtGruboscLinii.Text = "2";
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 3;
            mcTrackBarGruboscLinii.Value = 3;
            mcTxtGruboscLinii.Text = "3";
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 4;
            mcTrackBarGruboscLinii.Value = 4;
            mcTxtGruboscLinii.Text = "4";
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 5;
            mcTrackBarGruboscLinii.Value = 5;
            mcTxtGruboscLinii.Text = "5";
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 6;
            mcTrackBarGruboscLinii.Value = 6;
            mcTxtGruboscLinii.Text = "6";
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 7;
            mcTrackBarGruboscLinii.Value = 7;
            mcTxtGruboscLinii.Text = "7";
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 8;
            mcTrackBarGruboscLinii.Value = 8;
            mcTxtGruboscLinii.Text = "8";
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 9;
            mcTrackBarGruboscLinii.Value = 9;
            mcTxtGruboscLinii.Text = "9";
        }
        private void toolStripMenuItem11_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].BorderWidth = 10;
            mcTrackBarGruboscLinii.Value = 10;
            mcTxtGruboscLinii.Text = "10";
        }
        private void mcCmbStylLiniiWykresu_SelectedIndexChanged(object sender, EventArgs e) {
            ChartDashStyle mcWybranyStylLinii() {
                switch (mcCmbStylLiniiWykresu.SelectedIndex) {
                    case 0: return ChartDashStyle.Dash;
                    case 1: return ChartDashStyle.DashDot;
                    case 2: return ChartDashStyle.DashDotDot;
                    case 3: return ChartDashStyle.Dot;
                    case 4: return ChartDashStyle.Solid;
                    default: return ChartDashStyle.Solid;
                }
            }
            mcChWykres.Series[0].BorderDashStyle = mcWybranyStylLinii();           
        }
        private void mcTxtGruboscLinii_TextChanged(object sender, EventArgs e) {
            mcTrackBarGruboscLinii.Value = int.Parse(mcTxtGruboscLinii.Text);
            mcChWykres.Series[0].BorderWidth = mcTrackBarGruboscLinii.Value;
        }
        private void mcRdbOsieUkladuWspolrzednychBezOpisu_CheckedChanged(object sender, EventArgs e) {
            mcChWykres.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            mcChWykres.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            mcChWykres.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.None;
            mcChWykres.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.None;
        }

        private void mcRdbOsieUkladuWspolrzednychZOpisem_CheckedChanged(object sender, EventArgs e) {
            mcChWykres.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
            mcChWykres.ChartAreas[0].AxisY.LabelStyle.Enabled = true;
            mcChWykres.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            mcChWykres.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
        }

        private void liniowyToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].ChartType = SeriesChartType.Line;
        }

        private void kolumnowyToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].ChartType = SeriesChartType.Column;
        }

        private void słupkowyToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].ChartType = SeriesChartType.Bar;
        }

        private void punktowyToolStripMenuItem_Click(object sender, EventArgs e) {
            mcChWykres.Series[0].ChartType = SeriesChartType.Point;
        }
        private void stylCzcionkiToolStripMenuItem_Click(object sender, EventArgs e) {
            FontDialog mcOknoFormatowaniaCzcionki = new FontDialog();
            if (mcOknoFormatowaniaCzcionki.ShowDialog() == DialogResult.OK)
                mcDgvTabelaFx.Font = mcOknoFormatowaniaCzcionki.Font;
        }
        private void kolorCzcionkiToolStripMenuItem_Click(object sender, EventArgs e) {
            if (mcColorDialog.ShowDialog() == DialogResult.OK) {
                this.ForeColor = mcColorDialog.Color;
            }
            foreach (Control mcKontrolka in this.Controls) {
                mcKontrolka.ForeColor = mcColorDialog.Color;
            }
        }
        private void mcBtnResetuj_Click(object sender, EventArgs e) {
            mcTxtX.Enabled = true;
            mcTxtX.Text = "";
            mcTxtEps.Enabled = true;
            mcTxtEps.Text = "";
            mcTxtXd.Enabled = true;
            mcTxtXd.Text = "";
            mcTxtXg.Enabled = true;
            mcTxtXg.Text = "";
            mcTxtH.Enabled = true;
            mcTxtH.Text = "";
            mcDgvTabelaFx.Visible = false;
            mcChWykres.Visible = false;
            mcPgGrafikaPowitalna.Visible = true;
            mcBtnWybierzKolorLinii.Enabled = false;
            mcBtnWybierzKolorTla.Enabled = false;
            mcBtnObliczWartoscFunkcji.Enabled = true;
            mcBtnTabelarycznaWizualizacjaFunkcji.Enabled = true;
            mcBtnGraficznaWizualizacjaFunkcji.Enabled = true;
            mcBtnResetuj.Enabled = true;
            mcTrackBarGruboscLinii.Enabled = false;
            mcTxtGruboscLinii.ReadOnly = true;
            mcTxtWybranyKolorLinii.BackColor = Color.WhiteSmoke;
            mcTxtWybranyKolorLinii.Enabled = false;
            mcTxtWybranyKolorTla.BackColor = Color.WhiteSmoke;
            mcTxtWybranyKolorTla.Enabled = false;
            mcCmbStylLiniiWykresu.Enabled = false;
            mcRdbOsieUkladuWspolrzednychBezOpisu.Enabled = false;
            mcRdbOsieUkladuWspolrzednychBezOpisu.Checked = false;
            mcRdbOsieUkladuWspolrzednychZOpisem.Enabled = false;
            mcRdbOsieUkladuWspolrzednychZOpisem.Checked = false;
            kolorTłaWykresuToolStripMenuItem.Enabled = false;
            kolorLiniiWykresuToolStripMenuItem.Enabled = false;
            styleLiniiToolStripMenuItem.Enabled = false;
            stylCzcionkiToolStripMenuItem.Enabled = false;
            grubośćLiniiToolStripMenuItem.Enabled = false;
            typWykresuToolStripMenuItem.Enabled = false;
            mcChWykres.Titles.Clear();
            mcChWykres.Series[0].Points.Clear();
            mcDgvTabelaFx.Rows.Clear();
            mcTrackBarGruboscLinii.Value = 1;
            mcTxtGruboscLinii.Text = "1";
        }
        private void mcAnalizator_Funkcji_Fx_FormClosing(object sender, FormClosingEventArgs e) {
            DialogResult mcPytanie = MessageBox.Show("Czy na pewno chcesz zakończyć działanie programu?", this.Text, 
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            if (mcPytanie == DialogResult.Yes) {
                e.Cancel = false;
                MessageBox.Show("Program wykonał: Michał Cieślik, Numer albumu: 47180");
            }
            else if (mcPytanie == DialogResult.No) {
                e.Cancel = true;
            }
            else 
                e.Cancel = true;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
