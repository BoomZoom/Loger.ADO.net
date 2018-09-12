using System;
using ConsoleApp3.Class.AbstractClass;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp3.Class.ConcreteClass.Event
{
    class ErrorEvent : EventData
    {
        public override void Init()
        {
            Console.WriteLine("");
        }

        public override void Save()
        {
            Console.WriteLine("Входные параметры указаны не верно!!!");
        }
    }
}
