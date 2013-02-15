using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unvell.UIControl.PlainGraph;

namespace Unvell.UIControl.PlainGraphDemo
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			DataSource ds = new DataSource("企業件数レポート");

			DataRecord dr = new DataRecord("中小企業");
			dr.Color = Color.SkyBlue;  // 線の色を青に設定
			dr.LineWeight = 2f; // 線の太さを2F設定

			dr.AddData("2010", 3000); // 2010年3000件
			dr.AddData("2011", 3500); // 2011年3500件
			dr.AddData("2012", 5000); // 2012年5000件

			ds.AddData(dr); // 作成したDataRecordをDataSourceに追加

			// DataRecord を DataSource への簡単追加方法
			ds.AddData("大手企業", new Dictionary<string, double>(){
				{ "2010", 2100 },
				{ "2011", 2700 },
				{ "2012", 2550 },
			}).LineWeight = 3f; // 線の太さを3Fに設定

			graph.DataSource = ds; //DataSourceをChartに設定
			graph.GraphType = PlainGraphType.Column; // カラム形式に変更
		}
	}
}
