namespace WindowsFormsApp3
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
            this.thread1 = new System.Windows.Forms.Button();
            this.thread2 = new System.Windows.Forms.Button();
            this.thread3 = new System.Windows.Forms.Button();
            this.thread4 = new System.Windows.Forms.Button();
            this.openThreads = new System.Windows.Forms.Button();
            this.pauseThreads = new System.Windows.Forms.Button();
            this.stopThreads = new System.Windows.Forms.Button();
            this.resumeThreads = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // thread1
            // 
            this.thread1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.thread1.Location = new System.Drawing.Point(44, 63);
            this.thread1.Name = "thread1";
            this.thread1.Size = new System.Drawing.Size(130, 75);
            this.thread1.TabIndex = 0;
            this.thread1.Text = "Thread1";
            this.thread1.UseVisualStyleBackColor = true;
            this.thread1.Click += new System.EventHandler(this.thread1_Click);
            // 
            // thread2
            // 
            this.thread2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.thread2.Location = new System.Drawing.Point(223, 63);
            this.thread2.Name = "thread2";
            this.thread2.Size = new System.Drawing.Size(130, 75);
            this.thread2.TabIndex = 1;
            this.thread2.Text = "Thread2";
            this.thread2.UseVisualStyleBackColor = true;
            this.thread2.Click += new System.EventHandler(this.thread2_Click);
            // 
            // thread3
            // 
            this.thread3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.thread3.Location = new System.Drawing.Point(409, 63);
            this.thread3.Name = "thread3";
            this.thread3.Size = new System.Drawing.Size(130, 75);
            this.thread3.TabIndex = 2;
            this.thread3.Text = "Thread3";
            this.thread3.UseVisualStyleBackColor = true;
            this.thread3.Click += new System.EventHandler(this.thread3_Click);
            // 
            // thread4
            // 
            this.thread4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.thread4.Location = new System.Drawing.Point(589, 63);
            this.thread4.Name = "thread4";
            this.thread4.Size = new System.Drawing.Size(130, 75);
            this.thread4.TabIndex = 3;
            this.thread4.Text = "Thread4";
            this.thread4.UseVisualStyleBackColor = true;
            this.thread4.Click += new System.EventHandler(this.thread4_Click);
            // 
            // openThreads
            // 
            this.openThreads.BackColor = System.Drawing.Color.Plum;
            this.openThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.openThreads.Location = new System.Drawing.Point(44, 211);
            this.openThreads.Name = "openThreads";
            this.openThreads.Size = new System.Drawing.Size(250, 75);
            this.openThreads.TabIndex = 5;
            this.openThreads.Text = "Open all threads";
            this.openThreads.UseVisualStyleBackColor = false;
            this.openThreads.Click += new System.EventHandler(this.openThreads_Click);
            // 
            // pauseThreads
            // 
            this.pauseThreads.BackColor = System.Drawing.Color.PaleTurquoise;
            this.pauseThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.pauseThreads.Location = new System.Drawing.Point(469, 211);
            this.pauseThreads.Name = "pauseThreads";
            this.pauseThreads.Size = new System.Drawing.Size(250, 75);
            this.pauseThreads.TabIndex = 6;
            this.pauseThreads.Text = "Pause all threads";
            this.pauseThreads.UseVisualStyleBackColor = false;
            this.pauseThreads.Click += new System.EventHandler(this.pauseThreads_Click);
            // 
            // stopThreads
            // 
            this.stopThreads.BackColor = System.Drawing.Color.IndianRed;
            this.stopThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.stopThreads.Location = new System.Drawing.Point(469, 338);
            this.stopThreads.Name = "stopThreads";
            this.stopThreads.Size = new System.Drawing.Size(250, 75);
            this.stopThreads.TabIndex = 7;
            this.stopThreads.Text = "Stop all threads";
            this.stopThreads.UseVisualStyleBackColor = false;
            this.stopThreads.Click += new System.EventHandler(this.stopThreads_Click);
            // 
            // resumeThreads
            // 
            this.resumeThreads.BackColor = System.Drawing.Color.PaleGreen;
            this.resumeThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.resumeThreads.Location = new System.Drawing.Point(44, 338);
            this.resumeThreads.Name = "resumeThreads";
            this.resumeThreads.Size = new System.Drawing.Size(250, 75);
            this.resumeThreads.TabIndex = 8;
            this.resumeThreads.Text = "Resume all threads";
            this.resumeThreads.UseVisualStyleBackColor = false;
            this.resumeThreads.Click += new System.EventHandler(this.resumeThreads_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 453);
            this.Controls.Add(this.resumeThreads);
            this.Controls.Add(this.stopThreads);
            this.Controls.Add(this.pauseThreads);
            this.Controls.Add(this.openThreads);
            this.Controls.Add(this.thread4);
            this.Controls.Add(this.thread3);
            this.Controls.Add(this.thread2);
            this.Controls.Add(this.thread1);
            this.Name = "MainForm";
            this.Text = "Thread Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button thread1;
        private System.Windows.Forms.Button thread2;
        private System.Windows.Forms.Button thread3;
        private System.Windows.Forms.Button thread4;
        private System.Windows.Forms.Button openThreads;
        private System.Windows.Forms.Button pauseThreads;
        private System.Windows.Forms.Button stopThreads;
        private System.Windows.Forms.Button resumeThreads;
    }
}

