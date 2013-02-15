using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unvell.UIControl.PlainGraph;
using System.Drawing.Printing;

namespace Unvell.UIControl.PlainGraphTest
{
	public partial class DemoForm : Form
	{
		public DemoForm()
		{
			InitializeComponent();

			comboBox1.Items.AddRange(Enum.GetNames(typeof(PlainGraphType)));
		}

		private DataSource ds = new DataSource();

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			comboBox1.SelectedIndex = 2;

			ds.Caption = "PlainGraph サンプルチャート";

			ds.XTitle = "年度";
			ds.YTitle = "会社種別";

			// record 1
			Dictionary<int, double> data1 = new Dictionary<int, double>();
			data1.Add(2005, 300);
			data1.Add(2006, 450);
			data1.Add(2007, 500);
			data1.Add(2008, 530);
			data1.Add(2009, 680);
			data1.Add(2010, 890);
			data1.Add(2011, 1330);
			DataRecord record = ds.AddData("個人事業", data1, Color.OliveDrab);

			record.Set[6].Style.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;

			Dictionary<int, double> data2 = new Dictionary<int, double>();
			data2.Add(2005, 110);
			data2.Add(2006, 150);
			data2.Add(2007, 180);
			data2.Add(2008, 378);
			data2.Add(2009, 750);
			data2.Add(2010, 1290);
			data2.Add(2011, 1630);
			ds.AddData("中小企業", data2, Color.Orchid);

			Dictionary<int, double> data3 = new Dictionary<int, double>();
			data3.Add(2005, 320);
			data3.Add(2006, 410);
			data3.Add(2007, 560);
			data3.Add(2008, 595);
			data3.Add(2009, 600);
			data3.Add(2010, 670);
			data3.Add(2011, 820);
			ds.AddData("大手企業", data3, Color.SaddleBrown);

			graph.DataSource = ds;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			graph.GraphType = (PlainGraphType)Enum.GetValues(typeof(PlainGraphType)).GetValue(comboBox1.SelectedIndex);
		}

		private void chkShowLegend_CheckedChanged(object sender, EventArgs e)
		{
			graph.IsShowLegend = chkShowLegend.Checked;
		}

		private void chkShowEntityName_CheckedChanged(object sender, EventArgs e)
		{
			graph.IsShowEntityName = chkShowEntityName.Checked;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutBox().ShowDialog();
		}

		private void printReviewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PrintPreviewDialog ppd = new PrintPreviewDialog()
			{
				Document = new PrintDocument()
			};
			graph.Print(ppd.Document);

			ppd.PrintPreviewControl.Zoom = 1d;

			Rectangle screenSize = Screen.FromControl(this).WorkingArea;
			ppd.SetBounds(50, 50, screenSize.Width / 2, screenSize.Height - 100);

			ppd.ShowDialog();
		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PrintDialog pd = new PrintDialog()
			{
				Document = new PrintDocument(),
			};
			pd.UseEXDialog = true;

			graph.Print(pd.Document);

			pd.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
