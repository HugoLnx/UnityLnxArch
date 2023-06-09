using UnityEngine;

namespace LnxArch
{
    public class EnableObjectLinker : ObjectLinker<OnEnableObservable>
    {
        public EnableObjectLinker(GameObject trigger, GameObject target)
        : base(trigger, target)
        {}

        protected override void PerformLinking()
        {
            _target.SetActive(true);
        }
    }
}
