using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class MovingFigure : Form
    {
        private ManualResetEvent pauseEvent = new ManualResetEvent(false);
        public event EventHandler Paused;

        private int figureSize = 50;
        private int maxSize = 150;
        private int minSize = 50;
        private int sizeChange = 3;
        private bool increasing = true;
        private bool isPaused = false;

        public MovingFigure()
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

                if (increasing)
                {
                    figureSize += sizeChange;
                    if (figureSize >= maxSize)
                    {
                        increasing = false;
                    }
                }
                else
                {
                    figureSize -= sizeChange;
                    if (figureSize <= minSize)
                    {
                        increasing = true;
                    }
                }

                Invalidate();
                Thread.Sleep(40);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int centerX = this.Width / 2;
            int centerY = this.Height / 3;

            int x = centerX - figureSize / 2;
            int y = centerY - figureSize / 2;

            e.Graphics.FillRectangle(Brushes.Red, x, y, figureSize, figureSize);
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

        private void MovingFigure_Load(object sender, EventArgs e)
        {

        }
    }
}
