using System;
using System.Collections.Generic;
using Configs.Scriptable;
using Configs.Scriptable.Battle;

namespace GameSystems.Implementation.BattleSystem
{
    public class InvasionData
    {
        public List<(BattleUnitConfigSO config, int Amount)> Invaders = new ();
    }
}