using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DoANCNPM
{
    public partial class TrangChu : System.Web.UI.Page
    {
       
        public static List<String> listTable = new List<string>();
        public static List<String> listColumnNameChoosed = new List<string>();
        public static List<String> listTableNameChoosed= new List<string>();


        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {

                labelLoi.Text = "";
            }

        }



        protected void CheckBoxListColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxListColumn.Items)
            {

                if (item.Selected)
                {
                    listColumnNameChoosed.Add(item.Text.ToString());
                    listTableNameChoosed.Add(item.Value.ToString());
                }
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("Tên Cột", Type.GetType("System.String"));
            dt.Columns.Add("Tên Bảng", Type.GetType("System.String"));

            string[] listCot = listColumnNameChoosed.ToArray();
            string[] listBang = listTableNameChoosed.ToArray();
         

            for (int i = 0; i < listCot.GetLength(0); i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["Tên Cột"] = listCot[i];
                dt.Rows[i]["Tên Bảng"] = listBang[i];
            }

            //Clear cột nếu k tích nữa
            listTableNameChoosed.Clear();
            listColumnNameChoosed.Clear();

            TBColumnChoosed.DataSource = dt;
            TBColumnChoosed.DataBind();
        }

        protected void CheckBoxListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxListColumn.Items.Clear();
            listTable.Clear();

            foreach (ListItem item in CheckBoxListTable.Items)
            {
                if (item.Selected)
                {
                    listTable.Add(item.Text);
                }
            }
            for (int i = 0; i < listTable.Count; ++i)
            {
                getColumn(listTable[i].ToString()); // lấy ra các column trong ds table đã tích
            }
           
        }

        private void getColumn(String tableName) // để lấy các checkbox column của 1 table 
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager
                        .ConnectionStrings["QLVT_DHConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' AND COLUMN_NAME NOT LIKE 'rowguid%'";

                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    conn.Open();
                    int i = 0;
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            ListItem item = new ListItem();

                            item.Text = data["COLUMN_NAME"].ToString();
                            item.Value = tableName.ToString();
                            CheckBoxListColumn.Items.Add(item);
                            CheckBoxListColumn.RepeatColumns = 5;
                            CheckBoxListColumn.AutoPostBack = true;
                            i++;
                        }
                    }
                    conn.Close();
                }
            }
        }
        private void removeColumn()
        {

        }

        protected void TBColumnChoosed_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected Boolean checkData(String tenTable,String tenCot)
        {
            String data_type = "";
            String cmd ="SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='"+tenTable+ "' AND "+ "COLUMN_NAME ='"+tenCot+"'";
            SqlConnection conn;
            String connectionString = "Data Source=DESKTOP-RAH6IHC\\NHATNGUYEN;Initial Catalog=QLVT_DH;Integrated Security=True";
            conn = new SqlConnection(connectionString);
            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlDataReader dr = sqlcmd.ExecuteReader();
            while (dr.Read())
            {
                 data_type= dr["DATA_TYPE"].ToString();
                System.Diagnostics.Debug.WriteLine("datatype: " + data_type);

            }
            conn.Close();
            if(data_type.Equals("int"))
            {
                return true;
            }
            return false;
        }
        protected DataTable layData(String tenTable)
        {
            
            String cmd = "exec [dbo].[TimTTBangChuaKhoaNgoai] " + "'" + tenTable + "'";
            SqlConnection conn;
            String connectionString = "Data Source=DESKTOP-RAH6IHC\\NHATNGUYEN;Initial Catalog=QLVT_DH;Integrated Security=True";
            conn = new SqlConnection(connectionString);
            SqlCommand sqlcmd = new SqlCommand(cmd, conn);
            DataTable dt = new DataTable();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            
            List<String> tabletoQuery = new List<string>();
            
            for (int i = 0; i < TBColumnChoosed.Rows.Count; i++)
            {
                tabletoQuery.Add(TBColumnChoosed.Rows[i].Cells[4].Text); // lấy list table có trong cau query
            }

            IEnumerable<string> uniqueTablechoosed = tabletoQuery.Distinct<String>();

            string txtloi = "";
            string QueryStr = "";
            DataTable data = new DataTable();
            string tableName = string.Join(",", uniqueTablechoosed);
            string[] tableNameList = tableName.Split(',');
            String columnName = "";
            QueryStr = "SELECT ";
            String dk = "";
            String where = "WHERE ";
            String sapxep = " ORDER BY ";
            String nhom = " GROUP BY ";
            String tenDaiDien="";
            for (int i = 0; i < tableNameList.Length; i++)
            {
                data.Clear();
                System.Diagnostics.Debug.WriteLine("table: "+tableNameList[i]);
                
                data = layData(tableNameList[i]);
                for (int j = 0; j < data.Rows.Count; j++)
                {
                    for(int t=i+1;t<tableNameList.Length;t++)
                    {
                        if(tableNameList[t].Equals(data.Rows[j]["tableFK"]))
                        {
                            where = where + tableNameList[i] + "." + data.Rows[j]["columnPK"] + "=" + data.Rows[j]["tableFK"] + "." + data.Rows[j]["columnFK"] + " AND ";
                            System.Diagnostics.Debug.WriteLine("where update: " + where);
                        }
                        
                    }    
                 
                }

            }
            Boolean check = false;
            for (int i = 0; i < TBColumnChoosed.Rows.Count; i++)
            {
                String strBang, strCot;

                TextBox dieuKien = (TextBox)TBColumnChoosed.Rows[i].Cells[1].FindControl("TextBoxDieuKien");
                DropDownList listStatus = (DropDownList)TBColumnChoosed.Rows[i].Cells[0].FindControl("DropDownList1");
                DropDownList sapxepList = (DropDownList)TBColumnChoosed.Rows[i].Cells[2].FindControl("DropDownList2");


                if (dieuKien.Text.ToString() != "")
                {
                    strBang = TBColumnChoosed.Rows[i].Cells[4].Text;
                    strCot = TBColumnChoosed.Rows[i].Cells[3].Text;
                    dk += strBang + "." + strCot + dieuKien.Text.ToString() + " AND ";
                }

                strBang = TBColumnChoosed.Rows[i].Cells[4].Text;
                strCot = TBColumnChoosed.Rows[i].Cells[3].Text;

                if(listStatus.SelectedValue.ToString().Equals("AVG"))
                {
                    if(checkData(strBang,strCot)==true)
                    {
                        check = true;
                        columnName = listStatus.SelectedValue.ToString() + "(" + strBang + "." + strCot + ")" + " AS " + listStatus.SelectedValue.ToString() + "_" + strCot;
                        tenDaiDien = listStatus.SelectedValue.ToString() + "_" + strCot;
                    }
                    else
                    {
                        txtloi = "Không thể sử dụng hàm AVG cho cột " + strCot;
                        labelLoi.Visible = true;
                        labelLoi.Text = txtloi;
                        return;
                    }
                }    

                else if (!listStatus.SelectedValue.ToString().Equals("None"))
                {
                    check = true;
                    columnName = listStatus.SelectedValue.ToString() + "(" + strBang + "." + strCot + ")" + " AS " + listStatus.SelectedValue.ToString() + "_" + strCot;
                    tenDaiDien = listStatus.SelectedValue.ToString() + "_" + strCot;
                }
                else if (check == true)
                {
                    if (nhom.Equals(" GROUP BY "))
                    {
                        nhom += "(" + strBang + "." + strCot + ")";
                        columnName = strBang + "." + strCot;
                    }
                    else
                    {
                        nhom += ",(" + strBang + "." + strCot + ")";
                        columnName = strBang + "." + strCot;
                    }    


                }
                else
                {
                    columnName = strBang + "." + strCot;
                }

                if (!sapxepList.SelectedValue.ToString().Equals("NONE"))
                {
                    if(sapxepList.SelectedValue.ToString().Equals("ASC"))
                        {
                            
                        sapxep +=  tenDaiDien + " ASC "+",";
                        }
                    else 
                    {
                        sapxep += tenDaiDien + " DESC "+",";
                    }    

                }
                

                


                if (i < TBColumnChoosed.Rows.Count - 1)
                {
                    columnName += ", ";
                }
                QueryStr += columnName;

            }

            
            System.Diagnostics.Debug.WriteLine("DK: " + dk);
             

            if (sapxep.Equals(" ORDER BY "))
            {
                sapxep = "";
            }
            else
            {
                sapxep = sapxep.Remove(sapxep.Length - 1);

            }

         
            if (!dk.Equals(""))
            {
                dk = dk.Trim();
                dk = dk.TrimEnd(' ').Remove(dk.LastIndexOf(" ") + 1);
                dk.Trim();
            
                
            }
            if(nhom.Equals(" GROUP BY "))
            {
                nhom = "";
            }    
            if (!where.Equals("WHERE "))
            {
                
                if(dk.Equals(""))
                {
                    where = where.Trim();
                    where = where.TrimEnd(' ').Remove(where.LastIndexOf(" ") + 1); 
                }    
              
            }
            else if(dk.Equals("") && where.Equals("WHERE "))
            {
                where = "";
            }

            



            QueryStr += " FROM " + tableName +" "+ where + dk+nhom +sapxep;
          
            txtNDQuery.Text = QueryStr;



        }
   
        protected void btnRP_Click(object sender, EventArgs e)
        {

            try
            {
                String query = txtNDQuery.Text.ToString();
                Session["query"] = query;
                Session["title"] = txtTieuDeBaoCao.Text.ToString();

                Response.Redirect("WebForm1.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch(Exception ex)
            {
               
            }



        }

        protected void txtNDQuery_TextChanged(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }
    }
}