using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class MainForm : Form
    {
        private Thread ballThread, figureThread, sinThread, fireThread;

        private BallForm ballForm;
        private MovingFigure figureForm;
        private SinForm sinForm;
        private FireForm fireForm;

        private Point mainPosition = new Point(20, 200);
        private Point figurePosition = new Point(1080, 50);
        private Point ballPosition = new Point(630, 50);
        private Point firePosition = new Point(1080, 450);
        private Point sinPosition = new Point(630, 450);

        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = mainPosition;
        }

        private void StartBallForm()
        {
            ballForm = new BallForm();
            ballForm.StartPosition = FormStartPosition.Manual;
            ballForm.Location = ballPosition;

            ballThread = new Thread(ballForm.Animation);
            ballThread.IsBackground = true;

            ballForm.Paused += (sender, e) =>
            {
                if (ballForm.IsPaused)
                {
                    ballForm.pauseThread_Click(sender, e);
                }
                else
                {
                    ballForm.resumeThread_Click(sender, e);
                }
            };

            ballThread.Start();
            ballForm.Show();
        }

        private void StartFigureForm()
        {
            figureForm = new MovingFigure();
            figureForm.StartPosition = FormStartPosition.Manual;
            figureForm.Location = figurePosition;

            figureThread = new Thread(figureForm.Animation);
            figureThread.IsBackground = true;

            figureForm.Paused += (sender, e) =>
            {
                if (figureForm.IsPaused)
                {
                    figureForm.pauseThread_Click(sender, e);
                }
                else
                {
                    figureForm.resumeThread_Click(sender, e);
                }
            };

            figureThread.Start();
            figureForm.Show();
        }

        private void StartSinForm()
        {
            sinForm = new SinForm();
            sinForm.StartPosition = FormStartPosition.Manual;
            sinForm.Location = sinPosition;

            sinThread = new Thread(sinForm.Animation);
            sinThread.IsBackground = true;

            sinForm.Paused += (sender, e) =>
            {
                if (sinForm.IsPaused)
                {
                    sinForm.pauseThread_Click(sender, e);
                }
                else
                {
                    sinForm.resumeThread_Click(sender, e);
                }
            };

            sinThread.Start();
            sinForm.Show();
        }

        private void StartFireForm()
        {
            fireForm = new FireForm();
            fireForm.StartPosition = FormStartPosition.Manual;
            fireForm.Location = firePosition;

            fireThread = new Thread(fireForm.Animation);
            fireThread.IsBackground = true;

            fireForm.Paused += (sender, e) =>
            {
                if (fireForm.IsPaused)
                {
                    fireForm.pauseThread_Click(sender, e);
                }
                else
                {
                    fireForm.resumeThread_Click(sender, e);
                }
            };

            fireThread.Start();
            fireForm.Show();
        }

        private void PauseAll()
        {
            ballForm.pauseThread_Click(this, EventArgs.Empty);
            figureForm.pauseThread_Click(this, EventArgs.Empty);
            sinForm.pauseThread_Click(this, EventArgs.Empty);
            fireForm.pauseThread_Click(this, EventArgs.Empty);
        }

        private void ResumeAll()
        {
            ballForm.resumeThread_Click(this, EventArgs.Empty);
            figureForm.resumeThread_Click(this, EventArgs.Empty);
            sinForm.resumeThread_Click(this, EventArgs.Empty);
            fireForm.resumeThread_Click(this, EventArgs.Empty);
        }

        private void OpenThreads()
        {
            StartFireForm();
            StartBallForm();
            StartFigureForm();
            StartSinForm();      
        }

        private void StopThreads()
        {
            ballForm.Close();
            figureForm.Close();
            sinForm.Close();
            fireForm.Close();
        }

        private void thread1_Click(object sender, EventArgs e)
        {
            StartBallForm();
        }

        private void thread2_Click(object sender, EventArgs e)
        {
            StartFigureForm();
        }

        private void thread3_Click(object sender, EventArgs e)
        {
            StartSinForm();
        }

        private void thread4_Click(object sender, EventArgs e)
        {
            StartFireForm();
        }

        private void openThreads_Click(object sender, EventArgs e)
        {
            OpenThreads();
        }

        private void pauseThreads_Click(object sender, EventArgs e)
        {
            PauseAll();
        }

        private void resumeThreads_Click(object sender, EventArgs e)
        {
            ResumeAll();
        }

        private void stopThreads_Click(object sender, EventArgs e)
        {
            StopThreads();
        }
    }
}