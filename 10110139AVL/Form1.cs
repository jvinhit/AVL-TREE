using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar;
using System.IO;
using System.Threading;

namespace _10110139AVL
{
    public partial class Form1 : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Form1()
        {
            #region Form Khoi dong
            Thread t = new Thread(new ThreadStart(LoadFormKhoidong));
            t.Start();
            Thread.Sleep(5000);
            t.Abort();
            #endregion

            InitializeComponent();
            root = null;    //Ban đầu gốc  = null  
            //Hiển thị PictureBox, rộng và cao. 
            bm = new Bitmap(this.pictureBoxhienThi.Width, this.pictureBoxhienThi.Height);
            // vẽ trên cái bm vừa tạo
            graph = Graphics.FromImage(bm);
            // hien thi tren cai picbox la cai bm vừa r
            pictureBoxhienThi.Image = bm;
            // Show ra
            pictureBoxhienThi.Show();

        }
        // cấu trúc của 1 nút 

        public class Node
        {
            public int key;                //Khóa
            public int balFactor;          //Chỉ số cân bằng
            public Node pLeft, pRight;     //Cây con trái, cây con phải của node
            public int X, Y;               //Tọa độ của Node
            public float khoang_cach;      //Khoảng cách từ Node cha đến Node con gần nhất theo chiều X

        }
        //Định nghĩa chỉ số cân bằng
        public const int LH = -1;       //Cây con trái cao hơn
        public const int EH = 0;       //Hai cây con bằng nhau
        public const int RH = 1;       //Cây con phải cao hơn 

        //Trỏ đến gốc gốc cây
        public Node root;

        public int n;       //Số Node
        public int[] arrKey;   //mảng key

        //Các biến để vẽ lên PictureBox
        Bitmap bm;
        Graphics graph;




        public void rotateLL(ref Node T)
        {

            Node T1 = T.pLeft;
            T.pLeft = T1.pRight;
            T1.pRight = T;
            switch (T1.balFactor)
            {
                case LH:
                    T.balFactor = EH;
                    T1.balFactor = EH;
                    break;
                case EH:
                    T.balFactor = LH;
                    T1.balFactor = RH;
                    break;
            }
            T = T1;
        }

        //Hàm quay đơn Right-Right
        public void rotateRR(ref Node T)
        {
            Node T1 = T.pRight;
            T.pRight = T1.pLeft;
            T1.pLeft = T;
            switch (T1.balFactor)
            {
                case RH:
                    T.balFactor = EH;
                    T1.balFactor = EH;
                    break;
                case EH:
                    T.balFactor = RH;
                    T1.balFactor = LH;
                    break;
            }
            T = T1;
        }

        //Hàm quay kép Left-Right
        public void rotateLR(ref Node T)
        {
            Node T1 = T.pLeft;
            Node T2 = T1.pRight;
            T.pLeft = T2.pRight;
            T2.pRight = T;
            T1.pRight = T2.pLeft;
            T2.pLeft = T1;
            switch (T2.balFactor)
            {
                case LH:
                    T.balFactor = RH;
                    T1.balFactor = EH;
                    break;
                case EH:
                    T.balFactor = EH;
                    T1.balFactor = EH;
                    break;
                case RH:
                    T.balFactor = EH;
                    T1.balFactor = LH;
                    break;
            }
            T2.balFactor = EH;
            T = T2;
        }

        //Hàm quay kép Right-Left
        public void rotateRL(ref Node T)
        {
            Node T1 = T.pRight;
            Node T2 = T1.pLeft;
            T.pRight = T2.pLeft;
            T2.pLeft = T;
            T1.pLeft = T2.pRight;
            T2.pRight = T1;
            switch (T2.balFactor)
            {
                case RH:
                    T.balFactor = LH;
                    T1.balFactor = EH;
                    break;
                case EH:
                    T.balFactor = EH;
                    T1.balFactor = EH;
                    break;
                case LH:
                    T.balFactor = EH;
                    T1.balFactor = RH;
                    break;
            }
            T2.balFactor = EH;
            T = T2;
        }





