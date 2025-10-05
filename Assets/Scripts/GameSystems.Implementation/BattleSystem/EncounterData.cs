using System;
using System.Collections.Generic;

namespace GameSystems.Implementation.BattleSystem
{
    public class EncounterData
    {
        public List<(Guid UnitConfigId, int Amount)> Encounters = new ();
    }
}