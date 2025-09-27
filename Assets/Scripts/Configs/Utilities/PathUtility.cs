using UnityEngine;

namespace Configs.Utilities
{
    public class PathUtility
    {
        public static string AssetsPath => Application.dataPath;

        public static string ConfigsPath => AssetsPath + "/GameConfigs/";
    }
}