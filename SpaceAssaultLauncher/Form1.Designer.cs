namespace SpaceAssaultLauncher
{
    partial class FormLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLauncher));
            this.updateButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(171, 35);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(87, 19);
            this.updateButton.TabIndex = 0;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.Update_Click);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(14, 59);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(245, 19);
            this.runButton.TabIndex = 1;
            this.runButton.Text = "Launch Space Assault";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.Run_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(14, 10);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(245, 19);
            this.progressBar.TabIndex = 2;
            // 
            // FormLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 94);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.updateButton);
            this.Font = new System.Drawing.Font("ModeNine", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(289, 133);
            this.Name = "FormLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Space Assault";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.ProgressBar progressBar;

    }
}

