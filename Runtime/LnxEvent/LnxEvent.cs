using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LnxArch
{
    [LnxAutoAdd]
    public abstract class LnxEvent<TEvent, TArgs> : MonoBehaviour
    where TEvent : LnxEvent<TEvent, TArgs>
    {
        private HashSet<LnxEventListener<TEvent, TArgs>> _listeners = new();

        public void Trigger(object sender, TArgs args)
        {
            foreach (var listener in _listeners)
            {
                listener.Trigger(sender, args);
            }
        }

        internal void Subscribe(LnxEventListener<TEvent, TArgs> listener)
        {
            _listeners.Add(listener);
        }

        internal void Unsubscribe(LnxEventListener<TEvent, TArgs> listener)
        {
            _listeners.Remove(listener);
        }

        // TODO: Inspector button to trigger
    }
}
