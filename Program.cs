using Geometryclass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba12._2
{
    public class Program
    {
        static void Main(string[] args)
        {
            var hashTable = new MyHashTable<Geometryfigure1>();

            var rect = new Rectangle1(5, 10) { Name = "Прямоугольник1" };
            var circle = new Circle1(7) { Name = "Круг1" };
            var parallelepiped = new Parallelepiped1(3) { Name = "Параллелепипед1" };

            // 1. Добавление элементов с контролем коэффициента заполненности
            hashTable.AddWithLoadFactorCheck("rect1", rect);
            hashTable.AddWithLoadFactorCheck("circle1", circle);
            hashTable.AddWithLoadFactorCheck("par1", parallelepiped);

            Console.WriteLine("\n1. Таблица после добавления элементов:");
            hashTable.PrintTable();
            Console.WriteLine();

            // 2. Поиск элемента
            try
            {
                var found = hashTable.Find("circle1");
                Console.WriteLine($"2. Найден элемент: {found.Name}");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("2. Элемент не найден");
            }
            Console.WriteLine();

            // 3. Удаление элемента (логическое)
            try
            {
                hashTable.Remove("circle1");
                Console.WriteLine("3. Элемент 'circle1' помечен как удаленный");
                hashTable.PrintTable();
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("3. Элемент для удаления не найден");
            }
            Console.WriteLine();

            // 4. Попытка поиска удаленного элемента
            try
            {
                var foundAfterRemoval = hashTable.Find("circle1");
                Console.WriteLine($"4. Найден элемент: {foundAfterRemoval.Name}");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("4. Элемент 'circle1' не найден (ожидаемо, так как он удален)");
            }
            Console.WriteLine();

            // 5. Добавление нового элемента на место удаленного
            var newCircle = new Circle1(10) { Name = "НовыйКруг" };
            hashTable.AddWithLoadFactorCheck("newCircle", newCircle);
            Console.WriteLine("\n5. После добавления нового элемента на место удаленного:");
            hashTable.PrintTable();
            Console.WriteLine();

            // 6. Демонстрация работы коэффициента заполненности
            Console.WriteLine("6. Попытка заполнить таблицу до предела:");
            try
            {
                for (int i = 0; i < 15; i++)
                {
                    var newRect = new Rectangle1(i, i + 1) { Name = $"Прямоугольник{i}" };
                    hashTable.AddWithLoadFactorCheck($"rect{i}", newRect);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nИтоговое состояние таблицы:");
            hashTable.PrintTable();
        }
    }
}

