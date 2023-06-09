using System;

namespace LnxArch
{
    [Serializable]
    public class LnxObjectLinkConfig
    {
        public bool ThisAffectsTarget = true;
        public bool TargetAffectsThis = true;
        public ObjectLinkEvent EventType { get; }

        public LnxObjectLinkConfig(ObjectLinkEvent eventType)
        {
            EventType = eventType;
        }
    }
}
