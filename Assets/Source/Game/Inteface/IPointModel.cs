namespace Game
{
    public interface IPointModel
    {
        int Point { get; }
        void AddPoint();
        void Resset();
        void RemovePoint();
    }
}