using System;
using System.Collections.Generic;

namespace GameSystems.Implementation.BattleSystem
{
    public class InvasionData
    {
        public List<(Guid UnitConfigId, int Amount)> Invaders = new ();
    }
}