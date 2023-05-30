using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LnxArch
{
    public sealed class AutofetchService
    {
        public static AutofetchService Instance { get; } = new(AutofetchExecutor.Instance);
        private readonly AutofetchExecutor _executor;
        private AutofetchService(AutofetchExecutor executor)
        {
            this._executor = executor;
        }
        public void AutofetchAllExistingEntites()
        {
            LnxEntity[] allEntities = GameObject.FindObjectsByType<LnxEntity>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            AutofetchEntities(allEntities);
        }

        public void StartAutofetchChainAt(GameObject gameObject)
        {
            LnxEntity rootEntity = gameObject.GetComponentsInParent<LnxEntity>(includeInactive: true)
                .LastOrDefault(entity => !entity.WasAutofetched);
            GameObject chainRoot = rootEntity?.gameObject ?? gameObject;
            AutofetchChildrenOf(gameObject);
        }

        public void AutofetchChildrenOf(GameObject gameObject)
        {
            AutofetchChildrenOf(new GameObject[] { gameObject });
        }

        public void AutofetchChildrenOf(IEnumerable<GameObject> gameObjects)
        {
            IEnumerable<LnxEntity> entities =
                gameObjects
                .SelectMany(
                    obj => obj.GetComponentsInChildren<LnxEntity>(includeInactive: true)
                )
                .Distinct();
            AutofetchEntities(entities);
        }

        public void AutofetchEntities(IEnumerable<LnxEntity> entities)
        {
            HashSet<LnxEntity> pendingEntities =
                entities
                .Where(entity => !entity.WasAutofetched)
                .ToHashSet();

            IEnumerable<LnxEntity> rootEntities =
                pendingEntities
                .Select(entity =>
                    entity
                    .GetComponentsInParent<LnxEntity>(includeInactive: true)
                    .Last(entity => pendingEntities.Contains(entity))
                )
                .Distinct();

            IEnumerable<MonoBehaviour> pendingBehaviours =
                rootEntities
                .SelectMany(entity => entity.GetComponentsInChildren<MonoBehaviour>(includeInactive: true))
                .Distinct();

            AutofetchBehaviours(pendingBehaviours);

            foreach (LnxEntity pendingEntity in pendingEntities)
            {
                pendingEntity.WasAutofetched = true;
            }
        }

        private void AutofetchBehaviours(IEnumerable<MonoBehaviour> allBehaviours)
        {
            foreach (MonoBehaviour behaviour in allBehaviours)
            {
                _executor.ExecuteAutofetchOn(behaviour);
            }
        }
    }
}
