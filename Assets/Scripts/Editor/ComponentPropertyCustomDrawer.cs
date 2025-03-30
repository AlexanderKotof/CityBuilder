using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Component))]
    public class ComponentPropertyCustomDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the default property field for GameObject
            EditorGUI.PropertyField(position, property, label);

            // Set up the area where drag-and-drop detection will occur
            Rect dragArea = new Rect(position.x, position.y, position.width, position.height);
        
            // Handle drag-and-drop for GameObjects
            HandleDragAndDrop(dragArea, property);
        }

        private void HandleDragAndDrop(Rect dragArea, SerializedProperty property)
        {
            Event currentEvent = Event.current;
            
            if (currentEvent.type is EventType.Layout or EventType.Repaint)
            {
                return;
            }
            
            //Debug.Log($"[{nameof(Component)}] Event type {currentEvent.type}");
            
            if (currentEvent.type == EventType.DragExited || currentEvent.type == EventType.DragPerform)
            {
                if (dragArea.Contains(currentEvent.mousePosition) == false)
                {
                    //Debug.Log($"[{nameof(Component)}] Not in the drag area. Drag area {dragArea} and position {currentEvent.mousePosition}.");
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
                    //Debug.LogError($"[{nameof(Component)}] Drag is invalid!");

                }
            }
        }

        private static void OnDragPerformed(SerializedProperty property, Object draggedObject)
        {
            Debug.Log($"[{nameof(Component)}] Drag performed!");
            SelectComponentPopupEditorWindow.Show(draggedObject, (component) => OnOptionSelected(property, component));
        }

        private static void OnOptionSelected(SerializedProperty property, Component selectedComponent)
        {
            Debug.Log($"[{nameof(Component)}] Option selected!");
            property.objectReferenceValue = selectedComponent;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}