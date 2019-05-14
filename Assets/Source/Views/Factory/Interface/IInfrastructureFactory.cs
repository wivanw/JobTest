using Data.Enum;
using Views;

namespace View.Factory
{
    public interface IInfrastructureFactory
    {
        InfrastructureView Cteate(Infrastructure element);
    }
}