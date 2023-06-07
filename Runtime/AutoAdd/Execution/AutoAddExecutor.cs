using UnityEngine;
using System;

namespace LnxArch
{
    public class AutoAddExecutor
    {
        public IOwnerLookup OwnerLookup { get; }
        public IAutoAdder AutoAdder { get; }

        public AutoAddExecutor(IOwnerLookup ownerLookup, IAutoAdder autoAdder)
        {
            OwnerLookup = ownerLookup;
            AutoAdder = autoAdder;
        }

        public MonoBehaviour ExecuteAt(MonoBehaviour dependencyHolder, Type componentType)
        {
            GameObject owner = OwnerLookup.Fetch(dependencyHolder, componentType);
            return AutoAdder.AddOn(owner, componentType);
        }
    }
}
