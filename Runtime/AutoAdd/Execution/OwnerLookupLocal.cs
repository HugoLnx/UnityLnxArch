using UnityEngine;
using System;

namespace LnxArch
{
    public class OwnerLookupLocal : IOwnerLookup
    {
        public GameObject Fetch(MonoBehaviour dependencyHolder, Type componentType)
        {
            GameObject obj = new($"{componentType.Name} [LnxAutoAdd]");
            obj.transform.SetParent(dependencyHolder.transform);
            return obj;
        }
    }
}
