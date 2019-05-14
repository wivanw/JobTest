using System;
using EventHabs;

namespace Game
{
    public class PointModel : IPointModel, ChangePointEvent.IValidated, ChangePointEvent.IEventSource
    {
        public int Point { get; private set; }

        public void AddPoint()
        {
            Point++;
            Event?.Invoke();
        }

        public void Resset()
        {
            Point = 0;
            Event?.Invoke();
        }

        public void RemovePoint()
        {
            Point--;
            if (Point < 0)
            {
                Point = 0;
            }
            else
            {
                Event?.Invoke();
            }
        }

        public bool Validate(int sendValue, out string errorMessage)
        {
            errorMessage = string.Empty;
            return sendValue >= 0;
        }

        public event Action Event;
    }
}