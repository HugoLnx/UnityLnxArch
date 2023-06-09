using UnityEngine;
using static LnxArch.ComponentUtility;

namespace LnxArch
{
    public abstract class ObjectLinker<TObservable> : IObjectLinker
    where TObservable : Component, ISimpleObservable
    {
        protected GameObject _trigger;
        protected GameObject _target;
        private TObservable _observable;
        private TObservable TriggerEventObservable => _observable ??= EnsureComponent<TObservable>(_trigger);

        protected ObjectLinker(GameObject trigger, GameObject target)
        {
            _trigger = trigger;
            _target = target;
        }

        protected abstract void PerformLinking();
        public void Link()
        {
            TriggerEventObservable.Callbacks += PerformLinking;
        }

        public void Unlink()
        {
            TriggerEventObservable.Callbacks -= PerformLinking;
        }
    }
}
