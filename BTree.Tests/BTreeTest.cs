using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using BTree;

namespace BTree.Tests
{
    [TestClass]
    public class BTreeTest
    {
        [TestMethod]
        public void TestIsEmpty()
        {
            BTree<string, string> tree = new BTree<string, string>();
            Assert.IsTrue(tree.IsEmpty());
        }

        [TestMethod]
        public void TestSize()
        {
            BTree<string, string> tree = new BTree<string, string>();
            Assert.AreEqual(0, tree.Size());

            tree.Put("foo", "bar");
            Assert.AreEqual(1, tree.Size());
        }

        [TestMethod]
        public void TestHeight()
        {
            BTree<string, int> tree = new BTree<string, int>();

            Assert.AreEqual(0, tree.Height());
            tree.Put("a", 1);
            tree.Put("b", 2);
            tree.Put("c", 3);
            tree.Put("d", 4);
            Assert.AreEqual(1, tree.Height());
            tree.Put("e", 5);
            tree.Put("f", 6);
            tree.Put("g", 7);
            tree.Put("h", 8);
            Assert.AreEqual(2, tree.Height());
            tree.Put("i", 9);
            tree.Put("j", 10);
            tree.Put("k", 11);
            tree.Put("l", 12);
            tree.Put("m", 13);
            tree.Put("n", 14);
            tree.Put("o", 15);
            tree.Put("p", 16);
            Assert.AreEqual(3, tree.Height());
        }

        [TestMethod]
        public void TestGet_ReferenceType()
        {
            BTree<string, string> tree = new BTree<string, string>();
            tree.Put("foo", "bar");

            Assert.AreEqual("bar", tree.Get("foo"));
            Assert.IsNull(tree.Get("baz"));
        }

        [TestMethod]
        public void TestGet_ValueType()
        {
            BTree<string, int> tree = new BTree<string, int>();
            tree.Put("foo", 25);

            Assert.AreEqual(25, tree.Get("foo"));
            Assert.AreEqual(0, tree.Get("baz"));
        }

        [TestMethod]
        public void TestGet_NullThrowsException()
        {
            BTree<string, int> tree = new BTree<string, int>();
            Assert.ThrowsException<ArgumentException>(() => tree.Get(null));
        }

        [TestMethod]
        public void TestPut()
        {
            BTree<string, string> tree = new BTree<string, string>();
            tree.Put("a", "1");
            tree.Put("b", "2");
            tree.Put("c", "3");

            Assert.AreEqual(3, tree.Size());
        }

        [TestMethod]
        public void TestPut_NullKeyThrowsException()
        {
            BTree<string, string> tree = new BTree<string, string>();
            Assert.ThrowsException<ArgumentException>(() => tree.Put(null, "null"));
        }
    }
}
