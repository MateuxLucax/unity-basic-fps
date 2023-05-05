using Sun_Temple.PostProcessing.Editor.Attributes;
using Sun_Temple.PostProcessing.Runtime.Models;
using UnityEditor;

namespace Sun_Temple.PostProcessing.Editor.Models
{
    [PostProcessingModelEditor(typeof(DitheringModel))]
    public class DitheringModelEditor : PostProcessingModelEditor
    {
        public override void OnInspectorGUI()
        {
            if (profile.grain.enabled && target.enabled)
                EditorGUILayout.HelpBox("Grain is enabled, you probably don't need dithering !", MessageType.Warning);
            else
                EditorGUILayout.HelpBox("Nothing to configure !", MessageType.Info);
        }
    }
}
