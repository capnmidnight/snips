/*
 * FILE: PriorityQueue__test.cs
 * AUTHOR: Sean T. McBeth
 * DATE: JAN-24-2007
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace STM.Common.Tests
{
    /// <summary>
    /// Unit tests for PriorityQueue class.
    /// </summary>
    [TestFixture]
    public class PriorityQueue__test
    {
        /// <summary>
        /// Constructor for test fixture is called once when the test
        /// assembly is loaded into the testing framework.
        /// </summary>
        public PriorityQueue__test()
        {
            //empty
        }

        /// <summary>
        /// Initialize is called once before the test run begins. 
        /// It is not called before each individual test. This setup
        /// method may be called multiple times as multiple test
        /// runs are issued from the testing framework.
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            //empty
        }

        /// <summary>
        /// Teardown is called once after the completion of the
        /// test run. It is not called after each individual test.
        /// This teardown method may be called multiple times as
        /// multiple test runs are issued from the testing framework.
        /// </summary>
        [TestFixtureTearDown]
        public void Teardown()
        {
            //empty
        }

        /// <summary>
        /// To be a proper collection, it should extend the IEnumberable interface
        /// </summary>
        [Test]
        public void ExtendsIEnumerable()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsInstanceOfType(typeof(IEnumerable), pq);
        }

        [Test]
        public void ExtendsGenericIEnumerable()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsInstanceOfType(typeof(IEnumerable<object>), pq);
        }

        [Test]
        public void ExtendsICollection()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsInstanceOfType(typeof(ICollection), pq);
        }

        /// <summary>
        /// The queue needs a simple default constructor
        /// </summary>
        [Test]
        public void ConstructorSimple()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsNotNull(pq);
        }

        /// <summary>
        /// The queue needs a constructor that takes a Comparer
        /// </summary>
        [Test]
        public void ConstructorWithComparer()
        {
            IComparer<object> comp = new MockComparer();
            PriorityQueue<object> pq = new PriorityQueue<object>(comp);
            Assert.IsNotNull(pq);
        }

        /// <summary>
        /// using a null comparer parameter should throw a NullReferenceException
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorWithNullComparer()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>(null);
            Assert.IsNotNull(pq);
        }

        /// <summary>
        /// The queue should return the Comparer that it was created with.
        /// </summary>
        [Test]
        public void ComparerPropertyExplicit()
        {
            IComparer<object> comp = new MockComparer();
            PriorityQueue<object> pq = new PriorityQueue<object>(comp);
            Assert.AreSame(comp, pq.Comparer);
        }

        /// <summary>
        /// The queue should return a Comparer for the type it was created for.
        /// </summary>
        [Test]
        public void ComparerPropertyImplicit1()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsInstanceOfType(typeof(IComparer<object>), pq.Comparer);
        }

        /// <summary>
        /// The queue should return a Comparer for the type it was created for.
        /// </summary>
        [Test]
        public void DefaultComparerReturnsSameResultsAsNormalComparer()
        {
            PriorityQueue<int> pq = new PriorityQueue<int>();
            IComparer<int> comp = pq.Comparer;
            int x = 11;
            int y = 13;
            Assert.AreEqual(x.CompareTo(y), comp.Compare(y, x));
        }
        /// <summary>
        /// When first created, the Count should return 0
        /// </summary>
        [Test]
        public void InitiallyEmpty()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.AreEqual(0, pq.Count);
        }

        /// <summary>
        /// Adding items to the queue should increase the Count of the queue
        /// </summary>
        [Test]
        public void EnqueueIncreasesCount()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            Assert.AreEqual(1, pq.Count);
        }

        /// <summary>
        /// Dequeuing an item should decrease the Count of the queue
        /// </summary>
        [Test]
        public void DequeueDecreasesCount()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            pq.Enqueue(new object());
            pq.Enqueue(new object());
            Assert.AreEqual(3, pq.Count);
            pq.Dequeue();
            Assert.AreEqual(2, pq.Count);
        }

        /// <summary>
        /// Trying to enqueue a null object should throw an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void EnqueueingNullThrowsException()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(null);
        }

        /// <summary>
        /// Dequeueing an empty queue should throw an Exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DequeueThrowsExceptionOnEmptyQueue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Dequeue();
        }

        /// <summary>
        /// Peeking an empty queue should thow an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PeekThrowsExceptionOnEmptyQueue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Peek();
        }

        /// <summary>
        /// Peeking should show the first element in the queue
        /// </summary>
        [Test]
        public void PeekGetsTheOnlyItemInQueue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            object obj = new object();
            pq.Enqueue(obj);
            Assert.AreSame(obj, pq.Peek());
        }
        
        /// <summary>
        /// Peeking should not change the size of the queue
        /// </summary>
        [Test]
        public void PeekDoesntChangeCount()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            object obj = new object();
            pq.Enqueue(obj);
            pq.Peek();
            Assert.AreEqual(1, pq.Count);
        }

        /// <summary>
        /// Peeking before Dequeueing should return the same object
        /// </summary>
        [Test]
        public void PeekRetrievesTheSameItemAsDequeue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            Assert.AreSame(pq.Peek(), pq.Dequeue());
        }

        /// <summary>
        /// Non-comparable objects placed in the priority queue should make the PQ act like a normal queue
        /// </summary>
        [Test]
        public void NonComparableObjectsCreatesNormalQueue()
        {
            object o1, o2, o3;
            o1 = new object();
            o2 = new object();
            o3 = new object();

            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(o1);
            pq.Enqueue(o2);
            pq.Enqueue(o3);

            Assert.AreSame(o1, pq.Dequeue());
            Assert.AreSame(o2, pq.Dequeue());
            Assert.AreSame(o3, pq.Dequeue());
        }

        /// <summary>
        /// A priority queue on integers should result in a sorted list when sequentially
        /// dequeueing the items.
        /// </summary>
        [Test]
        public void PQSortsInts()
        {
            PriorityQueue<int> pq = new PriorityQueue<int>();
            Random rand = new Random();
            for (int i = 0; i < 10; ++i)
            {
                pq.Enqueue(rand.Next(10));
            }
            int last = -1;
            while (pq.Count > 0)
            {
                int i = pq.Dequeue();
                Console.WriteLine("{0} -> {1}", last, i);
                Assert.IsTrue(last <= i, "items out of sequence in queue");
                last = i;
            }
        }

        /// <summary>
        /// A queue of strings should sort the strings lexicographically (aka "alphabetically")
        /// </summary>
        [Test]
        public void PQSortsStringsLexicographically()
        {
            PriorityQueue<string> pq = new PriorityQueue<string>();
            pq.Enqueue("Hello");
            pq.Enqueue("Bob");
            pq.Enqueue("World");
            pq.Enqueue("tOM");
            Assert.AreEqual("Bob", pq.Dequeue());
            Assert.AreEqual("Hello", pq.Dequeue());
            Assert.AreEqual("tOM", pq.Dequeue());
            Assert.AreEqual("World", pq.Dequeue());
        }

        /// <summary>
        /// Enqueueing a single object should return that object when it is dequeued
        /// </summary>
        [Test]
        public void EnqueueDequeueOneItem()
        {
            object obj = new object();
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(obj);
            Assert.AreSame(obj, pq.Dequeue());
        }

        /// <summary>
        /// Clearing the queue should reset the count to 0
        /// </summary>
        [Test]
        public void ClearResetsCountToZero()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            for (int i = 0; i < 10; ++i) pq.Enqueue(new object());
            pq.Clear();
            Assert.AreEqual(0, pq.Count);
        }

        /// <summary>
        /// Clearing an empty queue should have no effect
        /// </summary>
        [Test]
        public void ClearHasNoEffectOnEmptyQueue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Clear();
            Assert.AreEqual(0, pq.Count);
        }

        /// <summary>
        /// Checking for an object in an empty queue should return false
        /// </summary>
        [Test]
        public void ContainsReturnsFalseOnEmpty()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsFalse(pq.Contains(new object()));
        }
        /// <summary>
        /// Checking for an object that isn't in the queue should return false
        /// </summary>
        [Test]
        public void ContainsReturnsFalseOnNonExistant()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            Assert.IsFalse(pq.Contains(new object()));
        }

        /// <summary>
        /// Checking for an ojbect that is in the queue should return true
        /// </summary>
        [Test]
        public void ContainsFindsReferenceTypes()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            object o = new object();
            pq.Enqueue(new object());
            pq.Enqueue(o);
            pq.Enqueue(new object());
            Assert.IsTrue(pq.Contains(o));
        }

        /// <summary>
        /// Checking for an ojbect that is in the queue should return true
        /// </summary>
        [Test]
        public void ContainsFindsValueTypes()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(1);
            pq.Enqueue(1);
            pq.Enqueue(1);
            pq.Enqueue(2);
            pq.Enqueue(2);
            pq.Enqueue(3);
            pq.Enqueue(3);
            pq.Enqueue(3);
            Assert.IsTrue(pq.Contains(2));
        }

        /// <summary>
        /// Getting an enumerator on an empty queue should not be null
        /// </summary>
        [Test]
        public void GetEnumeratorOnEmpty()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsNotNull(pq.GetEnumerator());
        }

        /// <summary>
        /// Copy the queue to an array from the very beginning
        /// </summary>
        [Test]
        public void CopyToWithOneElement()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            object[] arr = new object[1];
            pq.CopyTo(arr, 0);
            Assert.AreSame(pq.Peek(), arr[0]);
        }
        /// <summary>
        /// Copy the queue to an array from the very beginning
        /// </summary>
        [Test]
        public void CopyToWithOneElementAndOffset()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            object[] arr = new object[2];
            pq.CopyTo(arr, 1);
            Assert.IsNull(arr[0]);
            Assert.AreSame(pq.Peek(), arr[1]);
        }
        /// <summary>
        /// Copy the queue to an array from the very beginning
        /// </summary>
        [Test]
        public void CopyToWithTenElements()
        {
            PriorityQueue<int> pq = new PriorityQueue<int>();
            Random r = new Random();
            for (int i = 0; i < 10; ++i)
            {
                pq.Enqueue(r.Next(10));
            }
            int[] arr = new int[10];
            pq.CopyTo(arr, 0);
            for(int i = 0; i < arr.Length; ++i)
            {
                Assert.IsTrue(pq.Contains(arr[i]));
            }
        }
        /// <summary>
        /// Copy the queue to an array from the very beginning
        /// </summary>
        [Test]
        public void CopyToWithTenElementsWithOffset()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            for (int i = 0; i < 10; ++i)
            {
                pq.Enqueue(new object());
            }
            object[] arr = new object[13];
            pq.CopyTo(arr, 3);
            for (int i = 0; i < arr.Length; ++i)
            {
                if (i < 3)
                {
                    Assert.IsNull(arr[i]);
                }
                else
                {
                    Assert.IsTrue(pq.Contains(arr[i]));
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToThrowsExceptionFromNullArray()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            pq.CopyTo(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToThrowsExceptionIfIndexIsLessThanZero()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            object[] arr = new object[1];
            pq.CopyTo(arr, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToThrowsExceptionIfIndexIsPastEnd()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            object[] arr = new object[1];
            pq.CopyTo(arr, 1);
        }
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToThrowsExceptionIfArrayIsTooSmall()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            pq.Enqueue(new object());
            pq.Enqueue(new object());
            pq.Enqueue(new object());
            object[] arr = new object[1];
            pq.CopyTo(arr, 0);
        }

        [Test]
        public void SyncRootReturnsNonNullReference()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsNotNull(pq.SyncRoot);
        }

        [Test]
        public void SyncRootReturnsUniqueReferences()
        {
            PriorityQueue<object> pq1 = new PriorityQueue<object>();
            PriorityQueue<object> pq2 = new PriorityQueue<object>();
            Assert.AreNotSame(pq1.SyncRoot, pq2.SyncRoot);
        }

        /// <summary>
        /// Since the queue is thread safe, it should always return true for IsSynchronized
        /// </summary>
        [Test]
        public void IsSynchronizedReturnsTrue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            Assert.IsTrue(pq.IsSynchronized);
        }
        /// <summary>
        /// The queue should support a means for creating a raw array of the items contained within
        /// </summary>
        [Test]
        public void CreateArrayFromQueue()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            for (int i = 0; i < 10; ++i)
            {
                pq.Enqueue(new object());
            }
            object[] arr = pq.ToArray();
            for(int i = 0; i < 10; ++i)
            {
                Assert.IsTrue(pq.Contains(arr[i]));
            }
        }
    }
    class MockComparer : IComparer<object>
    {
        public int Compare(object obj1, object obj2)
        {
            return 0;
        }
    }
}