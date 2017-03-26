using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimeManager
{
    class databaseFunctions
    {

        SqlConnection con = new SqlConnection();
        string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Projekti\\_Razno\\TimeManager\\TimeManager\\Database1.mdf;Integrated Security=True";

        public void loginTime(double minutesDelay)
        {
            con.ConnectionString = connection;

            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "INSERT INTO [Table] (Prijava) VALUES (@Prijava)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Prijava", DateTime.Now.AddMinutes(minutesDelay));

                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string greska = ex.Message.ToString();
                con.Close();
                //MessageBox.Show("Dogodila se pogreška!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void logoutTime(double minutesDelay)
        {
            con.ConnectionString = connection;

            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlCommand cmd2 = new SqlCommand();

                cmd.CommandText = "UPDATE [Table] SET Odjava = (@Odjava) WHERE Id = (SELECT TOP 1 Id FROM [Table] ORDER BY Id DESC)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Odjava", DateTime.Now.AddMinutes(minutesDelay));

                //cmd2.CommandText = "UPDATE [Table] SET Razlika = (@Odjava) WHERE Id = (SELECT TOP 1 Id FROM [Table] ORDER BY Id DESC)";
                cmd2.CommandText = "UPDATE [Table] SET Razlika = DATEDIFF (minute, (SELECT TOP 1 (Prijava) FROM [Table] ORDER BY Id DESC),(SELECT TOP 1 (Odjava) FROM [Table] ORDER BY Id DESC)) WHERE Id = (SELECT TOP 1 Id FROM [Table] ORDER BY Id DESC)";
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = con;

                con.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string greska = ex.Message.ToString();
                con.Close();
                //MessageBox.Show("Dogodila se pogreška!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int total()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string month = DateTime.Now.Month.ToString();
            string total = "";
            con.ConnectionString = connection;
            try
            {
                con.Open();
                //radi
                using (var cmd = new SqlCommand("SELECT SUM(Razlika) AS SumRazlika FROM [Table] WHERE MONTH(Prijava) = " + month, con))
                //using (var cmd = new SqlCommand("SELECT SUM(Razlika) AS SumRazlika FROM [Table]", con))
                //using (var cmd = new SqlCommand("SELECT Prijava SUM(Razlika) FROM (SELECT DATEDIFF(week, '"+today+"', Prijava) AS SumRazlika  FROM table)", con))
                //using (var cmd = new SqlCommand("SET DATEFIRST 1 SELECT SUM(Razlika) AS SumRazlika FROM [Table] WHERE(WorkDate >= dateadd(day, 1-datepart(dw, getdate()), CONVERT(date,getdate())) AND WorkDate < dateadd(day, 8 - datepart(dw, getdate()), CONVERT(date, getdate())))", con))

                {
                    SqlDataReader re = cmd.ExecuteReader();

                    if (re.Read())
                    {
                        total = re["SumRazlika"].ToString();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
            }

            //int sumTotal = Int32.Parse(total);
            int sumTotal = 0;
            Int32.TryParse(total, out sumTotal);
            return sumTotal;
        }
    }
}

