using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace RankingProjectPostgres
{
    public partial class Form1 : Form
    {

        // PostgeSQL-style connection string to database
        string connstring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};",
           "localhost", 5222, "postgres",
            "data2019", "RankingStudents");

        private NpgsqlConnection conn;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private int rowIndex = -1;
        private int selectedId;
        private int beforeId;
        private int afterId;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Making connection with Npgsql provider
            conn = new NpgsqlConnection(connstring);

            Select();

        }
        private void Select()
        {
            try
            {
                conn.Open();
                sql = @"select * from categorymember_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                dgvData.DataSource = null; // reset datagridview
                dgvData.DataSource = dt;

                dgvData.ClearSelection();

            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error:" + ex.Message);
            }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Select();

        }

        private void txtFirstname_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblMidname_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose student to delete");
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_delete(:_id)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Delete student successfull");
                    rowIndex = -1;
                    Select();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Deleted fail. Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose student to update");
                return;
            }
            // txtFirstname.Enabled = txtMidname.Enabled = txtLastname.Enabled = true;
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rowIndex = e.RowIndex;

                selectedId = int.Parse(dgvData.Rows[e.RowIndex].Cells["thingid"].Value.ToString());
                if (e.RowIndex == 0)
                {
                    beforeId = 0000;
                    lblbeforethingid.Text = "no element";

                }
                else

                {
                    beforeId = int.Parse(dgvData.Rows[e.RowIndex - 1].Cells["thingid"].Value.ToString());
                    lblbeforethingid.Text = beforeId.ToString();

                }
                if (e.RowIndex == dgvData.Rows.Count - 1)

                {
                    afterId = 0000;
                    lblafterthingid.Text = "no element";


                }
                else
                {
                    afterId = int.Parse(dgvData.Rows[e.RowIndex + 1].Cells["thingid"].Value.ToString());
                    lblafterthingid.Text = afterId.ToString();


                }

                lblselectedthingid.Text = selectedId.ToString();

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int result = 0;
            if (rowIndex < 0) //insert
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_insert(:_firstname, :_midname, :_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    //cmd.Parameters.AddWithValue("_firstname",txtFirstname.Text);
                    //cmd.Parameters.AddWithValue("_midname", txtMidname.Text);
                    //cmd.Parameters.AddWithValue("_lastname", txtLastname.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Inserted  new student successfully");
                        Select();

                    }
                    else
                    {
                        MessageBox.Show("Inserted fail");
                        Select();

                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Inserted fail. Error: " + ex.Message);
                }
            }
            else //update
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_update(:_id,  :_firstname, :_midname, :_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                    //cmd.Parameters.AddWithValue("_firstname", txtFirstname.Text);
                    //cmd.Parameters.AddWithValue("_midname", txtMidname.Text);
                    //cmd.Parameters.AddWithValue("_lastname", txtLastname.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Update successfully");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("Update failed");
                        Select();

                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Update fail. Error: " + ex.Message);
                }

            }
            result = 0;
            //txtFirstname.Text = txtMidname.Text = txtLastname.Text = null;
            //txtFirstname.Enabled = txtMidname.Enabled = txtLastname.Enabled = false;
        }

        private void dgvData_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
           
            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose Element first");
                return;
            }
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT cat_place_item(1,@selected,@before,TRUE)", conn))
                {

                    cmd.Parameters.AddWithValue("selected", selectedId);
                    cmd.Parameters.AddWithValue("before", beforeId);
                    cmd.ExecuteNonQuery();

                }


                if ((int)cmd.ExecuteScalar() == 1)
                {
                    // MessageBox.Show("up successfull");
                    //rowindex = -1;
                    conn.Close();
              
                    Select();

                    dgvData.Rows[rowIndex - 1].Selected = true;
                }
                conn.Close();


            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("up fail. error: " + ex.Message);
            }




            //if (rowIndex < 0)
            //{
            //    MessageBox.Show("Please choose Element first");
            //    return;
            //}
            //try
            //{
            //    conn.Open();
            //    sql = @"select cat_place_item(:_id,:_selectedid,:_beforeid,:_true)";
            //    cmd = new NpgsqlCommand(sql, conn);
            //    cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex-1].Cells["thingid"].Value.ToString()));
            //    cmd.Parameters.AddWithValue("_selectedid",selectedId);
            //    cmd.Parameters.AddWithValue("_beforeid",beforeId);
            //    cmd.Parameters.AddWithValue("_true", true);

            //  if ((int)cmd.ExecuteScalar() == 1)
            //    {
            //        MessageBox.Show("UP successfull");
            //     //rowIndex = -1;
            //    // Select();
            //    }

            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    conn.Close();
            //    MessageBox.Show("UP fail. Error: " + ex.Message);
            //}

        }

        private void btnDown_Click(object sender, EventArgs e)
        {

            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose Element first");
                return;
            }
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT cat_place_item(1,@selected,@after,FALSE)", conn))
                {

                    cmd.Parameters.AddWithValue("selected", selectedId);
                    cmd.Parameters.AddWithValue("after", afterId);
                    cmd.ExecuteNonQuery();

                }


                if ((int)cmd.ExecuteScalar() == 1)
                {
                    //MessageBox.Show("up successfull");
                    //rowindex = -1;

                    conn.Close();
                    Select();
                    dgvData.Rows[rowIndex + 1].Selected = true;
                }



            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("up fail. error: " + ex.Message);
            }

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblafterthingid_Click(object sender, EventArgs e)
        {

        }

        private void refreshSelection(int RowIndex)
        {


            if (RowIndex >= 0)
            {
                rowIndex = RowIndex;

                selectedId = int.Parse(dgvData.Rows[RowIndex].Cells["thingid"].Value.ToString());
                if (RowIndex == 0)
                {
                    beforeId = 0000;
                    lblbeforethingid.Text = "no element";

                }
                else

                {
                    beforeId = int.Parse(dgvData.Rows[RowIndex - 1].Cells["thingid"].Value.ToString());
                    lblbeforethingid.Text = beforeId.ToString();

                }
                if (RowIndex == dgvData.Rows.Count - 1)

                {
                    afterId = 0000;
                    lblafterthingid.Text = "no element";


                }
                else
                {
                    afterId = int.Parse(dgvData.Rows[RowIndex + 1].Cells["thingid"].Value.ToString());
                    lblafterthingid.Text = afterId.ToString();


                }

                lblselectedthingid.Text = selectedId.ToString();

            }
        }

        private void dgvData_CurrentCellChanged(object sender, EventArgs e)
        {

        }
    }
}
