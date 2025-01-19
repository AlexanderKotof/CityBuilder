using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    public class SelectComponentPopupEditorWindow : EditorWindow
    {
        float _sizeX = 200;
        float _sizeY = 20;
        
        private Component[] _components;
        private Action<Component> _onComponentSelected;

        public static void Show(Object selectedObject, Action<Component> onComponentSelected)
        {
            SelectComponentPopupEditorWindow popup = CreateInstance<SelectComponentPopupEditorWindow>();

            popup.Init(selectedObject, onComponentSelected);
        }

        private void Init(Object selectedObject, Action<Component> onComponentSelected)
        {
            _components = selectedObject.GetComponents<Component>();
            if (_components.Length == 0)
            {
                this.Close();
                return;
            }

            if (_components.Length == 1)
            {
                onComponentSelected.Invoke(_components[0]);
                this.Close();
                return;
            }
            
            titleContent = new GUIContent("Select Component");
            _onComponentSelected = onComponentSelected;
            
            this.ShowUtility();
        }

        private void OnGUI()
        {
            minSize = new Vector2(_sizeX, _components.Length * _sizeY + 5);
            maxSize = new Vector2(_sizeX, _components.Length * _sizeY + 5);
            
            foreach (var component in _components)
            {
                if (GUILayout.Button(component.GetType().Name))
                {
                    _onComponentSelected?.Invoke(component);
                    this.Close();
                }
            }
        }
    }
}