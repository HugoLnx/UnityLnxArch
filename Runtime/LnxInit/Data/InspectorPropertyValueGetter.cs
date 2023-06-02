using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public class InspectorPropertyValueGetter : IInspectorValueGetter
    {
        private readonly PropertyInfo _propertyInfo;

        public InspectorPropertyValueGetter(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public Component GetOne(MonoBehaviour behaviour)
        {
            return (Component) _propertyInfo.GetValue(behaviour);
        }

        public IEnumerable<Component> GetMany(MonoBehaviour behaviour)
        {
            return (IEnumerable<Component>) _propertyInfo.GetValue(behaviour);
        }
    }
}
