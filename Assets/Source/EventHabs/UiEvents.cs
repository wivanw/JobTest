using VBCM;

namespace EventHabs
{
    public class ChangePointEvent : Hub<ChangePointEvent, int, int>{}
    public class GameRessetEvent : Hub<GameRessetEvent, object, object>{}
}