using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a collection of <see cref="Slot"/>.
    /// </summary>
    /// <typeparam name="TSlot">Type of <see cref="Slot"/> to store.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class SlotCollection<TSlot> : ICollection<TSlot>, INotifyCollectionChanged where TSlot : Slot, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SlotCollection{TSlot}"/> class.
        /// </summary>
        public SlotCollection()
        {
            _slots = new List<TSlot>(4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlotCollection{TSlot}"/> class with the specified <see cref="IEnumerator{T}"/> class.
        /// </summary>
        /// <param name="enumerable"><see cref="IEnumerator{T}"/> object from which to load the <see cref="SlotCollection{TSlot}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is null.</exception>
        public SlotCollection(IEnumerable<TSlot> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            _slots = new List<TSlot>(enumerable);
        }
        #endregion

        #region Fields & Properties
        private readonly List<TSlot> _slots;

        bool ICollection<TSlot>.IsReadOnly => false;

        /// <summary>
        /// Gets or sets the <typeparamref name="TSlot"/> at the specified index.
        /// </summary>
        /// <param name="index">Index at which to read.</param>
        /// <returns>The <typeparamref name="TSlot"/> at the specified index.</returns>
        public TSlot this[int index]
        {
            get
            {
                return _slots[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (index >= Count)
                {
                    _slots.Add(value);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                }
                else
                {
                    var oldItem = _slots[index];
                    if (oldItem == null)
                    {
                        _slots.Add(value);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                    }
                    else
                    {
                        _slots[index] = value;
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
                    }
                }
            }
        }

        /// <summary>
        /// Raised when a <typeparamref name="TSlot"/> is added, removed or when the <see cref="SlotCollection{TSlot}"/> is cleared.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets the number of <typeparamref name="TSlot"/> in the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        public int Count => _slots.Count;
        #endregion

        #region Methods
        /// <summary>
        /// Adds a <typeparamref name="TSlot"/> to the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// <param name="slot">The <typeparamref name="TSlot"/> to add to the <see cref="SlotCollection{TSlot}"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="slot"/> is null.</exception>
        public void Add(TSlot slot)
        {
            if (slot == null)
                throw new ArgumentNullException(nameof(slot));

            _slots.Add(slot);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, slot));
        }

        /// <summary>
        /// Removes the specified <typeparamref name="TSlot"/> from the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// <param name="slot">The <typeparamref name="TSlot"/> to remove from the <see cref="SlotCollection{TSlot}"/>.</param>
        /// <returns>Returns <c>true</c> if <paramref name="slot"/> was successfully removed; otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="slot"/> is null.</exception>
        public bool Remove(TSlot slot)
        {
            if (slot == null)
                throw new ArgumentNullException(nameof(slot));

            var removed = _slots.Remove(slot);
            if (removed)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, slot));

            return removed;
        }

        /// <summary>
        /// Removes all <typeparamref name="TSlot"/> from the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        public void Clear()
        {
            _slots.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the <see cref="SlotCollection{TSlot}"/> contains a specific <typeparamref name="TSlot"/>.
        /// </summary>
        ///
        /// <param name="slot">The <typeparamref name="TSlot"/> to locate in the <see cref="SlotCollection{TCsvData}"/>.</param>
        /// 
        /// <returns>
        /// <c>true</c> if the <paramref name="slot"/> was found in the <see cref="SlotCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="slot"/> is null.</exception>
        public bool Contains(TSlot slot)
        {
            if (slot == null)
                throw new ArgumentNullException(nameof(slot));

            return _slots.Contains(slot);
        }

        /// <summary>
        /// Copies the entire <see cref="SlotCollection{TSlot}"/> to a specified compatible one-dimensional array, at the specified
        /// index to array.
        /// </summary>
        /// <param name="array">Target one-dimensional array to copy the <see cref="SlotCollection{TCsvData}"/>.</param>
        /// <param name="arrayIndex">Index at which to start copying the <see cref="SlotCollection{TCsvData}"/> in the target array.</param>
        public void CopyTo(TSlot[] array, int arrayIndex)
        {
            _slots.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies the element of the <see cref="SlotCollection{TSlot}"/> to a new array.
        /// </summary>
        /// <returns>Array containing the copied <see cref="SlotCollection{TSlot}"/>.</returns>
        public TSlot[] ToArray()
        {
            return _slots.ToArray();
        }

        /// <summary>
        /// Returns the first <typeparamref name="TSlot"/> in the <see cref="SlotCollection{TSlot}"/> with the
        /// same <see cref="Slot.Id"/> as the specified data ID.
        /// </summary>
        /// 
        /// <param name="dataId">Data ID of the <typeparamref name="TSlot"/> to look for.</param>
        /// 
        /// <returns>
        /// Returns null if not found, otherwise the instance of <typeparamref name="TSlot"/> with
        /// the same <see cref="Slot.Id"/> as <paramref name="dataId"/>.
        /// </returns>
        public TSlot GetSlot(int dataId)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_slots[i].Id == dataId)
                    return _slots[i];
            }
            return null;
        }

        /// <summary>
        /// Use this to raise the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TSlot}"/> for the <see cref="SlotCollection{TSlot}"/>.</returns>
        public IEnumerator<TSlot> GetEnumerator() => _slots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _slots.GetEnumerator();
        #endregion
    }
}
