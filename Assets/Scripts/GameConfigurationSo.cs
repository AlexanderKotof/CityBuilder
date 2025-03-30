using CityBuilder.BuildingSystem;
using ResourcesSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "GameConfiguration")]
public class GameConfigurationSo : ScriptableObject
{
    public BuildingsConfigSo BuildingsConfig;
    public ResourcesConfigurationSO ResourcesConfig;
}