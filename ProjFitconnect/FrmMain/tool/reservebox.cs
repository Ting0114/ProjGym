using mid_Coonect.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmMain
{
    public delegate void Dshow(reservebox p);
    public partial class reservebox : UserControl
    {
        public event Dshow showReserve;
        private tclasses _tc;
        private tclass_schedule _ts;
        //private tclass_reserve _tr;
        private tIdentity _i;
        private ttimes_detail _td;
        public tclasses tc
        {
            get { return _tc; }
            set
            {
                _tc = value;
                label5.Text = _tc.class_name;
                if (!string.IsNullOrEmpty(_tc.class_photo))
                {
                    string path = Application.StartupPath + "\\ClassPic";
                    pictureBox1.Image = new Bitmap(path + "\\" + _tc.class_photo);
                }
            }
        }
        //public tIdentity i { get { return _i; } set { _i = value;} }
        //public tclass_schedule ts { get { return _ts; } set { _ts = value;} }
        //public ttimes_detail td { get { return _td; } set { _td = value; } }

        public string cls 
        {
            get { return this.label5.Text; }
            set { this.label5.Text = value;}
        }
        public string ids 
        {
            get { return this.label6.Text; }
            set { this.label6.Text = value; }
        }
        public string csch { set { this.label7.Text = value; } }
        public string time { set { this.label8.Text = value; } }

        public reservebox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.showReserve != null) this.showReserve(this);
        }
    }
}
