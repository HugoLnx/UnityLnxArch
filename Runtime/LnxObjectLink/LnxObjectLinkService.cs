using System.Collections.Generic;
using LnxArch;
using UnityEngine;

namespace LnxArch
{
    public class LnxObjectLinkService
    {
        private readonly GameObject _objA;
        private readonly GameObject _objB;
        private readonly ObjectLinkerFactory _factory;
        private readonly Dictionary<(ObjectLinkEvent eventType, bool isAToB), IObjectLinker> _linkers = new();

        public LnxObjectLinkService(GameObject objA, GameObject objB, ObjectLinkerFactory factory = null)
        {
            _objA = objA;
            _objB = objB;
            _factory = factory ?? new ObjectLinkerFactory();
        }

        public void Link(ObjectLinkEvent eventType, bool isAToB)
        {
            GetLinkerFor(eventType, isAToB).Link();
        }

        public void Unlink(ObjectLinkEvent eventType, bool isAToB)
        {
            GetLinkerFor(eventType, isAToB).Unlink();
        }

        private IObjectLinker GetLinkerFor(ObjectLinkEvent eventType, bool isAToB)
        {
            (ObjectLinkEvent eventType, bool isAToB) linkerKey = (eventType, isAToB);
            IObjectLinker linker = _linkers.GetValueOrDefault(linkerKey);
            if (linker == null)
            {
                (GameObject trigger, GameObject target) = isAToB ? (_objA, _objB) : (_objB, _objA);
                linker ??= _factory.CreateLinker(trigger, target, eventType);
                _linkers.Add(linkerKey, linker);
            }
            return linker;
        }
    }
}
