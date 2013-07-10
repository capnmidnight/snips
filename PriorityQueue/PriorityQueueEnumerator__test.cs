/*
 * FILE: PriorityQueue.cs
 * AUTHOR: Sean T. McBeth
 * DATE: JAN-25-2007
 */
using System;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;

namespace STM.Common.Tests
{
    /// <summary>
    /// Unit tests for PriorityQueueEnumerator class.
    /// </summary>
    [TestFixture]
    public class PriorityQueueEnumerator__test
    {
        /// <summary>
        /// Constructor for test fixture is called once when the test
        /// assembly is loaded into the testing framework.
        /// </summary>
        public PriorityQueueEnumerator__test()
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

        private PriorityQueue<object> MakeBasicPQ()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            for (int i = 0; i < 3; ++i)
            {
                pq.Enqueue(new object());
            }
            return pq;
        }

        /// <summary>
        /// The enumerator is accessed by calling PriorityQueue.GetEnumerator()
        /// </summary>
        [Test]
        public void GetThroughPriorityQueue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsInstanceOfType(typeof(PriorityQueueEnumerator<object>), en);
        }

        [Test]
        public void ExtendsGenericIEnumerator()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsInstanceOfType(typeof(IEnumerator), en);
        }
        [Test]
        public void ExtendsIEnumerator()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsInstanceOfType(typeof(IEnumerator<object>), en);
        }
        [Test]
        public void ExtendsIDisposable()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsInstanceOfType(typeof(IDisposable), en);
        }
        /// <summary>
        /// Getting an enumerator on a queue with only 1 item
        /// </summary>
        [Test]
        public void MoveNextOnEmptyQueueReturnsFalse()
        {
            PriorityQueue<object> pq = new PriorityQueue<object>();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsFalse(en.MoveNext(), "MoveNext did not return false");
        }

        /// <summary>
        /// Getting an enumerator on a queue with only 1 item
        /// </summary>
        [Test]
        public void MoveNextReturnsTrue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsTrue(en.MoveNext(), "MoveNext did not return true");
        }

        /// <summary>
        /// Moving past the end of the enumerator should return false
        /// </summary>
        [Test]
        public void MoveNextReturnsFalsePastEnd()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            en.MoveNext();
            en.MoveNext();
            Assert.IsFalse(en.MoveNext(), "MoveNext did not return false");
        }

        /// <summary>
        /// MoveNext should throw an exception if the underlying connection is changed
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNextThrowsExceptionAfterEnqueue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            pq.Enqueue(new object());
            en.MoveNext();
        }
        /// <summary>
        /// MoveNext should throw an exception if the underlying connection is changed
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNextThrowsExceptionAfterDequeue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            pq.Dequeue();
            en.MoveNext();
        }
        /// <summary>
        /// MoveNext should throw an exception if the underlying connection is changed
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveNextThrowsExceptionAfterDispose()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator<object> en = pq.GetEnumerator();
            en.MoveNext();
            en.Dispose();
            en.MoveNext();
        }
        /// <summary>
        /// Before MoveNext is called for the first time, Current should throw an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CurrentThrowsExceptionAtBeginning()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            Assert.IsNotNull(en.Current);
        }
        /// <summary>
        /// After MoveNext returns false, Current should throw an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CurrentThrowsExceptionPastEnd()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            while (en.MoveNext()) ;
            Assert.IsNotNull(en.Current);
        }

        /// <summary>
        /// The enumerator should only access items that were in the original queue
        /// </summary>
        [Test]
        public void CurrentGetsItemsThatAreInQueue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            while (en.MoveNext())
            {
                Assert.IsTrue(pq.Contains(en.Current), "found an item that wasn't in the queue");
            }
        }

        /// <summary>
        /// Changing the queue should invalidate the enumeration
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CurrentThrowsExceptionAfterEnqueue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            pq.Enqueue(new object());
            Assert.IsNotNull(en.Current);
        }
        /// <summary>
        /// Changing the queue should invalidate the enumeration
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CurrentThrowsExceptionAfterDequeue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            pq.Dequeue();
            Assert.IsNotNull(en.Current);
        }
        /// <summary>
        /// Changing the queue should invalidate the enumeration
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CurrentThrowsExceptionAfterDispose()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator<object> en = pq.GetEnumerator();
            en.MoveNext();
            en.Dispose();
            Assert.IsNotNull(en.Current);
        }
        /// <summary>
        /// Changing the queue should invalidate the enumeration
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResetThrowsExceptionAfterEnqueue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            pq.Enqueue(new object());
            en.Reset();
        }

        /// <summary>
        /// Changing the queue should invalidate the enumeration
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResetThrowsExceptionAfterDequeue()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            pq.Dequeue();
            en.Reset();
        }

        /// <summary>
        /// Calling reset should return the enumeration to its original state,
        /// allowing access to elements again.
        /// </summary>
        [Test]
        public void ResetReturnsToBeginning()
        {
            PriorityQueue<object> pq = MakeBasicPQ();
            IEnumerator en = pq.GetEnumerator();
            en.MoveNext();
            object first = en.Current;
            while (en.MoveNext()) ;
            en.Reset();
            en.MoveNext();
            Assert.AreSame(first, en.Current);
        }
    }
}
