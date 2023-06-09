using UnityEngine;

namespace LnxArch
{
    public class LnxObjectLink : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private LnxObjectLinkerConfig _config;
        private LnxObjectLinkService _service;

        [LnxInit]
        private void Init()
        {
            _service = new LnxObjectLinkService(gameObject, _target);
            EnforceLinks();
        }

        public void EnforceLinks()
        {
            foreach (LnxObjectLinkConfig linkConfig in _config.AllLinks)
            {
                if (linkConfig.ThisAffectsTarget)
                {
                    _service.Link(linkConfig.EventType, isAToB: true);
                }
                else
                {
                    _service.Unlink(linkConfig.EventType, isAToB: true);
                }
                if (linkConfig.TargetAffectsThis)
                {
                    _service.Link(linkConfig.EventType, isAToB: false);
                }
                else
                {
                    _service.Unlink(linkConfig.EventType, isAToB: false);
                }
            }
        }
    }
}
