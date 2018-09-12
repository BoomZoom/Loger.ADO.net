using System;
using ConsoleApp3.Interface;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp3.Class.AbstractClass
{
    abstract class EventData : IEventData
    {
        static string connectionString = @"server=DESKTOP-PC73D7E\SQLEXPRESS;database=test;integrated Security=SSPI;";
        protected SqlConnection connectionDB;
        protected SqlCommand comand;
        public EventData()
        {
            Conect();
        }
        ~EventData()
        {

        }
        protected void Conect()
        {
            connectionDB = new SqlConnection(connectionString);
            Console.WriteLine("Conection");


            string queryStatement = "SELECT TOP 5 * FROM dbo.Log";
            comand = new SqlCommand(queryStatement, connectionDB);

            try
            {
                connectionDB.Open();

                //DataTable customerTable = new DataTable("Log");
                //SqlDataAdapter sda = new SqlDataAdapter(comand);

                //sda.Fill(customerTable);

                //DataTable table = new DataTable();
                //SqlDataReader reader = comand.ExecuteReader();
                //int line = 0;

                //do
                //{
                //    while (reader.Read())
                //    {
                //        if (line == 0)
                //        {
                //            for (int i = 0; i < reader.FieldCount; i++)
                //            {
                //                table.Columns.Add(reader.GetName(i));
                //            }
                //            line++;
                //        }

                //        DataRow row = table.NewRow();
                //        for (int i = 0; i < reader.FieldCount; i++)
                //        {
                //            row[i] = reader[i];
                //           // Console.WriteLine(row[i].ToString());
                //        }
                //        table.Rows.Add(row);
                //    }
                //} while (reader.NextResult());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // conn.Close();
            }


        }

        protected void Disconect()
        {
            comand.Dispose();
            connectionDB.Close();
            connectionDB.Dispose();
        }
        abstract public void InIt();
        abstract public void Save();

    }
}
