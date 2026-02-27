using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CityBuilder.Editor
{
     public static class ScriptGuidFixer
    {
        private static readonly string[] ASSET_SEARCH_FILTER = { "t:Prefab", "t:ScriptableObject" };
        
        private static readonly Regex ScriptReferenceRegex = new Regex(
            @"m_Script:\s*\{fileID:\s*11500000,\s*guid:\s*[0-9a-f]{32},\s*type:\s*3\}(\s*#.*)?",
            RegexOptions.Compiled | RegexOptions.Multiline
        );
        
        private static readonly Regex EditorClassIdentifierRegex = new Regex(
            @"m_EditorClassIdentifier:\s*[^:]+::(?:.+\.)?(\w+)",
            RegexOptions.Compiled
        );
        
        private static readonly Regex GuidExtractRegex = new Regex(
            @"guid:\s*([0-9a-f]{32})",
            RegexOptions.Compiled
        );
        
        private static readonly Regex CommentExtractRegex = new Regex(
            @"#\s*(\w+)(?:\.cs)?\s*$",
            RegexOptions.Compiled
        );

        [MenuItem("Tools/Fix Broken Script GUIDs...", false, 1000)]
        public static void ShowFixWindow()
        {
            if (EditorUtility.DisplayDialog(
                "Fix Broken Script GUIDs",
                "This tool will:\n" +
                "1. Scan all C# scripts and build mapping ScriptName → GUID\n" +
                "2. Find broken script references in prefabs & ScriptableObjects\n" +
                "3. Replace invalid GUIDs using script names from YAML comments\n\n" +
                "⚠️ MAKE SURE TO COMMIT YOUR CHANGES TO VERSION CONTROL FIRST!\n" +
                "A backup folder 'Assets/Backup_ScriptFix_' will be created.",
                "Fix Scripts",
                "Cancel"))
            {
                FixBrokenScriptGuids();
            }
        }

        private static void FixBrokenScriptGuids()
        {
            // string backupPath = $"Assets/Backup_ScriptFix_{DateTime.Now:yyyyMMdd_HHmmss}";
            // Directory.CreateDirectory(backupPath);
            // Debug.Log($"📁 Created backup folder: {backupPath}");

            // Шаг 1: Собираем маппинг имя скрипта → GUID
            var scriptGuidMap = BuildScriptGuidMap();
            if (scriptGuidMap.Count == 0)
            {
                Debug.LogError("❌ No scripts found! Check Assets folder structure.");
                return;
            }
            Debug.Log($"✅ Found {scriptGuidMap.Count} valid scripts \n" +
                      string.Join(",\n ", scriptGuidMap.Keys.ToArray()));

            // Шаг 2: Находим все ассеты для обработки
            string[] assetPaths = AssetDatabase.GetAllAssetPaths()
                .Where(path => path.EndsWith(".prefab") || path.EndsWith(".asset"))
                .Where(path => path.Contains("_Project/Configs") || path.Contains("_Project/Prefabs"))
                .ToArray();

            Debug.Log($"🔍 Found {assetPaths.Length} assets to process");

            int totalFixed = 0;
            int assetsModified = 0;

            // Шаг 3: Обрабатываем каждый ассет
            foreach (string assetPath in assetPaths)
            {
                string fullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), assetPath);
                string content = File.ReadAllText(fullPath);
                bool wasModified = false;
                int fixesInAsset = 0;

                // Ищем все ссылки на скрипты
                var scriptGuidMatches = ScriptReferenceRegex.Matches(content).ToList();

                var scriptsNamesMatches = EditorClassIdentifierRegex.Matches(content);
                
                Debug.Log($"Processing {Path.GetFileNameWithoutExtension(fullPath)} : Script Guids {scriptGuidMatches.Count}, Script Names {scriptsNamesMatches.Count}");
                
                foreach (Match match in scriptsNamesMatches)
                {
                    if (scriptGuidMatches.Any() == false)
                    {
                        Debug.LogError($"No more scripts matching '{match.Value}'");
                        break;
                    }

                    string scriptName = match.Groups[1].Value;
                    var thisIndex = match.Index;
                    
                    var associatedGuid = scriptGuidMatches.LastOrDefault(m => m.Index < thisIndex);
                    if (associatedGuid == null)
                    {
                        Debug.LogError($"Not found script guid for {scriptName}");
                        break;
                    }
                    scriptGuidMatches.Remove(associatedGuid);
                    
                    //Debug.Log($"Script name: {scriptName}, associated GUID: {associatedGuid}");
                    
                    string line = associatedGuid.Value;
                    
                    // Находим правильный GUID по имени
                    if (scriptGuidMap.TryGetValue(scriptName.ToLowerInvariant(), out string correctGuid) == false)
                    {
                        Debug.LogError($"🔧 {scriptName} guid not found...");
                        continue;
                    }
                    
                    // Извлекаем текущий GUID
                    var guidMatch = GuidExtractRegex.Match(line);
                    if (guidMatch.Success)
                    {
                        string currentGuid = guidMatch.Groups[1].Value;
                            
                        // Если GUID не совпадает — заменяем
                        if (currentGuid != correctGuid)
                        {
                            string fixedLine = line.Replace(
                                $"guid: {currentGuid}",
                                $"guid: {correctGuid}"
                            );
                                
                            content = content.Replace(line, fixedLine);
                            wasModified = true;
                            fixesInAsset++;
                            totalFixed++;
                                
                            Debug.Log($"🔧 Fixed in {Path.GetFileNameWithoutExtension(fullPath)}: {scriptName} ({currentGuid} → {correctGuid})");
                        }
                    }
                    else
                    {
                        Debug.LogError($"🔧 cannot replace guid...");
                    }
                }

                // Сохраняем изменения
                if (wasModified)
                {
                    File.WriteAllText(fullPath, content);
                    assetsModified++;
                    Debug.Log($"✅ Modified {assetPath} ({fixesInAsset} fixes)");
                }
            }

            // Обновляем базу ассетов
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog(
                "Fix Complete",
                $"Processed {assetPaths.Length} assets\n" +
                $"Modified {assetsModified} files\n" +
                $"Fixed {totalFixed} broken script references\n\n",
                "OK"
            );
            
            Debug.Log($"✨ DONE! Fixed {totalFixed} references in {assetsModified} assets.");
        }

        private static Dictionary<string, string> BuildScriptGuidMap()
        {
            var map = new Dictionary<string, string>();
            string[] scriptPaths = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            
            foreach (string scriptPath in scriptPaths)
            {
                // Пропускаем папку Editor если нужно (но обычно скрипты там тоже нужны)
                if (scriptPath.Contains("/Plugins/") || scriptPath.Contains("\\Plugins\\")) 
                    continue;
                
                string metaPath = scriptPath + ".meta";
                if (!File.Exists(metaPath)) continue;
                
                string metaContent = File.ReadAllText(metaPath);
                var guidMatch = GuidExtractRegex.Match(metaContent);
                
                if (guidMatch.Success)
                {
                    string guid = guidMatch.Groups[1].Value;
                    string scriptName = Path.GetFileNameWithoutExtension(scriptPath);
                    
                    if (map.TryAdd(scriptName.ToLowerInvariant(), guid) == false)
                    {
                        Debug.LogError($"Script with name {scriptName} already exists!");
                    }
                }
            }
            
            return map;
        }
    }
}