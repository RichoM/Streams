﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Streams;
using System.Text;

namespace StreamsTest
{
    [TestClass]
    public class TextStreamTest
    {
        [TestMethod]
        public void TextStreamsCanUseIOStreamsAsSource()
        {
            byte[] bytes = Encoding.Default.GetBytes("ABC");
            System.IO.MemoryStream iostream = new System.IO.MemoryStream(bytes);
            using (TextStream stream = new TextStream(iostream))
            {
                Assert.AreEqual('A', stream.Next());
                Assert.AreEqual('B', stream.Next());
                Assert.AreEqual('C', stream.Next());
                Assert.IsTrue(stream.AtEnd);
            }
        }

        [TestMethod]
        public void TextStreamsCanAlsoUseTextReadersAsSource()
        {
            System.IO.TextReader reader = new System.IO.StringReader("ABC");
            using (TextStream stream = new TextStream(reader))
            {
                Assert.AreEqual('A', stream.Next());
                Assert.AreEqual('B', stream.Next());
                Assert.AreEqual('C', stream.Next());
                Assert.IsTrue(stream.AtEnd);
            }
        }

        [TestMethod]
        public void UpToEndReturnsAStringInsteadOfIEnumerable()
        {
            using (TextStream stream = new TextStream("ABC"))
            {
                Assert.AreEqual("ABC", stream.UpToEnd());
            }
        }

        [TestMethod]
        public void UpToReturnsAStringInsteadOfIEnumerable()
        {
            using (TextStream stream = new TextStream("ABCDEFG"))
            {
                Assert.AreEqual("ABC", stream.UpTo('D'));
            }
        }

        [TestMethod]
        public void NextWithCountArgReturnsAStringInsteadOfIEnumerable()
        {
            using (TextStream stream = new TextStream("ABCDEFG"))
            {
                Assert.AreEqual("ABC", stream.Next(3));
            }
        }

        [TestMethod]
        public void TextStreamsIncludeNewLineCharacters()
        {
            using(TextStream stream = new TextStream("ABC\nDEF\r\nGHI"))
            {
                stream.Skip(3);
                Assert.AreEqual('\n', stream.Next());
                stream.Skip(3);
                Assert.AreEqual("\r\n", stream.Next(2));
            }
        }

        [TestMethod]
        public void TextStreamsUpToNewLineWorksWithAnyNewLineCharacters()
        {
            using (TextStream stream = new TextStream("ABC\nDEF\r\nGHI"))
            {
                Assert.AreEqual("ABC", stream.UpToNewLine());
                Assert.AreEqual("DEF", stream.UpToNewLine());
                Assert.AreEqual("GHI", stream.UpToNewLine());
                Assert.IsTrue(stream.AtEnd);
            }
        }
    }
}
