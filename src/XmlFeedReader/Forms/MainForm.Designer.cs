namespace XmlFeedReader.Forms
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
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonOpenOutputFolder = new System.Windows.Forms.Button();
            this.buttonSelectOutputFolder = new System.Windows.Forms.Button();
            this.textBoxEndDescriptions = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxStartDescriptions = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxPriceRounding = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFeedUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxProgess = new System.Windows.Forms.GroupBox();
            this.textBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonGetProducts = new System.Windows.Forms.Button();
            this.progressBarGetProducts = new System.Windows.Forms.ProgressBar();
            this.groupBoxSettings.SuspendLayout();
            this.groupBoxProgess.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSettings.Controls.Add(this.buttonSaveSettings);
            this.groupBoxSettings.Controls.Add(this.buttonOpenOutputFolder);
            this.groupBoxSettings.Controls.Add(this.buttonSelectOutputFolder);
            this.groupBoxSettings.Controls.Add(this.textBoxEndDescriptions);
            this.groupBoxSettings.Controls.Add(this.label5);
            this.groupBoxSettings.Controls.Add(this.textBoxStartDescriptions);
            this.groupBoxSettings.Controls.Add(this.label4);
            this.groupBoxSettings.Controls.Add(this.checkBoxPriceRounding);
            this.groupBoxSettings.Controls.Add(this.label3);
            this.groupBoxSettings.Controls.Add(this.textBoxOutputFolder);
            this.groupBoxSettings.Controls.Add(this.label2);
            this.groupBoxSettings.Controls.Add(this.textBoxFeedUrl);
            this.groupBoxSettings.Controls.Add(this.label1);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(720, 290);
            this.groupBoxSettings.TabIndex = 0;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Settings";
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveSettings.Location = new System.Drawing.Point(624, 207);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 40);
            this.buttonSaveSettings.TabIndex = 12;
            this.buttonSaveSettings.Text = "Save";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            // 
            // buttonOpenOutputFolder
            // 
            this.buttonOpenOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenOutputFolder.Location = new System.Drawing.Point(639, 50);
            this.buttonOpenOutputFolder.Name = "buttonOpenOutputFolder";
            this.buttonOpenOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenOutputFolder.TabIndex = 11;
            this.buttonOpenOutputFolder.Text = "Open";
            this.buttonOpenOutputFolder.UseVisualStyleBackColor = true;
            // 
            // buttonSelectOutputFolder
            // 
            this.buttonSelectOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectOutputFolder.Location = new System.Drawing.Point(597, 50);
            this.buttonSelectOutputFolder.Name = "buttonSelectOutputFolder";
            this.buttonSelectOutputFolder.Size = new System.Drawing.Size(36, 23);
            this.buttonSelectOutputFolder.TabIndex = 10;
            this.buttonSelectOutputFolder.Text = "...";
            this.buttonSelectOutputFolder.UseVisualStyleBackColor = true;
            // 
            // textBoxEndDescriptions
            // 
            this.textBoxEndDescriptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEndDescriptions.Location = new System.Drawing.Point(117, 190);
            this.textBoxEndDescriptions.Multiline = true;
            this.textBoxEndDescriptions.Name = "textBoxEndDescriptions";
            this.textBoxEndDescriptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEndDescriptions.Size = new System.Drawing.Size(474, 85);
            this.textBoxEndDescriptions.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "End of Descriptions:";
            // 
            // textBoxStartDescriptions
            // 
            this.textBoxStartDescriptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStartDescriptions.Location = new System.Drawing.Point(117, 99);
            this.textBoxStartDescriptions.Multiline = true;
            this.textBoxStartDescriptions.Name = "textBoxStartDescriptions";
            this.textBoxStartDescriptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStartDescriptions.Size = new System.Drawing.Size(474, 85);
            this.textBoxStartDescriptions.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Start of Descriptions:";
            // 
            // checkBoxPriceRounding
            // 
            this.checkBoxPriceRounding.AutoSize = true;
            this.checkBoxPriceRounding.Location = new System.Drawing.Point(117, 79);
            this.checkBoxPriceRounding.Name = "checkBoxPriceRounding";
            this.checkBoxPriceRounding.Size = new System.Drawing.Size(38, 17);
            this.checkBoxPriceRounding.TabIndex = 5;
            this.checkBoxPriceRounding.Text = "    ";
            this.checkBoxPriceRounding.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Price Rounding:";
            // 
            // textBoxOutputFolder
            // 
            this.textBoxOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFolder.Location = new System.Drawing.Point(117, 53);
            this.textBoxOutputFolder.Name = "textBoxOutputFolder";
            this.textBoxOutputFolder.Size = new System.Drawing.Size(474, 20);
            this.textBoxOutputFolder.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output root folder:";
            // 
            // textBoxFeedUrl
            // 
            this.textBoxFeedUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFeedUrl.Location = new System.Drawing.Point(117, 27);
            this.textBoxFeedUrl.Name = "textBoxFeedUrl";
            this.textBoxFeedUrl.Size = new System.Drawing.Size(474, 20);
            this.textBoxFeedUrl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Feed URL:";
            // 
            // groupBoxProgess
            // 
            this.groupBoxProgess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProgess.Controls.Add(this.progressBarGetProducts);
            this.groupBoxProgess.Controls.Add(this.buttonGetProducts);
            this.groupBoxProgess.Controls.Add(this.textBoxLog);
            this.groupBoxProgess.Location = new System.Drawing.Point(12, 308);
            this.groupBoxProgess.Name = "groupBoxProgess";
            this.groupBoxProgess.Size = new System.Drawing.Size(720, 281);
            this.groupBoxProgess.TabIndex = 1;
            this.groupBoxProgess.TabStop = false;
            this.groupBoxProgess.Text = "Progress";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxLog.Location = new System.Drawing.Point(6, 65);
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.Size = new System.Drawing.Size(708, 210);
            this.textBoxLog.TabIndex = 14;
            this.textBoxLog.Text = "";
            // 
            // buttonGetProducts
            // 
            this.buttonGetProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetProducts.Location = new System.Drawing.Point(9, 19);
            this.buttonGetProducts.Name = "buttonGetProducts";
            this.buttonGetProducts.Size = new System.Drawing.Size(171, 40);
            this.buttonGetProducts.TabIndex = 15;
            this.buttonGetProducts.Text = "Get Products ";
            this.buttonGetProducts.UseVisualStyleBackColor = true;
            // 
            // progressBarGetProducts
            // 
            this.progressBarGetProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarGetProducts.Location = new System.Drawing.Point(186, 27);
            this.progressBarGetProducts.Name = "progressBarGetProducts";
            this.progressBarGetProducts.Size = new System.Drawing.Size(528, 23);
            this.progressBarGetProducts.TabIndex = 17;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 601);
            this.Controls.Add(this.groupBoxProgess);
            this.Controls.Add(this.groupBoxSettings);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.groupBoxProgess.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.CheckBox checkBoxPriceRounding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOutputFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFeedUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEndDescriptions;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxStartDescriptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOpenOutputFolder;
        private System.Windows.Forms.Button buttonSelectOutputFolder;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.GroupBox groupBoxProgess;
        private System.Windows.Forms.RichTextBox textBoxLog;
        private System.Windows.Forms.ProgressBar progressBarGetProducts;
        private System.Windows.Forms.Button buttonGetProducts;
    }
}