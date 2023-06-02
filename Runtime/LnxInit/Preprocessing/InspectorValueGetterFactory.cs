using System.Collections.Generic;
using System.Reflection;

namespace LnxArch
{
    public static class InspectorValueGetterFactory
    {
        public static IInspectorValueGetter TryBuildFor(InitType declaringType, string lookupBaseName)
        {
            foreach (string lookupName in BuildLookupNames(lookupBaseName))
            {
                if (declaringType.InspectorVisibleFields.TryGetValue(lookupName, out FieldInfo field))
                {
                    return new InspectorFieldValueGetter(field);
                }
            }
            return null;
        }

        private static IEnumerable<string> BuildLookupNames(string baseName)
        {
            string normalized = baseName.TrimStart('_');
            string capitalized = normalized[0].ToString().ToUpper() + normalized.Substring(1);
            string decapitalized = normalized[0].ToString().ToLower() + normalized.Substring(1);
            yield return $"_{decapitalized}";
            yield return decapitalized;
            yield return capitalized;
            yield return $"_{capitalized}";

            // For properties with [field: SerializeField]
            yield return $"<{capitalized}>k__BackingField";
            yield return $"<{decapitalized}>k__BackingField";
            yield return $"<_{decapitalized}>k__BackingField";
            yield return $"<_{capitalized}>k__BackingField";
        }
    }
}
