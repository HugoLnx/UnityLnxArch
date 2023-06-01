using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public class InspectorFieldValueGetter : IInspectorValueGetter
    {
        private readonly FieldInfo _fieldInfo;

        public InspectorFieldValueGetter(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public Component GetOne(MonoBehaviour behaviour)
        {
            return (Component) _fieldInfo.GetValue(behaviour);
        }

        public IEnumerable<Component> GetMany(MonoBehaviour behaviour)
        {
            return (IEnumerable<Component>) _fieldInfo.GetValue(behaviour);
        }
    }
}
