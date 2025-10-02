using CityBuilder.BuildingSystem;
using ResourcesSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "GameConfiguration")]
public class GameConfigurationSo : ScriptableObject
{
    public ResourcesConfigurationSO ResourcesConfig;
    public LayerMask InteractionRaycastLayerMask;
    public string SelectionAssetReferenceKey;
}