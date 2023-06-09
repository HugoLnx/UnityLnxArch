using System;
using UnityEngine;

namespace LnxArch
{
    public class OnDestroyObservable : MonoBehaviour, ISimpleObservable
    {
        public event Action Callbacks;

        private void OnDestroy()
        {
            Callbacks?.Invoke();
        }
    }
}
