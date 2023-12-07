namespace WindowsFormsApp3
{
    partial class FireForm
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
            this.stopThread = new System.Windows.Forms.Button();
            this.resumeThread = new System.Windows.Forms.Button();
            this.pauseThread = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stopThread
            // 
            this.stopThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.stopThread.Location = new System.Drawing.Point(415, 271);
            this.stopThread.Name = "stopThread";
            this.stopThread.Size = new System.Drawing.Size(130, 70);
            this.stopThread.TabIndex = 5;
            this.stopThread.Text = "Stop";
            this.stopThread.UseVisualStyleBackColor = true;
            this.stopThread.Click += new System.EventHandler(this.stopThread_Click);
            // 
            // resumeThread
            // 
            this.resumeThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.resumeThread.Location = new System.Drawing.Point(227, 271);
            this.resumeThread.Name = "resumeThread";
            this.resumeThread.Size = new System.Drawing.Size(130, 70);
            this.resumeThread.TabIndex = 4;
            this.resumeThread.Text = "Resume";
            this.resumeThread.UseVisualStyleBackColor = true;
            this.resumeThread.Click += new System.EventHandler(this.resumeThread_Click);
            // 
            // pauseThread
            // 
            this.pauseThread.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.pauseThread.Location = new System.Drawing.Point(44, 271);
            this.pauseThread.Name = "pauseThread";
            this.pauseThread.Size = new System.Drawing.Size(130, 70);
            this.pauseThread.TabIndex = 3;
            this.pauseThread.Text = "Pause";
            this.pauseThread.UseVisualStyleBackColor = true;
            this.pauseThread.Click += new System.EventHandler(this.pauseThread_Click);
            // 
            // FireForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 353);
            this.Controls.Add(this.stopThread);
            this.Controls.Add(this.resumeThread);
            this.Controls.Add(this.pauseThread);
            this.Name = "FireForm";
            this.Text = "FireForm";
            this.Load += new System.EventHandler(this.FireForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button stopThread;
        private System.Windows.Forms.Button resumeThread;
        private System.Windows.Forms.Button pauseThread;
    }
}