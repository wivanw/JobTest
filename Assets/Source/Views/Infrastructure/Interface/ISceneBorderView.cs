using Data;

namespace Game.Infrastructure
{
    public interface ISceneBorderView
    {
        void SetParams(BorderParamContainer parameters, Data.Enum.Border type);
    }
}