using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmMain
{
    public partial class FrmClassReserved : Form
    {
        public tIdentity identity { get; set; }
        public FrmClassReserved()
        {
            InitializeComponent();
        }

        private void FrmClassReserved_Load(object sender, EventArgs e)
        {
            ShowClassReserved();

        }

        private void ShowClassReserved()
        {
            gymEntities db = new gymEntities();
            int findID = this.identity.id;
            var classreserved = from c in db.tclasses
                                from cr in db.tclass_reserve
                                from i in db.tIdentity
                                from cs in db.tclass_schedule
                                from t in db.ttimes_detail
                                where cr.member_id == findID
                                where cr.class_schedule_id == cs.class_schedule_id
                                where cs.class_id == c.class_id
                                where cs.coach_id == i.id
                                where cs.course_time_id == t.time_id
                                where cr.reserve_status == true
                                select new { classes = c.class_name, identity = i.name, classSchedule = cs.course_date, time = t.time_name };

            foreach (var item in classreserved)
            {
                reservebox rb = new reservebox();

                rb.Width = 700;
                rb.Height = 180;
                rb.Location = new System.Drawing.Point(flowLayoutPanel1.Width / 2 - rb.Width / 2);

                rb.cls = item.classes;
                rb.ids = item.identity;
                rb.csch = item.classSchedule.ToString();
                rb.time = item.time;
                rb.showReserve += showReserve;
                flowLayoutPanel1.Controls.Add(rb);
            }
        }

        private void showReserve(reservebox p)
        {
            using (gymEntities db = new gymEntities())
            {
                tclass_reserve reservation = db.tclass_reserve.FirstOrDefault(r => r.member_id == this.identity.id && r.tclass_schedule.tclasses.class_name == p.cls);
                if (reservation != null)
                {
                    reservation.reserve_status = false;
                    db.SaveChanges();
                    db.Entry(reservation).Reload();

                    MessageBox.Show("取消成功");
                    this.flowLayoutPanel1.Controls.Clear();
                    ShowClassReserved();
                }
            }
        }
    }
}
