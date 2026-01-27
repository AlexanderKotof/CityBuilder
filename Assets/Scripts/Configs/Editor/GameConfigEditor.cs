using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Configs.Editor;
using Configs.Schemes;
using Configs.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Configs
{
    // public class GameConfigEditor : EditorWindow
    // {
    //     private List<Type> configTypes;
    //     private int selectedConfigIndex = -1;
    //     private object currentConfigInstance;
    //     private string currentConfigJson;
    //     private Vector2 scrollPos;
    //
    //     [MenuItem("Tools/Game Config Editor")]
    //     public static void ShowWindow()
    //     {
    //         GetWindow<GameConfigEditor>("Game Configs");
    //     }
    //
    //     private void OnEnable()
    //     {
    //         LoadConfigTypes();
    //     }
    //
    //     private void LoadConfigTypes()
    //     {
    //         configTypes = new List<Type>();
    //         var allTypes = AppDomain.CurrentDomain.GetAssemblies()
    //             .SelectMany(a => a.GetTypes())
    //             .Where(t => typeof(IGameConfigScheme).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    //             .ToList();
    //
    //         configTypes.AddRange(allTypes);
    //         if (configTypes.Count > 0)
    //         {
    //             selectedConfigIndex = 0;
    //             LoadSelectedConfig();
    //         }
    //     }
    //
    //     private void LoadSelectedConfig()
    //     {
    //         if (selectedConfigIndex < 0 || selectedConfigIndex >= configTypes.Count)
    //             return;
    //
    //         Type type = configTypes[selectedConfigIndex];
    //         string path = GetConfigPath(type);
    //
    //         if (File.Exists(path))
    //         {
    //             currentConfigJson = File.ReadAllText(path);
    //             try
    //             {
    //                 currentConfigInstance = JsonUtility.FromJson(currentConfigJson, type);
    //             }
    //             catch (Exception e)
    //             {
    //                 Debug.LogError($"Failed to parse config {type.Name}: {e.Message}");
    //                 currentConfigInstance = Activator.CreateInstance(type);
    //             }
    //         }
    //         else
    //         {
    //             // Создаём новый экземпляр по умолчанию
    //             currentConfigInstance = Activator.CreateInstance(type);
    //             SaveCurrentConfig(); // сразу сохраняем как шаблон
    //         }
    //     }
    //
    //     private string GetConfigPath(Type type)
    //     {
    //         return type.GetConfigPath();
    //     }
    //
    //     private void SaveCurrentConfig()
    //     {
    //         if (currentConfigInstance == null) return;
    //
    //         string json = JsonUtility.ToJson(currentConfigInstance, true);
    //         string path = GetConfigPath(currentConfigInstance.GetType());
    //
    //         // Убедимся, что директория существует
    //         Directory.CreateDirectory(Path.GetDirectoryName(path));
    //
    //         File.WriteAllText(path, json);
    //         currentConfigJson = json;
    //         Debug.Log($"Saved config to {path}");
    //     }
    //
    //     private void OnGUI()
    //     {
    //         if (configTypes == null || configTypes.Count == 0)
    //         {
    //             EditorGUILayout.HelpBox("No IGameConfigScheme implementations found.", MessageType.Warning);
    //             return;
    //         }
    //
    //         EditorGUILayout.BeginHorizontal();
    //         {
    //             if (GUILayout.Button("Reload Types"))
    //                 LoadConfigTypes();
    //
    //             if (GUILayout.Button("Refresh Config"))
    //                 LoadSelectedConfig();
    //         }
    //         EditorGUILayout.EndHorizontal();
    //
    //         EditorGUILayout.Space();
    //
    //         // Выбор конфига
    //         string[] options = configTypes.Select(t => t.Name).ToArray();
    //         int newIndex = EditorGUILayout.Popup("Config", selectedConfigIndex, options);
    //         if (newIndex != selectedConfigIndex)
    //         {
    //             selectedConfigIndex = newIndex;
    //             LoadSelectedConfig();
    //         }
    //
    //         if (currentConfigInstance == null)
    //         {
    //             EditorGUILayout.HelpBox("Config not loaded.", MessageType.Error);
    //             return;
    //         }
    //
    //         EditorGUILayout.Space();
    //
    //         // Редактирование полей через SerializedObject
    //         SerializedObject serializedObject = new SerializedObject(currentConfigInstance);
    //         scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true); // скрыть, если нужно
    //         SerializedProperty iterator = serializedObject.GetIterator();
    //         bool enterChildren = true;
    //         while (iterator.NextVisible(enterChildren))
    //         {
    //             enterChildren = false;
    //             // Пропускаем служебные поля Unity (например, m_Script)
    //             if (iterator.name == "m_Script") continue;
    //             EditorGUILayout.PropertyField(iterator, true);
    //         }
    //         EditorGUILayout.EndScrollView();
    //
    //         serializedObject.ApplyModifiedProperties();
    //
    //         EditorGUILayout.Space();
    //
    //         if (GUILayout.Button("Save Config"))
    //         {
    //             SaveCurrentConfig();
    //         }
    //
    //         // Опционально: показать JSON
    //         if (GUILayout.Button("Show JSON"))
    //         {
    //             EditorUtility.DisplayDialog("Config JSON", currentConfigJson, "OK");
    //         }
    //     }
    // }
}