        //Cân bằng khi cây lệch trái
        public int balanceLeft(ref Node T)
        {
            Node T1 = T.pLeft;

            switch (T1.balFactor)
            {
                case LH:
                    rotateLL(ref T);
                    return 2;
                case EH:
                    rotateLL(ref T);
                    return 1;
                case RH:
                    rotateLR(ref T);
                    return 2;
            }
            return 0;
        }

        //Cân bằng khi cây lệch phải
        public int balanceRight(ref Node T)
        {
            Node T1 = T.pRight;

            switch (T1.balFactor)
            {
                case LH:
                    rotateRL(ref T);
                    return 2;
                case EH:
                    rotateRR(ref T);
                    return 1;
                case RH:
                    rotateRR(ref T);
                    return 2;
            }
            return 0;
        }
        //



        //////////////////////////////////////////////////////////////////////////
        // thêm 1 phần tử

        ///
        public int insertNode(ref Node T, int x)
        {
            int res;

            //Nếu tồn tại cây
            if (T != null)
            {
                if (T.key == x) //Tồn tại x trong cây
                    return 0;
                if (x < T.key)
                {
                    res = insertNode(ref T.pLeft, x);
                    if (res < 2)
                        return res;

                    //Gán lại chỉ số cân bằng cho các Node trong cây
                    switch (T.balFactor)
                    {
                        case RH:
                            T.balFactor = EH;
                            return 1;
                        case EH:
                            T.balFactor = LH;
                            return 2;
                        case LH:
                            balanceLeft(ref T);
                            return 1;
                    }
                }
                else
                {
                    res = insertNode(ref T.pRight, x);
                    if (res < 2)
                        return res;

                    //Gán lại chỉ số cân bằng cho các Node trong cây
                    switch (T.balFactor)
                    {
                        case LH:
                            T.balFactor = EH;
                            return 1;
                        case EH:
                            T.balFactor = RH;
                            return 2;
                        case RH:
                            balanceRight(ref T);
                            return 1;
                    }
                }
            }

            T = new Node();
            if (T == null)
                return -1;  //Thiếu bộ nhớ
            T.key = x;
            T.balFactor = EH;
            T.pLeft = T.pRight = null;
            return 2;   //Thành công, chiều cao tăng
        }
        ////////
        ////
        ////
        /////





        ////
        //
        /*
        * Hàm trả về các giá trị:
        * 1:   Thành công
        * 0:   Không tìm thấy x trong cây
        * 2:   Thành công, chiều cao cây giãm
        */

