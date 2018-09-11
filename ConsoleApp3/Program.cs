using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    enum events
    {
        Login,
        Logout
    }
    class Program
    {
        static void Main(string[] args)
        {
            EventData eventA;
            CreatorEvent creator;
            // if (args[0] != string.Empty)
            {
                //try
                //{
                if (args.Length != 0)
                {

                    switch (args[0])
                    {
                        case "Login":
                            {
                                creator = new CreatorLoginEvent();
                            }
                            break;
                        case "Logout":
                            {
                                creator = new CreatorLogoutEvent();
                            }
                            break;
                        default:
                            creator = new CreatorError();
                            break;

                    }
                    eventA = creator.Creator();
                    eventA.InIt();
                    eventA.Save();
                }
                else
                {
                    Console.WriteLine("входных параметров нет!!!");
                }

                //}
                //catch (IndexOutOfRangeException)
                //{
                //    Console.WriteLine("входных параметров нет!!!");
                //}

            }

            //if (Console.Read() == (int)ConsoleKey.A)
            //{
            //    creator = new CreatorLoginEvent();
            //}
            //else
            //{
            //    creator = new CreatorLogoutEvent();
            //}
            //eventA = creator.Creator();
            //eventA.InIt();
            //eventA.Save();

        }
    }

    #region factory Event

    #region concrete class

    #region event

    class LoginEvent : EventData
    {
        public LoginEvent()
        {
            comand.CommandText = @"INSERT INTO log (Type,Time,UserName,PcName) VALUES (@Type,@Time,@UserName,@PcName)";

            comand.Parameters.Add("@Type", SqlDbType.TinyInt);
            comand.Parameters.Add("@Time", SqlDbType.DateTime);
            comand.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
            comand.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);
        }
        public override void InIt()
        {
            Console.WriteLine("EventLogin Вошли");
        }

        public override void Save()
        {
            comand.Parameters["@Type"].Value = events.Login;
            comand.Parameters["@Time"].Value = DateTime.Now;
            comand.Parameters["@UserName"].Value = Environment.UserName;
            comand.Parameters["@PcName"].Value = Environment.MachineName;

            comand.ExecuteNonQuery();

            Console.WriteLine("EventLogin Сохранено!!!");

        }
    }

    class EventLogout : EventData
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
    class ErrorEvent : EventData
    {
        public override void InIt()
        {
            Console.WriteLine("");
        }

        public override void Save()
        {
            Console.WriteLine("Входные параметры указаны не верно!!!");
        }
    }

    #endregion event

    #region creator

    class CreatorLoginEvent : CreatorEvent
    {
        public override EventData Creator()
        {
            return new LoginEvent();
        }
    }

    class CreatorLogoutEvent : CreatorEvent
    {
        public override EventData Creator()
        {
            return new EventLogout();
        }
    }

    class CreatorError : CreatorEvent
    {
        public override EventData Creator()
        {
            return new ErrorEvent();
        }
    }

    #endregion creator


    #endregion concrete class

    #region abstract class
    abstract class CreatorEvent : IEventCreator
    {
        public abstract EventData Creator();
    }

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

    #endregion abstract class

    #region interface
    internal interface IEventCreator
    {
        EventData Creator();
    }
    internal interface IEventData
    {
        /// <summary>
        /// сохраняет событие
        /// </summary>
    	void Save();
        void InIt();
    }
    #endregion interface
    #endregion factory event
}
