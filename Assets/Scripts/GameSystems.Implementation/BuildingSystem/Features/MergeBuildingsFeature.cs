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
        private readonly BuildingFactory _buildingFactory;

        private IEnumerable<MergeBuildingsRecipeSo> MergeRecipes => _configuration.MergeRecipes;

        private bool _isInProcess;

        public MergeBuildingsFeature(BuildingManager buildingsManager, BuildingsModel buildingsModel, MergeFeatureConfigurationSo configuration, BuildingsViewFeature viewFeature, BuildingFactory buildingFactory)
        {
            _buildingsManager = buildingsManager;
            _buildingsModel = buildingsModel;
            _configuration = configuration;
            _viewFeature = viewFeature;
            _buildingFactory = buildingFactory;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
        
        public bool TryMergeBuildingsFromTo(BuildingModel fromBuilding, BuildingModel toBuilding)
        {
            if (_isInProcess)
                return false;
            
            if (CanLevelUpMerge(toBuilding, fromBuilding, out var buildingModels))
            {
                ProcessMergeUpgrade(toBuilding, buildingModels).Forget();
                return true;
            }

            if (CanRecipeMerge(toBuilding, fromBuilding, out var recipe, out var involvedBuildings))
            {           
                ProcessMergeWithRecipe(toBuilding, involvedBuildings, recipe).Forget();
                return true;
            }

            return false;
        }

        private async UniTaskVoid ProcessMergeUpgrade(BuildingModel toBuilding, IEnumerable<BuildingModel> buildingModels)
        {
            _isInProcess = true;
            
            var array = buildingModels.Select(building => MergeBuildingsTo(building, toBuilding));
            await UniTask.WhenAll(array);
                
            _buildingsManager.IncreaseBuildingLevel(toBuilding);

            _isInProcess = false;
                
            Debug.Log($"Building level upgraded to {toBuilding.Level}");
        }
        
        private async UniTaskVoid ProcessMergeWithRecipe(BuildingModel toBuilding,
            IEnumerable<BuildingModel> buildingModels,
            MergeBuildingsRecipeSo recipe)
        {
            _isInProcess = true;
            
            var array = buildingModels.Select(building => MergeBuildingsTo(building, toBuilding));
            await UniTask.WhenAll(array);
            
            var position = toBuilding.OccupiedCells.First();
            _buildingsModel.RemoveBuilding(toBuilding);

            var newBuilding = _buildingFactory.Create(recipe, position);
            _buildingsModel.AddBuilding(newBuilding, position);

            _isInProcess = false;
            Debug.LogError("Buildings merge with recipe", recipe);
        }

        private async UniTask MergeBuildingsTo(BuildingModel buildingModel, BuildingModel toBuilding)
        {
            var view = _viewFeature.GetBuildingView(buildingModel);
            if (view != null)
            {
                await view.MergeTo(toBuilding.WorldPosition.Value);
                await UniTask.Yield();
            }
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

            var suggestedRecipes = MergeRecipes.Where(recipe =>
                recipe.RequiredBuildings.Contains(toBuilding.Config) &&
                recipe.RequiredBuildings.Contains(fromBuilding.Config)).ToArray();

            if (suggestedRecipes.Length == 0)
            {
                Debug.LogError("No recipes found for merge");
                return false;
            }
            
            // Берем все клетки вокруг таргет здания
            var nearCells = toBuilding.GetAllNearCellsExceptOwn();

            // все найденные здания в клетках, соответствующие условиям
            var allBuildingsNear = nearCells.Select(BuildingsSelector)
                .Where(b => b != null && b.IsMaxLevel && b != fromBuilding)
                .Append(fromBuilding)
                .ToArray();

            var involved = new List<BuildingModel>();
            
            // в дальнейшем можем возвращать список рецептов и зданий
            foreach (var recipe in suggestedRecipes)
            {
                var hashset = recipe.RequiredBuildings.ToHashSet();
                
                hashset.Remove(toBuilding.Config);
                
                // var involved = allBuildingsNear.Where(b => hashset.Remove(b.Config) && b == toBuilding );
                foreach (var building in allBuildingsNear)
                {
                    if (hashset.Count == 0)
                        break;

                    if (!hashset.Remove(building.Config)) 
                        continue;
                    
                    involved.Add(building);
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
                fromBuilding.Level.Value != toBuilding.Level.Value ||
                fromBuilding.IsMaxLevel)
                return false;
            
            var level = fromBuilding.Level.Value;
            var config = fromBuilding.Config;
            
            // Берем все клетки вокруг таргет здания
            var nearCells = toBuilding.GetAllNearCellsExceptOwn();
            // Собираем все аналогичные здания в округе
            var allBuildingsNear = nearCells.Select(BuildingsSelector)
                .Where(b => b != null && b.Level.Value == level && b.Config == config && b != fromBuilding).ToList();

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