        public int delNode(ref Node T, int x)
        {
            int res;

            //Nếu cây rổng
            if (T == null) return 0;

            //x nằm bên trái
            if (x < T.key)
            {
                res = delNode(ref T.pLeft, x);
                if (res < 2) return res;
                switch (T.balFactor)
                {
                    case LH:
                        T.balFactor = EH;
                        return 2;
                    case EH:
                        T.balFactor = RH;
                        return 1;
                    case RH:
                        return balanceRight(ref T);
                    default: return -2;
                }
            }

            //x nằm bên phải
            else if (x > T.key)
            {
                res = delNode(ref T.pRight, x);
                if (res < 2) return res;
                switch (T.balFactor)
                {
                    case RH:
                        T.balFactor = EH;
                        return 2;
                    case EH:
                        T.balFactor = LH;
                        return 1;
                    case LH:
                        return balanceLeft(ref T);
                    default: return -2;
                }
            }

            //Tìm thấy x
            else
            {
                Node p = T;
                //Nếu con trái của node đang xóa là null, thì gán node đang xóa bằng con phải của node đang xóa
                if (T.pLeft == null)
                {
                    T = T.pRight;
                    res = 2;
                }

                //Nếu con phải của node đang xóa là null, thì gán node đang xóa bằng con trái của node đang xóa
                else if (T.pRight == null)
                {
                    T = T.pLeft;
                    res = 2;
                }

                //Node đang xóa có cả 2 con
                else
                {
                    //Tìm phần tử thế mạng
                    res = SearchStandFor(ref p, ref T.pRight);
                    if (res < 2) return res;

                    switch (T.balFactor)
                    {
                        case RH:
                            T.balFactor = EH;
                            return 2;
                        case EH:
                            T.balFactor = LH;
                            return 1;
                        case LH:
                            return balanceLeft(ref T);
                        default: return -2;
                    }
                }
                return res;
            }
        }
        //Hàm tìm phần tử thế mạng cho Node đang xóa
        public int SearchStandFor(ref Node p, ref Node q)
        {
            int res;
            if (q.pLeft != null)
            {
                res = SearchStandFor(ref p, ref q.pLeft);
                if (res < 2)
                {
                    return res;
                }
                switch (q.balFactor)
                {
                    case LH:
                        q.balFactor = EH;
                        return 2;
                    case EH:
                        q.balFactor = RH;
                        return 1;
                    case RH:
                        return balanceRight(ref q);
                    default: return -2;
                }
            }
            else
            {
                p.key = q.key;
                p = q;
                q = q.pRight;
                return 2;
            }
        }
        //Trả về 1 Node khi tìm thấy nó trong cây AVL
        public Node SearchNode(int x, Node T, Graphics graph, Bitmap bm, Form1 f)
        {
            if (T == null)
            {
                return null;    //Không thấy
            }
            else
            {
                VeCay(root, graph, bm, this);
                NodeNow(T, graph, bm, f);
                f.pictureBoxhienThi.Image = bm;
                System.Threading.Thread.Sleep(1000);
                Application.DoEvents();

                if (T.key == x)
                {
                    return T;   //Tìm thấy
                }
                else if (x < T.key)
                    return SearchNode(x, T.pLeft, graph, bm, this);  //Tìm bên trái
                else
                    return SearchNode(x, T.pRight, graph, bm, this); //Tìm bên phải
            }
        }



        //Hàm xác định vị trí của các Node
        public void TimViTri(Node T, Node node)
        {
            if (node != null)
            {
                POS(T, node);                //Xác định vị trí của Node đang xét
                TimViTri(node, node.pLeft);     //Xác định vị trí của Node con trái
                TimViTri(node, node.pRight);    //Xác định vị trí của Node con phải
            }
        }

