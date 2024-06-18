using UnityEditor;
using UnityEditor.UI;

namespace RecycableScroll
{
    [CustomEditor(typeof(RecycableScrollRect))]
    public class RecycableScrollRectEditor : ScrollRectEditor
    {
        private SerializedProperty _layoutSettings;
        private SerializedProperty _viewBorders;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _layoutSettings = serializedObject.FindProperty("_layoutSettings");
            _viewBorders = serializedObject.FindProperty("_viewBorders");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_layoutSettings);
            EditorGUILayout.PropertyField(_viewBorders);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}