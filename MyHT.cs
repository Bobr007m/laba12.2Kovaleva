using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometryclass;
using laba12;

namespace laba12._2
{
    public class MyHashTable<T> where T : Geometryfigure1
    {
        private const int DefaultCapacity = 10;
        private HashTableEntry[] _table;
        private int _count;
        public int Count => _count;
        public double LoadFactor => (double)_count / Capacity;
        public int Capacity => DefaultCapacity;
        public MyHashTable(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Размер должен быть неотрицательным.", nameof(capacity));

            _table = new HashTableEntry[capacity];
            _count = 0;
        }

        private class HashTableEntry
        {
            public string Key { get; }
            public T Value { get; }
            public bool IsDeleted { get; set; }

            public HashTableEntry(string key, T value)
            {
                Key = key;
                Value = value;
                IsDeleted = false;
            }
        }
        public MyHashTable()
        {
            _table = new HashTableEntry[Capacity];
            _count = 0;
        }

        private int GetHash(string key)
        {
            return Math.Abs(key.GetHashCode()) % Capacity;
        }
        public bool Contains(string key)
        {
            if (key == null) return false;

            int index = GetHash(key); // Получаем начальный индекс
            int originalIndex = index;
            int i = 1;

            // Проверяем начальную позицию
            if (_table[index] != null)
            {
                if (!_table[index].IsDeleted && _table[index].Key == key)
                {
                    return true;
                }
            }
            else
            {
                return false; // Ячейка пуста, элемента нет
            }

            // Линейное пробирование при коллизии
            while (true)
            {
                index = (originalIndex + i) % _table.Length;

                // Если вернулись к начальной позиции
                if (index == originalIndex)
                    return false;

                if (_table[index] == null)
                {
                    return false; // Пустая ячейка - элемента нет
                }
                else if (!_table[index].IsDeleted && _table[index].Key == key)
                {
                    return true; // Нашли элемент
                }

                i++;
            }
        }
        // 1. Добавление элемента
        public void Add(string key, T value)
        {
            if (LoadFactor >= 0.72)
                throw new InvalidOperationException("Хеш-таблица заполнена на 72% и более");

            int index = GetHash(key);
            int originalIndex = index;
            int i = 1;

            while (_table[index] != null && !_table[index]?.IsDeleted == true)
            {
                if (_table[index]?.Key == key)
                    throw new ArgumentException("Элемент с таким ключом уже существует");

                // Линейное пробирование
                index = (originalIndex + i) % Capacity;
                i++;

                if (i > Capacity)
                    throw new InvalidOperationException("Не удалось найти свободную ячейку");
            }

            _table[index] = new HashTableEntry(key, value);
            _count++;
        }
        public void Resize()
        {
            // Увеличиваем размер в 2 раза
            int newCapacity = _table.Length * 2;
            var newTable = new HashTableEntry[newCapacity];
            int newCount = 0;

            // Перехеширование всех элементов
            for (int i = 0; i < _table.Length; i++)
            {
                var entry = _table[i];
                if (entry == null || entry.IsDeleted) continue;

                int newIndex = Math.Abs(entry.Key.GetHashCode()) % newCapacity;

                // Линейное пробирование для новой таблицы
                for (int j = 0; j < newCapacity; j++)
                {
                    int currentIndex = (newIndex + j) % newCapacity;
                    if (newTable[currentIndex] == null)
                    {
                        newTable[currentIndex] = entry;
                        newCount++;
                        break;
                    }
                }
            }

            _table = newTable;
            _count = newCount;
        }
        // 2. Поиск элемента
        public T Find(string key)
        {
            int index = GetHash(key);
            int originalIndex = index;
            int i = 1;

            while (_table[index] != null)
            {
                if (_table[index]?.Key == key && !_table[index]?.IsDeleted == true)
                    return _table[index]?.Value ?? throw new NullReferenceException();

                // Линейное пробирование
                index = (originalIndex + i) % Capacity;
                i++;

                if (i > Capacity)
                    break;
            }

            throw new KeyNotFoundException("Элемент не найден");
        }
        // 3. Удаление элемента (логическое удаление)
        public void Remove(string key)
        {
            int index = GetHash(key);
            int originalIndex = index;
            int i = 1;

            while (_table[index] != null)
            {
                if (_table[index]?.Key == key && !_table[index]?.IsDeleted == true)
                {
                    _table[index].IsDeleted = true;
                    _count--;
                    return;
                }

                // Линейное пробирование
                index = (originalIndex + i) % Capacity;
                i++;

                if (i > Capacity)
                    break;
            }

            throw new KeyNotFoundException("Элемент не найден");
        }
        public void PrintTable()
        {
            Console.WriteLine($"Хеш-таблица (LoadFactor = {LoadFactor:P0}):");
            for (int i = 0; i < Capacity; i++)
            {
                if (_table[i] != null)
                {
                    var status = _table[i]?.IsDeleted == true ? "[УДАЛЕН]" : "";
                    Console.WriteLine($"[{i}]: {_table[i]?.Key} - {_table[i]?.Value} {status}");
                }
                else
                {
                    Console.WriteLine($"[{i}]: Пусто");
                }
            }
        }
        // Метод для демонстрации переполнения
        public void AddWithLoadFactorCheck(string key, T value)
        {
            try
            {
                Add(key, value);
                Console.WriteLine($"Добавлен {key}, LoadFactor = {LoadFactor:P0}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Не удалось добавить {key}: {ex.Message}");
            }
        }

    }
}

