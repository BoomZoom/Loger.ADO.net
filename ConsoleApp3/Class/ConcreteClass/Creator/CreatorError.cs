using ConsoleApp3.Class.AbstractClass;
using ConsoleApp3.Class.ConcreteClass.Event;

namespace ConsoleApp3.Class.ConcreteClass.Creator
{
    class CreatorError : CreatorEvent
    {
        public override EventData Creator()
        {
            return new ErrorEvent();
        }
    }
}
