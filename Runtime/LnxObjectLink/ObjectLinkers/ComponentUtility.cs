using UnityEngine;

namespace LnxArch
{
    public static class ComponentUtility
    {
        public static T EnsureComponent<T>(GameObject gameObject)
        where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }
    }
}
