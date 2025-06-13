#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Pharos.Extensions.Mediation
{
    internal class MediatorMappingInfo : MonoBehaviour
    {
        public Type ViewType { get; internal set; }

        public Type MediatorType { get; internal set; }
    }
}
#endif