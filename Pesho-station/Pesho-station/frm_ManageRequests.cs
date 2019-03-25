﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pesho_station
{
    public partial class frm_ManageRequests : Form
    {
        DataTable dTable = new DataTable();

        public frm_ManageRequests()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.pnl_main.AutoScroll = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }
        public string CarType { get; set; }
        public int PickupTime { get; set; }
        public int NumberPassengers { get; set; }
        public string Note { get; set; }

        private void frm_ManageRequests_Load(object sender, EventArgs e)
        {
            LoadDataSet();
            HideDataGridView();
        }

        private void HideDataGridView()
        {
            if (!HasRows())
            {
                dataGridView1.Visible = false;
            }
            else
            {
                dataGridView1.Visible = true;
            }
        }

        private bool HasRows()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeleteSelectedRow()
        {
            if (HasRows())
            {
                //deleting a row
                MySqlConnection con = new MySqlConnection("user id=peshoStation;server=212.233.159.21;database=test;password=123123;persistsecurityinfo=True");
                con.Open();
                string cmdString = "delete from taxi WHERE id=@Id";
                MySqlCommand cmd = new MySqlCommand(cmdString, con);
                cmd.Parameters.AddWithValue("@Id", Id);
                var deleter = cmd.ExecuteReader();
                deleter.Read();
                deleter.Close();
                int maxid = FindMaxId(con);
                AlterAutoIncrement(con, maxid);
                LoadDataSet();

            }
        }
        private int FindMaxId(MySqlConnection con)  //selecting max id in the DB
        {
            string cmdString2 = "select max(id) from taxi";
            MySqlCommand cmd2 = new MySqlCommand(cmdString2, con);
            var maxnumber = cmd2.ExecuteReader();
            maxnumber.Read();
            int maxId;
            if (dataGridView1.Rows.Count > 1)
            {
                maxId = maxnumber.GetInt32(0);
                maxId += 1;
            }
            maxId = 1;
            maxnumber.Close();

            return maxId;
        }

        private void AlterAutoIncrement(MySqlConnection con,int maxid) //altering auto increment value with the maxId found
        {
            string cmdString3 = "alter table taxi auto_increment=@maxId";
            MySqlCommand cmd3 = new MySqlCommand(cmdString3, con);
            cmd3.Parameters.AddWithValue("@maxid", maxid);
            var refreshAutoIncrement = cmd3.ExecuteReader();
            refreshAutoIncrement.Read();
            refreshAutoIncrement.Close();
        }

        private void LoadDataSet()
        {
            dTable.Rows.Clear();
            MySqlConnection con = new MySqlConnection("user id=peshoStation;server=212.233.159.21;database=test;password=123123;persistsecurityinfo=True");
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from taxi", con);
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            MyAdapter.Fill(dTable);
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.DataSource = dTable;
            dataGridView1.AreAllCellsSelected(true);
            con.Close();
            HideDataGridView();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadDataSet();

        }

        private void btn_accept_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                Id = int.Parse(row.Cells["id"].Value.ToString());
                FullName = row.Cells["callerFullName"].Value.ToString();
                PhoneNumber = row.Cells["callerPhone"].Value.ToString();
                CarType = row.Cells["type"].Value.ToString();
                NumberPassengers = int.Parse(row.Cells["passengers"].Value.ToString());
                PickupTime = int.Parse(row.Cells["pickupTime"].Value.ToString());
                PickupAddress = row.Cells["pickUpAdress"].Value.ToString();
                DropoffAddress = row.Cells["DropUpAdress"].Value.ToString();
                Note = row.Cells["driverNote"].Value.ToString();
            }
        }
           
    }
}
