namespace ProgramVisualizer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.expressionTxtBox = new System.Windows.Forms.TextBox();
            this.normalRadBtn = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.prefixRadBtn = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.exportBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.generateBtn = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.saveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // expressionTxtBox
            // 
            this.expressionTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionTxtBox.BackColor = System.Drawing.Color.White;
            this.expressionTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expressionTxtBox.Font = new System.Drawing.Font("Consolas", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expressionTxtBox.ForeColor = System.Drawing.Color.Black;
            this.expressionTxtBox.Location = new System.Drawing.Point(9, 27);
            this.expressionTxtBox.Margin = new System.Windows.Forms.Padding(4);
            this.expressionTxtBox.Name = "expressionTxtBox";
            this.expressionTxtBox.Size = new System.Drawing.Size(563, 25);
            this.expressionTxtBox.TabIndex = 0;
            // 
            // normalRadBtn
            // 
            this.normalRadBtn.AutoSize = true;
            this.normalRadBtn.Checked = true;
            this.normalRadBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.normalRadBtn.Location = new System.Drawing.Point(8, 28);
            this.normalRadBtn.Margin = new System.Windows.Forms.Padding(4);
            this.normalRadBtn.Name = "normalRadBtn";
            this.normalRadBtn.Size = new System.Drawing.Size(78, 23);
            this.normalRadBtn.TabIndex = 2;
            this.normalRadBtn.TabStop = true;
            this.normalRadBtn.Text = "Normal";
            this.normalRadBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.expressionTxtBox);
            this.groupBox1.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(580, 65);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Expression";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.prefixRadBtn);
            this.groupBox2.Controls.Add(this.normalRadBtn);
            this.groupBox2.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(604, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(164, 65);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notation";
            // 
            // prefixRadBtn
            // 
            this.prefixRadBtn.AutoSize = true;
            this.prefixRadBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefixRadBtn.Location = new System.Drawing.Point(94, 28);
            this.prefixRadBtn.Margin = new System.Windows.Forms.Padding(4);
            this.prefixRadBtn.Name = "prefixRadBtn";
            this.prefixRadBtn.Size = new System.Drawing.Size(67, 23);
            this.prefixRadBtn.TabIndex = 3;
            this.prefixRadBtn.Text = "Prefix";
            this.prefixRadBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.pictureBox);
            this.groupBox3.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(16, 88);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(752, 523);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output";
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(4, 23);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(744, 496);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.exportBtn);
            this.groupBox4.Controls.Add(this.clearBtn);
            this.groupBox4.Controls.Add(this.generateBtn);
            this.groupBox4.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(775, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(279, 65);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Actions";
            // 
            // exportBtn
            // 
            this.exportBtn.Enabled = false;
            this.exportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportBtn.Location = new System.Drawing.Point(191, 22);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(82, 35);
            this.exportBtn.TabIndex = 2;
            this.exportBtn.Text = "&Export...";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.ExportBtnClick);
            // 
            // clearBtn
            // 
            this.clearBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearBtn.Location = new System.Drawing.Point(99, 22);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(82, 35);
            this.clearBtn.TabIndex = 1;
            this.clearBtn.Text = "&Clear";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.ClearBtnClick);
            // 
            // generateBtn
            // 
            this.generateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generateBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateBtn.Location = new System.Drawing.Point(6, 22);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(82, 35);
            this.generateBtn.TabIndex = 0;
            this.generateBtn.Text = "&Generate";
            this.generateBtn.UseVisualStyleBackColor = true;
            this.generateBtn.Click += new System.EventHandler(this.GenerateBtnClick);
            // 
            // propertyGrid
            // 
            this.propertyGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.propertyGrid.CommandsBackColor = System.Drawing.Color.White;
            this.propertyGrid.CommandsBorderColor = System.Drawing.Color.Black;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid.HelpBorderColor = System.Drawing.Color.White;
            this.propertyGrid.HelpForeColor = System.Drawing.Color.Black;
            this.propertyGrid.LineColor = System.Drawing.Color.White;
            this.propertyGrid.Location = new System.Drawing.Point(3, 22);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.Gray;
            this.propertyGrid.Size = new System.Drawing.Size(273, 498);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.ViewBorderColor = System.Drawing.Color.White;
            this.propertyGrid.ViewForeColor = System.Drawing.Color.Black;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.propertyGrid);
            this.groupBox5.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(775, 88);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(279, 523);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Information";
            // 
            // saveImageFileDialog
            // 
            this.saveImageFileDialog.DefaultExt = "png";
            this.saveImageFileDialog.Filter = "Portable Network Graphics|*.png|Portable Document Format|*.pdf|Scalable Vector Gr" +
    "aphics|*.svg|Jpeg image|*.jpg|Bitmap image|*.bmp";
            this.saveImageFileDialog.RestoreDirectory = true;
            this.saveImageFileDialog.Title = "Choose a file to export the program tree";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1067, 624);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Genetica.NET - Program Visualizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox expressionTxtBox;
        private System.Windows.Forms.RadioButton normalRadBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton prefixRadBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.SaveFileDialog saveImageFileDialog;
    }
}

