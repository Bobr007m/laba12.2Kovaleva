using Geometryclass;
using laba12._2;
using System;
using System.Collections.Generic;

namespace HashTableDemo
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            // Создаем хеш-таблицу с начальным размером 5
            var hashTable = new MyHashTable<Geometryfigure1>(5);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ДЕМОНСТРАЦИИ ХЕШ-ТАБЛИЦЫ ===");
                Console.WriteLine("1. Заполнить таблицу случайными фигурами");
                Console.WriteLine("2. Показать содержимое таблицы");
                Console.WriteLine("3. Найти фигуру по ключу");
                Console.WriteLine("4. Удалить фигуру по ключу");
                Console.WriteLine("5. Очистить таблицу");
                Console.WriteLine("6. Демонстрация поведения при переполнении");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        FillWithRandomFigures(hashTable);
                        break;
                    case "2":
                        ShowHashTable(hashTable);
                        break;
                    case "3":
                        FindFigure(hashTable);
                        break;
                    case "4":
                        RemoveFigure(hashTable);
                        break;
                    case "5":
                        hashTable = new MyHashTable<Geometryfigure1>(5);
                        Console.WriteLine("Таблица очищена.");
                        break;
                    case "6":
                        DemonstrateOverflow(hashTable);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Ошибка: Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void FillWithRandomFigures(MyHashTable<Geometryfigure1> hashTable)
        {
            try
            {
                Console.Write("Введите количество фигур для добавления: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int count) || count <= 0)
                {
                    Console.WriteLine("Ошибка: Введите положительное число.");
                    return;
                }

                Console.WriteLine($"\nДобавление {count} случайных фигур...");

                for (int i = 0; i < count; i++)
                {
                    string name = $"Фигура_{i + 1}";

                    int figureType = rand.Next(3);
                    Geometryfigure1 figure = null;

                    if (figureType == 0)
                    {
                        figure = new Rectangle1(rand.Next(1, 10), rand.Next(1, 10)) { Name = name };
                    }
                    else if (figureType == 1)
                    {
                        figure = new Circle1(rand.Next(1, 10)) { Name = name };
                    }
                    else if (figureType == 2)
                    {
                        figure = new Parallelepiped1(rand.Next(1, 10), rand.Next(1, 10), rand.Next(1, 10)) { Name = name };
                    }
                    else
                    {
                        throw new NotImplementedException("Неизвестный тип фигуры");
                    }

                    try
                    {
                        hashTable.AddWithLoadFactorCheck(name, figure);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Пропущено (дубликат ключа): {name} — {ex.Message}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Ошибка при добавлении {name}: {ex.Message}");
                        Console.WriteLine("Хеш-таблица переполнена. Рассмотрите увеличение ёмкости или удаление старых записей.");
                        break;
                    }
                }

                Console.WriteLine("Добавление завершено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        static void ShowHashTable(MyHashTable<Geometryfigure1> hashTable)
        {
            Console.WriteLine("\nТекущее состояние хеш-таблицы:");
            hashTable.PrintTable();
        }

        static void FindFigure(MyHashTable<Geometryfigure1> hashTable)
        {
            Console.Write("\nВведите ключ (имя фигуры) для поиска: ");
            string key = Console.ReadLine();

            try
            {
                var figure = hashTable.Find(key);
                Console.WriteLine($"Найдена фигура: {figure.Name} ({figure.GetType().Name})");
                Console.WriteLine(figure.ToString());
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Ошибка: Фигура с указанным ключом не найдена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка поиска: {ex.Message}");
            }
        }

        static void RemoveFigure(MyHashTable<Geometryfigure1> hashTable)
        {
            Console.Write("\nВведите ключ (имя фигуры) для удаления: ");
            string key = Console.ReadLine();

            try
            {
                bool removed = hashTable.Remove(key);
                if (removed)
                {
                    Console.WriteLine("Фигура успешно удалена.");
                    Console.WriteLine("\nСостояние таблицы после удаления:");
                    hashTable.PrintTable();
                }
                else
                {
                    Console.WriteLine("Фигура с указанным ключом не найдена или уже удалена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }

        //  Метод для демонстрации поведения при переполнении
        static void DemonstrateOverflow(MyHashTable<Geometryfigure1> hashTable)
        {
            Console.WriteLine("\nДемонстрация поведения при попытке добавления в переполненную таблицу...");

            try
            {
                // Очищаем таблицу и устанавливаем фиксированный размер
                hashTable = new MyHashTable<Geometryfigure1>(capacity: 2, loadFactor: 0.72);

                // Добавляем ровно столько, сколько помещается до Resize
                hashTable.AddWithLoadFactorCheck("Фигура_1", new Rectangle1(1, 1));
                hashTable.AddWithLoadFactorCheck("Фигура_2", new Circle1(1));

                Console.WriteLine("\nПопытка добавить третий элемент (вызовет Resize)...");

                // Третий элемент должен вызвать Resize()
                hashTable.AddWithLoadFactorCheck("Фигура_3", new Parallelepiped1(1, 1, 1));

                Console.WriteLine("Третий элемент успешно добавлен после увеличения таблицы.");

                Console.WriteLine("\nПопытка добавить четвёртый элемент (может вызвать исключение)...");
                hashTable.AddWithLoadFactorCheck("Фигура_4", new Rectangle1(2, 2));

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
                Console.WriteLine("Таблица не может принять больше элементов.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            Console.WriteLine("\nСостояние таблицы после попыток:");
            hashTable.PrintTable();
        }
    }
}