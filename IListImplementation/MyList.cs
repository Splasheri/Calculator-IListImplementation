using System;
using System.Collections;
using System.Collections.Generic;

namespace IListImplementation
{
    internal class MyList<T> : IList<T>
    {
        private T[] _content;
        private int _count;
        public MyList()
        {
            _content = new T[] { };
            _count = 0;
        }
        public T this[int index]
        {
            get => _content[index];
            set => _content[index] = value;
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            _content[_count] = item;
            _count++;
        }

        public void Clear()
        {
            _content = new T[] { };
            _count = 0;
        }

        public bool Contains(T item)
        {
            foreach (T obj in _content)
            {
                if (item.Equals(obj))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < _count; i++)
            {
                array.SetValue(_content[i], arrayIndex + i);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyListEnumerator<T>(_content);
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_content[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if ((_count + 1 <= _content.Length) && (index < Count) && (index >= 0))
            {
                _count++;

                for (int i = Count - 1; i > index; i--)
                {
                    _content[i] = _content[i - 1];
                }
                _content[index] = item;
            }
        }

        public bool Remove(T item)
        {
            if (Contains(item))
            {
                RemoveAt(IndexOf(item));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                for (int i = index; i < Count - 1; i++)
                {
                    _content[i] = _content[i + 1];
                }
                _count--;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }
    }

    internal class MyListEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _array;
        private int _currentIndex;
        public MyListEnumerator(T[] array)
        {
            _array = array;
            _currentIndex = -1;
        }
        public T Current
        {
            get
            {
                if (_currentIndex > -1 && _currentIndex < _array.Length)
                {
                    return _array[_currentIndex];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_currentIndex < _array.Length - 1)
            {
                _currentIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }
}