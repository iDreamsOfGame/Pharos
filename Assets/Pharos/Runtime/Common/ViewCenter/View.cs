using UnityEngine;

namespace Pharos.Common.ViewCenter
{
    public abstract class View : MonoBehaviour, IView
    {
        protected virtual void Awake()
        {
            ViewRegistry.Instance.RegisterView(this);
        }

        protected virtual void OnDestroy()
        {
            ViewRegistry.Instance.RemoveView(this);
        }
    }
}