using System;
using System.Collections.Generic;

namespace CSLight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();
            aquarium.Work();
        }
    }

    class Aquarium
    {
        private const int MaxAge = 1000;
        private const int MaxFishCount = 6;

        private List<Fish> _fish;
        private int _passedDaysCount;

        public Aquarium()
        {
            _passedDaysCount = 0;
            _fish = new List<Fish>();
        }

        private enum ActionOption
        {
            AddNewFish = 1,
            RemoveFish,
            SkipDay
        }

        public void Work()
        {
            while (_passedDaysCount < MaxAge)
            {
                Console.Clear();

                ShowDaysPassed();
                ShowFishInfo();
                Console.WriteLine("\n" +
                    $"{(int)ActionOption.AddNewFish}) Добавить рыбу\n" +
                    $"{(int)ActionOption.RemoveFish}) Убрать рыбу\n" +
                    $"{(int)ActionOption.SkipDay}) Пропустить день" +
                    "\n");

                switch ((ActionOption)ConsoleInputMethods.GetInt())
                {
                    case ActionOption.AddNewFish:
                        AddFish();
                        break;
                    case ActionOption.RemoveFish:
                        RemoveFish();
                        break;
                    case ActionOption.SkipDay:
                        SkipDay();
                        break;
                    default:
                        Console.WriteLine("Коменда не корректна\n");
                        break;
                }
            }
        }

        private void ShowDaysPassed()
        {
            ColorTextMaker.Write($"Прошло дней: {_passedDaysCount}\n", ConsoleColor.Cyan);
        }

        private void ShowFishInfo()
        {
            if (_fish.Count > 0)
            {
                for (int i = 0; i < _fish.Count; i++)
                {
                    if (_fish[i].IsAlive)
                    {
                        WriteFishStatus(i, _fish, ConsoleColor.Yellow);
                    }
                    else
                    {
                        WriteFishStatus(i, _fish, ConsoleColor.Red);
                    }
                }
            }
            else
            {
                Console.WriteLine("В аквариуме нет рыб");
            }
        }

        private void WriteFishStatus(int index, List<Fish> fish, ConsoleColor consoleColor)
        {
            ColorTextMaker.Write($"{index + 1}) Возраст - {fish[index].Age} Возраст смерти {fish[index].MaxAge}", consoleColor);
        }

        private void AddFish()
        {
            if (_fish.Count < MaxFishCount)
            {
                _fish.Add(new Fish());
            }
            else
            {
                Console.WriteLine("В аквариуме слишком много рыб");
                Console.ReadLine();
            }
        }

        private void RemoveFish()
        {
            if (_fish.Count > 0)
            {
                Console.WriteLine("Какую рыбу хотите достать?");
                int fishIndex = ConsoleInputMethods.GetInt() - 1;

                if (fishIndex >= 0 && fishIndex < _fish.Count)
                {
                    _fish.Remove(_fish[fishIndex]);
                }
                else
                {
                    Console.WriteLine("Ошибка: не верно ");
                }
            }
            else
            {
                Console.WriteLine("В аквариуме нет рыб, нельзя убрать");
                Console.ReadLine();
            }
        }

        private void SkipDay()
        {
            foreach (var fish in _fish)
            {
                fish.Grow();
            }

            _passedDaysCount++;
        }
    }

    class Fish
    {
        public Fish()
        {
            int MinLifeTime = 5;
            int MaxLifeTime = 10;
            Age = 0;
            MaxAge = RandomMethods.GetRandomNumber(MinLifeTime, MaxLifeTime);
        }

        public int MaxAge { get; private set; }
        public int Age { get; private set; }
        public bool IsAlive => Age < MaxAge;

        public void Grow()
        {
            if (IsAlive)
            {
                Age++;
            }
        }
    }

    class ColorTextMaker
    {
        static public void Write(string text, ConsoleColor UsedColor)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = UsedColor;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }
    }

    class RandomMethods
    {
        static private Random _random = new Random();

        public static int GetRandomNumber(int minimum, int maximum)
        {
            return _random.Next(minimum, maximum);
        }
    }

    class ConsoleInputMethods
    {
        public static int GetInt()
        {
            bool isConverted;
            int number;

            do
            {
                string input = Console.ReadLine();
                isConverted = int.TryParse(input, out number);

                if (isConverted == false)
                {
                    Console.WriteLine("Входные данные не в целочисленном формате");
                }

            } while (isConverted == false);

            return number;
        }
    }
}