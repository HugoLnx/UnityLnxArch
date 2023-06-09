using System;

namespace LnxArch
{
    [Serializable]
    public class LnxObjectLinkerConfig
    {
        public LnxObjectLinkConfig LinkEnable = new(ObjectLinkEvent.Enable);
        public LnxObjectLinkConfig LinkDisable = new(ObjectLinkEvent.Disable);
        public LnxObjectLinkConfig LinkDestroy = new(ObjectLinkEvent.Destroy);
        public LnxObjectLinkConfig[] AllLinks { get; }

        public LnxObjectLinkerConfig()
        {
            AllLinks = new LnxObjectLinkConfig[] {
                LinkEnable,
                LinkDisable,
                LinkDestroy
            };
        }
    }
}
