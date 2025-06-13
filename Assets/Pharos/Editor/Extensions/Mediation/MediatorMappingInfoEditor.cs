using Pharos.Extensions.Mediation;
using UnityEditor;

namespace PharosEditor.Extensions.Mediation
{
    [CustomEditor(typeof(MediatorMappingInfo))]
    internal class MediatorMappingInfoEditor : Editor
    {
        private MediatorMappingInfo mediatorMappingInfo;

        public override void OnInspectorGUI()
        {
            var viewTypeName = mediatorMappingInfo?.ViewType.Name;
            EditorGUILayout.LabelField("View Type", viewTypeName);

            var mediatorType = mediatorMappingInfo?.MediatorType;
            if (mediatorType != null)
            {
                var monoScript = MonoScriptCacher.GetMonoScript(mediatorType);
                if (monoScript)
                {
                    EditorGUILayout.ObjectField("Mediator", monoScript, typeof(MonoScript), false);
                }
                else
                {
                    EditorGUILayout.LabelField ("Mediator Type", mediatorType.Name);
                }
            }
        }

        private void OnEnable()
        {
            if (!mediatorMappingInfo)
                mediatorMappingInfo = target as MediatorMappingInfo;
        }
    }
}
