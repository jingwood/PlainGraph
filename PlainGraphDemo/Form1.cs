///////////////////////////////////////////////////////////////////////////////
// 
// PlainGraph
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//
// License: GNU Lesser General Public License (LGPLv3)
//
// Jing, Lu (lujing@unvell.com)
//
// Copyright (C) unvell.com, 2013. All Rights Reserved
//
///////////////////////////////////////////////////////////////////////////////

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

			// new data source
			DataSource ds = new DataSource("PlainGraph Sample");

			// new record
			DataRecord dr = new DataRecord("DVD");
			dr.Color = Color.SkyBlue; 
			dr.LineWeight = 2f;       // line weight

			dr.AddData("2010", 3000);
			dr.AddData("2011", 3500);
			dr.AddData("2012", 5000);

			// add record
			ds.AddData(dr); 

			// simple way to create record and add
			ds.AddData("Book", new Dictionary<string, double>(){
				{ "2010", 2100 },
				{ "2011", 2700 },
				{ "2012", 2550 },
			}).LineWeight = 3f;

			graph.DataSource = ds; // update data source
			graph.GraphType = PlainGraphType.Column; // change chart type
		}
	}
}
