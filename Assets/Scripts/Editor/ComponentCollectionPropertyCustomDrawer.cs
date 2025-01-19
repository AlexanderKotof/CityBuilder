using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ComponentCollection))]
    public class ComponentCollectionPropertyCustomDrawer : PropertyDrawer
    {
        private const string CollectionPropertyPath = "_components";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var collectionProperty = property.FindPropertyRelative(CollectionPropertyPath);
            EditorGUI.PropertyField(position, collectionProperty, label, true);
            
            if (collectionProperty.isExpanded)
            {
                // Set up the area where drag-and-drop detection will occur
                Rect dragArea = new Rect(position.x, position.y, position.width, 25);
                
                // Handle drag-and-drop for GameObjects
                HandleDragAndDrop(dragArea, collectionProperty);
            }
            
            EditorGUI.EndProperty();
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty arrayProp = property.FindPropertyRelative(CollectionPropertyPath);
            // You can make the height dynamic based on the list size, for example:
            return EditorGUI.GetPropertyHeight(arrayProp);
        }
    
        private void HandleDragAndDrop(Rect dragArea, SerializedProperty property)
        {
            Event currentEvent = Event.current;
    
            if (currentEvent.type == EventType.Layout || currentEvent.type == EventType.Repaint)
            {
                return;
            }
            
            Debug.Log($"[{nameof(ComponentCollection)}] Event type {currentEvent.type}");
            if (currentEvent.type == EventType.ExecuteCommand || currentEvent.type == EventType.DragPerform)
            {
                if (dragArea.Contains(currentEvent.mousePosition) == false)
                {
                    Debug.Log($"[{nameof(ComponentCollection)}] Not in the drag area. Drag area {dragArea} and position {currentEvent.mousePosition}.");
                    return;
                }
                
                bool isDragValid = DragAndDrop.objectReferences.Length > 0 &&
                                   DragAndDrop.objectReferences[0] is GameObject;
                // Check if the dragged object is a GameObject
                if (isDragValid)
                {
                    // Change the visual appearance to show valid drag target
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    OnDragPerformed(property, DragAndDrop.objectReferences[0]);
                    
                    currentEvent.Use(); // Consume the event
                }
                else
                {
                    // Change the visual appearance if the drag is not valid
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    Debug.LogError($"[{nameof(ComponentCollection)}] Drag is invalid!");
                }
            }
        }
    
        private static void OnDragPerformed(SerializedProperty property, Object draggedObject)
        {
            Debug.Log($"[{nameof(ComponentCollection)}] Drag performed!");
            SelectComponentPopupEditorWindow.Show(draggedObject, (component) => OnOptionSelected(property, component));
        }
    
        private static void OnOptionSelected(SerializedProperty property, Component selectedComponent)
        {
            Debug.Log($"[{nameof(ComponentCollection)}] Option selected!");
            property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = selectedComponent;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}