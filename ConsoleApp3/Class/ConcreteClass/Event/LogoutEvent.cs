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
                commandEvent.Transaction = sqlT;

                commandEvent.CommandText = @"INSERT INTO log (Type,Time,UserName,PcName) VALUES (@Type,@Time,@UserName,@PcName)";

                commandEvent.Parameters.Add("@Type", SqlDbType.TinyInt);
                commandEvent.Parameters.Add("@Time", SqlDbType.DateTime);
                commandEvent.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
                commandEvent.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);

                commandEvent.Parameters["@Type"].Value = events.Logout;
                commandEvent.Parameters["@Time"].Value = DateTime.Now;
                commandEvent.Parameters["@UserName"].Value = Environment.UserName;
                commandEvent.Parameters["@PcName"].Value = Environment.MachineName;

                commandEvent.ExecuteNonQuery();

                commandEvent.CommandText = @"INSERT INTO WorkTime (LogId) VALUES (@LogId)";


                SqlCommand command = new SqlCommand(
                    @"SELECT TOP(1) Id                  
                        FROM Log                        
                        WHERE Type=@Type                    
                            AND UserName=@UserName      
                            AND PcName=@PcName          
                        GROUP BY Time DESC", connection: connectionDB);//TODO test it

                command.Transaction = sqlT;

                command.Parameters.Add("@Type", SqlDbType.TinyInt);
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
                command.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);

                command.Parameters["@Type"].Value = events.Login;
                command.Parameters["@UserName"].Value = Environment.UserName;
                command.Parameters["@PcName"].Value = Environment.MachineName;


                int logId = (int)command.ExecuteScalar();

                SqlCommand sqlCommand = new SqlCommand(@"
                    SELECT TOP(1) Time
                        FROM Log
                        WHERE Id=@Id", connectionDB);
                sqlCommand.Parameters.Add("@Id", SqlDbType.Int);
                sqlCommand.Parameters["@Id"].Value = logId;



                commandEvent.Parameters.Add("@LogId", SqlDbType.Int);
                commandEvent.Parameters["@LogId"].Value = logId;
                commandEvent.Parameters.Add("@WorkTime", SqlDbType.Int);
                commandEvent.Parameters["@WorkTime"].Value =
                    Convert.ToInt32(DateTime.Now - (DateTime)sqlCommand.ExecuteScalar());//TODO test it


                commandEvent.ExecuteNonQuery();

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
