using UnityEngine;
using System;

namespace LnxArch
{
    [LnxAutoAdd]
    public abstract class LnxEventListener<TEvent, TArgs> : MonoBehaviour
    where TEvent : LnxEvent<TEvent, TArgs>
    {
        public EventHandler<TArgs> OnTrigger;
        private TEvent _event;

        [LnxInit]
        protected void Init(TEvent lnxEvent)
        {
            // Debug.Log($"[LnxEventListener] Init {this} with {lnxEvent}");
            _event = lnxEvent;
            _event.Subscribe(this);
        }

        private void OnEnable()
        {
            // Debug.Log($"[OnEnable] Subscribing {this} to {_event}");
            _event.Subscribe(this);
        }

        private void OnDisable()
        {
            // Debug.Log($"[OnDisable] Unsubscribing {this} from {_event}");
            _event.Unsubscribe(this);
        }

        internal void Trigger(object sender, TArgs args)
        {
            OnTrigger?.Invoke(sender, args);
        }
    }
}
