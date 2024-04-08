using FrmMain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Gym
{
    public partial class FrmFollow : Form
    {
        public tIdentity identity { get; set; }
        private tmember_follow _member_follow;
        private tIdentity _identity;
        private int _id;
        public tmember_follow member_follow
        {
            get
            {
                return _member_follow;
            }
            set
            {
                _member_follow = value;
                if (_member_follow != null)
                {
                    _member_follow.coach_id = _id;
                }
            }
        }
        public FrmFollow()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtRate.Visible = true;
            btnSave.Visible = true;
            cbStar.Visible = true;
            lblRate.Visible = true;
            label5.Visible = true;
        }
        private void FrmFollow_Load(object sender, EventArgs e)
        {
            gymEntities db = new gymEntities();
            cbCoach.Items.Clear();
            var name = from n in db.tIdentity
                       where n.role_id == 2
                       select n;
            name.ToList().ForEach(n => { cbCoach.Items.Add(n.name); });
        }

        private void btnFollow_Click(object sender, EventArgs e)
        {
            label2.Text = string.Empty;
            if (checkcoach() == -1) { return; }
            gymEntities db = new gymEntities();
            int findID = this.identity.id;

            var existingFollow = db.tmember_follow.FirstOrDefault(f => f.member_id == findID && f.status_id == 1);

            if (existingFollow != null)
            {
                db.tmember_follow.Remove(existingFollow);
                db.SaveChanges();
                MessageBox.Show("取消追蹤");
            }
            else
            {
                tmember_follow MemberFollow = new tmember_follow();
                MemberFollow.member_id = findID;
                MemberFollow.status_id = 1;
                MemberFollow.coach_id = _id;
                db.tmember_follow.Add(MemberFollow);
                db.SaveChanges();
                MessageBox.Show("追蹤成功");
            }

        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            label2.Text = string.Empty;
            if (checkcoach() == -1) { return; }
            gymEntities db = new gymEntities();
            int findID = this.identity.id;

            var existingBlacklist = db.tmember_follow.FirstOrDefault(f => f.member_id == findID && f.status_id == 2);

            if (existingBlacklist != null)
            {
                db.tmember_follow.Remove(existingBlacklist);
                db.SaveChanges();
                MessageBox.Show("已移除黑名單");
            }
            else
            {
                tmember_follow MemberFollow = new tmember_follow();
                MemberFollow.member_id = findID;
                MemberFollow.status_id = 2;
                MemberFollow.coach_id = _id;
                db.tmember_follow.Add(MemberFollow);
                db.SaveChanges();
                MessageBox.Show("已加入黑名單");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            label2.Text = label3.Text = label4.Text =string.Empty;
            if (check() == -1) { return; }
            gymEntities db = new gymEntities();
            tmember_follow t = new tmember_follow();
            t.member_id = this.identity.id;
            t.status_id = 3;
            t.coach_id = _id;
            t.stars = cbStar.SelectedIndex;
            t.describe = this.txtRate.Text;
            db.tmember_follow.Add(t);
            if (afterCourseSubmit() == 0)
            {
                db.SaveChanges();
                MessageBox.Show("評價成功");
                this.Close();
            }
            else
                return;
        }
        private int checkcoach()
        {
            bool hasError = false;
            if (cbCoach.SelectedIndex == -1)
            {
                label2.Text = "請選擇教練"; hasError = true;
            }
            if (hasError)
                return -1;
            else
                return 0;
        }
        private int check()
        {
            bool hasError = false;
            if (string.IsNullOrEmpty(txtRate.Text))
            {
                label3.Text = "請輸入評價"; hasError = true;
            }
            if (cbCoach.SelectedIndex == -1)
            {
                label2.Text = "請選擇教練"; hasError = true;
            }
            if (cbStar.SelectedIndex == -1)
            {
                label4.Text = "請選擇星數"; hasError = true;
            }
            if (hasError)
                return -1;
            else
                return 0;
        }
        private int afterCourseSubmit()
        {
            DialogResult result = MessageBox.Show("確認送出?", "提交評價", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK) { return 0; }
            else { return -1; }
        }

        private void cbCoach_SelectedIndexChanged(object sender, EventArgs e)
        {
            gymEntities db = new gymEntities();
            tIdentity i = db.tIdentity.FirstOrDefault(x => x.name.Equals(this.cbCoach.SelectedItem.ToString()));
            _id = i.id;
        }
    }
}
