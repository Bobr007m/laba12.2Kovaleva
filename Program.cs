using Geometryclass;
using laba12._2;
using System;

class Program
{
    static void Main()
    {
        var table = new MyHashTable<string, Geometryfigure1>();
        var rand = new Random();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Добавить элемент");
            Console.WriteLine("2. Найти элемент");
            Console.WriteLine("3. Удалить элемент");
            Console.WriteLine("4. Вывести таблицу");
            Console.WriteLine("5. Заполнить случайными фигурами");
            Console.WriteLine("6. Демонстрация переполнения таблицы");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddElementManually(table);
                        break;
                    case "2":
                        FindElement(table);
                        break;
                    case "3":
                        RemoveElement(table);
                        break;
                    case "4":
                        Console.WriteLine(table.GetTableInfo());
                        break;
                    case "5":
                        FillRandomly(table, rand);
                        break;
                    case "6":
                        DemonstrateOverflow(table);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
        }
    }

    static void AddElementManually(MyHashTable<string, Geometryfigure1> table)
    {
        Console.Write("Введите ключ: ");
        var key = Console.ReadLine();

        Console.WriteLine("Выберите тип фигуры (1-Круг, 2-Прямоугольник, 3-Параллелепипед): ");
        var type = Console.ReadLine();

        Geometryfigure1 figure = null;

        if (type == "1")
        {
            figure = new Circle1(GetRandomSize());
        }
        else if (type == "2")
        {
            figure = new Rectangle1(GetRandomSize(), GetRandomSize());
        }
        else if (type == "3")
        {
            figure = new Parallelepiped1(GetRandomSize(), GetRandomSize(), GetRandomSize());
        }
        else
        {
            throw new ArgumentException("Неверный тип фигуры");
        }

        table.Add(key, figure);
        Console.WriteLine("Элемент добавлен");
    }

    static void FindElement(MyHashTable<string, Geometryfigure1> table)
    {
        if (table.Count == 0)
        {
            Console.WriteLine("Таблица пуста");
            return;
        }

        Console.Write("Введите ключ: ");
        var key = Console.ReadLine();

        if (table.TryGetValue(key, out var value))
            Console.WriteLine($"Найдено: {value}");
        else
            Console.WriteLine("Элемент не найден");
    }

    static void RemoveElement(MyHashTable<string, Geometryfigure1> table)
    {
        if (table.Count == 0)
        {
            Console.WriteLine("Таблица пуста");
            return;
        }

        Console.Write("Введите ключ: ");
        var key = Console.ReadLine();

        if (table.Remove(key))
            Console.WriteLine("Элемент удален");
        else
            Console.WriteLine("Элемент не найден");
    }

    static void FillRandomly(MyHashTable<string, Geometryfigure1> table, Random rand)
    {
        Console.Write("Сколько элементов добавить? ");
        if (!int.TryParse(Console.ReadLine(), out var count) || count <= 0)
        {
            Console.WriteLine("Некорректное число");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // Генерируем уникальный ключ 
            string key = $"figure_{i}";

            // Создаем случайную фигуру
            int figureType = rand.Next(1, 4);
            Geometryfigure1 figure;

            if (figureType == 1)
            {
                figure = new Circle1(rand.Next(1, 10));
            }
            else if (figureType == 2)
            {
                figure = new Rectangle1(rand.Next(1, 10), rand.Next(1, 10));
            }
            else if (figureType == 3)
            {
                figure = new Parallelepiped1(rand.Next(1, 10), rand.Next(1, 10), rand.Next(1, 10));
            }
            else
            {
                throw new InvalidOperationException("Неизвестный тип фигуры");
            }

            // Добавляем элемент в таблицу
            table.Add(key, figure);
        }

        Console.WriteLine($"Добавлено {count} элементов");
    }

    static int GetRandomSize() => new Random().Next(1, 10);
    /// <summary>
    /// Метод для демонстрации поведения при попытке добавления в переполненную таблицу
    /// </summary>
    static void DemonstrateOverflow(MyHashTable<string, Geometryfigure1> hashTable)
    {
        Console.WriteLine("\nДЕМОНСТРАЦИЯ ПОВЕДЕНИЯ ХЭШ-ТАБЛИЦЫ ПРИ ПЕРЕПОЛНЕНИИ ");

        try
        {
            // Устанавливаем фиксированную ёмкость для демонстрации
            int initialCapacity = 2;
            double loadFactor = 0.72;

            Console.WriteLine($"Создаём таблицу с начальной ёмкостью {initialCapacity} и коэффициентом заполнения {loadFactor}");
            hashTable = new MyHashTable<string, Geometryfigure1>(initialCapacity, loadFactor);

            // Добавляем первый элемент
            Console.WriteLine("Добавляем первый элемент...");
            hashTable.Add("Фигура_1", new Rectangle1(1, 1));
            hashTable.GetTableInfo();

            // Добавляем второй элемент
            Console.WriteLine("Добавляем второй элемент...");
            hashTable.Add("Фигура_2", new Circle1(1));
            hashTable.GetTableInfo();

            // Третий элемент должен вызвать Resize()
            Console.WriteLine("Добавляем третий элемент — должно произойти автоматическое расширение...");
            hashTable.Add("Фигура_3", new Parallelepiped1(1, 1, 1));
            hashTable.GetTableInfo();

            // Попробуем добавить ещё один элемент
            Console.WriteLine("Добавляем четвёртый элемент...");
            hashTable.Add("Фигура_4", new Rectangle1(2, 2));
            hashTable.GetTableInfo();

            Console.WriteLine("\nВсе элементы успешно добавлены.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}");
            Console.WriteLine("Таблица достигла максимальной ёмкости или не поддерживает автоматическое расширение.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
        }

        Console.WriteLine("\nЗАВЕРШЕНИЕ ДЕМОНСТРАЦИИ ");
    }
}