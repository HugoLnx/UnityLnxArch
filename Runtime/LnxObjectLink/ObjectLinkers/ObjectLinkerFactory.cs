using System;
using UnityEngine;

namespace LnxArch
{
    public class ObjectLinkerFactory
    {
        public IObjectLinker CreateLinker(GameObject trigger, GameObject target, ObjectLinkEvent eventType)
        {
            return eventType switch
            {
                ObjectLinkEvent.Enable => new EnableObjectLinker(trigger, target),
                ObjectLinkEvent.Disable => new DisableObjectLinker(trigger, target),
                ObjectLinkEvent.Destroy => new DestroyObjectLinker(trigger, target),
                _ => throw new NotImplementedException($"EntityLinkEventType {eventType} is not implemented")
            };
        }
    }
}