        //Xác định vị trí của Node đang xét
        private void POS(Node T, Node node)
        {
            //Nếu là cây con phải
            if (node.key > T.key)
            {
                //Tìm tọa độ X của node
                node.X = T.X + Convert.ToInt32(T.khoang_cach);
            }
            //Nếu là cây con trái
            else
            {
                //Tìm tọa độ X của node
                node.X = T.X - Convert.ToInt32(T.khoang_cach);
            }
            node.Y = T.Y + 100;             //Tăng Y thêm 85 để tìm vị trí các Node con phía dưới
            node.khoang_cach = (T.khoang_cach) / 2;        //Giãm khoảng cách giữa các Node 
        }
        //public void VeNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        //{
        //    graph.FillEllipse(Brushes.BlueViolet, node.X, node.Y, 32, 32);
        //    //VẼ KEY VÀ BALFACTOR
        //    graph.DrawString(node.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.White, node.X + 8, node.Y + 10);
        //    switch (node.balFactor)
        //    {
        //        case -1: graph.DrawString("LH", new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, node.X + 6, node.Y + 32); break;
        //        case 0: graph.DrawString("EH", new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, node.X + 6, node.Y + 32); break;
        //        case 1: graph.DrawString("RH", new Font(FontFamily.GenericSansSerif, 10), Brushes.Black, node.X + 6, node.Y + 32); break;
        //    }
        //}
        public void VeNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.DrawEllipse(new Pen(Color.Cyan, 5), node.X, node.Y, 32, 32);
            graph.FillEllipse(new SolidBrush(Color.DarkOrange), node.X, node.Y, 32, 32);
            //VẼ KEY VÀ BALFACTOR
            graph.DrawString(node.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.White, node.X + 7, node.Y +8);
            switch (node.balFactor)
            {
                case -1: graph.DrawString("LH", new Font(FontFamily.GenericSansSerif, 9), Brushes.Black, node.X /*+ 20*/, node.Y + 32); break;
                case 0: graph.DrawString("EH", new Font(FontFamily.GenericSansSerif, 9), Brushes.Black, node.X/* + 20*/, node.Y + 32); break;
                case 1: graph.DrawString("RH", new Font(FontFamily.GenericSansSerif, 9), Brushes.Black, node.X /*+ 20*/, node.Y + 32); break;
            }
        }
        // Khi su dung ta phai khai bao r de noi rang do la ban kinh cua cong trong chua key node tronf do 


        //public void DrawNode(int key, float x, float y, Graphics graph, Bitmap bm, Form1 f)
        //{
        //    graph.DrawEllipse(new Pen(Color.Red, 2), x - f.r, y - f.r, 2 * f.r, 2 * f.r);
        //    graph.FillEllipse(Brushes.Orange, x - f.R, y - f.R, 2 * f.R, 2 * f.R);
        //    graph.DrawString(key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.White, x - f.r / 2, y - f.r / 2 + 3);
        //}

        //Node đang thực hiện
        public void NodeNow(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.FillEllipse(Brushes.Yellow, node.X, node.Y, 32, 32);
            //VẼ KEY VÀ BALFACTOR
            graph.DrawString(node.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, node.X + 8, node.Y + 10);

        }

        //Vẽ tất cả các Node trong cây
        private void VeTatCaNode(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            VeNode(node, graph, bm, f);
            if (node.pLeft != null)
                VeTatCaNode(node.pLeft, graph, bm, f);
            if (node.pRight != null)
                VeTatCaNode(node.pRight, graph, bm, f);
        }

        //Vẽ tất cả các line trong cây
        private void VeTatCaLine(Node node, Graphics graph, Bitmap bm, Form1 f)
        {
            if (node.pLeft != null)
               // graph.DrawLine(new Pen(Color.DarkGreen, 1f), node.X, node.Y + 32, node.pLeft.X + 32, node.pLeft.Y);
                graph.DrawLine(new Pen(Color.DarkGreen, 1f), node.X, node.Y+30, node.pLeft.X +30 , node.pLeft.Y);
            if (node.pRight != null)
                graph.DrawLine(new Pen(Color.DarkGreen, 1f), node.X+30 , node.Y+30 , node.pRight.X, node.pRight.Y);
            if (node.pLeft != null)
                VeTatCaLine(node.pLeft, graph, bm, f);
            if (node.pRight != null)
                VeTatCaLine(node.pRight, graph, bm, f);
        }
        //private void DrawAllLine(Node node, Graphics graph, Bitmap bm, Form1 f)
        //{
        //    if (node.pLeft != null)
        //        graph.DrawLine(new Pen(Color.DarkGreen, 1f), node.x, node.y, node.pLeft.x, node.pLeft.y);
        //    if (node.pRight != null)
        //        graph.DrawLine(new Pen(Color.DarkGreen, 1f), node.x, node.y, node.pRight.x, node.pRight.y);
        //    if (node.pLeft != null)
        //        DrawAllLine(node.pLeft, graph, bm, f);
        //    if (node.pRight != null)
        //        DrawAllLine(node.pRight, graph, bm, f);
        //}


        //Vẽ cây AVL
        public void VeCay(Node root, Graphics graph, Bitmap bm, Form1 f)
        {
            graph.SmoothingMode = SmoothingMode.HighQuality;
            VeTatCaLine(root, graph, bm, f);
            VeTatCaNode(root, graph, bm, f);
            f.pictureBoxhienThi.Image = bm;
            f.pictureBoxhienThi.Show();
        }

        //Tạo hiệu ứng nhấp nháy
        public void NhapNhay(Node p)
        {
            graph.FillEllipse(Brushes.Yellow, p.X, p.Y, 32, 32);
            graph.DrawString(p.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, p.X + 8, p.Y + 10);
            pictureBoxhienThi.Image = bm;
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            graph.FillEllipse(Brushes.BlueViolet, p.X, p.Y, 32, 32);
            graph.DrawString(p.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.White, p.X + 8, p.Y + 10);
            pictureBoxhienThi.Image = bm;
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            graph.FillEllipse(Brushes.Yellow, p.X, p.Y, 32, 32);
            graph.DrawString(p.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, p.X + 8, p.Y + 10);
            pictureBoxhienThi.Image = bm;
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            graph.FillEllipse(Brushes.BlueViolet, p.X, p.Y, 32, 32);
            graph.DrawString(p.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.White, p.X + 8, p.Y + 10);
            pictureBoxhienThi.Image = bm;
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            graph.FillEllipse(Brushes.Yellow, p.X, p.Y, 32, 32);
            graph.DrawString(p.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, p.X + 8, p.Y + 10);
            pictureBoxhienThi.Image = bm;
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            graph.FillEllipse(Brushes.BlueViolet, p.X, p.Y, 32, 32);
            graph.DrawString(p.key.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.White, p.X + 8, p.Y + 10);
            pictureBoxhienThi.Image = bm;
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string chuoi = textBoxX1.Text.Trim();
            if (chuoi != "")
            {
                //Loại bỏ khoảng trắng thừa giữa chuỗi
                while (chuoi.Contains("  ")) //2 khoảng trắng
                {
                    chuoi = chuoi.Replace("  ", " "); //Replace 2 khoảng trắng thành 1 khoảng trắng
                }
                //Sau khi được 1 chuổi hoàn hảo, thì tìm số phần tử = số lượng khoảng trắng + 1
                int dem = 0;
                for (int i = 0; i < chuoi.Length - 1; i++)
                {
                    if (chuoi[i] == ' ')
                        dem++;
                }
                string[] str = chuoi.Split(' ');
                n = dem + 1;  //Số node trong cây AVL            
                arrKey = new int[n];
                //Khởi tạo mảng key
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        arrKey[i] = Convert.ToInt32(str[i]);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBoxEx.Show("Chỉ được nhập các số nguyên !", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxX1.ResetText();
                        pictureBoxhienThi.Refresh();
                        //button2_Click(null, null);
                        return;
                    }
                }
                //Chèn n Node vào cây
                for (int i = 0; i < n; i++)
                {
                    insertNode(ref root, arrKey[i]);
                }

                //Tọa độ root
                root.X = pictureBoxhienThi.Width / 2;
                root.Y = 50;
                root.khoang_cach = pictureBoxhienThi.Width / 4;

                //Tìm tọa độ các Node khác
                TimViTri(root, root.pLeft);   //Cây con trái
                TimViTri(root, root.pRight);  //Cây con phải

                //Vẽ tree AVL
                VeCay(root, graph, bm, this);
            }
            else
                MessageBoxEx.Show("Chưa nhập node !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBoxX1.ResetText();
        }

        #region OpenFile
        private void Nhap(string path)
        {
            string[] S;
            FileStream f = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(f);
            n = Convert.ToInt32(sr.ReadLine());
            arrKey = new int[n];
            string str = sr.ReadLine();
            S = str.Split(' ');
            for (int i = 0; i < n; i++)
            {
                arrKey[i] = Convert.ToInt32(S[i]);
            }
            sr.Dispose();
            f.Dispose();
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OD = new OpenFileDialog();
            OD.Title = "Select file";
            OD.Filter = "Text File|*.txt";
            if (OD.ShowDialog() == DialogResult.OK)
                Nhap(OD.FileName);
            if (n != 0)
            {

                for (int i = 0; i < n; i++)
                {
                    insertNode(ref root, arrKey[i]);
                }
                root.X = pictureBoxhienThi.Width / 2;
                root.Y = 50;
                root.khoang_cach = pictureBoxhienThi.Width / 4;

                //Tìm tọa độ các Node khác
                TimViTri(root, root.pLeft);   //Cây con trái
                TimViTri(root, root.pRight);  //Cây con phải
                VeCay(root, graph, bm, this);
            }
        #endregion
        }

       

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Convert.ToInt32(txtThemXoaSua.Text);

                if (root == null)
                {
                    MessageBoxEx.Show("Chưa có cây !");
                    return;
                }
                Node p = SearchNode(x, root, graph, bm, this);
                if (p != null)
                {
                    System.Threading.Thread.Sleep(1000);
                    MessageBoxEx.Show("Tồn tại node trong cây!");
                    NhapNhay(p);
                    return;
                }
                else
                {
                   
                    System.Threading.Thread.Sleep(1000);
                    insertNode(ref root, x);
                   // MessageBoxEx.EnableGlass = false;
                    //MessageBoxEx.Show("Ban Muon <font color='#B02B2C'>Thoat</font>", "Thong Bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    MessageBoxEx.Show("Thêm thành công !");
                    //Tọa độ root
                    root.X = pictureBoxhienThi.Width / 2;
                    root.Y = 50;
                    root.khoang_cach = pictureBoxhienThi.Width / 4;

                    //Tìm tọa độ các Node khác
                    TimViTri(root, root.pLeft);   //Cây con trái
                    TimViTri(root, root.pRight);  //Cây con phải

                    //Vẽ tree AVL
                    graph.Clear(this.pictureBoxhienThi.BackColor);
                    pictureBoxhienThi.Image = bm;
                    VeCay(root, graph, bm, this);
                    pictureBoxhienThi.Image = bm;
                }
                txtThemXoaSua.ResetText();
            }
            catch (System.Exception ex)
            {
                MessageBoxEx.Show("Không hợp lệ !");
            }
        }

  #region LoadForm khoi dong
     private void LoadFormKhoidong()
        {
            Application.Run(new Form2());
        }
    #endregion

     private void btnXoa_Click(object sender, EventArgs e)
     {
         try
         {
             int x = Convert.ToInt32(txtThemXoaSua.Text);

             if (root == null)
             {
                 MessageBoxEx.Show("Chưa có cây !");
                 return;
             }
             Node p = SearchNode(x, root, graph, bm, this);
             if (p != null)
             {
                 System.Threading.Thread.Sleep(1000);
                 MessageBoxEx.Show("Tìm thấy - Xóa Thành Công !");
                 NhapNhay(p);

                 delNode(ref root, x);

                 //Tọa độ root
                 root.X = pictureBoxhienThi.Width / 2;
                 root.Y = 50;
                 root.khoang_cach = pictureBoxhienThi.Width / 4;

                 //Tìm tọa độ các Node khác
                 TimViTri(root, root.pLeft);   //Cây con trái
                 TimViTri(root, root.pRight);  //Cây con phải

                 //Vẽ tree AVL
                 graph.Clear(this.pictureBoxhienThi.BackColor);
                 pictureBoxhienThi.Image = bm;
                 VeCay(root, graph, bm, this);
                 pictureBoxhienThi.Image = bm;
             }
             else
             {
                 System.Threading.Thread.Sleep(1000);
                 VeCay(root, graph, bm, this);
                 pictureBoxhienThi.Image = bm;
                 MessageBoxEx.Show("Không thấy - Không xóa Được !");
             }
             txtThemXoaSua.ResetText();
         }
         catch (System.Exception ex)
         {
             MessageBoxEx.Show("Không hợp lệ !");
         }
     }

     private void btnTim_Click(object sender, EventArgs e)
     {
         try
         {
             int x = Convert.ToInt32(txtThemXoaSua.Text);
             if (root == null)
             {
                 MessageBoxEx.Show("Chưa có cây !");
                 return;
             }

             Node p = SearchNode(x, root, graph, bm, this);
             if (p != null)
             {
                 System.Threading.Thread.Sleep(1000);
                 MessageBoxEx.Show("Tìm thấy !");
                 NhapNhay(p);
                 VeCay(root, graph, bm, this);
                 pictureBoxhienThi.Image = bm;
             }
             else
             {
                 System.Threading.Thread.Sleep(1000);
                 VeCay(root, graph, bm, this);
                 pictureBoxhienThi.Image = bm;
                 MessageBoxEx.Show("Không thấy !");
             }
             txtThemXoaSua.ResetText();
         }
         catch (System.Exception ex)
         {
             MessageBoxEx.Show("Không hợp lệ !");
         }
     }

     private void labelX6_Click(object sender, EventArgs e)
     {

     }

     private void btnX1Reset_Click(object sender, EventArgs e)
     {
         Application.Restart();

     }

   
    
    }
}
