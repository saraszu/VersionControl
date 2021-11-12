using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using week08right.Abstraction;
using week08right.Entities;

namespace week08right
{
    public partial class Form1 : Form
    {

        private Toy _nextToy;

        List<Toy> _balls = new List<Toy>();

        private BallFactory _toyfactory;
        public BallFactory ToyFactory
        {
            get { return _toyfactory; }
            set
            {
                _toyfactory = value;
                DisplayNext();
            }
        }



        public Form1()
        {
            InitializeComponent();

            ToyFactory = new BallFactory();

        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var toy = ToyFactory.CreateNew();
            _balls.Add(toy);
            toy.Left = -toy.Width;
            mainPanel.Controls.Add(toy);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxPosition = 0;
            foreach (var ball in _balls)
            {
                ball.MoveToy();
                if (ball.Left > maxPosition)
                    maxPosition = ball.Left;
            }

            if (maxPosition > 1000)
            {
                var oldestToy = _balls[0];
                mainPanel.Controls.Remove(oldestToy);
                _balls.Remove(oldestToy);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            ToyFactory = new CarFactory();

        }

        private void btnBall_Click(object sender, EventArgs e)
        {
            ToyFactory = new BallFactory
            {
                BallColor = btnCar.BackColor
            };
        }

        private void DisplayNext()
        {
            if (_nextToy != null)
                Controls.Remove(_nextToy);
            _nextToy = ToyFactory.CreateNew();
            _nextToy.Top = lblNext.Top + lblNext.Height + 20;
            _nextToy.Left = lblNext.Left;
            Controls.Add(_nextToy);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var colorPicker = new ColorDialog();

            colorPicker.Color = button.BackColor;
            if (colorPicker.ShowDialog() != DialogResult.OK)
                return;
            button.BackColor = colorPicker.Color;
        }
    }
}
