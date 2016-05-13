using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Streams;

namespace StreamsTest
{
    [TestClass]
    public class StreamTest
    {
        [TestMethod]
        public void EmptyStreamStartsAtEnd()
        {
            Stream<int> stream = new Stream<int>(new int[0]);
            Assert.IsTrue(stream.AtEnd);
        }

        [TestMethod]
        public void EmptyStreamReturnsDefaultValue()
        {
            Stream<int> s1 = new Stream<int>(new int[0]);
            Assert.AreEqual(0, s1.Next());
            Assert.AreEqual(0, s1.Peek());

            Stream<object> s2 = new Stream<object>(new object[0]);
            Assert.IsNull(s2.Next());
            Assert.IsNull(s2.Peek());
        }

        [TestMethod]
        public void SendingNextToEmptyStreamAlwaysReturnsDefaultValue()
        {
            Stream<object> stream = new Stream<object>(new object[0]);
            Assert.IsNull(stream.Next());
            Assert.IsNull(stream.Next());
            Assert.IsNull(stream.Next());
        }

        [TestMethod]
        public void NextAdvancesTheStreamPosition()
        {
            Stream<int> stream = new Stream<int>(new int[3] { 1, 2, 3 });
            Assert.AreEqual(0, stream.Position);
            stream.Next();
            Assert.AreEqual(1, stream.Position);
        }

        [TestMethod]
        public void NextAdvancesTheStreamPositionUntilTheEnd()
        {
            Stream<int> stream = new Stream<int>(new int[3] { 1, 2, 3 });
            stream.Next();
            stream.Next();
            stream.Next();
            Assert.IsTrue(stream.AtEnd);
            stream.Next();
            Assert.IsTrue(stream.AtEnd);
        }

        [TestMethod]
        public void PeekDoesNotAdvancesTheStreamPosition()
        {
            Stream<int> stream = new Stream<int>(new int[3] { 1, 2, 3 });
            Assert.AreEqual(0, stream.Position);
            stream.Peek();
            Assert.AreEqual(0, stream.Position);
        }

        [TestMethod]
        public void NextReturnsTheStreamElementsInOrder()
        {
            Stream<int> stream = new Stream<int>(new int[3] { 1, 2, 3 });
            Assert.AreEqual(1, stream.Next());
            Assert.AreEqual(2, stream.Next());
            Assert.AreEqual(3, stream.Next());
        }

        [TestMethod]
        public void UpToReturnsTheElementsFromTheCurrentPositionToTheFirstOccurrence()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 10));
            IEnumerable<int> expected = new int[2] { 2, 3 };
            stream.Next(); // Discard first
            Assert.IsTrue(expected.SequenceEqual(stream.UpTo(4)));
        }

        [TestMethod]
        public void UpToLimitIsNotInclusive()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 10));
            stream.UpTo(4);
            Assert.AreEqual(5, stream.Next());
        }

        [TestMethod]
        public void UpToStopsAtTheEndIfOccurrenceNotFound()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 3));
            IEnumerable<int> expected = new int[3] { 1, 2, 3 };
            Assert.IsTrue(expected.SequenceEqual(stream.UpTo(11)));
        }

        [TestMethod]
        public void UpToEndReturnsTheElementsFromCurrentPositionToTheEnd()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 5));
            IEnumerable<int> expected = new int[3] { 3, 4, 5 };
            stream.Next(); // Discard first
            stream.Next(); // Discard second
            Assert.IsTrue(expected.SequenceEqual(stream.UpToEnd()));
        }

        [TestMethod]
        public void UpToAlsoAcceptsFunctionsAsParameter()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 5));
            Func<int, bool> even = (n) => n % 2 == 0;
            Assert.IsTrue(stream.UpTo(even).SequenceEqual(new int[1] { 1 }));
            Assert.IsTrue(stream.UpTo(even).SequenceEqual(new int[1] { 3 }));
            Assert.IsTrue(stream.UpTo(even).SequenceEqual(new int[1] { 5 }));
            Assert.IsTrue(stream.UpTo(even).SequenceEqual(new int[0]));
        }

        [TestMethod]
        public void UpToAcceptsANullParameter()
        {
            Stream<string> stream = new Stream<string>(new string[3] { "a", null, "b" });
            Assert.IsTrue(stream.UpTo((string)null).SequenceEqual(new string[1] { "a" }));
        }

        [TestMethod]
        public void NextAcceptsAnIntegerArgAndReturnsAsManyElementsAsTheArgSays()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 10));
            Assert.IsTrue(stream.Next(3).SequenceEqual(new int[3] { 1, 2, 3 }));
            Assert.IsTrue(stream.Next(2).SequenceEqual(new int[2] { 4, 5 }));
        }

        [TestMethod]
        public void IfNextReachesTheEndItStopsRightThereRegardlessOfTheCountParameter()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 10));
            stream.Next(5); // Discard the first half
            Assert.IsTrue(stream.Next(10).SequenceEqual(new int[5] { 6, 7, 8, 9, 10 }));
        }

        [TestMethod]
        public void SkipAllowsMeToQuicklyDiscardElements()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 10));
            stream.Skip(5); // Discard the first half
            Assert.AreEqual(5, stream.Position);
            Assert.AreEqual(6, stream.Next());
        }

        [TestMethod]
        public void MovingOverTheEndShouldNotIncrementThePosition()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 3));
            stream.Next(10);
            Assert.AreEqual(3, stream.Position);
        }

        [TestMethod]
        public void SkippingOverTheEndShouldNotIncrementThePosition()
        {
            Stream<int> stream = new Stream<int>(Enumerable.Range(1, 3));
            stream.Skip(10);
            Assert.AreEqual(3, stream.Position);
        }
    }
}
