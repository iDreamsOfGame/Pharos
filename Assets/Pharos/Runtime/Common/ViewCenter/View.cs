using System;
using UnityEngine;

namespace Pharos.Common.ViewCenter
{
    public abstract class View : MonoBehaviour, IView
    {
        public event Action<IView> Destroying;

        protected virtual void Awake()
        {
            ViewRegistry.Instance.RegisterView(this);
        }

        protected virtual void OnDestroy()
        {
            Destroying?.Invoke(this);
        }
    }
}