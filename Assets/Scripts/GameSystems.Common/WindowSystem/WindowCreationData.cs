namespace GameSystems.Common.WindowSystem
{
    public struct WindowCreationData
    {
        public WindowCreationData(string assetKey, int layer)
        {
            AssetKey = assetKey;
            Layer = layer;
        }

        public int Layer { get; set; }
        public string AssetKey { get; set; }
    }
}