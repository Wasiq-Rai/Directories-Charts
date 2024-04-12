using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace CompositeFolders
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            // Documents Folder
            string cDrivePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            UpdateTreeView(cDrivePath);
            pictureBox1.Visible = false;
            DisplayBarChartForSelectedDirectory();


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DisplayBarChartForSelectedDirectory();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = Environment.CurrentDirectory;

                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    UpdateTreeView(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void UpdateTreeView(string path)
        {
            try
            {
                treeView1.Nodes.Clear();

                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                if (directoryInfo.Parent != null)
                {
                    TreeNode parentTreeNode = new TreeNode(directoryInfo.Name);
                    parentTreeNode.Tag = directoryInfo.Parent.FullName; 
                    parentTreeNode.ImageIndex = 0; 
                    treeView1.Nodes.Add(parentTreeNode);
                }

                // Add subdirectories to the TreeView
                AddSubDirectoriesToTreeView(directoryInfo, treeView1.Nodes);

                // Add files to the TreeView
                AddFilesToTreeView(directoryInfo, treeView1.Nodes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AddSubDirectoriesToTreeView(DirectoryInfo directoryInfo, TreeNodeCollection nodes)
        {
            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                try
                {
                    TreeNode treeNode = new TreeNode(subDirectory.Name);
                    treeNode.Tag = subDirectory.FullName; 
                    treeNode.StateImageIndex = 0; 
                    nodes.Add(treeNode);

                    // Recursively add subdirectories
                    AddSubDirectoriesToTreeView(subDirectory, treeNode.Nodes);
                }
                catch (UnauthorizedAccessException)
                {
                    continue; 
                }
            }
        }

        private void AddFilesToTreeView(DirectoryInfo directoryInfo, TreeNodeCollection nodes)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                try
                {
                    TreeNode treeNode = new TreeNode(file.Name);
                    treeNode.Tag = file.FullName;
                    treeNode.StateImageIndex = 1; 
                    nodes.Add(treeNode);
                }
                catch (UnauthorizedAccessException)
                {
                    continue; 
                }
            }
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null && node.Tag != null)
            {
                toolTip1.SetToolTip(treeView1, node.Tag.ToString());
            }
            else
            {
                toolTip1.SetToolTip(treeView1, "");
            }
        }

        private void BarChartBtn_Click_1(object sender, EventArgs e)
        {
            visualizationPanel.Controls.Clear();
            pictureBox1.Visible = false;
            visualizationPanel.Visible = true;


            DirectoryInfo directoryInfo = new DirectoryInfo(treeView1.SelectedNode.Tag.ToString());

            DrawBarChart(directoryInfo);
        }




        private long CalculateDirectorySize(DirectoryInfo directoryInfo)
        {
            long totalSize = 0;

            // Calculate size of files in the directory
            foreach (var file in directoryInfo.GetFiles())
            {
                totalSize += file.Length;
            }

            // Calculate size of subdirectories
            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                totalSize += CalculateDirectorySize(subDirectory);
            }

            return totalSize;
        }


        private void DisplayBarChartForSelectedDirectory()
        {
            visualizationPanel.Controls.Clear();

            string selectedDirectoryPath = treeView1.SelectedNode?.Tag.ToString();
            if (selectedDirectoryPath == null || !Directory.Exists(selectedDirectoryPath))
                return;

            DirectoryInfo directoryInfo = new DirectoryInfo(selectedDirectoryPath);
            if(visualizationPanel.Visible)
            DrawBarChart(directoryInfo);
            else
            DrawPieChart(directoryInfo);
        }


        private void DrawBarChart(DirectoryInfo directoryInfo)
        {
            int chartWidth = visualizationPanel.Width - 20;
            int chartHeight = 200;
            int barHeight = 20;
            int barGap = 10;

            var subDirectories = directoryInfo.GetDirectories();
            var files = directoryInfo.GetFiles();

            double totalSizeMB = files.Sum(file => file.Length) / (1024.0 * 1024.0); // Convert total size to MB

            float maxLength = 0;

            // Calculate the maximum length of all bars
            foreach (var directory in subDirectories)
            {
                long subDirectorySize = directory.GetFiles().Sum(file => file.Length);
                double subDirectorySizeMB = subDirectorySize / (1024.0 * 1024.0); // Convert subdirectory size to MB

                maxLength = Math.Max(maxLength, (float)subDirectorySizeMB);
            }

            foreach (var file in files)
            {
                double fileSizeMB = file.Length / (1024.0 * 1024.0); // Convert file size to MB

                maxLength = Math.Max(maxLength, (float)fileSizeMB);
            }

            int totalBarHeight = (barHeight + barGap) * (subDirectories.Length + files.Length); // Calculate the total height of all bars

            if (totalBarHeight > chartHeight)
            {
                // If the total height of bars exceeds the height of the panel, set the panel's AutoScrollMinSize
                visualizationPanel.AutoScrollMinSize = new Size(0, totalBarHeight);
            }

            Bitmap bitmap = new Bitmap(chartWidth, totalBarHeight); // Use totalBarHeight as the height of the bitmap
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                visualizationPanel.Controls.Clear();

                int y = 10;
                Random random = new Random();

                foreach (var directory in subDirectories)
                {
                    long subDirectorySize = directory.GetFiles().Sum(file => file.Length);
                    double subDirectorySizeMB = subDirectorySize / (1024.0 * 1024.0); // Convert subdirectory size to MB

                    int barLength = (int)(subDirectorySizeMB / maxLength * chartWidth); // Calculate bar length relative to maximum length

                    Rectangle barRect = new Rectangle(0, y, barLength, barHeight);
                    graphics.FillRectangle(Brushes.LightBlue, barRect);

                    string subDirInfoText = $"({subDirectorySizeMB:0.##} MB) {directory.Name}";
                    SizeF textSize = graphics.MeasureString(subDirInfoText, Font);
                    graphics.DrawString(subDirInfoText, Font, Brushes.Black, 0, y); // Adjust y-coordinate to place the text above the bar

                    y += barHeight + barGap;
                }

                foreach (var file in files)
                {
                    double fileSizeMB = file.Length / (1024.0 * 1024.0); // Convert file size to MB

                    int barLength = (int)(fileSizeMB / maxLength * chartWidth); // Calculate bar length relative to maximum length

                    Rectangle barRect = new Rectangle(0, y, barLength, barHeight);
                    graphics.FillRectangle(Brushes.LightSalmon, barRect);

                    string fileInfoText = $"({fileSizeMB:0.##} MB) {file.Name}";
                    SizeF textSize = graphics.MeasureString(fileInfoText, Font);
                    graphics.DrawString(fileInfoText, Font, Brushes.Black, 0, y); // Adjust y-coordinate to place the text above the bar

                    y += barHeight + barGap;
                }
            }

            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = bitmap;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            visualizationPanel.Controls.Add(pictureBox);
        }


        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }


        private void DrawPieChart(DirectoryInfo directoryInfo)
        {
            pictureBox1.Image = null;

            var subDirectories = directoryInfo.GetDirectories();
            var files = directoryInfo.GetFiles();

            double totalSizeMB = subDirectories.Sum(dir => GetTotalSize(dir)) / (1024.0 * 1024.0) + files.Sum(file => file.Length) / (1024.0 * 1024.0);

            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                int centerX = pictureBox1.Width / 2;
                int centerY = pictureBox1.Height / 2;
                int radius = Math.Min(centerX, centerY) - 20;

                float startAngle = 0;
                Random random = new Random();

                foreach (var directory in subDirectories)
                {
                    double sizeMB = GetTotalSize(directory) / (1024.0 * 1024.0); // Size in MB
                    double percentage = sizeMB / totalSizeMB * 360; // Calculate percentage
                    float sweepAngle = (float)percentage;

                    Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

                    graphics.FillPie(new SolidBrush(randomColor), centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);

                    float midAngle = startAngle + (sweepAngle / 2);

                    float labelX = (float)(centerX + (radius * 0.8 * Math.Cos(midAngle * Math.PI / 180)));
                    float labelY = (float)(centerY + (radius * 0.8 * Math.Sin(midAngle * Math.PI / 180)));

                    string label = $"{directory.Name}, {sizeMB:0.##} MB"; // Display size in MB
                    SizeF labelSize = graphics.MeasureString(label, Font);
                    graphics.DrawString(label, Font, Brushes.Black, labelX - labelSize.Width / 2, labelY - labelSize.Height / 2);

                    startAngle += sweepAngle;
                }

                foreach (var file in files)
                {
                    double sizeMB = file.Length / (1024.0 * 1024.0); // Size in MB
                    double percentage = sizeMB / totalSizeMB * 360; // Calculate percentage
                    float sweepAngle = (float)percentage;

                    Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

                    graphics.FillPie(new SolidBrush(randomColor), centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);

                    float midAngle = startAngle + (sweepAngle / 2);

                    float labelX = (float)(centerX + (radius * 0.8 * Math.Cos(midAngle * Math.PI / 180)));
                    float labelY = (float)(centerY + (radius * 0.8 * Math.Sin(midAngle * Math.PI / 180)));

                    string label = $"{file.Name}, {sizeMB:0.##} MB"; // Display size in MB
                    SizeF labelSize = graphics.MeasureString(label, Font);
                    graphics.DrawString(label, Font, Brushes.Black, labelX - labelSize.Width / 2, labelY - labelSize.Height / 2);

                    startAngle += sweepAngle;
                }
            }

            pictureBox1.Image = bitmap;
        }

        private long GetTotalSize(DirectoryInfo directory)
        {
            long totalSize = directory.GetFiles().Sum(file => file.Length);
            totalSize += directory.GetDirectories().Sum(dir => GetTotalSize(dir));
            return totalSize;
        }

        private void PieChartBtn_Click(object sender, EventArgs e)
        {
            visualizationPanel.Controls.Clear();
            visualizationPanel.Visible = false;
            pictureBox1.Visible = true;

            DirectoryInfo directoryInfo = new DirectoryInfo(treeView1.SelectedNode.Tag.ToString());

            DrawPieChart(directoryInfo);

        }
    }


}
