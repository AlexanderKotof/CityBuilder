using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configs
{
    public interface IConfigLoader
    {
        Task<IReadOnlyCollection<(Type, string)>> LoadConfigs();
    }
}