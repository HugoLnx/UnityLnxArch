using System;
using UnityEngine;

namespace LnxArch
{
    public class OnEnableObservable : MonoBehaviour
    {
        public event Action Callbacks;

        private void OnEnable()
        {
            Callbacks?.Invoke();
        }
    }
}
