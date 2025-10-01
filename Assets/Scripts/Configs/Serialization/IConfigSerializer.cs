using System;
using Configs.Schemes;

namespace Configs
{
    public interface IConfigSerializer
    {
        IGameConfigScheme Deserialize(string content, Type type);

        string Serialize(IGameConfigScheme configScheme);
    }
}