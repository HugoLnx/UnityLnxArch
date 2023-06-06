using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    public interface IInspectorValueGetter
    {
        Component GetOne(MonoBehaviour behaviour);
        IEnumerable<Component> GetMany(MonoBehaviour behaviour);
    }
}
