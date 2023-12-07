using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class FireForm : Form
    {
        private Random random;
        private FireParticle[] particles;
        private ManualResetEvent pauseEvent = new ManualResetEvent(false);
        public event EventHandler Paused;

        private const int numParticles = 200;
        private const int particleSize = 5;
        private const int maxY = 200;
        private bool isPaused = false;

        public FireForm()
        {
            InitializeComponent();
            random = new Random();
            particles = new FireParticle[numParticles];

            for (int i = 0; i < numParticles; i++)
            {
                particles[i] = new FireParticle(random, maxY);
            }
        }

        public void Animation()
        {
            while (true)
            {
                if (isPaused)
                {
                    pauseEvent.WaitOne();
                }

                foreach (var particle in particles)
                {
                    particle.Update();
                }

                Invalidate();
                Thread.Sleep(40);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var particle in particles)
            {
                e.Graphics.FillEllipse(Brushes.Red, particle.X, particle.Y, particleSize, particleSize);
            }
        }

        private class FireParticle
        {
            public int X { get; set; }
            public int Y { get; set; }
            private int speed;
            private int maxY;
            private Random random;

            public FireParticle(Random random, int maxY)
            {
                X = random.Next(0, 400);
                Y = random.Next(0, maxY);
                speed = random.Next(1, 5);
                this.maxY = maxY;
                this.random = random;
            }

            public void Update()
            {
                Y -= speed;
                if (Y < 0)
                {
                    Y = maxY;
                    X = random.Next(0, 400);
                }
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

        private void FireForm_Load(object sender, EventArgs e)
        {

        }
    }
}
