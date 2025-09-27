using UnityEditor;
using UnityEngine;

namespace Configs
{
    public class PathUtility
    {
        public static string AssetsPath => Application.dataPath;

        public static string ConfigsPath => AssetsPath + "/GameConfigs/";
    }
}