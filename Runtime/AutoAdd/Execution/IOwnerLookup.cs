using UnityEngine;
using System;

namespace LnxArch
{
    public interface IOwnerLookup
    {
        GameObject Fetch(MonoBehaviour dependencyHolder, Type componentType);
    }
}
