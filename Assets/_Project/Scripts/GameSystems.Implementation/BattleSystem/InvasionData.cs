using System.Collections.Generic;
using CityBuilder.Configs.Scriptable.Battle;

namespace CityBuilder.GameSystems.Implementation.BattleSystem
{
    public class InvasionData
    {
        public List<(BattleUnitConfigSO config, int Amount)> Invaders = new ();
    }
}