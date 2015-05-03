using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace _10110139AVL
{
    public partial class Form2 : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBarX1.Increment(1);
            if (progressBarX1.Value == 100)
            {
                timer1.Stop();

            }
            progressBarX1.Text = "Đang mở chương trình... xin hãy đợi_ " + progressBarX1.Value.ToString() + "%";
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
