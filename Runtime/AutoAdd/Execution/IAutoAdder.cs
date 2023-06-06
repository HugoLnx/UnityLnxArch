using UnityEngine;
using System;

namespace LnxArch
{
    public interface IAutoAdder
    {
        MonoBehaviour AddOn(GameObject owner, Type componentType);
    }
}
