using System;

namespace Configs.Schemes
{
    public interface IGameConfigPiece : IConfigBase
    {
        Guid Id { get; set; }
    }

    public class ConfigBase : IGameConfigPiece
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}