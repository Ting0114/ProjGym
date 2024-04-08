using FrmMain;
using ProjGym;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mid_Coonect
{
    public partial class FrmAdmin_ClassUpdate : Form
    {
        private DialogResult _isOK;
        public DialogResult isOK { get { return _isOK; } }
        private string _imagepath;
        private tclasses _course;
        int _index = 1;
        public tclasses course
        {
            get
            {
                if (_course == null) { _course = new tclasses(); }
                _course.class_id = Convert.ToInt32(lbl_ID.Text);
                _course.class_sort1_id = Convert.ToInt32(cb_Sort1.Text);
                _course.class_sort2_id = Convert.ToInt32(cb_Sort2.Text);
                _course.limited_gender = Convert.ToInt32(cb_Limit.Text);
                //_course.class_status = Convert.ToInt32(cb_Status.Text);
                _course.class_name = txt_ClassName.Text;
                _course.class_introduction = this.txt_Introduction.Text;
                _course.class_photo = _imagepath;
                return _course;
            }
            set
            {
                _course = value;
                lbl_ID.Text = _course.class_id.ToString();
                cb_Sort1.Text = _course.class_sort1_id.ToString();
                cb_Sort2.Text = _course.class_sort2_id.ToString();
                cb_Limit.Text = _course.limited_gender.ToString();
                //cb_Status.Text = _course.class_status.ToString();
                txt_ClassName.Text = _course.class_name;
                txt_Introduction.Text = _course.class_introduction;
                _imagepath = _course.class_photo;
                if (!string.IsNullOrEmpty(_imagepath))
                {
                    string path = Application.StartupPath + "\\ClassPic";
                    pb_ClassPhoto.Image = new Bitmap(path + "\\" + _imagepath);
                }
            }
        }
        public FrmAdmin_ClassUpdate()
        {
            InitializeComponent();
        }

        private void pb_ClassPhoto_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "課程照片|*.jpg;*.png";
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            _imagepath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            string path = Application.StartupPath + "\\ClassPic";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            File.Copy(openFileDialog1.FileName, path + "\\" + _imagepath);
            this.pb_ClassPhoto.Image = new Bitmap(path + "\\" + _imagepath);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            _isOK = DialogResult.Cancel;
            this.Close();
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            if (this.lbl_ID.Text.ToString().Equals("id")) return;
            _isOK = DialogResult.OK;
            try
            {
                int id = Convert.ToInt32(lbl_ID.Text);
                dbEdit(id);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            finally { ShowdbCalss(); }
        }
        private void ShowdbCalss()
        {
            gymEntities db = new gymEntities();
            var classsort = from r in db.tclasses
                            join c in db.tclass_status_detail on r.class_status equals c.class_status_id
                            select new
                            {
                                編號 = r.class_id,
                                課程名稱 = r.class_name,
                                類型 = r.class_sort1_id,
                                分類 = r.class_sort2_id,
                                照片 = r.class_photo,
                                介紹 = r.class_introduction,
                                開放 = c.class_status_discribe,
                                限制 = r.limited_gender
                            };
            this.dataGridView_ClassSortList.DataSource = classsort.ToList();
            this.dataGridView_ClassSortList.Columns[6].Width = 200;
            this.dataGridView_ClassSortList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
        }


        private void FrmClassUpdate_Load(object sender, EventArgs e)
        {
            gymEntities gymEntities = new gymEntities();
            var sort1s = from sort1 in gymEntities.tclass_sort_有氧 select sort1;
            foreach (var sort1 in sort1s)
                this.cb_Sort1.Items.Add(sort1.class_sort1_id);

            var sort2s = from sort2 in gymEntities.tclass_sort_訓練 select sort2;
            foreach (var sort2 in sort2s)
                this.cb_Sort2.Items.Add(sort2.class_sort2_id);
            var limits = from limit in gymEntities.tclass_limit_details select limit;
            foreach (var limit in limits)
                this.cb_Limit.Items.Add(limit.class_limited_id);
            var statuses = from status in gymEntities.tmember_status_details select status;
            foreach (var status in statuses)
                this.cb_Status.Items.Add(status.status_id);
            ShowdbCalss();
            dbSelect(1);
        }
        private void dbSelect(int id)
        {
            gymEntities db = new gymEntities();
            pb_ClassPhoto.Image = null;
            tclasses classsort = db.tclasses.FirstOrDefault(x => x.class_id == id);
            if (classsort == null) return;
            this.course = classsort;
            lbl_ID.Text = classsort.class_id.ToString();
            cb_Sort1.Text = classsort.class_sort1_id.ToString();
            cb_Sort2.Text = classsort.class_sort2_id.ToString();
            cb_Limit.Text = classsort.limited_gender.ToString();
            cb_Status.Text = classsort.class_status.ToString();
            txt_ClassName.Text = classsort.class_name;
            txt_Introduction.Text = classsort.class_introduction;
            _imagepath = classsort.class_photo;
            if (!string.IsNullOrEmpty(_imagepath))
            {
                string path = Application.StartupPath + "\\ClassPic";
                pb_ClassPhoto.Image = new Bitmap(path + "\\" + _imagepath);
            }
            Change_lbl_CurrentIndex();
        }
        private void dbEdit(int id)
        {
            /*
            using (gymEntities db = new gymEntities())
            {
                var classsort = db.tclasses.FirstOrDefault(x => x.class_id == id);
                if (classsort == null) return;
                classsort.class_sort1_id = Convert.ToInt32(cb_Sort1.Text);
                classsort.class_sort2_id = Convert.ToInt32(cb_Sort2.Text);
                classsort.limited_gender = Convert.ToInt32(cb_Limit.Text);
                classsort.class_name = txt_ClassName.Text;
                classsort.class_introduction = txt_Introduction.Text;

                if (!string.IsNullOrEmpty(_imagepath))
                {
                    string path = Application.StartupPath + "\\ClassPic";
                    pb_ClassPhoto.Image = new Bitmap(path + "\\" + _imagepath);
                }
                classsort.class_photo = _imagepath;
                MessageBox.Show("資料儲存成功");
                db.SaveChanges();
            }
            */
            gymEntities db = new gymEntities();
            var classsort = db.tclasses.FirstOrDefault(x => x.class_id == id);
            if (classsort == null) return;
            classsort.class_sort1_id = Convert.ToInt32(cb_Sort1.Text);
            classsort.class_sort2_id = Convert.ToInt32(cb_Sort2.Text);
            classsort.limited_gender = Convert.ToInt32(cb_Limit.Text);
            classsort.class_name = txt_ClassName.Text;
            classsort.class_introduction = txt_Introduction.Text;
            classsort.class_status = Convert.ToInt32(cb_Status.Text);
            if (!string.IsNullOrEmpty(_imagepath))
            {
                string path = Application.StartupPath + "\\ClassPic";
                pb_ClassPhoto.Image = new Bitmap(path + "\\" + _imagepath);
            }
            classsort.class_photo = _imagepath;
            MessageBox.Show("資料儲存成功");
            db.SaveChanges();
        }
        private void DataGridView_ClassSortList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _index = (int)dataGridView_ClassSortList.Rows[e.RowIndex].Cells[0].Value;
            dbSelect(_index);
        }
        private void Btn_Begin_Click(object sender, EventArgs e)
        {
            _index = 1;
            dbSelect(_index);
            this.dataGridView_ClassSortList.CurrentCell = dataGridView_ClassSortList.Rows[_index - 1].Cells[0];
            Change_lbl_CurrentIndex();
        }


        private void Btn_Previous_Click(object sender, EventArgs e)
        {
            _index--;
            if (_index < 1) _index = 1;

            dbSelect(_index);
            this.dataGridView_ClassSortList.CurrentCell = dataGridView_ClassSortList.Rows[_index - 1].Cells[0];
            Change_lbl_CurrentIndex();
        }

        private void Btn_Next_Click(object sender, EventArgs e)
        {
            _index++;
            int total = TotalAmount();
            if (_index > total)
                _index = total;

            dbSelect(_index);
            this.dataGridView_ClassSortList.CurrentCell = dataGridView_ClassSortList.Rows[_index - 1].Cells[0];
            Change_lbl_CurrentIndex();
        }

        private void Btn_End_Click(object sender, EventArgs e)
        {
            int total = TotalAmount();
            _index = total;

            dbSelect(_index);
            this.dataGridView_ClassSortList.CurrentCell = dataGridView_ClassSortList.Rows[_index - 1].Cells[0];
            Change_lbl_CurrentIndex();
        }

        private static int TotalAmount()
        {
            gymEntities gymEntities = new gymEntities();
            var courses = from course in gymEntities.tclasses select course;
            int total = courses.Count();
            return total;
        }

        private void Change_lbl_CurrentIndex()
        {
            int total = TotalAmount();
            this.lbl_CurrentIndex.Text = $"{_index}/{total}";
        }

    }
}
