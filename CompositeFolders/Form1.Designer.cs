namespace CompositeFolders
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            treeView1 = new TreeView();
            toolTip1 = new ToolTip(components);
            BarChartBtn = new Button();
            PieChartBtn = new Button();
            visualizationPanel = new Panel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.BackColor = SystemColors.AppWorkspace;
            button1.Location = new Point(89, 543);
            button1.Name = "button1";
            button1.Size = new Size(122, 46);
            button1.TabIndex = 1;
            button1.Text = "Browse";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            treeView1.Location = new Point(15, 19);
            treeView1.Name = "treeView1";
            treeView1.ShowNodeToolTips = true;
            treeView1.Size = new Size(212, 518);
            treeView1.TabIndex = 2;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // BarChartBtn
            // 
            BarChartBtn.BackColor = SystemColors.AppWorkspace;
            BarChartBtn.Location = new Point(256, 19);
            BarChartBtn.Name = "BarChartBtn";
            BarChartBtn.Size = new Size(104, 41);
            BarChartBtn.TabIndex = 3;
            BarChartBtn.Text = "Bar Chart";
            BarChartBtn.UseVisualStyleBackColor = false;
            BarChartBtn.Click += BarChartBtn_Click_1;
            // 
            // PieChartBtn
            // 
            PieChartBtn.BackColor = SystemColors.AppWorkspace;
            PieChartBtn.Location = new Point(379, 19);
            PieChartBtn.Name = "PieChartBtn";
            PieChartBtn.Size = new Size(103, 41);
            PieChartBtn.TabIndex = 4;
            PieChartBtn.Text = "Pie Chart";
            PieChartBtn.UseVisualStyleBackColor = false;
            PieChartBtn.Click += PieChartBtn_Click;
            // 
            // visualizationPanel
            // 
            visualizationPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            visualizationPanel.AutoScroll = true;
            visualizationPanel.Location = new Point(256, 89);
            visualizationPanel.Name = "visualizationPanel";
            visualizationPanel.Size = new Size(1145, 448);
            visualizationPanel.TabIndex = 5;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(539, 55);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(500, 380);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1455, 603);
            Controls.Add(pictureBox1);
            Controls.Add(visualizationPanel);
            Controls.Add(PieChartBtn);
            Controls.Add(BarChartBtn);
            Controls.Add(treeView1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private TreeView treeView1;
        private ToolTip toolTip1;
        private Button BarChartBtn;
        private Button PieChartBtn;
        private Panel visualizationPanel;
        private PictureBox pictureBox1;
    }
}
