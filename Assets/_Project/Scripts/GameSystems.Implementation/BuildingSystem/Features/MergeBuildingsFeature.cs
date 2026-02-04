using System.Collections.Generic;
using System.Linq;
using CityBuilder.Configs.Scriptable.Buildings.Merge;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Extensions;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BuildingSystem.Features
{
    public class MergeBuildingsFeature
    {
        private readonly BuildingManager _buildingsManager;
        private readonly BuildingsModel _buildingsModel;
        private readonly MergeFeatureConfigurationSo _configuration;
        private readonly BuildingsViewFeature _viewFeature;
        private readonly BuildingFactory _buildingFactory;
        private readonly PlayerActionsService _actionService;

        private IEnumerable<MergeBuildingsRecipeSo> MergeRecipes => _configuration.MergeRecipes;

        private bool _isInProcess;

        public MergeBuildingsFeature(
            BuildingManager buildingsManager,
            BuildingsModel buildingsModel,
            MergeFeatureConfigurationSo configuration,
            BuildingsViewFeature viewFeature,
            BuildingFactory buildingFactory,
            PlayerActionsService actionService)
        {
            _buildingsManager = buildingsManager;
            _buildingsModel = buildingsModel;
            _configuration = configuration;
            _viewFeature = viewFeature;
            _buildingFactory = buildingFactory;
            _actionService = actionService;
        }
        
        public void MergeWithRecipe(BuildingModel fromBuilding, BuildingModel toBuilding, MergeBuildingsRecipeSo recipe, IReadOnlyCollection<BuildingModel> involvedBuildings)
        {
            var action = new PlayerAction(() => ProcessMergeWithRecipe(fromBuilding, toBuilding, involvedBuildings, recipe), NameOf: nameof(ProcessMergeWithRecipe));
            _actionService.QueueAction(action).Forget();
        }

        public void MergeUpgrade(BuildingModel fromBuilding, BuildingModel toBuilding,
            IReadOnlyCollection<BuildingModel> involvedBuildings)
        {
            var counter = involvedBuildings.Count;
            var mergedCount = counter / _configuration.MergeBuildingsCountForMultiLevelUp;

            if (mergedCount > 0)
            {
                var action = new PlayerAction(() => ProcessMultiMergeUpgrade(fromBuilding, toBuilding, involvedBuildings),
                    NameOf: nameof(ProcessMultiMergeUpgrade));
                _actionService.QueueAction(action).Forget();
            }
            else
            {
                var action = new PlayerAction(() => ProcessSingleMergeUpgrade(fromBuilding, toBuilding, involvedBuildings),
                    NameOf: nameof(ProcessSingleMergeUpgrade));
                _actionService.QueueAction(action).Forget();
            }
        }

        private async UniTask<IResult> ProcessSingleMergeUpgrade(BuildingModel fromBuilding, BuildingModel toBuilding,
            IReadOnlyCollection<BuildingModel> buildingModels)
        {
            //TODO: validation of request
            var mergeTasks = buildingModels
                .Take(_configuration.MergeBuildingsCountForLevelUp, b => b != toBuilding)
                .Select(building => MergeBuildingsTo(building, toBuilding));
            await UniTask.WhenAll(mergeTasks);
                
            _buildingsManager.IncreaseBuildingLevel(toBuilding);
                
            Debug.Log($"Building level upgraded to {toBuilding.Level}");

            return new Success(RevertMerge);

            void RevertMerge()
            {
                
            }
        }

        private async UniTask<IResult> ProcessMultiMergeUpgrade(BuildingModel fromBuilding, BuildingModel toBuilding,
            IReadOnlyCollection<BuildingModel> buildingModels)
        {
            //TODO: validation of request
            
            var counter = buildingModels.Count;

            // выдаем по 2 апгрейда для каждого мульти мерджа
            var mergedCount = Mathf.FloorToInt((float)counter / _configuration.MergeBuildingsCountForMultiLevelUp) * 2;
            var remainder = counter % _configuration.MergeBuildingsCountForMultiLevelUp;
            
            // выдаем доп апгрейд если хватает еще на обычный мердж
            if (remainder >= _configuration.MergeBuildingsCountForLevelUp)
            {
                mergedCount += 1;
                remainder -= _configuration.MergeBuildingsCountForLevelUp;
            }

            var involved = buildingModels.ToHashSet();

            if (remainder > 0)
            {
                involved.RemoveManyWhere(remainder, b => b != fromBuilding && b != toBuilding);
            }
            
            var upgradedBuildings = involved.Take(mergedCount, b => b != fromBuilding).ToHashSet();
            involved.RemoveMany(upgradedBuildings);
            
            var mergeBuildingsTasks = involved.Select(building => MergeBuildingsTo(building, toBuilding));
            await UniTask.WhenAll(mergeBuildingsTasks);
            
            foreach (var building in upgradedBuildings)
            {
                _buildingsManager.IncreaseBuildingLevel(building);
            }
            
            return new Success(RevertMerge);
            void RevertMerge()
            {
                
            }
        }

        private async UniTask<IResult> ProcessMergeWithRecipe(BuildingModel fromBuilding, BuildingModel toBuilding,
            IReadOnlyCollection<BuildingModel> buildingModels,
            MergeBuildingsRecipeSo recipe)
        {
            //TODO: validation of request
            
            var array = buildingModels.Select(building => MergeBuildingsTo(building, toBuilding));
            await UniTask.WhenAll(array);
            
            var position = toBuilding.OccupiedCells.First();
            _buildingsModel.RemoveBuilding(toBuilding);

            var newBuilding = _buildingFactory.Create(recipe, position);
            _buildingsModel.AddBuilding(newBuilding, position);
            
            Debug.LogError("Buildings merge with recipe", recipe);
            
            return new Success(RevertMerge);

            void RevertMerge()
            {
                
            }
        }

        private async UniTask MergeBuildingsTo(BuildingModel buildingModel, BuildingModel toBuilding)
        {
            var view = _viewFeature.GetBuildingView(buildingModel);
            if (view != null)
            {
                var center = toBuilding.GetBuildingCenterVisualPosition();
                await view.MergeTo(center);
                await UniTask.Yield();
            }
            _buildingsModel.RemoveBuilding(buildingModel);
        }

        // ПОка использует простой поиск ближайщих зданий
        public bool CanRecipeMerge(BuildingModel toBuilding, BuildingModel fromBuilding, [CanBeNull] out MergeBuildingsRecipeSo recipeSo, [CanBeNull] out IReadOnlyCollection<BuildingModel> buildingsInvolved)
        {
            recipeSo = null;
            buildingsInvolved = null;
            
            if (toBuilding.IsMaxLevel == false || fromBuilding.IsMaxLevel == false)
            {
                //Debug.LogError("No max level reached");
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
        
        /// <summary>
        /// Проверяет, можем ли смержить здания, и возвращает массив задействованных в мердже зданий в случае успеха
        /// </summary>
        /// <param name="toBuilding"></param>
        /// <param name="fromBuilding"></param>
        /// <param name="buildingsInvolved"></param>
        /// <returns></returns>
        public bool CanLevelUpMerge(BuildingModel toBuilding, BuildingModel fromBuilding, out IReadOnlyCollection<BuildingModel> buildingsInvolved)
        {
            buildingsInvolved = null;
            
            if (Equals(fromBuilding.Config, toBuilding.Config) == false ||
                fromBuilding.Level.Value != toBuilding.Level.Value ||
                fromBuilding.IsMaxLevel)
                return false;
            
            if (!CollectBuildings(toBuilding, fromBuilding, ref buildingsInvolved)) 
                return false;
            
            return buildingsInvolved.Count >= _configuration.MergeBuildingsCountForLevelUp;
        }

        private bool CollectBuildings(BuildingModel toBuilding, BuildingModel fromBuilding,
            ref IReadOnlyCollection<BuildingModel> buildingsInvolved)
        {
            var level = fromBuilding.Level.Value;
            var config = fromBuilding.Config;
            var grid = toBuilding.OccupiedCells.First().GridModel;
            var allBuildingsOfRequired = _buildingsModel.Buildings
                .Where(b => b.Level.Value == level && b.Config == config)
                .ToHashSet();

            if (allBuildingsOfRequired.Count < _configuration.MergeBuildingsCountForLevelUp)
                return false;

            var allBuildingsCells = allBuildingsOfRequired
                .SelectMany(b => b.OccupiedCells)
                .Distinct()
                .ToHashSet();

            var visited = new bool[grid.Size.x, grid.Size.y];
            var islands = new List<HashSet<BuildingModel>>();

            // Основной цикл обхода матрицы
            for (int i = 0; i < grid.Size.x; i++)
            {
                for (int j = 0; j < grid.Size.y; j++)
                {
                    var cell = grid.GetCell(j, i);
                    if (visited[i, j])
                        continue;

                    if (cell.Content.Value == null)
                        continue;

                    if (allBuildingsCells.Contains(cell) == false)
                        continue;

                    var island = new HashSet<BuildingModel>();
                    islands.Add(island);
                    DFS(i, j, island);
                }
            }

            var selected = islands.First(island => island.Contains(toBuilding));
            selected.Add(fromBuilding);
            buildingsInvolved = selected;
            return true;
            
            // Поиск в глубину (рекурсивный)
            void DFS(int i, int j, HashSet<BuildingModel> island)
            {
                if (i < 0 || i >= grid.Size.x ||
                    j < 0 || j >= grid.Size.y)
                    return;

                if (visited[i, j])
                    return;

                visited[i, j] = true;

                var cell = grid.GetCell(j, i);
                if (cell.Content.Value == null)
                    return;

                if (allBuildingsCells.Contains(cell) == false)
                    return;

                island.Add(cell.Content.Value as BuildingModel);

                // Рекурсивный обход 4 направлений
                DFS(i + 1, j, island); // вниз
                DFS(i - 1, j, island); // вверх
                DFS(i, j + 1, island); // вправо
                DFS(i, j - 1, island); // влево
            }
        }
    }
}