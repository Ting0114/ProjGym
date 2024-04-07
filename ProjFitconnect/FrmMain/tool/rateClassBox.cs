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

namespace FrmMain.tool
{
    public delegate void DRateClass(rateClassBox p);

    public partial class rateClassBox : UserControl
    {
        public event DRateClass DRateClass;

        private tIdentity _i;
        private tmember_rate_class _rc;
        private tclass_schedule _cs;
        private tclasses _c;
        private tclass_reserve _cr;

        public tIdentity i { get { return _i; } set { _i = value; lblCoach.Text = _i.name; } }
        public tmember_rate_class rc { get { return _rc; } set { _rc = value; txtrate.Text = _rc.rate.ToString(); txtFeedback.Text = _rc.describe; } }
        public tclass_schedule cs { get { return _cs; } set { _cs = value; lblDate.Text = _cs.course_date.ToString(); } }
        public tclasses c { get { return _c; } set { _c = value; lblClassName.Text = _c.class_name; } }

        public tclass_reserve cr { get { return _cr; } set { _cr = value; } }

        public TextBox FeedbackTextBox { get { return txtFeedback; } }
        public TextBox rateTextBox { get { return txtrate; } }
        public rateClassBox()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DRateClass != null) this.DRateClass(this);
        }
    }
}
