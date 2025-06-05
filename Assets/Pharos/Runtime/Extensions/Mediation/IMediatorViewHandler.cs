using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public interface IMediatorViewHandler : IViewHandler
    {
        void AddMapping(IMediatorMapping mapping);

        void RemoveMapping(IMediatorMapping mapping);
    }
}