#if UNITY_EDITOR
using Pharos.Common.ViewCenter;
using UnityEngine;

namespace Pharos.Extensions.Mediation
{
    internal class MediatorMappingInfo : MonoBehaviour
    {
        public IView View { get; internal set; }

        public IMediator Mediator { get; internal set; }
    }
}
#endif