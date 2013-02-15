using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Unvell.UIControl.PlainGraph
{
	#region Graph Control Host
	public partial class GraphControl : Control
	{
		private PlainGraphType graphType = PlainGraphType.Line;

		/// <summary>
		/// Specify which one of Chart type should be displayed
		/// </summary>
		public PlainGraphType GraphType
		{
			get { return graphType; }
			set
			{
				if (graphType != value)
				{
					graphType = value;

					if (graphType != lastGraphType)
					{
						graph = PlainGraphFactory.CreatePlainGraph(graphType);

						graph.Margin = graphMargin;
						graph.IsShowLegend = isShowLegend;
						graph.Bounds = ClientRectangle;
						graph.DataSource = DataSource;
						lastGraphType = graphType;
						Invalidate();
					}
				}
			}
		}

		private PlainGraphType lastGraphType = PlainGraphType.Line;

		private PlainCommonGraph graph = new LineGraph();

		/// <summary>
		/// PlainGraph core object to render chart
		/// </summary>
		public PlainCommonGraph Graph
		{
			get { return graph; }
		}

		#region Settings

		private bool isAntiAlias = true;

		[DefaultValue(true)]
		public bool IsAntiAlias
		{
			get { return isAntiAlias; }
			set { isAntiAlias = value;
			Invalidate();
			}
		}

		private bool isShowLegend = true;

		/// <summary>
		/// Specify whether legend should be displayed 
		/// </summary>
		[DefaultValue(true)]
		public bool IsShowLegend
		{
			get { return graph.IsShowLegend; }
			set
			{
				isShowLegend = graph.IsShowLegend = value;
				graph.UpdateBounds(ClientRectangle);
				Invalidate();
			}
		}

		private bool isShowEntityName = true;

		/// <summary>
		/// Specify whether the name of data entity should be displayed
		/// </summary>
		[DefaultValue(false)]
		public bool IsShowEntityName
		{
			get { return graph.IsShowEntityName; }
			set
			{
				if (isShowEntityName != value)
				{
					isShowEntityName = graph.IsShowEntityName = value;
					Invalidate();
				}
			}
		}

		private bool isShowDataTip = false;

		/// <summary>
		/// Specify whether tip of data should be displayed
		/// </summary>
		[DefaultValue(false)]
		public bool IsShowDataTip
		{
			get { return graph.IsShowDataTip; }
			set
			{
				if (isShowDataTip != value)
				{
					isShowDataTip = graph.IsShowDataTip = value;
					Invalidate();
				}
			}
		}

		private bool isShowDataValue = false;

		/// <summary>
		/// Specify whether value of data should be displayed
		/// </summary>
		[DefaultValue(false)]
		public bool IsShowDataValue
		{
			get { return graph.IsShowDataValue; }
			set
			{
				if (isShowDataValue != value)
				{
					isShowDataValue = graph.IsShowDataValue = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Font of x-axis ruler
		/// </summary>
		public Font XRulerFont
		{
			get { return graph.XRulerFont; }
			set { graph.XRulerFont = value; Invalidate(); }
		}

		/// <summary>
		/// Font of y-axis ruler
		/// </summary>
		public Font YRulerFont
		{
			get { return graph.YRulerFont; }
			set { graph.YRulerFont = value; Invalidate(); }
		}

		/// <summary>
		/// Font of legend
		/// </summary>
		public Font LegendFont
		{
			get { return graph.LegendFont; }
			set { graph.LegendFont = value; Invalidate(); }
		}

		private Padding graphMargin;

		/// <summary>
		/// Margin for chart
		/// </summary>
		public Padding GraphMargin
		{
			get { return graphMargin = graph.Margin; }
			set { graphMargin = graph.Margin = value;
			graph.UpdateBounds(ClientRectangle);
			Invalidate();
			}
		}
		#endregion

		#region Constructor
		public GraphControl()
		{
			InitializeComponent();

			BackColor = Color.White;
			DoubleBuffered = true;
		}
		#endregion

		protected override void OnCreateControl()
		{
			graph.Bounds = ClientRectangle;
			base.OnCreateControl();
		}

		private DataSource dataSource;

		/// <summary>
		/// Data source to render chart
		/// </summary>
		public DataSource DataSource
		{
			get { return dataSource; }
			set
			{
				dataSource = value;
				graph.DataSource = value;
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Draw(pe.Graphics, pe.ClipRectangle);
		}

		/// <summary>
		/// Print chart. currenly print format is not supported
		/// </summary>
		/// <param name="doc"></param>
		public void Print(PrintDocument doc)
		{
			doc.PrintPage += (sender, e) =>
			{
				Draw(e.Graphics, e.MarginBounds);
			};
		}

		internal void Draw(Graphics g, Rectangle clip)
		{
			graph.Font = Font;

			SmoothingMode oldSmoothingMode = SmoothingMode.Default;
			if (isAntiAlias)
			{
				oldSmoothingMode = g.SmoothingMode;
				g.SmoothingMode = SmoothingMode.AntiAlias;
			}
			graph.Draw(g, clip);
			if (isAntiAlias)
			{
				g.SmoothingMode = oldSmoothingMode;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			graph.UpdateBounds(ClientRectangle);
			Invalidate();
		}
	}
	#endregion

	#region Common Graph
	public enum PlainGraphType
	{
		Line,
		LinePoint,
		LineArea,
		Column,
		StackedColumn,
		StackedPercentColumn,
		Pie,
		//Pie3D,
		//ExplodedPie,
	}

	public abstract class PlainCommonGraph
	{
		#region Border Attributes
		private Rectangle bounds;

		public Rectangle Bounds
		{
			get { return bounds; }
			set {
				UpdateBounds(value);
			}
		}

		private Rectangle borderBounds;

		public Rectangle BorderBounds
		{
		  get { return borderBounds; }
		  set { borderBounds = value; }
		}

		private Rectangle captionBounds;

		public Rectangle CaptionBounds
		{
			get { return captionBounds; }
			set { captionBounds = value; }
		}

		private Rectangle graphBounds;

		public Rectangle GraphBounds
		{
			get { return graphBounds; }
			set { graphBounds = value; }
		}

		private Rectangle legendBounds;

		public Rectangle LegendBounds
		{
			get { return legendBounds; }
			set { legendBounds = value; }
		}
		
		private Padding margin;

		public Padding Margin
		{
			get { return margin; }
			set { margin = value; }
		}
		#endregion

		#region UI Attributes
		private Font font = SystemFonts.DefaultFont;

		public Font Font
		{
			get { return font; }
			set { font = value; }
		}

		private Color captionColor;

		public Color CaptionColor
		{
			get { return captionColor; }
			set { captionColor = value; }
		}

		private Font titleFont = new Font(SystemFonts.DefaultFont.FontFamily, 14f, FontStyle.Bold);

		public Font TitleFont
		{
			get { return titleFont; }
			set { titleFont = value; }
		}

		private Font xRulerFont = SystemFonts.DefaultFont;

		public Font XRulerFont
		{
			get { return xRulerFont; }
			set { xRulerFont = value; }
		}

		private Font yRulerFont = SystemFonts.DefaultFont;

		public Font YRulerFont
		{
			get { return yRulerFont; }
			set { yRulerFont = value; }
		}

		private Font legendFont = SystemFonts.DefaultFont;

		public Font LegendFont
		{
			get { return legendFont; }
			set { legendFont = value; }
		}

		private int legendWidth = 80;

		public int LegendWidth
		{
			get { return legendWidth; }
			set { legendWidth = value; }
		}
		#endregion

		#region Behavior Attributes

		private bool isShowLegend = true;

		public bool IsShowLegend
		{
			get { return isShowLegend; }
			set { isShowLegend = value; }
		}

		private bool isShowEntityName = false;

		public bool IsShowEntityName
		{
			get { return isShowEntityName; }
			set { isShowEntityName = value; }
		}

		private bool isShowDataTip = false;

		public bool IsShowDataTip
		{
			get { return isShowDataTip; }
			set { isShowDataTip = value; }
		}

		private bool isShowDataValue = false;

		public bool IsShowDataValue
		{
			get { return isShowDataValue; }
			set { isShowDataValue = value; }
		}

		//private bool isShowPosAssistLineX = false;
		//private bool isShowPosAssistLineY = false;

		#endregion

		#region Constructor
		public PlainCommonGraph()
		{
		}

		public PlainCommonGraph(Rectangle bounds)
		{
			this.Bounds = bounds;
		}
		#endregion

		#region Data Attributes
		private DataSource dataSource;

		public DataSource DataSource
		{
			get { return dataSource; }
			set { UpdateDataSource(value); }
		}

		private PlainGraphDisplayFormat keyDisplayFormat = PlainGraphDisplayFormat.Integer;

		public PlainGraphDisplayFormat KeyDisplayFormat
		{
			get { return keyDisplayFormat; }
			set { keyDisplayFormat = value; }
		}

		private PlainGraphDisplayFormat valueDisplayFormat = PlainGraphDisplayFormat.Integer;

		public PlainGraphDisplayFormat ValueDisplayFormat
		{
			get { return valueDisplayFormat; }
			set { valueDisplayFormat = value; }
		}

		protected DataInfo recordInfo = new DataInfo();

		private List<string> xDataKeys = new List<string>();

		public List<string> XDataKeys
		{
			get { return xDataKeys; }
			set { xDataKeys = value; }
		}

		private Dictionary<Color, string> legends = new Dictionary<Color, string>();

		public Dictionary<Color, string> Legends
		{
			get { return legends; }
			set { legends = value; }
		}

		#endregion

		#region Update
		public virtual void UpdateBounds(Rectangle newBounds)
		{
			bounds = newBounds;

			borderBounds = new Rectangle(bounds.Left + margin.Left, bounds.Top + margin.Top,
				bounds.Right - margin.Right - margin.Left, bounds.Bottom - margin.Bottom - margin.Top);

			captionBounds = new Rectangle(borderBounds.Left + 20, borderBounds.Top + 10, borderBounds.Width - 20, 24);

			if (isShowLegend)
			{
				legendBounds = new Rectangle(borderBounds.Right - legendWidth - 10,
					captionBounds.Bottom + 10, legendWidth,
					borderBounds.Height - captionBounds.Height - 10);
			}
			else
			{
				legendBounds = Rectangle.Empty;
			}

			graphBounds = new Rectangle(borderBounds.Left + 10, 
				CaptionBounds.Bottom + 10,
				borderBounds.Width - legendBounds.Width - 30,
				borderBounds.Height - captionBounds.Bottom - 10);

			OnUpdateBounds(borderBounds);
		}

		protected abstract void OnUpdateBounds(Rectangle bounds);

		public void UpdateDataSource(DataSource dataSource)
		{
			this.dataSource = dataSource;

			recordInfo = new DataInfo();

			// preprocess
			if (dataSource != null && dataSource.Records != null && dataSource.Records.Count != 0)
			{
				DataRecord maxSetRecord = null;
				bool autoFindSetKeys = DataSource.XDataKeys == null || DataSource.XDataKeys.Count == 0;

				recordInfo.maxEntityCount = dataSource.Records.Max(r => r.Set == null ? 0 : r.Set.Count());
				recordInfo.columnTotal = new double[recordInfo.maxEntityCount];
				recordInfo.columnMax = new double[recordInfo.maxEntityCount];
				recordInfo.columnMin = new double[recordInfo.maxEntityCount];

				recordInfo.recordCount = dataSource.Records.Count;
				recordInfo.recordTotal = new double[recordInfo.recordCount];
				recordInfo.recordMax = new double[recordInfo.recordCount];
				recordInfo.recordMin = new double[recordInfo.recordCount];

				if (DataSource.Records != null)
				{
					for (int r = 0; r < DataSource.Records.Count; r++)
					{
						DataRecord row = DataSource.Records[r];

						if (autoFindSetKeys && (maxSetRecord == null || row.Set.Count > maxSetRecord.Set.Count)) maxSetRecord = row;

						double recordTotal = 0;

						for (int i = 0; i < row.Set.Count; i++)
						{
							DataEntity entity = row.Set[i];

							if ((this is PieGraph))
							{
								if (entity.Style == null) entity.Style = new DataEntityStyle();
								if (entity.Style.Color.IsEmpty && (this is PieGraph))
								{
									entity.Style.Color = PlainGraphToolkit.GetRandomColor();
								}
							}
						
							recordTotal += entity.Value;

							recordInfo.columnTotal[i] += entity.Value;
							recordInfo.columnTotalMax = Math.Max(recordInfo.columnTotalMax, recordInfo.columnTotal[i]);

							recordInfo.columnMax[i] = Math.Max(recordInfo.columnMax[i], entity.Value);
							recordInfo.columnMin[i] = Math.Min(recordInfo.columnMin[i], entity.Value);

							recordInfo.maxValue = Math.Max(recordInfo.maxValue, entity.Value);
							recordInfo.minValue = Math.Min(recordInfo.minValue, entity.Value);
						}

						recordInfo.recordTotal[r] = recordTotal;
						recordInfo.recordMax[r] = Math.Max(recordInfo.recordMax[r], recordTotal);
						recordInfo.recordMin[r] = Math.Min(recordInfo.recordMin[r], recordTotal);

						recordInfo.total += recordTotal;
					}
				}

				if (autoFindSetKeys && maxSetRecord != null)
					XDataKeys = (from r in maxSetRecord.Set select r.Key.ToString()).ToList();
				else
					XDataKeys = DataSource.XDataKeys;

			}
			else
			{
				recordInfo.recordTotal = new double[0];
				recordInfo.recordCount = 0;
			}


			//UpdateBounds(bounds);
			OnUpdateDataSource();

			legends.Clear();
			SelectLegends();
		}

		protected abstract void OnUpdateDataSource();

		protected abstract void SelectLegends();

		#endregion

		#region Draw

		public void DrawToImage(Image img, bool isAntiAlias)
		{
			Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
			UpdateBounds(rect);
			using (Graphics g = Graphics.FromImage(img))
			{
				if (isAntiAlias)
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				}
				Draw(g, rect);
			}
		}

		internal virtual void Draw(Graphics g, Rectangle clip)
		{
			if (bounds.Width == 0 || bounds.Height == 0) return;

			// draw border
			using (Pen p = new Pen(Color.Gray))
			{
				p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

				p.Color = Color.Black;
				//g.DrawRectangle(p, borderBounds);

				p.Color = Color.Gray;
				//g.DrawRectangle(p, captionBounds);
				//g.DrawRectangle(p, graphBounds);
				//g.DrawRectangle(p, rowTitleBounds);
				//g.DrawRectangle(p, columnTitleBounds);
				//g.DrawRectangle(p, legendBounds);
			}

			DrawGraph(g);
			if (isShowLegend) DrawLegend(g);

			if (dataSource != null && !string.IsNullOrEmpty(dataSource.Caption))
			{
				g.DrawString(dataSource.Caption, titleFont, Brushes.Black, captionBounds);
			}
		}
		
		protected abstract void DrawGraph(Graphics g);

		protected virtual void DrawLegend(Graphics g)
		{
			if(dataSource==null) return;

			int x = legendBounds.Left;

			int totalHeight = 0;

			Rectangle[] itemRects = new Rectangle[legends.Count];
			Rectangle[] colorRects = new Rectangle[legends.Count];

			int i=0;
			foreach (Color c in legends.Keys)
			{
				itemRects[i] = new Rectangle(legendBounds.Left, 0, legendBounds.Width, 20);

				SizeF legendStrSize = g.MeasureString(legends[c], font, legendBounds.Width - 20);
				int height = (int)legendStrSize.Height;
				if (height < 20) height = 20;
				itemRects[i].Height = height;
				totalHeight += height;

				colorRects[i] = new Rectangle(itemRects[i].Left, 0, 12, 12);

				i++;
			}

			i=0;

			int y = legendBounds.Top + (legendBounds.Height - totalHeight) / 2;
			foreach (Color c in legends.Keys)
			{
				itemRects[i].Y = y;
				colorRects[i].Y = y;

				Rectangle textRect = new Rectangle(colorRects[i].Right + 2, y, legendBounds.Width, itemRects[i].Height);
				using (Brush b = new SolidBrush(c))
				{
					g.FillRectangle(b, colorRects[i]);
				}
				using (StringFormat sf = new StringFormat())
				{
					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Near;
					g.DrawString(legends[c], font, Brushes.Black, textRect);
				}

				y += itemRects[i].Height;
			}
		}
		#endregion

		#region Behavior
		public void OnMouseMove(MouseEventArgs e)
		{
			DoMouseMove(e);
		}

		public void OnMouseUp(MouseEventArgs e)
		{
		}

		public void OnMouseDown(MouseEventArgs e)
		{
		}

		internal virtual void DoMouseMove(MouseEventArgs e) { }
		internal virtual void DoMouseUp(MouseEventArgs e) { }
		internal virtual void DoMouseDown(MouseEventArgs e) { }
		#endregion
	}
	#endregion

	#region Utilities
	public struct DataInfo
	{
		internal int maxEntityCount;
		internal int recordCount;

		internal double[] columnTotal;
		internal double[] columnMax;
		internal double[] columnMin;

		internal double[] recordTotal;
		internal double[] recordMax;
		internal double[] recordMin;

		internal double total;
		internal double maxValue;
		internal double minValue;

		internal double columnTotalMax;

		internal double yRuleMax;
		internal double yRuleMin;
	}

	public class PlainGraphToolkit
	{
		public static readonly Color[] validColors = new Color[] { Color.Orange, Color.DarkBlue, Color.DarkGreen, 
			Color.Magenta, Color.LimeGreen, Color.DimGray, Color.SkyBlue, Color.Pink, Color.RosyBrown};
		public static Color GetUnusedColor(DataSource dataSource)
		{
			Color color = new List<Color>(validColors).FirstOrDefault(c => dataSource.Records.FirstOrDefault(r => r.Color == c) == null);
			if (color.IsEmpty) color = Color.Black;
			return color;
		}
		private static readonly Random rand = new Random();
		public static Color GetRandomColor()
		{
			return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
		}
		public static Color GetRandomDarkColor()
		{
			return Color.FromArgb(rand.Next(200), rand.Next(200), rand.Next(200));
		}

		internal static int CalcFator(int v, int lvl)
		{
			int a = v.ToString().Length;
			int d = (v / lvl).ToString().Length;
			int b = (int)(Math.Pow(10, d - 1));
			int c = v / lvl;
			return (c - c % b);
		}
	}

	public enum PlainGraphDisplayFormat
	{
		Integer,
		Float2,
		Double,
		Percent,
	}
	#endregion

	#region CoordinateGraph
	public abstract class CoordinateGraph : PlainCommonGraph
	{
		private Point origin = Point.Empty;

		public Point Origin
		{
			get { return origin; }
			set { origin = value; }
		}

		private bool yAssistLine = true;

		public bool YAssistLine
		{
			get { return yAssistLine; }
			set { yAssistLine = value; }
		}

		private bool xAssistLine = true;

		public bool XAssistLine
		{
			get { return xAssistLine; }
			set { xAssistLine = value; }
		}

		#region Border Attributes
		private Rectangle rowTitleBounds;

		public Rectangle RowTitleBounds
		{
			get { return rowTitleBounds; }
			set { rowTitleBounds = value; }
		}

		private Rectangle columnTitleBounds;

		public Rectangle ColumnTitleBounds
		{
			get { return columnTitleBounds; }
			set { columnTitleBounds = value; }
		}
		#endregion

		#region Drawing
		protected override void DrawGraph(Graphics g)
		{
			// draw border
			//using (Pen p = new Pen(Color.Gray))
			//{
			//  p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			//  p.Color = Color.Blue;
			//  g.DrawRectangle(p, rowTitleBounds);
			//  g.DrawRectangle(p, columnTitleBounds);
			//}
			//MessageBox.Show(string.Format("DPIX: {0}%, DPIY: {1}%", g.DpiX *100/96f, g.DpiY*100/96f));

			Rectangle gb = GraphBounds;

			int x = 0;
			int y = 0;

			// x
			int interval = XDataKeys.Count == 0 ? gb.Width - 10 : (int)((gb.Width - 10) / (XDataKeys.Count));
			x = gb.Left+5;
			y = ColumnTitleBounds.Top;
			for (int i = 0; i <= XDataKeys.Count; i++)
			{
				g.DrawLine(Pens.Red, x, gb.Bottom - 4, x, gb.Bottom);

				if (xAssistLine)
				{
					using (Pen p = new Pen(Brushes.Silver))
					{
						p.DashStyle = DashStyle.Dot;
						g.DrawLine(p, x, gb.Top, x, gb.Bottom - 5);
					}
				}

				if (i < XDataKeys.Count)
				{
					using (StringFormat sf = new StringFormat())
					{
						sf.Alignment = StringAlignment.Center;
						//sf.FormatFlags |= StringFormatFlags.DirectionVertical;

						Rectangle rect = new Rectangle(x, y, interval, ColumnTitleBounds.Height);

						SizeF strSize = g.MeasureString(XDataKeys[i], Font);
						if (strSize.Width > rect.Width)
						{
							if (i % 2 == 0)
							{
								//rect.X -= rect.Width;
								sf.Alignment = StringAlignment.Near;
								rect.Width += interval;
								g.DrawString(XDataKeys[i], XRulerFont, Brushes.Black, rect, sf);
							}
						}
						else
							g.DrawString(XDataKeys[i], XRulerFont, Brushes.Black, rect, sf);
					}

					x += interval;
				}
			}

			// y
			y = gb.Bottom - 5;
			x = RowTitleBounds.Right;
			int recordHeight = recordInfo.recordCount == 0 ? (GraphBounds.Height - 5) : (gb.Height - 10) / (recordInfo.recordCount);
			int rowHeight = (gb.Height - 10) / (Level-1);
			for (int i = 0; i < Level; i++)
			{
				g.DrawLine(Pens.Red, gb.Left, y, gb.Left + 4, y);

				if (yAssistLine)
				{
					using (Pen p = new Pen(Brushes.Silver))
					{
						p.DashStyle = DashStyle.Dot;
						g.DrawLine(p, gb.Left + 6, y, gb.Right, y);
					}
				}

				if (levelValues.Count >= i + 1)
				{
					string str = ((int)LevelValues[i]).ToString();

					using (StringFormat sf = new StringFormat())
					{
						sf.Alignment = StringAlignment.Far;
						g.DrawString(str, YRulerFont, Brushes.Black, RowTitleBounds.Right, y - Font.Height / 2, sf);
					}
				}
	
				y -= rowHeight;
			}

			g.DrawLine(Pens.Red, gb.Left + 5, gb.Top, gb.Left + 5, gb.Bottom);
			g.DrawLine(Pens.Red, gb.Left, gb.Bottom - 5, gb.Right, gb.Bottom - 5);
			
			// graph
			if (DataSource != null && DataSource.Records != null && DataSource.Records.Count > 0)
			{
				for(int i=0;i<recordInfo.recordCount;i++)
				{
					DrawRecord(g, i, interval);
				}
			}
		}

		protected abstract void DrawRecord(Graphics g, int index, int interval);

		

		protected void DrawValue(Graphics g, int x, int y, DataEntity entity)
		{
			using (Brush b = entity.Style == null ? Brushes.Black : new SolidBrush(entity.Style.Color))
			{
				g.DrawString(entity.Value.ToString(), Font, b, x, y);
			}
		}
		#endregion

		#region Updating
		private int rowTitleWidth = 40;
		private int columnTitleHeight = 15;
	
		private int colWidth = 0;

		private int level = 4;

		public int Level
		{
			get { return level; }
			set { level = value; }
		}

		private List<double> levelValues = new List<double>();

		public List<double> LevelValues
		{
			get { return levelValues; }
			set { levelValues = value; }
		}

		protected override void OnUpdateBounds(Rectangle bounds)
		{
			rowTitleBounds = new Rectangle(bounds.Left + 10,
				CaptionBounds.Bottom + 10, rowTitleWidth,
				bounds.Height - CaptionBounds.Bottom - columnTitleHeight - 10);

			GraphBounds = new Rectangle(rowTitleBounds.Right + 2, CaptionBounds.Bottom + 10,
				bounds.Width - rowTitleBounds.Right - LegendBounds.Width - 10,
				bounds.Height - CaptionBounds.Bottom - columnTitleHeight - 10);

			LegendBounds = new Rectangle(bounds.Right - LegendWidth - 10,
				CaptionBounds.Bottom + 10, LegendWidth,
				bounds.Height - CaptionBounds.Bottom - 10);

			columnTitleBounds = new Rectangle(rowTitleBounds.Right + 2, GraphBounds.Bottom + 2,
				bounds.Width - rowTitleBounds.Right - LegendWidth - 10,
				bounds.Height - GraphBounds.Bottom);

			colWidth = columnTitleBounds.Width / (recordInfo.maxEntityCount == 0 ? 1 : recordInfo.maxEntityCount);
		}

		protected override void OnUpdateDataSource()
		{
			origin = new Point(GraphBounds.Left, GraphBounds.Bottom);
			//recordInfo.yRuleMax = recordInfo.e;
		
			level = 4;
			float f = 1;
			f = PlainGraphToolkit.CalcFator((int)GetRulerYMax(), 4);
			if (f > 0) while (f * (level - 1) < recordInfo.maxValue) level++;

			levelValues.Clear();
			for (int i = 0; i < level; i++)
				levelValues.Add(i * f);
			recordInfo.yRuleMax = levelValues[level - 1];
		
		}

		protected override void SelectLegends()
		{
			if (DataSource != null && DataSource.Records != null)
			foreach (var r in from r in DataSource.Records
												select new { Color = r.Color, Text = r.Key })
				Legends.Add(r.Color, r.Text);
		}

		protected abstract double GetRulerYMax();
		#endregion

		#region Behavior
		//private ToolTip tip = new ToolTip();

		internal override void DoMouseMove(MouseEventArgs e)
		{
			if (IsShowDataTip)
			{
				//tip.Show(
			}
		}

		#endregion
	}

	#endregion

	#region PlainGraph Item

	public class PlainGraphText
	{
		private string text;

		public string Text
		{
			get { return text; }
			set { text = value; }
		}
	}

	public class PlainGraphItem
	{
		private Rectangle bounds;

		public Rectangle Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}
	}
	#endregion

	#region LineGraph
	public class LineGraph : CoordinateGraph
	{
		protected override void DrawRecord(Graphics g, int index, int interval)
		{
			DataRecord record = DataSource.Records[index];

			Rectangle bounds = GraphBounds;

			Point curP = new Point(bounds.Left + 5 + interval / 2, bounds.Bottom);
			Point lastP = Point.Empty;

			for (int i = 0; i < record.Set.Count; i++)
			{
				DataEntity entity = record.Set[i];

				if (entity != null)
				{
					curP.Y = bounds.Bottom - 5 - (recordInfo.yRuleMax == 0? 0 : (int)(entity.Value * bounds.Height / recordInfo.yRuleMax));

					if (!lastP.IsEmpty)
						using (Pen p = new Pen(record.Color))
						{
							p.Width = record.LineWeight;

							if (entity.Style != null)
							{
								if (entity.Style.EndCap != LineCap.Flat)
								{
									int minB = Math.Min(interval, bounds.Height);

									switch (entity.Style.EndCap)
									{
										case LineCap.DiamondAnchor:
											p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(minB * 0.07f, minB * 0.1f);
											break;
									}
								}

								//p.EndCap = entity.Style.EndCap;
								p.StartCap = entity.Style.StartCap;
								p.DashStyle = entity.Style.LineStyle;
								
							}
							g.DrawLine(p, lastP, curP);
						}

					DrawPoint(g, entity, curP, record);
				}

				lastP = curP;
				curP.Offset(interval,0);
			}
		}

		protected virtual void DrawPoint(Graphics g, DataEntity entity, Point p, DataRecord record) { }

		protected override double GetRulerYMax()
		{
			return recordInfo.maxValue;
		}
	}
	#endregion

	#region LinePointGraph
	public class LinePointGraph : LineGraph
	{
		private static readonly int PointSize = 5;

		protected override void DrawPoint(Graphics g, DataEntity entity, Point p, DataRecord record)
		{
			if (entity.Style == null || entity.Style.EndCap == LineCap.Flat)
			{
				int size = (int)(record.LineWeight * 0.75f * PointSize);
				if (size < 5) size = 5;

				using (SolidBrush sb = new SolidBrush(record.Color))
				{
					g.FillEllipse(sb, p.X - size / 2, p.Y - size / 2, size, size);
				}
			}
		}
	}
	#endregion

	#region LineAreaGraph
	public class LineAreaGraph : LineGraph
	{
		protected override void DrawRecord(Graphics g, int index, int interval)
		{
			DataRecord record = DataSource.Records[index];

			Rectangle bounds = GraphBounds;

			Point curP = new Point(bounds.Left + 5 + interval / 2, bounds.Bottom);

			List<Point> points = new List<Point>();

			int minY = bounds.Bottom;

			for (int i = 0; i < record.Set.Count; i++)
			{
				DataEntity entity = record.Set[i];

				if (entity != null)
				{
					curP.Y = bounds.Bottom - 5 - (recordInfo.yRuleMax == 0 ? 0 : (int)(entity.Value * bounds.Height / recordInfo.yRuleMax));
				}
				else
					curP.Y = bounds.Bottom;

				minY = Math.Min(curP.Y, minY);

				points.Add(curP);

				curP.Offset(interval, 0);
			}

			if (points.Count > 1)
			{
				points.Insert(0, new Point(bounds.Left + 5, bounds.Bottom - 5));
				points.Add(new Point(bounds.Right - 5, bounds.Bottom - 5));

				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddLines(points.ToArray());
					path.CloseAllFigures();

					Color transparentRecordColor1 = Color.FromArgb(50, ControlPaint.Light(record.Color));
					Color transparentRecordColor2 = Color.FromArgb(120, ControlPaint.Light(record.Color));
					using (LinearGradientBrush linear = new LinearGradientBrush(
						new Rectangle(bounds.Left, minY, bounds.Width, bounds.Bottom - minY),
						transparentRecordColor1, transparentRecordColor2, 90f))
					{
						g.FillPath(linear, path);
					}
				}

				using (Pen pen = new Pen(record.Color))
				{
					g.DrawLines(pen, points.ToArray());
				}
			}
		}
	}
	#endregion

	#region ColumnGraph
	public class ColumnGraph : CoordinateGraph
	{
		protected override void DrawRecord(Graphics g, int index, int interval)
		{
			DataRecord record = DataSource.Records[index];
			Rectangle rect = new Rectangle(GraphBounds.Left + 5, GraphBounds.Top, GraphBounds.Width - 10, GraphBounds.Height - 5);

			int offsetX = interval / 4;
			int colWidth = (int)(interval * 0.5f / recordInfo.recordCount);

			for(int i=0;i<record.Set.Count;i++)
			{
				int x = rect.Left + offsetX + i * interval + index * colWidth;
				DataEntity entity = record.Set[i];

				int y = rect.Bottom - (int)(entity.Value * rect.Height / recordInfo.yRuleMax);

				using (SolidBrush b = new SolidBrush(record.Color))
				{
					g.FillRectangle(b, x, y, colWidth, rect.Bottom - y );
				}

				x += colWidth;
			}
		}
		protected override double GetRulerYMax()
		{
			return recordInfo.maxValue;
		}
	}
	#endregion

	#region StackedColumnGraph
	public class StackedColumnGraph : CoordinateGraph
	{
		protected override void DrawRecord(Graphics g, int rowIndex, int interval)
		{
			DataRecord record = DataSource.Records[rowIndex];
			
			Rectangle rect = new Rectangle(GraphBounds.Left + 5, GraphBounds.Top, GraphBounds.Width - 10, GraphBounds.Height - 5);

			int colWidth = interval / 2;

			for (int i = 0; i < record.Set.Count; i++)
			{
				int x = (int)(rect.Left + i * interval + colWidth - colWidth * 0.375f);
				DataEntity entity = record.Set[i];

				int y = 0;
				for (int k = 0; k < rowIndex; k++)
					y += (int)(DataSource.Records[k].Set[i].Value * rect.Height / recordInfo.columnTotalMax);

				int height = (int)(entity.Value * rect.Height / recordInfo.columnTotalMax);


				y = rect.Bottom - y - height;

				using (SolidBrush b = new SolidBrush(record.Color))
				{
					g.FillRectangle(b, x, y, colWidth*0.75f, height);
				}
			}
		}
		protected override double GetRulerYMax()
		{
			return recordInfo.columnTotalMax;
		}
	}
	public class StackedPercentColumnGraph : CoordinateGraph
	{
		protected override void DrawRecord(Graphics g, int rowIndex, int interval)
		{
			DataRecord record = DataSource.Records[rowIndex];

			Rectangle rect = new Rectangle(GraphBounds.Left + 5, GraphBounds.Top, GraphBounds.Width - 10, GraphBounds.Height - 5);

			int colWidth = interval / 2;

			for (int i = 0; i < record.Set.Count; i++)
			{
				int x = (int)(rect.Left + i * interval + colWidth - colWidth * 0.375f);
				DataEntity entity = record.Set[i];

				int y = 0;
				for (int k = 0; k < rowIndex; k++)
					y += (int)(DataSource.Records[k].Set[i].Value * rect.Height / recordInfo.columnTotal[i]);

				int height = (int)(entity.Value * rect.Height / recordInfo.columnTotal[i]);

				y = rect.Bottom - y - height;

				using (SolidBrush b = new SolidBrush(record.Color))
				{
					g.FillRectangle(b, x, y, colWidth * 0.75f, height);
				}
			}
		}
		protected override double GetRulerYMax()
		{
			return recordInfo.columnTotalMax;
		}
	}
	#endregion

	#region Pie
	public class PieGraph : PlainCommonGraph
	{
		Rectangle pieBounds = new Rectangle();

		protected override void OnUpdateBounds(Rectangle bounds)
		{
			int w = 10;
			int h = 7;

			float s = Math.Min(GraphBounds.Width / w, GraphBounds.Height / h);

			w = (int)(s * 10f)-50;
			h = (int)(s * 7f)-50;

			pieBounds = new Rectangle(GraphBounds.Left + (GraphBounds.Width - w) / 2,
				GraphBounds.Top + (GraphBounds.Height - h) / 2, w, h);
		}

		protected override void OnUpdateDataSource()
		{
		}

		protected override void SelectLegends()
		{
			if (DataSource != null && DataSource.Records != null && DataSource.Records.Count > 0)
				foreach (var r in from r in DataSource.Records[0].Set
												select new { Color = ( r.Style.Color.IsEmpty ? PlainGraphToolkit.GetRandomColor() : r.Style.Color) , Text = r.Key.ToString() })
				Legends.Add(r.Color, r.Text);
		}

		protected override void DrawGraph(Graphics g)
		{
			if (DataSource == null || DataSource.Records == null || DataSource.Records.Count == 0) return;

			DataRecord record = DataSource.Records[0];
			float angle = 0;

			for(int i=0;i<record.Set.Count;i++)
			{
				DataEntity entity = record.Set[i];
				float off = GetEntityAngle(entity);
				DrawEntityPie(g, pieBounds, record, entity, angle, off);
				angle += off;
			}
		}

		private float GetEntityAngle(DataEntity entity)
		{
			return (float)( entity.Value * 360f / recordInfo.recordTotal[0] );
		}

		protected virtual void DrawEntityPie(Graphics g, Rectangle pieBounds,
			DataRecord record, DataEntity entity, float startAngle, float endAngle)
		{
			if (pieBounds.Width <= 0 || pieBounds.Height <= 0) return;

			Color color = entity.Style.Color;

			using (Brush b = new SolidBrush(color))
			{
				g.FillPie(b, pieBounds, startAngle, endAngle);
			}

			DrawPieString(g, pieBounds, record, entity, startAngle, endAngle);
		}

		protected virtual void DrawPieString(Graphics g, Rectangle pieBounds,
			DataRecord record, DataEntity entity, float startAngle, float endAngle)
		{
			if (IsShowEntityName)
			{
				float w = pieBounds.Width / 3.5f;
				float h = pieBounds.Height / 3.5f;

				// angle -> radian
				float radian = (float)((startAngle + endAngle / 2) * Math.PI / 180f);
				double x = w * Math.Cos(radian);
				double y = h * Math.Sin(radian);

				if (x != double.NaN && y != double.NaN)
				{
					string str = entity.Key.ToString(); //string.Format("{0}%", Math.Round(endAngle / 360f * 100f));
					SizeF strSize = g.MeasureString(str, Font);

					g.DrawString(str,
						Font, Brushes.Black, pieBounds.Left + pieBounds.Width / 2 + (int)x - strSize.Width / 2,
						pieBounds.Top + pieBounds.Height / 2 + (int)y - strSize.Height / 2);
				}
			}
		}
	}

	#endregion

	#region Pie3DGraph
	public class Pie3DGraph : PieGraph
	{
		internal static PointF PointAtArc(PointF origin, RectangleF rect, float angle)
		{
			float radian = (float)((angle) * Math.PI / 180f);
			float x = (float)(origin.X + (rect.Width * Math.Cos(radian)));
			float y = (float)(origin.Y + (rect.Height * Math.Sin(radian)));

			return new PointF(x, y);
		}
		internal static float FixedAngle(float angle, RectangleF rect)
		{
			return (float)(180.0f / Math.PI * Math.Atan2(
				Math.Sin((angle) * Math.PI / 180f) * rect.Height / rect.Width,
				Math.Cos((angle) * Math.PI / 180f)));
		}
		protected override void DrawEntityPie(Graphics g, Rectangle pieBounds,
			DataRecord record, DataEntity entity, float startAngle, float endAngle)
		{
			if (pieBounds.Width <= 0 || pieBounds.Height <= 0) return;

			//if (record.Set.IndexOf(entity) != 4) return;

			float sa = startAngle + 2f;
			float ea = endAngle - 2f;

			Color color = entity.Style.Color;

			int w = (int)(pieBounds.Width/2f);
			int h = (int)(pieBounds.Height/2f);

			PointF origin = new PointF(pieBounds.Left + w, pieBounds.Top + h);

			RectangleF originRect = new RectangleF(origin.X - 5, origin.Y - 5, 10, 10);
			PointF pto = PointAtArc(origin, originRect, sa + ea / 2);

			RectangleF halfRect = new RectangleF(origin .X - 5, origin.Y - 5, 10, 10);
			PointF pt1 = PointAtArc(pto, halfRect, sa);
			PointF pt2 = PointAtArc(pto, halfRect, sa + ea);

			//using (GraphicsPath gp = new GraphicsPath( FillMode.Winding))
			//{
			//  gp.AddLine(x1, y1, origial.X, origial.Y);
			//  gp.AddLine(origial.X, origial.Y, origial.X, origial.Y + 20);
			//  Rectangle rect = pieBounds;
			//  rect.Offset(0, 20);
			//  gp.AddArc(rect, sa + ea, -ea);

			//  gp.CloseAllFigures();

			//  using (Brush b = new SolidBrush(color))
			//  {
			//    //g.FillPath(b, gp);
			//  }
			//}

			//using (GraphicsPath gp = new GraphicsPath())
			//{
			//  gp.AddArc(pieBounds, sa, ea);
			//  gp.AddLine(origial.X, origial.Y, x1, y1);
			//  //g.DrawLine(Pens.Black, origial.X + x2, origial.Y + y2+20, origial.X, origial.Y);
			//  using (Brush b = new SolidBrush(ControlPaint.Light(color)))
			//  {
			//    //g.FillPath(b, gp);
			//  }
			//}

			float fixedAngle = FixedAngle(sa - 0.1f, halfRect);
			float fixedEndAngle = FixedAngle((sa + ea) - 0.1f, halfRect);

			float a2 = fixedEndAngle - fixedAngle;
			if (a2 < 0) a2 += 360;
			Debug.WriteLine("a2=" + a2);

			g.DrawLine(Pens.Black, pto.X, pto.Y, pt1.X, pt1.Y);
			g.DrawArc(Pens.Black, pieBounds, fixedAngle, a2);
			g.DrawLine(Pens.Black, pt2.X, pt2.Y, pto.X, pto.Y);

			Rectangle rect2 = pieBounds;
			rect2.Offset(0, 20);
			//g.DrawLine(Pens.Blue, origin.X, origin.Y, pt1.X, pt1.Y + 20);
			//g.DrawArc(Pens.Blue, rect2, fixedAngle, a2);
			//g.DrawLine(Pens.Blue, pt2.X, pt2.Y + 20, origin.X, origin.Y);


			DrawPieString(g, pieBounds, record, entity, sa, ea);
		}
	}
	#endregion

	#region ExplodedPieGraph
	public class ExplodedPieGraph : PieGraph
	{
		protected override void DrawEntityPie(Graphics g, Rectangle pieBounds,
			DataRecord record, DataEntity entity, float startAngle, float endAngle)
		{
			if (pieBounds.Width <= 0 || pieBounds.Height <= 0) return;

			Color color = entity.Style.Color;

			float w = pieBounds.Width * 1.2f;
			float h = pieBounds.Height * 1.2f;

			float radian = (float)((startAngle + endAngle / 2) * Math.PI / 180f);
			double x = w * Math.Cos(radian);
			double y = h * Math.Sin(radian);

			//if (record.Set.IndexOf(entity) == 3)
			//{
			//  //pieBounds.Inflate(-30, -30);
			//  //pieBounds = new Rectangle((int)x-pieBounds.Width/2, (int)y-pieBounds.Height/2, pieBounds.Width, pieBounds.Height);
			//}
			//else
				pieBounds.Inflate(-20, -20);

			using (Brush b = new SolidBrush(color))
			{
				g.FillPie(b, pieBounds, startAngle, endAngle);
			}

			DrawPieString(g, pieBounds, record, entity, startAngle, endAngle);
		}
	}
	#endregion
}
