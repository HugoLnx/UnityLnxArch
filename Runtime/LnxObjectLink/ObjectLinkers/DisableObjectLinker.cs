using UnityEngine;

namespace LnxArch
{
    public class DisableObjectLinker : ObjectLinker<OnDisableObservable>
    {
        public DisableObjectLinker(GameObject trigger, GameObject target)
        : base(trigger, target)
        {}

        protected override void PerformLinking()
        {
            _target.SetActive(false);
        }
    }
}
