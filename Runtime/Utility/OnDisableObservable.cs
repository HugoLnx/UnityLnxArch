using System;
using UnityEngine;

namespace LnxArch
{
    public class OnDisableObservable : MonoBehaviour, ISimpleObservable
    {
        public event Action Callbacks;

        private void OnDisable()
        {
            Callbacks?.Invoke();
        }
    }
}
