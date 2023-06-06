using System;
using UnityEngine;

namespace LnxArch
{
    public class OnDisableObservable : MonoBehaviour
    {
        public event Action Callbacks;

        private void OnDisable()
        {
            Callbacks?.Invoke();
        }
    }
}
