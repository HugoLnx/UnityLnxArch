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
            GameObject.Destroy(_target);
        }
    }
}
