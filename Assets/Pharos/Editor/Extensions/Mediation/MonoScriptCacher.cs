using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PharosEditor.Extensions.Mediation
{
    internal static class MonoScriptCacher
    {
        private static Dictionary<Type, MonoScript> typeToMonoScriptCache;

        public static MonoScript GetMonoScript(Type type)
        {
            TryGenerateTypeToMonoScriptCache();
            return typeToMonoScriptCache.GetValueOrDefault(type);
        }

        private static void TryGenerateTypeToMonoScriptCache()
        {
            if (typeToMonoScriptCache != null)
                return;

            typeToMonoScriptCache = new Dictionary<Type, MonoScript>();
            var monoScripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            foreach (var monoScript in monoScripts)
            {
                var monoScriptType = monoScript.GetClass();
                if (monoScriptType == null)
                    continue;

                typeToMonoScriptCache[monoScriptType] = monoScript;
            }
        }
    }
}