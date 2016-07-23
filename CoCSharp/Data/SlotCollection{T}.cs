using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a collection of <see cref="Slot"/>.
    /// </summary>
    /// <typeparam name="TSlot">Type of <see cref="Slot"/> to store.</typeparam>
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
        #endregion

        #region Fields & Properties
        private List<TSlot> _slots;

        /// <summary>
        /// Raised when a <typeparamref name="TSlot"/> is added, removed or when the <see cref="SlotCollection{TSlot}"/> is cleared.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        ///  Gets or sets a value indicating whether the <see cref="SlotCollection{TSlot}"/> is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets the number of <typeparamref name="TSlot"/> in the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _slots.Count;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a <typeparamref name="TSlot"/> to the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// <param name="slot">The <typeparamref name="TSlot"/> to add to the <see cref="SlotCollection{TSlot}"/>.</param>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="slot"/> is null.</exception>
        public void Add(TSlot slot)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("SlotCollection object is readonly.");
            if (slot == null)
                throw new ArgumentNullException("slot");

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, slot));
            _slots.Add(slot);
        }

        /// <summary>
        /// Removes the specified <typeparamref name="TSlot"/> from the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// <param name="slot">The <typeparamref name="TSlot"/> to remove from the <see cref="SlotCollection{TSlot}"/>.</param>
        /// <returns>Returns <c>true</c> if <paramref name="slot"/> was successfully removed; otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="slot"/> is null.</exception>
        public bool Remove(TSlot slot)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("SlotCollection object is readonly.");
            if (slot == null)
                throw new ArgumentNullException("slot");

            var removed = _slots.Remove(slot);
            if (removed)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, slot));
            return removed;
        }

        /// <summary>
        /// Removes all <typeparamref name="TSlot"/> from the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("SlotCollection object is readonly.");

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _slots.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="SlotCollection{TSlot}"/> contains a specific <typeparamref name="TSlot"/>.
        /// </summary>
        /// <param name="slot">The <typeparamref name="TSlot"/> to locate in the <see cref="SlotCollection{TCsvData}"/>.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="slot"/> was found in the <see cref="SlotCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="slot"/> is null.</exception>
        public bool Contains(TSlot slot)
        {
            if (slot == null)
                throw new ArgumentNullException("slot");

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
        /// Use this to raise the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="args">The arguments.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SlotCollection{TSlot}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TSlot}"/> for the <see cref="SlotCollection{TSlot}"/>.</returns>
        public IEnumerator<TSlot> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _slots.GetEnumerator();
        }
        #endregion
    }
}
