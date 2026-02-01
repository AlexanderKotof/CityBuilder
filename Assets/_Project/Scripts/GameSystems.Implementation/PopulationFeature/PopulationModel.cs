using System;
using System.Collections.Generic;
using CityBuilder.Reactive;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CityBuilder.GameSystems.Implementation.PopulationFeature
{
    //ToDo: model of person?
    public record Person
    {
        public Guid Id { get; } = Guid.NewGuid();
        
        public bool IsAlive { get; internal set; } = true;
    }
    
    public class PopulationModel
    {
        public ReactiveProperty<int> CurrentPopulation { get; } = new();
        public ReactiveProperty<int> EmployedPopulation { get; } = new(0);
        public ReactiveProperty<int> AvailableHouseholds { get; } = new();
        
        public readonly List<Person> People = new();
        
        //TODO: move to configs
        private readonly float _dayGrowthFactor = 0.1f;
        private readonly float _dayGrowthProbability = 0.1f;
        
        private readonly float _dayDeathFactor = 0.1f;
        private readonly float _dayDeathProbability = 0.1f;
        
        private readonly int _weekGrowthBase = 10;
        public const int StartingPopulation = 100;
        public const int StartingHouseholds = 100;
        
        public PopulationModel() : this(StartingPopulation, StartingHouseholds) { }

        public PopulationModel(int startingPopulation, int startingHouseholds)
        {
            CurrentPopulation.Value = startingPopulation;
            AvailableHouseholds.Value = startingHouseholds;
            
            for (int i = 0; i < startingPopulation; i++)
            {
                People.Add(new Person());
            }
        }

        public void OnWeekChanged()
        {
            CurrentPopulation.Value += _weekGrowthBase;
            for (int i = 0; i < _weekGrowthBase; i++)
            {
                People.Add(new Person());
            }
            
            Debug.Log($"New week starts! Current population: {CurrentPopulation.Value}");
        }
        
        public void OnDayChanged()
        {
            int growth = Random.value <= _dayGrowthProbability
                ? Mathf.FloorToInt(_dayGrowthFactor * CurrentPopulation.Value * Random.value)
                : 0;
            int died = Random.value <= _dayDeathProbability
                ? Mathf.FloorToInt(_dayDeathFactor * CurrentPopulation.Value * Random.value)
                : 0;

            int change = growth - died;

            if (change == 0)
            {
                return;
            }

            bool depopulation = change < 0;
            change = Mathf.Abs(change);
            
            for (int i = 0; i < change; i++)
            {
                if (depopulation)
                {
                    int index = Random.Range(0, People.Count);
                    var person = People[index];
                    
                    person.IsAlive = false;
                    
                    People.RemoveAt(index);
                }
                else
                {
                    People.Add(new Person());
                }
            }
            
            CurrentPopulation.Value += change;
            
            Debug.Log($"Population changed today by {change}, current value: {CurrentPopulation.Value}");
        }

        public void UpdateAvailableHouseholds(int value) => AvailableHouseholds.Set(value);

        public bool IsDied(Person person)
        {
            return person.IsAlive == false;
        }
    }
}