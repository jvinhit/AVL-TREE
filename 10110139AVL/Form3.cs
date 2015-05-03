using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace _10110139AVL
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private Object oDocument; 

        private void button1_Click(object sender, EventArgs e)
        {

            String strFileName;

            //Find the Office document.
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            strFileName = openFileDialog1.FileName;

            //If the user does not cancel, open the document.
            if (strFileName.Length != 0)
            {
                Object refmissing = System.Reflection.Missing.Value;
                oDocument = null;
                axWebBrowser1.Navigate(strFileName, ref refmissing, ref refmissing, ref refmissing, ref refmissing);
            }
        }

        public void Form1_Load(object sender, System.EventArgs e)
        {
            button1.Text = "Browse";
            openFileDialog1.Filter = "Office Documents(*.doc, *.xls, *.ppt)|*.doc;*.xls;*.ppt";
            openFileDialog1.FilterIndex = 1;
        }

        public void Form1_Closed(object sender, System.EventArgs e)
        {
            oDocument = null;
        }

        public void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
        {

            //Note: You can use the reference to the document object to 
            //      automate the document server.

            Object o = e.pDisp;

            oDocument = o.GetType().InvokeMember("Document", BindingFlags.GetProperty, null, o, null);

            Object oApplication = o.GetType().InvokeMember("Application", BindingFlags.GetProperty, null, oDocument, null);

            Object oName = o.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, oApplication, null);

            MessageBox.Show("File opened by: " + oName.ToString()); 
        }
    }
}
