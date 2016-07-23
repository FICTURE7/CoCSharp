using CoCSharp.Data;
using CoCSharp.Data.Slots;
using NUnit.Framework;
using System;
using System.Collections.Specialized;

namespace CoCSharp.Test.Data
{
    [TestFixture]
    public class SlotCollectionTests
    {
        private SlotCollection<ResourceAmountSlot> _collection;

        [SetUp]
        public void SetUp()
        {
            _collection = new SlotCollection<ResourceAmountSlot>();
        }

        [Test]
        public void Constructors()
        {
            new SlotCollection<ResourceCapacitySlot>();
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Add(null));
        }

        [Test]
        public void Add_IsReadOnly_Exception()
        {
            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Add(new ResourceAmountSlot()));
        }

        [Test]
        public void Add_ValidItem_ItemAdded_And_EventRaised()
        {
            _collection.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);

            var slot = new ResourceAmountSlot();
            Assert.AreEqual(0, _collection.Count);
            _collection.Add(slot);
            Assert.AreEqual(1, _collection.Count);
        }

        [Test]
        public void Remove_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Remove(null));
        }

        [Test]
        public void Remove_IsReadOnly_Exception()
        {
            var slot = AddSlot();
            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Remove(slot));
        }

        [Test]
        public void Remove_ValidItem_ItemRemoved_And_EventRaised()
        {
            var slot = AddSlot();
            _collection.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
            Assert.True(_collection.Remove(slot));
            Assert.AreEqual(0, _collection.Count);
        }

        [Test]
        public void Clear_IsReadOnly_Exception()
        {
            for (int i = 0; i < 50; i++)
                _collection.Add(new ResourceAmountSlot());

            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Clear());
        }

        [Test]
        public void Clear_ContainsItem_ItemCleared_And_EventRaised()
        {
            Assert.AreEqual(0, _collection.Count);
            for (int i = 0; i < 50; i++)
                _collection.Add(new ResourceAmountSlot());
            Assert.AreEqual(50, _collection.Count);

            _collection.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
            _collection.Clear();
            Assert.AreEqual(0, _collection.Count);
        }

        [Test]
        public void Contains_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Contains(null));
        }

        [Test]
        public void Contains_ContainsItem_ReturnsTrue()
        {
            var slot = AddSlot();
            Assert.True(_collection.Contains(slot));
        }

        public ResourceAmountSlot AddSlot()
        {
            var slot = new ResourceAmountSlot();
            _collection.Add(slot);
            return slot;
        }
    }
}
