using UnityEngine;
using System;

namespace LnxArch
{
    public class OwnerLookupEntity : IOwnerLookup
    {
        public GameObject Fetch(MonoBehaviour dependencyHolder, Type componentType)
        {
            LnxEntity entity = LnxEntity.FetchEntityOf(dependencyHolder);
            GameObject obj = new($"{componentType.Name} [LnxAutoAdd]");
            obj.transform.SetParent(entity.transform);
            return obj;
        }
    }
}
