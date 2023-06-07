using UnityEngine;
using System;

namespace LnxArch
{
    public class SimpleAutoAdder : IAutoAdder
    {
        public MonoBehaviour AddOn(GameObject owner, Type componentType)
        {
            return (MonoBehaviour) owner.AddComponent(componentType);
        }
    }
}
