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
using System.Text;

namespace Unvell.UIControl.PlainGraph
{
	public sealed class PlainGraphFactory
	{
		public static PlainCommonGraph CreatePlainGraph(PlainGraphType type)
		{
			PlainCommonGraph graph;

			switch (type)
			{
				case PlainGraphType.Line:
					graph = new LineGraph();
					break;
				case PlainGraphType.LinePoint:
					graph = new LinePointGraph();
					break;
				default:
				case PlainGraphType.LineArea:
					graph = new LineAreaGraph();
					break;
				case PlainGraphType.Column:
					graph = new ColumnGraph();
					break;
				case PlainGraphType.StackedColumn:
					graph = new StackedColumnGraph();
					break;
				case PlainGraphType.StackedPercentColumn:
					graph = new StackedPercentColumnGraph();
					break;
				case PlainGraphType.Pie:
					graph = new PieGraph();
					break;
				//case PlainGraphType.Pie3D:
				//  graph = new Pie3DGraph();           // not support yet
				//  break;
				//case PlainGraphType.ExplodedPie:
				//  graph = new ExplodedPieGraph();     // not support yet
				//  break;
			}

			return graph;
		}
	}
}
