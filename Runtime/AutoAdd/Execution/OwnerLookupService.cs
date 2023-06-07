using UnityEngine;
using System;

namespace LnxArch
{
    public class OwnerLookupService : IOwnerLookup
    {
        public GameObject Fetch(MonoBehaviour _, Type componentType)
        {
            return new($"{componentType.Name} [LnxService - AutoAdd]");
        }
    }
}
