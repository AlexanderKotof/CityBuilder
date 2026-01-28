using System;
using System.Collections.Generic;
using Configs.Scriptable;

namespace GameSystems.Implementation.BattleSystem
{
    public class InvasionData
    {
        public List<(BattleUnitConfigSO config, int Amount)> Invaders = new ();
    }
}