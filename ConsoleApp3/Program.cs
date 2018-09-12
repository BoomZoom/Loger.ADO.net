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
        static void Main(string[] args)
        {
            EventData eventA;
            CreatorEvent creator;            
            {                
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
            } 
        }
    }  
}
