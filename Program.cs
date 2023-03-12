using System;
using System.Collections.Generic;
using System.Threading;

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
        private const int MaxFishCount = 6;
        private const int LengthOfTheDay = 20;
        private const int IterationTime = 1;

        private List<Fish> _fish;
        private int _passedDaysCount;
        private int _passedTime;

        public Aquarium()
        {
            _passedDaysCount = 0;
            _passedTime = 0;
            _fish = new List<Fish>();

            for (int i = 0; i < MaxFishCount; i++)
            {
                AddFish();
            }
        }

        private enum ActionOption
        {
            AddNewFish = 1,
            RemoveFish
        }

        public void Work()
        {
            while (IsAnyFishAlive())
            {
                if (_passedTime == LengthOfTheDay)
                {
                    _passedTime = 0;
                    SkipDay();
                }

                Console.Clear();

                ShowDaysPassed();
                ShowFishInfo();
                Console.WriteLine("\n" +
                    $"{(int)ActionOption.AddNewFish}) Добавить новую рыбу\n" +
                    $"{(int)ActionOption.RemoveFish}) Убрать мёртвых рыб\n" +
                    "\n");

                if (Console.KeyAvailable)
                    HandleInput();

                Thread.Sleep(IterationTime);
                _passedTime++;
            }

            ShowEndGameScreen();
            Console.ReadKey();
        }

        private void ShowEndGameScreen()
        {
            Console.WriteLine("Вы проиграли, все рыбы умерли");
        }

        private bool IsAnyFishAlive()
        {
            int aliveFishes = 0;

            foreach (var fish in _fish)
            {
                if (fish.IsAlive)
                    aliveFishes++;
            }

            return aliveFishes > 0;
        }

        private void HandleInput()
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.NumPad1:
                    AddFish();
                    break;
                case ConsoleKey.NumPad2:
                    RemoveDeadFishes();
                    break;
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
        }

        private void RemoveDeadFishes()
        {
            if (_fish.Count > 0)
            {
                for (int i = _fish.Count - 1; i >= 0; i--)
                {
                    if (_fish[i].IsAlive == false)
                    {
                        _fish.RemoveAt(i);
                    }
                }
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