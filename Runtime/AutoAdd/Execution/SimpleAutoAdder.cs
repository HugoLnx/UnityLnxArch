using UnityEngine;
using System;

namespace LnxArch
{
    public class SimpleAutoAdder : IAutoAdder
    {
        public MonoBehaviour AddOn(GameObject owner, Type componentType)
        {
            owner.SetActive(false);
            MonoBehaviour behaviour = (MonoBehaviour) owner.AddComponent(componentType);
            InitService.Instance.InitBehaviour(behaviour);
            owner.SetActive(true);
            return behaviour;
        }
    }
}
