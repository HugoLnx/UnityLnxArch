using UnityEngine;
using System;

namespace LnxArch
{
    public class OwnerLookupEntity : IOwnerLookup
    {
        public GameObject Fetch(MonoBehaviour dependencyHolder, Type _)
        {
            return LnxEntity.FetchEntityOf(dependencyHolder).gameObject;
        }
    }
}
