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
        public tclasses tc { get { return _tc; } set { 
                _tc = value; 
                label5.Text = _tc.class_name;
                if (!string.IsNullOrEmpty(_tc.class_photo))
                {
                    string path = Application.StartupPath + "\\ClassPic";
                    pictureBox1.Image = new Bitmap(path + "\\" + _tc.class_photo);
                }
            } }
        public tIdentity i { get { return _i; } set { _i = value; label6.Text = _i.name; } }
        public tclass_schedule ts { get { return _ts; } set { _ts = value; label7.Text = _ts.course_date.ToShortDateString()/*.ToString()*/; } }
        public ttimes_detail td { get { return _td; } set { _td = value; label8.Text = _td.time_name; } }

        
        public reservebox()
        {
            InitializeComponent();
        }

    }
}
