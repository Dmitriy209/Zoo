using System;
using System.Collections.Generic;

namespace Zoo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Zoo zoo = new Zoo();
            zoo.Work();
        }
    }

    class Zoo
    {
        private List<AnimalEnclosure> _animalEnclosures = new List<AnimalEnclosure>();
        private int _lastAnimalEnclosureId = 1;

        public Zoo()
        {
            GenerateAnimalEnclosures();
        }

        public void Work()
        {
            Console.WriteLine($"В зоопарке доступно {_animalEnclosures.Count} вольеров.");

            _animalEnclosures[ReadNumberAviaries()].ShowStats();
        }

        private int ReadNumberAviaries()
        {
            int number;

            do
            {
                Console.WriteLine($"Введите порядковый номер вольера, чтобы подойти к нему.");
                number = ReadInt();

                number -= 1;
            }
            while (number < 0 || number >= _animalEnclosures.Count);

            return number;
        }

        private int ReadInt()
        {
            int number;

            while (int.TryParse(Console.ReadLine(), out number) == false)
                Console.WriteLine("Это не число.");

            return number;
        }

        private void GenerateAnimalEnclosures()
        {
            int numberAnimalEnclosure = 4;

            for (int i = 0; i < numberAnimalEnclosure; i++)
                _animalEnclosures.Add(CreatorAnimalEnclosure.GenerateAnimalEnclosure(GetAnimalEnclosureId()));
        }

        private int GetAnimalEnclosureId()
        {
            return _lastAnimalEnclosureId++;
        }
    }

    class CreatorAnimalEnclosure
    {
        public static AnimalEnclosure GenerateAnimalEnclosure(int id)
        {
            int minLimitAnimals = 1;
            int maxLimitAnimals = 4;
            
            int minLimitSpecies = 1;
            int maxLimitSpecies = 4;

            int numberAnimals = UserUtils.GenerateRandomNumber(minLimitAnimals, maxLimitAnimals);
            int numberSpecies = UserUtils.GenerateRandomNumber(minLimitSpecies, maxLimitSpecies);

            List<Animal> animals = new List<Animal>();

            for (int i = 0; i < numberAnimals; i++)
                animals.Add(CreatorAnimal.GenerateAnimal(numberSpecies));

            return new AnimalEnclosure(id, animals);
        }
    }

    class AnimalEnclosure
    {
        private string _name;
        private List<Animal> _animals;

        public AnimalEnclosure(int id, List<Animal> animals)
        {
            Id = id;
            _animals = animals;
            _name = $"Вольер №{Id}.";
        }

        public int Id { get; private set; }
        public int AnimalsCount => _animals.Count;

        public void ShowStats()
        {
            Console.WriteLine($"{_name}\n" +
                $"В вольере находится {_animals.Count} особей.");

            foreach (Animal animal in _animals)
                animal.ShowStats();
        }
    }

    class CreatorAnimal
    {
        private static List<string> _maleNames = new List<string>() { "Мансур", "Борис", "Рыжик", "Бим" };
        private static List<string> _femaleNames = new List<string>() { "Дина", "Долли", "Люся", "Сэма" };

        private static int counterGender;

        public static Animal GenerateAnimal(int numberSpecies)
        {
            Gender gender = GetGender();

            return new Animal(GetName(gender), gender, GetSpecies(numberSpecies));
        }

        private static Species GetSpecies(int numberSpecies)
        {
            int numberSpeciesPantera = 1;
            int numberSpeciesElephant = 2;
            int numberSpeciesBoar = 3;

            if (numberSpeciesPantera == numberSpecies)
                return new Pantera();
            else if (numberSpeciesElephant == numberSpecies)
                return new Elephant();
            else if (numberSpeciesBoar == numberSpecies)
                return new Boar();
            else
                return new Bear();
        }

        private static Gender GetGender()
        {
            counterGender++;

            int amountGender = 2;

            int numberGender = counterGender % amountGender;

            if (numberGender == 0)
                return new Male();
            else
                return new Female();
        }

        private static string GetName(Gender Gender)
        {
            List<string> names = new List<string>();

            string genderMale = "male";
            string genderFemale = "female";

            if (Gender.Name.ToLower() == genderMale)
                return _maleNames[UserUtils.GenerateRandomNumber(0, _maleNames.Count)];
            else if (Gender.Name.ToLower() == genderFemale)
                return _femaleNames[UserUtils.GenerateRandomNumber(0, _femaleNames.Count)];
            else
                return GetName();
        }

        private static string GetName()
        {
            List<string> names = new List<string>();

            UserUtils.UniteListString(names, _maleNames);
            UserUtils.UniteListString(names, _femaleNames);

            return names[UserUtils.GenerateRandomNumber(0, names.Count)];
        }
    }

    class Animal
    {
        protected string _name;
        protected Gender _gender;
        protected Species _species;
        protected SoundMade _soundMade;

        public Animal(string name, Gender gender, Species species)
        {
            _name = name;
            _gender = gender;
            _species = species;
            _soundMade = species.SoundMade;
        }

        public void ShowStats()
        {
            Console.WriteLine($"Меня зовут {_name}, я {_gender} пола, вида {_species} и я издаю звук {_soundMade}.");
        }
    }

    class Gender
    {
        public string Name { get; protected set; }
    }

    class Male : Gender
    {
        public Male()
        {
            Name = nameof(Male);
        }
    }

    class Female : Gender
    {
        public Female()
        {
            Name = nameof(Female);
        }
    }

    class Species
    {
        public string Name { get; protected set; }
        public SoundMade SoundMade { get; protected set; }
    }

    class Pantera : Species
    {
        public Pantera()
        {
            Name = nameof(Pantera);
            SoundMade = new Growl();
        }
    }

    class Elephant : Species
    {
        public Elephant()
        {
            Name = nameof(Elephant);
            SoundMade = new Blowing();
        }
    }

    class Boar : Species
    {
        public Boar()
        {
            Name = nameof(Boar);
            SoundMade = new Grunt();
        }
    }

    class Bear : Species
    {
        public Bear()
        {
            Name = nameof(Bear);
            SoundMade = new Growl();
        }
    }

    class SoundMade
    {
        public string Name { get; protected set; }
    }

    class Growl : SoundMade
    {
        public Growl()
        {
            Name = nameof(Growl);
        }
    }

    class Blowing : SoundMade
    {
        public Blowing()
        {
            Name = nameof(Blowing);
        }
    }

    class Grunt : SoundMade
    {
        public Grunt()
        {
            Name = nameof(Grunt);
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max);
        }

        public static void UniteListString(List<string> recipientList, List<string> senderList)
        {
            foreach (string line in senderList)
                recipientList.Add(line);
        }
    }
}
