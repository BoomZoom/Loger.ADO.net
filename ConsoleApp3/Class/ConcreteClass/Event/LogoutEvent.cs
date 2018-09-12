using System;
using ConsoleApp3.Class.AbstractClass;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp3.Class.ConcreteClass.Event
{
    class LogoutEvent : EventData
    {
        public override void InIt()
        {
            Console.WriteLine("EventLogout Вошли");
        }

        public override void Save()
        {
            SqlTransaction sqlT = connectionDB.BeginTransaction();
            try
            {
                comand.Transaction = sqlT;

                comand.CommandText = @"INSERT INTO log (Type,Time,UserName,PcName) VALUES (@Type,@Time,@UserName,@PcName)";

                comand.Parameters.Add("@Type", SqlDbType.TinyInt);
                comand.Parameters.Add("@Time", SqlDbType.DateTime);
                comand.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
                comand.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);

                comand.Parameters["@Type"].Value = events.Logout;
                comand.Parameters["@Time"].Value = DateTime.Now;
                comand.Parameters["@UserName"].Value = Environment.UserName;
                comand.Parameters["@PcName"].Value = Environment.MachineName;

                comand.ExecuteNonQuery();

                comand.CommandText = @"INSERT INTO WorkTime (LogId) VALUES (@LogId)";

                comand.Parameters.Add("@LogId", SqlDbType.Int);

                SqlCommand command = new SqlCommand("SELECT MAX(Id) FROM Log WHERE Type=@Type AND UserName=@UserName AND PcName=@PcName", connection: connectionDB);

                command.Transaction = sqlT;

                command.Parameters.Add("@Type", SqlDbType.TinyInt);
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
                command.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);

                command.Parameters["@Type"].Value = events.Login;
                command.Parameters["@UserName"].Value = Environment.UserName;
                command.Parameters["@PcName"].Value = Environment.MachineName;


                int logId = (int)command.ExecuteScalar();

                comand.Parameters["@LogId"].Value = logId;

                comand.ExecuteNonQuery();

                sqlT.Commit();

            }
            catch (Exception ex)
            {
                sqlT.Rollback();
                Console.WriteLine(ex.Message);
                // throw;
            }
            Console.WriteLine("EventLogout Сохранено!!!");
        }
    }
}
