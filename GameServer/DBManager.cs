using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer
{
    class DBManager
    {
        private DBManager() { }

        private static readonly Lazy<DBManager> _instance = new Lazy<DBManager>(()=> new DBManager());

        public static DBManager Instance
        {
            get { return _instance.Value; }
        }


        private string m_tempDB;

        public void DbUpdate(ListBox dbListBox)
        {
            dbListBox.Items.Clear();

            int i = 0;
            string myConn = "Server=localhost;Integrated security=SSPI;database=master";

            string str;

            using (SqlConnection conn = new SqlConnection(myConn))
            {
                conn.Open();

                str = "SELECT * FROM sys.sysdatabases";

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = conn;
                myCommand.CommandText = str;
                SqlDataReader myReader = myCommand.ExecuteReader();

                string[] dbName = new string[10];

                while (myReader.Read())
                {
                    dbName[i] = myReader.GetString(0);
                    dbListBox.Items.Add(dbName[i]);
                    i++;
                }
            }
        }

        public void SearchDB(ListBox dbListBox, ListBox tableListBox)
        {
            tableListBox.Items.Clear();
            int j = 0;
            String str;
            //string myConn = "Server=localhost;Integrated security=SSPI;database=master";
            string myConn = "Server=localhost;Integrated security=SSPI;database=" + dbListBox.SelectedItem;

            if (dbListBox.SelectedIndex == -1)
                return;

            using (SqlConnection conn = new SqlConnection(myConn))
            {
                conn.Open();

                str = "SELECT * FROM sys.tables";

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = conn;
                myCommand.CommandText = str;
                SqlDataReader myReader = myCommand.ExecuteReader();

                string[] db_name = new string[10];

                while (myReader.Read())
                {
                    db_name[j] = myReader.GetString(0);
                    tableListBox.Items.Add(db_name[j]);
                    j++;
                }
            }

            m_tempDB = dbListBox.SelectedItem.ToString();
        }

        public void SearchTable(ListBox tableListBox, DataGridView tableData)
        {
            tableData.Refresh();

            SqlConnection conn;

            string myConn = "Server=localhost;Integrated security=SSPI;database=" + m_tempDB;
            conn = new SqlConnection(myConn);

            conn.Open();

            if (tableListBox.SelectedIndex == -1)
                return;

            string str = "USE " + m_tempDB + ";" +
                         "SELECT * FROM " + tableListBox.SelectedItem + ";";

            DataSet ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter(str, conn);

            adapter.Fill(ds);
            conn.Close();

            tableData.DataSource = ds.Tables[0];
        }

        public bool Login(string userId, string userPw)
        {
            string myConn = "Server=localhost;Integrated security=SSPI;database=MyDatabase";

            using (SqlConnection conn = new SqlConnection(myConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Login", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameter ID
                SqlParameter input1 = new SqlParameter("@id", SqlDbType.NVarChar);
                input1.Direction = ParameterDirection.Input;
                input1.Value = userId;
                cmd.Parameters.Add(input1);

                // Input Parameter PW
                SqlParameter input2 = new SqlParameter("@pw", SqlDbType.NVarChar);
                input2.Direction = ParameterDirection.Input;
                input2.Value = userPw;
                cmd.Parameters.Add(input2);

                // Output Parameter
                SqlParameter output = new SqlParameter("@result", SqlDbType.Bit);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                // Return Value
                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                cmd.ExecuteNonQuery();

                if ((int)returnValue.Value != 0)
                {
                    return false;
                }

                return (bool)output.Value;
            }
        }

        public bool IdDuplicateCheck(string userId)
        {
            string myConn = "Server=localhost;Integrated security=SSPI;database=MyDatabase";

            using (SqlConnection conn = new SqlConnection(myConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("IdDuplicateCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameter ID
                SqlParameter input1 = new SqlParameter("@in", SqlDbType.NVarChar);
                input1.Direction = ParameterDirection.Input;
                input1.Value = userId;
                cmd.Parameters.Add(input1);

                // Output Parameter
                SqlParameter output = new SqlParameter("@out", SqlDbType.Bit);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                // Return Value
                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                cmd.ExecuteNonQuery();

                if ((int)returnValue.Value != 0)
                {
                    return false;
                }

                return (bool)output.Value;
            }
        }

        public bool SignUp(string userId, string userPw)
        {
            string myConn = "Server=localhost;Integrated security=SSPI;database=MyDatabase";

            using (SqlConnection conn = new SqlConnection(myConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SignUp", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameter ID
                SqlParameter input1 = new SqlParameter("@id", SqlDbType.NVarChar);
                input1.Direction = ParameterDirection.Input;
                input1.Value = userId;
                cmd.Parameters.Add(input1);

                // Input Parameter PW
                SqlParameter input2 = new SqlParameter("@pw", SqlDbType.NVarChar);
                input2.Direction = ParameterDirection.Input;
                input2.Value = userPw;
                cmd.Parameters.Add(input2);

                // Return Value
                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                cmd.ExecuteNonQuery();

                if ((int)returnValue.Value != 0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
