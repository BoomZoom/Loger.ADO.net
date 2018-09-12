using ConsoleApp3.Class.AbstractClass;
using ConsoleApp3.Class.ConcreteClass.Event;

namespace ConsoleApp3.Class.ConcreteClass.Creator
{
    class CreatorError : CreatorEvent
    {
        public override EventData Create()
        {
            return new ErrorEvent();
        }
    }
}
