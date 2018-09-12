using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp3.Interface;
using ConsoleApp3.Class.ConcreteClass.Creator;
using ConsoleApp3.Class.AbstractClass;

namespace ConsoleApp3
{
    enum events
    {
        Login,
        Logout
    }
    class Program
    {

        static public EventData GetEvent(string eventType)
        {
            CreatorEvent creator;
            switch (eventType)
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
                    throw new Exception("неправельный параметр!!!");
            }
            return creator.Create();
        }

        static public string GetEventLabel(int eventType)
        {
            switch (eventType)
            {
                case (int)events.Login:
                    return "LOGIN";
                case (int)events.Logout:
                    return "LOGOUT";
                default:
                    throw new ArgumentException("Invalid event type");
            }
}
        static void Main(string[] args)
        {
            EventData eventA;
            
            {                
                if (args.Length != 0)
                {
                    eventA = GetEvent(args[0]);
                    eventA.Init();
                    eventA.Save();
                }
                else
                {
                    SqlConnection connection = new SqlConnection(@"server=DESKTOP-PC73D7E\SQLEXPRESS;database=test;integrated Security=SSPI;");
                    connection.Open();
                    SqlCommand command = new SqlCommand(@"SELECT Time,UserName,PcName,WorkTime,Type
                        FROM Log as L
                        LEFT JOIN WorkTime AS WT
                            ON L.Id = WT.LogId 
                        WHERE Time < DATEADD(day,1,GETDATE())",
                        connection);
                    DataTable table = new DataTable();
                    SqlDataReader reader = command.ExecuteReader();
                    int line = 0;
                    while (reader.Read())
                    {
                        if (line==0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            ++line;
                        }
                        DataRow row = table.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);

                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var temp = table.Rows[i].ItemArray;

                        for (int j = 0; j < temp.Length-1; j++)
                        {

                            Console.Write(temp[j]+"\t");
                            
                        }
                        int eventType = Convert.ToInt32(temp[temp.Length - 1]);
                        Console.Write(GetEventLabel(eventType));
                        
                        Console.WriteLine();
                    }


                    connection.Dispose();

                }
            } 
        }
    }  
}
