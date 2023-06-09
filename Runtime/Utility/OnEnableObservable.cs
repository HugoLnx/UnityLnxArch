using System;
using UnityEngine;

namespace LnxArch
{
    public class OnEnableObservable : MonoBehaviour, ISimpleObservable
    {
        public event Action Callbacks;

        private void OnEnable()
        {
            Callbacks?.Invoke();
        }
    }
}
