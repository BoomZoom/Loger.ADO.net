using ConsoleApp3.Class.AbstractClass;
using System.Data;
using System.Data.SqlClient;
using System;

namespace ConsoleApp3.Class.ConcreteClass.Event
{
    class LoginEvent : EventData
    {
        public LoginEvent()
        {
            commandEvent.CommandText = @"INSERT INTO log (Type,Time,UserName,PcName) VALUES (@Type,@Time,@UserName,@PcName)";

            commandEvent.Parameters.Add("@Type", SqlDbType.TinyInt);
            commandEvent.Parameters.Add("@Time", SqlDbType.DateTime);
            commandEvent.Parameters.Add("@UserName", SqlDbType.NVarChar, 255);
            commandEvent.Parameters.Add("@PcName", SqlDbType.NVarChar, 255);
        }
        public override void Init()
        {
            Console.WriteLine("EventLogin Вошли");
        }

        public override void Save()
        {
            commandEvent.Parameters["@Type"].Value = Events.Login;
            commandEvent.Parameters["@Time"].Value = DateTime.Now;
            commandEvent.Parameters["@UserName"].Value = Environment.UserName;
            commandEvent.Parameters["@PcName"].Value = Environment.MachineName;

            commandEvent.ExecuteNonQuery();

            Console.WriteLine("EventLogin Сохранено!!!");

        }
    }
}
