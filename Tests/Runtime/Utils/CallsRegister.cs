using System;
using System.Collections.Generic;
using System.Linq;
using LnxArch;
using UnityEngine;

namespace LnxArch.TestTools
{
    public class CallsRegister : MonoBehaviour
    {
        private readonly List<string> _calls = new();
        public IReadOnlyList<string> Calls => _calls;

        public void Record(System.Type type, string methodName)
        {
            _calls.Add(entryNameFor(type, methodName));
        }

        public int OrderOf(System.Type type, string methodName)
        {
            var entryName = entryNameFor(type, methodName);
            return _calls.IndexOf(entryName);
        }

        public bool Contains(System.Type type, string methodName)
        {
            return OrderOf(type, methodName) > -1;
        }

        public int Count(Type type)
        {
            return _calls.Count(entry => entry.StartsWith($"{type.Name}."));
        }

        public void Clear()
        {
            _calls.Clear();
        }

        public static void FindAndRecordFor(Component component, string methodName)
        {
            LnxEntity
                .FetchEntityOf(component)
                .FetchFirst<CallsRegister>()
                ?.Record(component.GetType(), methodName);
        }

        private static string entryNameFor(Type type, string methodName)
            => $"{type.Name}.{methodName}";
    }
}
