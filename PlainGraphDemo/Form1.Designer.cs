namespace Unvell.UIControl.PlainGraphDemo
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.graph = new Unvell.UIControl.PlainGraph.GraphControl();
			this.SuspendLayout();
			// 
			// graph
			// 
			this.graph.BackColor = System.Drawing.Color.White;
			this.graph.DataSource = null;
			this.graph.GraphMargin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.graph.GraphType = Unvell.UIControl.PlainGraph.PlainGraphType.Line;
			this.graph.LegendFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.graph.Location = new System.Drawing.Point(22, 21);
			this.graph.Name = "graph";
			this.graph.Size = new System.Drawing.Size(368, 267);
			this.graph.TabIndex = 0;
			this.graph.Text = "graphControl1";
			this.graph.XRulerFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.graph.YRulerFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(421, 314);
			this.Controls.Add(this.graph);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private Unvell.UIControl.PlainGraph.GraphControl graph;

	}
}