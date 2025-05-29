using System;
using System.Collections.Generic;

namespace Pharos.Framework.Helpers
{
    internal class LifecycleTransition
    {
        private readonly List<LifecycleState> fromStates = new();

        private LifecycleState transitionState = LifecycleState.Destroyed;

        private LifecycleState toState = LifecycleState.Destroyed;

        public LifecycleTransition(LifecycleManager lifecycleManager)
        {
            LifecycleManager = lifecycleManager;
        }

        public LifecycleManager LifecycleManager { get; }

        public Action PreprocessCallback { get; set; }

        public Action ProcessingCallback { get; set; }

        public Action PostprocessCallback { get; set; }

        /// <summary>
        /// States that this transition is allowed to enter from. 
        /// </summary>
        /// <param name="states">Allowed states. </param>
        /// <returns>Self. </returns>
        public LifecycleTransition FromStates(params LifecycleState[] states)
        {
            foreach (var state in states)
            {
                fromStates.Add(state);
            }

            return this;
        }

        /// <summary>
        /// The states that this transition applies. 
        /// </summary>
        /// <param name="transitionState">The state that the target is put into during the transition. </param>
        /// <param name="toState">The state that the target is put into after the transition. </param>
        /// <returns>Self. </returns>
        public LifecycleTransition ToStates(LifecycleState transitionState, LifecycleState toState)
        {
            this.transitionState = transitionState;
            this.toState = toState;
            return this;
        }

        public void Enter(Action<Exception> callback = null)
        {
            if (LifecycleManager.State == toState)
            {
                callback?.Invoke(null);
                return;
            }

            if (LifecycleManager.State == transitionState)
            {
                ReportError(new InvalidOperationException("The transition is processing, no need to to it again. "), callback);
                return;
            }

            var isInvalid = fromStates.Count > 0 && !fromStates.Contains(LifecycleManager.State);
            if (isInvalid)
            {
                ReportError("Invalid transition", callback);
                return;
            }

            PreprocessCallback?.Invoke();
            LifecycleManager.SetState(transitionState);
            ProcessingCallback?.Invoke();
            LifecycleManager.SetState(toState);
            callback?.Invoke(null);
            PostprocessCallback?.Invoke();
        }

        private void ReportError(object message, Action<Exception> callback)
        {
            var exception = message as Exception ?? new Exception(message.ToString());
            ReportError(exception, callback);
        }

        private void ReportError<TException>(TException exception, Action<Exception> callback = null) where TException : Exception
        {
            var canThrow = !LifecycleManager.ReportError(exception);
            callback?.Invoke(exception);

            if (canThrow)
                throw exception;
        }
    }
}