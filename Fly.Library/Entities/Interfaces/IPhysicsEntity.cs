
namespace Fly.Entities
{
    using Fly.Components;

    public interface IPhysicsEntity : IFlyEntity
    {
        IPhysicable Physics
        {
            get;
        }
    }
}
