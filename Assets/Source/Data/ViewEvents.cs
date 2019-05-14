using Common;

namespace Data
{
    public class PointViewEvent : StringEnum<PointViewEvent>
    {
        public static readonly PointViewEvent BallTrigger = new PointViewEvent("BallTrigger");
        
        private PointViewEvent(string value) : base(value)
        {
        }
    }

    public class BottomEvent : StringEnum<BottomEvent>
    {
        public static readonly BottomEvent Drag = new BottomEvent("Drag");
        public static readonly BottomEvent ButtonUp = new BottomEvent("ButtonUp");
        public static readonly BottomEvent ButtonDown = new BottomEvent("ButtonDown");

        private BottomEvent(string value) : base(value)
        {
        }
    }
}