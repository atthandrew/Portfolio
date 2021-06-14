///<author>
///Andrew Thompson u0879848
/// </author>

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        [TestMethod()]
        public void IndexerBeforeKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["s"]);
        }

        [TestMethod()]
        public void IndexerSimpleTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.AddDependency("r", "t");
            Assert.AreEqual(2, t["t"]);
        }

        [TestMethod()]
        public void IndexerAfterRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.RemoveDependency("s", "t");
            Assert.AreEqual(0, t["t"]);
        }

        [TestMethod()]
        public void HasDependentsBeforeKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("s"));
        }

        [TestMethod()]
        public void HasDependentsAfterKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            Assert.IsTrue(t.HasDependents("s"));
        }

        [TestMethod()]
        public void HasDependentsAfterRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.RemoveDependency("s", "t");
            Assert.IsFalse(t.HasDependents("s"));
        }

        [TestMethod()]
        public void HasDependeesBeforeKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("t"));
        }

        [TestMethod()]
        public void HasDependeesAfterKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            Assert.IsTrue(t.HasDependees("t"));
        }

        [TestMethod()]
        public void HasDependeesAfterRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.RemoveDependency("s", "t");
            Assert.IsFalse(t.HasDependees("s"));
        }

        [TestMethod()]
        public void GetDependentsBeforeKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            IEnumerator<string> e = t.GetDependents("s").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        [TestMethod()]
        public void GetDependentsAfterKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "x");
            t.AddDependency("s", "y");
            t.AddDependency("s", "z");
            IEnumerator<string> e = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "x" && s2 == "y" && s3 == "z");
        }

        [TestMethod()]
        public void GetDependentsAfterRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "x");
            t.AddDependency("s", "y");
            t.AddDependency("s", "z");
            t.RemoveDependency("s", "y");
            IEnumerator<string> e = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "x" && s2 == "z");
        }

        [TestMethod()]
        public void GetDependeesBeforeKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            IEnumerator<string> e = t.GetDependees("s").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        [TestMethod()]
        public void GetDependeetsAfterKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "s");
            t.AddDependency("y", "s");
            t.AddDependency("z", "s");
            IEnumerator<string> e = t.GetDependees("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "x" && s2 == "y" && s3 == "z");
        }

        [TestMethod()]
        public void GetDependeesAfterRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "s");
            t.AddDependency("y", "s");
            t.AddDependency("z", "s");
            t.RemoveDependency("y", "s");
            IEnumerator<string> e = t.GetDependees("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "x" && s2 == "z");
        }

        [TestMethod()]
        public void AddDependencyNeitherKeyExistsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            Assert.AreEqual(1, t.Size);

            IEnumerator<string> e = t.GetDependees("t").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s3 == "s");

            IEnumerator<string> e2 = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            String s4 = e2.Current;
            Assert.IsFalse(e2.MoveNext());
            Assert.IsTrue(s4 == "t");
        }

        [TestMethod()]
        public void AddDependencyOnlyDependeeKeyExistsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.AddDependency("s", "r");
            Assert.AreEqual(2, t.Size);

            IEnumerator<string> e = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "t" && s2 == "r");

            IEnumerator<string> e2 = t.GetDependees("t").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            String s3 = e2.Current;
            Assert.IsFalse(e2.MoveNext());
            Assert.IsTrue(s3 == "s");

            IEnumerator<string> e3 = t.GetDependees("r").GetEnumerator();
            Assert.IsTrue(e3.MoveNext());
            String s4 = e3.Current;
            Assert.IsFalse(e3.MoveNext());
            Assert.IsTrue(s4 == "s");
        }

        [TestMethod()]
        public void AddDependencyOnlyDependentKeyExistsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.AddDependency("r", "t");
            Assert.AreEqual(2, t.Size);

            IEnumerator<string> e = t.GetDependees("t").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "s" && s2 == "r");

            IEnumerator<string> e2 = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            String s3 = e2.Current;
            Assert.IsFalse(e2.MoveNext());
            Assert.IsTrue(s3 == "t");

            IEnumerator<string> e3 = t.GetDependents("r").GetEnumerator();
            Assert.IsTrue(e3.MoveNext());
            String s4 = e3.Current;
            Assert.IsFalse(e3.MoveNext());
            Assert.IsTrue(s4 == "t");
        }

        [TestMethod()]
        public void AddDependencyBothKeysExistTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "u");
            t.AddDependency("r", "t");
            t.AddDependency("s", "t");
            Assert.AreEqual(3, t.Size);

            IEnumerator<string> e = t.GetDependees("t").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "r" && s2 == "s");

            IEnumerator<string> e2 = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            String s3 = e2.Current;
            Assert.IsTrue(e2.MoveNext());
            String s4 = e2.Current;
            Assert.IsFalse(e2.MoveNext());
            Assert.IsTrue(s3 == "u" && s4 == "t");

            IEnumerator<string> e3 = t.GetDependents("r").GetEnumerator();
            Assert.IsTrue(e3.MoveNext());
            String s5 = e3.Current;
            Assert.IsFalse(e3.MoveNext());
            Assert.IsTrue(s5 == "t");

            IEnumerator<string> e4 = t.GetDependees("u").GetEnumerator();
            Assert.IsTrue(e4.MoveNext());
            String s6 = e4.Current;
            Assert.IsFalse(e4.MoveNext());
            Assert.IsTrue(s6 == "s");
        }

        [TestMethod()]
        public void AddDependencyPairAlreadyExistsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.AddDependency("s", "t");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Test is to confirm no exception is thrown.
        /// </summary>
        [TestMethod()]
        public void RemoveDependencyBeforeKeyAddedTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("s", "t");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Test is to confirm no exception is thrown.
        /// </summary>
        [TestMethod()]
        public void RemoveDependencyAfterKeyAddedPairDoesNotExistTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "r");
            t.AddDependency("r", "t");
            t.RemoveDependency("s", "t");
            Assert.AreEqual(2, t.Size);
        }

        [TestMethod()]
        public void ReplaceDependentsWithOldDependentsAndNewDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.AddDependency("s", "u");
            t.AddDependency("s", "v");

            string[] newDependents = { "x", "y", "z" };

            t.ReplaceDependents("s", newDependents);
            IEnumerator<string> e = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "x" && s2 == "y" && s3 == "z");
        }

        [TestMethod()]
        public void ReplaceDependentsWithOnlyNewDependents()
        {
            DependencyGraph t = new DependencyGraph();

            string[] newDependents = { "x", "y", "z" };

            t.ReplaceDependents("s", newDependents);
            IEnumerator<string> e = t.GetDependents("s").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s3 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(s1 == "x" && s2 == "y" && s3 == "z");
        }

        /// <summary>
        /// No special handling of null arguments needed, as per the instructions on Piazza.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void ReplaceDependentsWithNullNewDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("s", "t");
            t.AddDependency("s", "u");
            t.AddDependency("s", "v");

            t.ReplaceDependents("s", null);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            t.AddDependency("c", "b");
            Assert.AreEqual(4, t.Size);
        }





        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest4()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("c", "b");
            t.RemoveDependency("a", "d");
            t.AddDependency("e", "b");
            t.AddDependency("b", "d");
            t.RemoveDependency("e", "b");
            t.RemoveDependency("x", "y");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest5()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }



        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

    }
}