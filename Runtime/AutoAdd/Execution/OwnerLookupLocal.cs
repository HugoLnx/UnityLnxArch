using UnityEngine;
using System;

namespace LnxArch
{
    public class OwnerLookupLocal : IOwnerLookup
    {
        public GameObject Fetch(MonoBehaviour dependencyHolder, Type _)
        {
            return dependencyHolder.gameObject;
        }
    }
}
