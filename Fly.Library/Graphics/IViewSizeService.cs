
namespace Fly.Graphics
{
    using Atom.Math;

    public interface IViewSizeService
    {
        Point2 ViewSize { get; }

        int ViewWidth { get; }

        int ViewHeight { get; }
    }
}
