using UnityEngine;
using System;

namespace LnxArch
{
    public class ServiceAutoAdder : IAutoAdder
    {
        public MonoBehaviour AddOn(GameObject owner, Type componentType)
        {
            owner.SetActive(false);
            var behaviour = (MonoBehaviour) owner.AddComponent(componentType);
            LnxServiceEntity serviceEntity = owner.AddComponent<LnxServiceEntity>();
            owner.SetActive(true);
            return behaviour;
        }
    }
}
