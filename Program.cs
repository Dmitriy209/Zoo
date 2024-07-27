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
                _animalEnclosures.Add(CreatorAnimalEnclosure.GenerateAnimalEnclosure(ReturnAnimalEnclosureId()));
        }

        private int ReturnAnimalEnclosureId()
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
        private static List<string> s_maleNames = new List<string>() { "Мансур", "Борис", "Рыжик", "Бим" };
        private static List<string> s_femaleNames = new List<string>() { "Дина", "Долли", "Люся", "Сэма" };

        private static int s_counterGender;

        public static Animal GenerateAnimal(int numberSpecies)
        {
            string gender = GetGender();

            return new Animal(GetName(gender), gender, GetSpecies(numberSpecies, out string soundMade), soundMade);
        }

        private static string GetSpecies(int numberSpecies, out string soundMade)
        {
            int numberSpeciesPantera = 1;
            int numberSpeciesElephant = 2;
            int numberSpeciesBoar = 3;

            if (numberSpeciesPantera == numberSpecies)
            {
                soundMade = "рычит";
                return "Pantera";
            }
            else if (numberSpeciesElephant == numberSpecies)
            {
                soundMade = "трубит";
                return "Elephant";
            }
            else if (numberSpeciesBoar == numberSpecies)
            {
                soundMade = "хрюкает";
                return "Boar";
            }
            else
            {
                soundMade = "рычит";
                return "Bear";
            }
        }

        private static string GetGender()
        {
            s_counterGender++;

            int amountGender = 2;

            int numberGender = s_counterGender % amountGender;

            if (numberGender == 0)
                return "male";
            else
                return "female";
        }

        private static string GetName(string gender)
        {
            List<string> names = new List<string>();

            string genderMale = "male";
            string genderFemale = "female";

            if (gender.ToLower() == genderMale)
                return s_maleNames[UserUtils.GenerateRandomNumber(0, s_maleNames.Count)];
            else if (gender.ToLower() == genderFemale)
                return s_femaleNames[UserUtils.GenerateRandomNumber(0, s_femaleNames.Count)];
            else
                return GetName();
        }

        private static string GetName()
        {
            List<string> names = new List<string>();

            UserUtils.UniteListString(names, s_maleNames);
            UserUtils.UniteListString(names, s_femaleNames);

            return names[UserUtils.GenerateRandomNumber(0, names.Count)];
        }
    }

    class Animal
    {
        private string _name;
        private string _gender;
        private string _species;
        private string _soundMade;

        public Animal(string name, string gender, string species, string soundMade)
        {
            _name = name;
            _gender = gender;
            _species = species;
            _soundMade = soundMade;
        }

        public void ShowStats()
        {
            Console.WriteLine($"Меня зовут {_name}, я {_gender} пола, вида {_species} и я издаю звук {_soundMade}.");
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
