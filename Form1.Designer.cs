namespace CharacterRecognitionMorariu
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
            this.TestImagePanel = new System.Windows.Forms.Panel();
            this.LoadOneImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.LoadAllImagesButton = new System.Windows.Forms.Button();
            this.ApplyPcaButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestImagePanel
            // 
            this.TestImagePanel.Location = new System.Drawing.Point(34, 22);
            this.TestImagePanel.Name = "TestImagePanel";
            this.TestImagePanel.Size = new System.Drawing.Size(20, 32);
            this.TestImagePanel.TabIndex = 0;
            // 
            // LoadOneImage
            // 
            this.LoadOneImage.Location = new System.Drawing.Point(34, 80);
            this.LoadOneImage.Name = "LoadOneImage";
            this.LoadOneImage.Size = new System.Drawing.Size(75, 23);
            this.LoadOneImage.TabIndex = 1;
            this.LoadOneImage.Text = "Add one image";
            this.LoadOneImage.UseVisualStyleBackColor = true;
            this.LoadOneImage.Click += new System.EventHandler(this.LoadFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LoadAllImagesButton
            // 
            this.LoadAllImagesButton.Location = new System.Drawing.Point(34, 109);
            this.LoadAllImagesButton.Name = "LoadAllImagesButton";
            this.LoadAllImagesButton.Size = new System.Drawing.Size(75, 23);
            this.LoadAllImagesButton.TabIndex = 2;
            this.LoadAllImagesButton.Text = "Load all char images";
            this.LoadAllImagesButton.UseVisualStyleBackColor = true;
            this.LoadAllImagesButton.Click += new System.EventHandler(this.LoadAllImagesButton_Click);
            // 
            // ApplyPcaButton
            // 
            this.ApplyPcaButton.Location = new System.Drawing.Point(34, 138);
            this.ApplyPcaButton.Name = "ApplyPcaButton";
            this.ApplyPcaButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyPcaButton.TabIndex = 3;
            this.ApplyPcaButton.Text = "Apply pca";
            this.ApplyPcaButton.UseVisualStyleBackColor = true;
            this.ApplyPcaButton.Click += new System.EventHandler(this.ApplyPcaButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 465);
            this.Controls.Add(this.ApplyPcaButton);
            this.Controls.Add(this.LoadAllImagesButton);
            this.Controls.Add(this.LoadOneImage);
            this.Controls.Add(this.TestImagePanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TestImagePanel;
        private System.Windows.Forms.Button LoadOneImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button LoadAllImagesButton;
        private System.Windows.Forms.Button ApplyPcaButton;
    }
}

