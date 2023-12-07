using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;

namespace WindowsFormsApp3
{
    public partial class BallForm : Form
    {
        private Point ballPosition;
        private ManualResetEvent pauseEvent = new ManualResetEvent(false);
        public event EventHandler Paused;

        private int ballSize = 35;
        private int xDirection = 1;
        private bool isPaused = false;

        public BallForm()
        {
            InitializeComponent();
            ballPosition = new Point(0, this.Height / 3);
        }

        public void Animation()
        {
            while (true)
            {
                if (isPaused)
                {
                    pauseEvent.WaitOne();
                }

                ballPosition.X += 5 * xDirection;
                ballPosition.Y = (int)(this.Height / 3.5 + Math.Cos(ballPosition.X / 20.0) * 50);

                if (ballPosition.X <= 0 || ballPosition.X >= this.Width - ballSize)
                {
                    xDirection *= -1;
                }

                Invalidate();
                Thread.Sleep(40);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillEllipse(Brushes.Blue, ballPosition.X, ballPosition.Y, ballSize, ballSize);
        }

        public void pauseThread_Click(object sender, EventArgs e)
        {
            isPaused = true;
            pauseEvent.Reset();
        }

        public void resumeThread_Click(object sender, EventArgs e)
        {
            isPaused = false;
            pauseEvent.Set();
        }

        public void stopThread_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool IsPaused { get; private set; }

        private void BallForm_Load(object sender, EventArgs e)
        {

        }
    }
}
