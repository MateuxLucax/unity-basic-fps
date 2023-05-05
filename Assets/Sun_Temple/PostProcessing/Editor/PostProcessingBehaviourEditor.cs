using System;
using System.Linq.Expressions;
using Sun_Temple.PostProcessing.Editor.Utils;
using Sun_Temple.PostProcessing.Runtime;
using UnityEditor;

namespace Sun_Temple.PostProcessing.Editor
{
    [CustomEditor(typeof(PostProcessingBehaviour))]
    public class PostProcessingBehaviourEditor : UnityEditor.Editor
    {
        SerializedProperty m_Profile;

        public void OnEnable()
        {
            m_Profile = FindSetting((PostProcessingBehaviour x) => x.profile);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Profile);

            serializedObject.ApplyModifiedProperties();
        }

        SerializedProperty FindSetting<T, TValue>(Expression<Func<T, TValue>> expr)
        {
            return serializedObject.FindProperty(ReflectionUtils.GetFieldPath(expr));
        }
    }
}
