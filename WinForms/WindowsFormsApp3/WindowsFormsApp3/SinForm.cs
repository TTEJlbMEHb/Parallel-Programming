using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class SinForm : Form
    {
        private ManualResetEvent pauseEvent = new ManualResetEvent(false);
        public event EventHandler Paused;

        private int xOffset = 0;
        private int waveLength = 300;
        private int amplitude = 100;
        private double frequency = 0.03;
        private bool isPaused = false;

        public SinForm()
        {
            InitializeComponent();
        }

        public void Animation()
        {
            while (true)
            {
                if (isPaused)
                {
                    pauseEvent.WaitOne();
                }

                xOffset += 5;
                if (xOffset > this.Width)
                {
                    xOffset = -waveLength;
                }

                Invalidate();
                Thread.Sleep(40);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int centerY = this.Height / 3;

            using (Pen pen = new Pen(Color.Blue, 3))
            {
                GraphicsPath path = new GraphicsPath();

                for (int x = 0; x < this.Width; x += 5)
                {
                    int y = centerY + (int)(amplitude * Math.Sin(frequency * (x + xOffset)));
                    path.AddLine(x, y, x, y);
                }

                e.Graphics.DrawPath(pen, path);
            }
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

        private void SinForm_Load(object sender, EventArgs e)
        {

        }
    }
}
