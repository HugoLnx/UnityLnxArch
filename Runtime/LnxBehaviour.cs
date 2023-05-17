using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using System.Reflection;

namespace LnxArch
{
    public abstract class LnxBehaviour : MonoBehaviour
    {
        private LnxEntity _entity;

        public LnxEntity Entity => _entity ??= LnxEntityLookup(this);
        public LnxEntity ParentEntity => ParentLnxEntityLookup(this);

        public static LnxEntity GetLnxEntity(Component component)
        {
            LnxBehaviour lnxBehaviour = component as LnxBehaviour;
            return lnxBehaviour?.Entity ?? LnxEntityLookup(component);
        }

        public static LnxEntity GetParentLnxEntity(Component component)
        {
            return ParentLnxEntityLookup(component);
        }

        private static LnxEntity LnxEntityLookup(Component component)
        {
            LnxEntity entity = LnxEntity.FetchEntityOf(component);
            Assert.IsNotNull(entity, $"{component.GetType().Name} must be inside a LnxEntity to be a LnxBehaviour or have an Autofetch method.");
            return entity;
        }

        private static LnxEntity ParentLnxEntityLookup(Component component)
        {
            LnxEntity ownerEntity = GetLnxEntity(component);
            return LnxEntity.FetchEntityParentOf(ownerEntity);
        }

    }
}
