using ConsoleApp3.Class.AbstractClass;
using ConsoleApp3.Class.ConcreteClass.Event;

namespace ConsoleApp3.Class.ConcreteClass.Creator
{
    class CreatorLoginEvent : CreatorEvent
    {
        public override EventData Creator()
        {
            return new LoginEvent();
        }
    }
}
