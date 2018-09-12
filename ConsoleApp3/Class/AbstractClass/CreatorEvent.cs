using ConsoleApp3.Interface;

namespace ConsoleApp3.Class.AbstractClass
{
    abstract class CreatorEvent : IEventCreator
    {
        public abstract EventData Create();
    }
}
