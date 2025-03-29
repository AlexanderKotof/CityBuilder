using UnityEditor;
using UnityEngine;

namespace Editor
{
    // [CustomPropertyDrawer(typeof(ComponentCollection))]
    // public class ComponentCollectionPropertyCustomDrawer : PropertyDrawer
    // {
    //     private const string CollectionPropertyPath = "_components";
    //    
    //     // // Reserve space for the label and calculate new position
    //     // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     // {
    //     //     Rect dragArea = new Rect(position.x, position.y, position.width, 25);
    //     //     
    //     //     position = EditorGUI.PrefixLabel(position, label);
    //     //
    //     //     var collectionProperty = property.FindPropertyRelative(CollectionPropertyPath);
    //     //     // Get the size of the list
    //     //     SerializedProperty arraySize = collectionProperty.FindPropertyRelative("Array.size");
    //     //
    //     //     EditorGUI.BeginProperty(position, label, property);
    //     //     
    //     //     // Calculate the height for each element
    //     //     float lineHeight = EditorGUIUtility.singleLineHeight;
    //     //     float padding = 2f; // Adjust padding between items if necessary
    //     //     float totalHeight = arraySize.intValue * (lineHeight + padding) + lineHeight;
    //     //     
    //     //     // Create a scrollable area for the list if necessary
    //     //     position.height = totalHeight;
    //     //
    //     //     // Draw the size property first (for the array size)
    //     //     EditorGUI.PropertyField(position, arraySize, true);
    //     //     position.y += lineHeight;
    //     //     
    //     //     EditorGUI.indentLevel++;
    //     //     
    //     //     for (int i = 0; i < arraySize.intValue; i++)
    //     //     {
    //     //         SerializedProperty element = collectionProperty.GetArrayElementAtIndex(i);
    //     //
    //     //         Rect elementPosition = new Rect(0, position.y + (lineHeight + padding) * i,  position.x + position.width, lineHeight);
    //     //         // Draw each component element in the collection
    //     //         EditorGUI.PropertyField(elementPosition, element, new GUIContent("Element " + i));
    //     //     }
    //     //     
    //     //     EditorGUI.indentLevel--;
    //     //     
    //     //     if (collectionProperty.isExpanded)
    //     //     {
    //     //         // Handle drag-and-drop for GameObjects
    //     //         HandleDragAndDrop(dragArea, collectionProperty);
    //     //     }
    //     //     
    //     //     EditorGUI.EndProperty();
    //     // }
    //     //
    //     // // Get the height of the list field
    //     // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //     // {
    //     //     var collectionProperty = property.FindPropertyRelative(CollectionPropertyPath);
    //     //     // Get the size of the list
    //     //     SerializedProperty arrayProp = collectionProperty.FindPropertyRelative("Array.size");
    //     //
    //     //     // Calculate the total height based on number of elements
    //     //     float lineHeight = EditorGUIUtility.singleLineHeight;
    //     //     float padding = 2f; // Adjust padding between items if necessary
    //     //
    //     //     return arrayProp.intValue * (lineHeight + padding) + lineHeight; // Add extra space for size field
    //     // }
    //
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         var collectionProperty = property.FindPropertyRelative(CollectionPropertyPath);
    //         EditorGUI.PropertyField(position, collectionProperty, label);
    //     }
    //     
    //     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //     {
    //         var collectionProperty = property.FindPropertyRelative(CollectionPropertyPath);
    //         // Get the size of the list
    //         SerializedProperty arrayProp = collectionProperty.FindPropertyRelative("Array.size");
    //     
    //         // Calculate the total height based on number of elements
    //         float lineHeight = EditorGUIUtility.singleLineHeight;
    //         float padding = 2f; // Adjust padding between items if necessary
    //     
    //         return arrayProp.intValue * (lineHeight + padding) + lineHeight * 4; // Add extra space for size field
    //     }
    //
    //     private void HandleDragAndDrop(Rect dragArea, SerializedProperty property)
    //     {
    //         Event currentEvent = Event.current;
    //
    //         if (currentEvent.type == EventType.Layout || currentEvent.type == EventType.Repaint)
    //         {
    //             return;
    //         }
    //         
    //         Debug.Log($"[{nameof(ComponentCollection)}] Event type {currentEvent.type} ");
    //         if (currentEvent.type == EventType.ExecuteCommand || currentEvent.type == EventType.DragPerform || currentEvent.type == EventType.DragExited)
    //         {
    //             if (dragArea.Contains(currentEvent.mousePosition) == false)
    //             {
    //                 Debug.Log($"[{nameof(ComponentCollection)}] Not in the drag area. Drag area {dragArea} and position {currentEvent.mousePosition}.");
    //                 return;
    //             }
    //             
    //             bool isDragValid = DragAndDrop.objectReferences.Length > 0 &&
    //                                DragAndDrop.objectReferences[0] is GameObject;
    //             // Check if the dragged object is a GameObject
    //             if (isDragValid)
    //             {
    //                 // Change the visual appearance to show valid drag target
    //                 DragAndDrop.visualMode = DragAndDropVisualMode.Link;
    //                 OnDragPerformed(property, DragAndDrop.objectReferences[0]);
    //                 
    //                 currentEvent.Use(); // Consume the event
    //             }
    //             else
    //             {
    //                 // Change the visual appearance if the drag is not valid
    //                 DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    //                 Debug.LogError($"[{nameof(ComponentCollection)}] Drag is invalid!");
    //             }
    //         }
    //     }
    //
    //     private static void OnDragPerformed(SerializedProperty property, Object draggedObject)
    //     {
    //         Debug.Log($"[{nameof(ComponentCollection)}] Drag performed!");
    //         SelectComponentPopupEditorWindow.Show(draggedObject, (component) => OnOptionSelected(property, component));
    //     }
    //
    //     private static void OnOptionSelected(SerializedProperty property, Component selectedComponent)
    //     {
    //         Debug.Log($"[{nameof(ComponentCollection)}] Option selected!");
    //         property.InsertArrayElementAtIndex(property.arraySize);
    //         property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = selectedComponent;
    //         property.serializedObject.ApplyModifiedProperties();
    //     }
    // }
}