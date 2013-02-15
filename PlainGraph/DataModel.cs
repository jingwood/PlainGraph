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

using System.Collections.Generic;
using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Unvell.UIControl.PlainGraph
{
	public class DataEntity
	{
		private string key;

		public string Key
		{
			get { return key; }
			set { key = value; }
		}
		
		private double value;

		public double Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		private string comment;

		public string Comment
		{
			get { return comment; }
			set { comment = value; }
		}

		private DataEntityStyle style;

		public DataEntityStyle Style
		{
			get { return style; }
			set { style = value; }
		}
	}


	public class DataRecord
	{
		private string key;

		public string Key
		{
			get { return key; }
			set { key = value; }
		}

		private Color color = Color.Blue;

		public Color Color
		{
			get { return color; }
			set { color = value; }
		}

		private float lineWeight = 1;

		public float LineWeight
		{
			get { return lineWeight; }
			set { lineWeight = value; }
		}

		List<DataEntity> set = new List<DataEntity>();

		public List<DataEntity> Set
		{
			get { return set; }
			set { set = value; }
		}

		public DataEntity AddData(string name, double value)
		{
			return AddData(name, value, Color.Empty);
		}

		public DataEntity AddData(string name, double value, Color color)
		{
			DataEntity entity = new DataEntity
			{
				Key = name,
				Value = value,
			};
			if (color != Color.Empty)
			{
				entity.Style = new DataEntityStyle
				{
					Color = color,
				};
			}
			set.Add(entity);
			return entity;
		}


		public DataRecord() { }

		public DataRecord(string name)
		{
			this.key = name;
		}
	}

	public class DataSource
	{
		private string caption = "PlainGraph";

		public string Caption
		{
			get { return caption; }
			set { caption = value; }
		}

		private string xTitle;

		public string XTitle
		{
			get { return xTitle; }
			set { xTitle = value; }
		}

		private string yTitle;

		public string YTitle
		{
			get { return yTitle; }
			set { yTitle = value; }
		}

		private List<string> xDataKeys = new List<string>();

		public List<string> XDataKeys
		{
			get { return xDataKeys; }
			set { xDataKeys = value; }
		}
		
		private List<DataRecord> records = new List<DataRecord>();

		public List<DataRecord> Records
		{
			get { return records; }
			set { records = value; }
		}
		public DataRecord AddData(Dictionary<string, double> values)
		{
			return AddData(string.Empty, values, PlainGraphToolkit.GetUnusedColor(this));
		}
		public DataRecord AddData(string title, Dictionary<string, double> values)
		{
			return AddData(title, values, PlainGraphToolkit.GetUnusedColor(this));
		}
		public DataRecord AddData(string title, Dictionary<string, double> values, Color color)
		{
			DataRecord record = new DataRecord()
			{
				Key = title,
				Color = color,
			};

			foreach (string key in values.Keys)
			{
				DataEntity set = new DataEntity()
				{
					Key = key,
					Value = values[key],
					Style = new DataEntityStyle
					{
						Color = PlainGraphToolkit.GetRandomColor(),
					},
				};
				record.Set.Add(set);
			}

			records.Add(record);

			return record;
		}

		public DataRecord AddData(string title, IQueryable<KeyValuePair<string, int>> values, Color color)
		{
			DataRecord record = new DataRecord()
			{
				Key = title,
				Color = color,
			};

			foreach (string key in values.Select(v => v.Key))
			{
				DataEntity set = new DataEntity()
				{
					Key = key,
					Value = values.FirstOrDefault(v => v.Key == key).Value,
					Style = new DataEntityStyle
					{
						Color = PlainGraphToolkit.GetRandomColor(),
					},
				};
				record.Set.Add(set);
			}

			records.Add(record);

			return record;
		}


		public DataRecord AddData(string title, Dictionary<int, double> values)
		{
			return AddData(title, values, PlainGraphToolkit.GetUnusedColor(this));
		}
		public DataRecord AddData(string title, Dictionary<int, double> values, Color color)
		{
			DataRecord record = new DataRecord()
			{
				Key = title,
				Color = color,
			};

			foreach (int key in values.Keys)
			{
				DataEntity set = new DataEntity()
				{
					Key = key.ToString(),
					Value = values[key],
					Style = new DataEntityStyle
					{
						Color = PlainGraphToolkit.GetRandomColor(),
					},
				};
				record.Set.Add(set);
			}

			records.Add(record);

			return record;
		}
		public void AddData(DataRecord record)
		{
			records.Add(record);
		}
		public DataRecord AddData(string name)
		{
			DataRecord record = new DataRecord(name);
			AddData(record);
			return record;
		}

		public DataSource() { }
		public DataSource(string name) { this.caption = name; }
	}

	public class DataEntityStyle
	{
		private Color color;

		public Color Color
		{
			get { return color; }
			set { color = value; }
		}

		private DashStyle lineStyle;

		public DashStyle LineStyle
		{
			get { return lineStyle; }
			set { lineStyle = value; }
		}

		private LineCap endCap;

		public LineCap EndCap
		{
			get { return endCap; }
			set { endCap = value; }
		}

		private LineCap startCap;

		public LineCap StartCap
		{
			get { return startCap; }
			set { startCap = value; }
		}
	}
}