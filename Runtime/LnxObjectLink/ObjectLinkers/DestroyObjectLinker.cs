using UnityEngine;

namespace LnxArch
{
    public class DestroyObjectLinker : ObjectLinker<OnDestroyObservable>
    {
        public DestroyObjectLinker(GameObject trigger, GameObject target)
        : base(trigger, target)
        {}

        protected override void PerformLinking()
        {
            _target.SetActive(false);
        }
    }
}
