using Configs.Implementation.Common;
using GameSystems.Implementation.GameTime;
using GameSystems.Implementation.PopulationFeature;
using ResourcesSystem;
using UnityEngine;
using VContainer;

namespace Installers
{
    public class DebugWindow : MonoBehaviour
    {
        [Inject] private DateModel _dateModel;
        [Inject] private PopulationModel _populationModel;
        [Inject] private PlayerResourcesModel _storage;

        private bool Initialized => _populationModel != null;
        private void OnGUI()
        {
            int MaxIndex = 5;
    
            if (!Initialized)
            {
                return;
            }
        
            for (int i = 0; i < MaxIndex; i++)
            {
                var resourceType = (ResourceType)i;
                var amount = _storage.GetResourceAmount(resourceType);
                GUI.Label(new Rect(20 + 50 * i, 20, 50, 50), new GUIContent($"{resourceType.ToString()}:\n{amount}"));
            }
            
            if (_dateModel != null)
            {
                GUI.Label(new Rect(20, 70, 1000, 50), new GUIContent(_dateModel.ToString()));
            }
            
            if (_populationModel != null)
            {
                GUI.Label(new Rect(20, 100, 100, 50),
                    new GUIContent($"Population: {_populationModel.CurrentPopulation.Value.ToString()} / {_populationModel.AvailableHouseholds.Value.ToString()} houses"));
            }
        }
    }
}