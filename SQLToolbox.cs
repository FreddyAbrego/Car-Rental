using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CarRental
{
    class SQLToolBox
    {
        /// <summary>
        /// myLocation string used to retrieve the connectionString in App.Config
        /// </summary>
        public string strMyLocation { get; private set; }
        /// <summary>
        /// Connection String used to connect to DB
        /// </summary>
        public string connectionString { get; private set; }
        /// <summary>
        ///  Creates an SQL connection with the connection string
        /// </summary>
        /// <param name="MyLocation"> The MyLocation string to use in AppConfig for the connection string</param>
        public SQLToolBox(string user)
        {
            switch (user)
            {
                case "test":
                    connectionString = "Data Source=73.1.128.112,1433;Initial Catalog=CarRental;User ID=test;Password=testuser";
                    break;
                case "sa":
                    connectionString = "Data Source=73.1.128.112,1433;Initial Catalog=CarRental;User ID=sa;Password=ModerNexus15!";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Executes a SQL Insert through a Non-Query Command.
        /// </summary>
        /// <param name="Query">SQL Insert Query</param>
        /// <returns>Returns true if there is an error.</returns>
        public bool InsertInto(string Query)
        {
            bool error = false;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCmd = new SqlCommand(Query, sqlCon))
            {
                //Setup Command properties
                sqlCmd.CommandTimeout = 0;

                try
                {
                    sqlCon.Open();
                    sqlCmd.ExecuteNonQuery();
                }
                catch
                { error = true; }
                finally
                { sqlCon.Close(); }
            }
            return error;
        }

        /// <summary>
        /// Fills a DataTable.
        /// SQL Query to Fill DataTable, but hold onto the forma
        /// </summary>
        /// <param name="Query">SQL Query to Fill DataTable.</param>
        /// <param name="Table">An initialized DataTable to send by reference.</param>
        /// <returns>Returns true if there is an error</returns>
        public bool FillDataTable(string Query, ref DataTable Table)
        {
            bool error = false;
            //clear Datatable or new data won't fill
            Table.Clear();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlDataAdapter sqlAdtpr = new SqlDataAdapter(Query, sqlCon))
            {
                sqlAdtpr.SelectCommand.CommandTimeout = 0;
                try
                {
                    sqlCon.Open();
                    sqlAdtpr.Fill(Table);
                }
                catch (Exception e)
                { error = true; }
                finally
                { sqlCon.Close(); }
            }
            return error;
        }

        /// <summary>
        /// Pulls the first answer from the SQL Database
        /// if an error occurs the answer will be null
        /// </summary>
        /// <param name="Query">SQL Query</param>
        /// <param name="Answer">An uninitialized string to place the answer into.</param>
        /// <returns>Returns true if there is an error</returns>
        public bool SingleAnswer(string Query, out string Answer)
        {
            bool error = false;

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCmd = new SqlCommand(Query, sqlCon))
            {
                sqlCmd.CommandTimeout = 0;
                try
                {
                    sqlCon.Open();
                    SqlDataReader sqlRdr = sqlCmd.ExecuteReader();
                    sqlRdr.Read();
                    Answer = sqlRdr[0].ToString();

                    //Dispose & Close
                    sqlRdr.Close();
                    sqlRdr.Dispose();
                }
                catch (Exception e)
                {
                    error = true;
                    Answer = null;
                }
                finally
                { sqlCon.Close(); }
            }
            return error;
        }
        /// <summary>
        /// Pulls the first answer from the SQL Database
        /// if an error occurs the answel will be -999
        /// </summary>
        /// <param name="Query">SQL Query</param>
        /// <param name="Answer">An uninitialized integer to place the answer into.</param>
        /// <returns> Returns true if there is an error.</returns>
        public bool SingleAnswer(string Query, out int Answer)
        {
            bool error = false;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCmd = new SqlCommand(Query, sqlCon))
            {
                sqlCmd.CommandTimeout = 0;
                try
                {
                    object _rawVal;

                    //-- Pull Value --
                    sqlCon.Open();
                    SqlDataReader sqlRdr = sqlCmd.ExecuteReader();
                    sqlRdr.Read();

                    //-- Check datatype --
                    //if no correct Convert
                    _rawVal = sqlRdr[0];
                    if (_rawVal.GetType() != typeof(int))
                    { Answer = Convert.ToInt32(_rawVal); }
                    else
                    { Answer = (int)_rawVal; }

                    //-- Dispose & Close --
                    sqlRdr.Close();
                    sqlRdr.Dispose();
                }
                catch (Exception e)
                {
                    error = true;
                    Answer = -999;
                }
                finally
                { sqlCon.Close(); }
            }
            return error;
        }
        /// <summary>
        /// Pulls the first answer from the SQL Database
        /// if an error occurs the answel will be -999
        /// </summary>
        /// <param name="Query">SQL Query</param>
        /// <param name="Answer">An uninitialized integer to place the answer into.</param>
        /// <returns>Returns true if there is an error.</returns>
        public bool SingleAnswer(string Query, out double Answer)
        {
            bool error = false;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            using (SqlCommand sqlCmd = new SqlCommand(Query, sqlCon))
            {
                sqlCmd.CommandTimeout = 0;
                try
                {
                    object _rawVal;

                    //-- Pull Value --
                    sqlCon.Open();
                    SqlDataReader sqlRdr = sqlCmd.ExecuteReader();
                    sqlRdr.Read();

                    //-- Check datatype --
                    //if no correct Convert
                    _rawVal = sqlRdr[0];
                    if (_rawVal.GetType() != typeof(double))
                    { Answer = Convert.ToInt32(_rawVal); }
                    else
                    { Answer = (double)_rawVal; }

                    //-- Dispose & Close --
                    sqlRdr.Close();
                    sqlRdr.Dispose();
                }
                catch (Exception e)
                {
                    error = true;
                    Answer = -999;
                }
                finally
                { sqlCon.Close(); }
            }
            return error;
        }
    }
}


