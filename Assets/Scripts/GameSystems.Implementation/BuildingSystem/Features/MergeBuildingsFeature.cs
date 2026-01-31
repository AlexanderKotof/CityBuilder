using System;
using System.Collections.Generic;
using System.Linq;
using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using Cysharp.Threading.Tasks;
using GameSystems.Implementation.BuildingSystem.Domain;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace GameSystems.Implementation.BuildingSystem.Features
{
    public class MergeBuildingsFeature : IInitializable, IDisposable
    {
        private readonly BuildingManager _buildingsManager;
        private readonly BuildingsModel _buildingsModel;
        private readonly MergeFeatureConfigurationSo _configuration;
        private readonly BuildingsViewFeature _viewFeature;

        private IEnumerable<MergeBuildingsRecipeSo> MergeRecipes => _configuration.MergeRecipes;

        public MergeBuildingsFeature(BuildingManager buildingsManager, BuildingsModel buildingsModel, MergeFeatureConfigurationSo configuration, BuildingsViewFeature viewFeature)
        {
            _buildingsManager = buildingsManager;
            _buildingsModel = buildingsModel;
            _configuration = configuration;
            _viewFeature = viewFeature;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
        
        public bool TryMergeBuildingsFromTo(BuildingModel fromBuilding, BuildingModel toBuilding)
        {
            if (CanLevelUpMerge(toBuilding, fromBuilding, out var buildingModels))
            {
                foreach (var building in buildingModels)
                {
                    MergeBuildingsTo(building, toBuilding);
                }
                
                _buildingsManager.IncreaseBuildingLevel(toBuilding);
                
                Debug.Log($"Building level upgraded to {toBuilding.Level}");

                return true;
            }

            if (CanRecipeMerge(toBuilding, fromBuilding, out var recipe, out var involvedBuildings))
            {           
                //TODO: implement merge mechanics
                
                Debug.LogError("Found recipe for merge", recipe);
                
                
            }

            return false;
        }

        private async UniTask MergeBuildingsTo(BuildingModel buildingModel, BuildingModel toBuilding)
        {
            //TODO: play animation
            var view = _viewFeature.GetBuildingView(buildingModel);

            await view.MergeTo(toBuilding.WorldPosition.Value);
            
            _buildingsModel.RemoveBuilding(buildingModel);
        }

        private bool CanRecipeMerge(BuildingModel toBuilding, BuildingModel fromBuilding, [CanBeNull] out MergeBuildingsRecipeSo recipeSo, [CanBeNull] out IEnumerable<BuildingModel> buildingsInvolved)
        {
            recipeSo = null;
            buildingsInvolved = null;
            
            if (toBuilding.IsMaxLevel == false || fromBuilding.IsMaxLevel == false)
            {
                Debug.LogError("No max level reached");
                return false;
            }
            
            //TODO: перетаскиваемое здание может стоять на соседней клетке...
            
            // Берем все клетки вокруг таргет здания
            var nearCells = toBuilding.GetAllNearCellsExceptOwn();

            if (MergeRecipes.Any(recipe =>
                    recipe.RequiredBuildings.Contains(toBuilding.Config) &&
                    recipe.RequiredBuildings.Contains(fromBuilding.Config)) == false)
            {
                Debug.LogError("No recipes found for merge");
                return false;
            }

            // все найденные здания в клетках, соответствующие условиям
            var allBuildingsNear = nearCells.Select(BuildingsSelector)
                .Where(b => b != null && b.IsMaxLevel)
                .Append(fromBuilding)
                .Append(toBuilding)
                .ToArray();
            
            var involved = new List<BuildingModel>()
            {
                toBuilding,
                fromBuilding,
            };
            
            // в дальнейшем можем возвращать список рецептов и зданий
            foreach (var recipe in MergeRecipes)
            {
                var hashset = recipe.RequiredBuildings.ToHashSet();
                foreach (var building in allBuildingsNear)
                {
                    if (hashset.Count == 0)
                        break;

                    if (hashset.Remove(building.Config))
                    {
                        involved.Add(building);
                    }
                }

                if (hashset.Count == 0)
                {
                    recipeSo = recipe;
                    buildingsInvolved = involved;
                    return true;
                }
            }
            
            return false;
        }

        private BuildingModel BuildingsSelector(CellModel cell)
        {
            if (cell.Content.Value != null && _buildingsManager.TryGetBuilding(cell, out var building))
            {
                return building;
            }

            return null;
        }

        private bool CanLevelUpMerge(BuildingModel toBuilding, BuildingModel fromBuilding, out IEnumerable<BuildingModel> buildingsInvolved)
        {
            buildingsInvolved = null;
            
            if (Equals(fromBuilding.Config, toBuilding.Config) == false ||
                fromBuilding.Level.Value != toBuilding.Level.Value)
                return false;
            
            var level = fromBuilding.Level.Value;
            var config = fromBuilding.Config;
            
            // Берем все клетки вокруг таргет здания
            var nearCells = toBuilding.GetAllNearCellsExceptOwn();
            // Собираем все аналогичные здания в округе
            var allBuildingsNear = nearCells.Select(BuildingsSelector)
                .Where(b => b != null && b.Level.Value == level && b.Config == config).ToList();

            if (allBuildingsNear.Count == 0)
            {
                return false;
            }

            // TODO: count buildings and make multi merge ?
            buildingsInvolved = allBuildingsNear.Take(1).Append(fromBuilding);
            return true;
        }
    }
}