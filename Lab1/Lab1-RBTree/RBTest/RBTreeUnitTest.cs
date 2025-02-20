namespace RBTest
{
    using Lab1_RBTree;

    internal class RBTreeUnitTest
    {
        RBTree<int> redBlackTree;

        [SetUp]
        public void Setup()
        {
            redBlackTree = new RBTree<int>();
        }

        /*************  TEST METHODS FOR RED-BLACK TREE PROPERTIES  **************/

        [Test]
        public void Insert_AfterInsert_RootIsAlwaysBlack([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createTree(inputSequence);
            Assert.That(redBlackTree.Root.Colour, Is.EqualTo(Colour.Black));
        }

        [Test]

        public void Insert_AfterInsert_AllRedNodesHaveBlackChildren([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createTree(inputSequence);
            HelperFunctionToCheckChildrenOfRedNodes(redBlackTree.Root);
        }

        [Test] 
        public void Insert_AfterInsert_BlackHeightIsEqualInAllNodes([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createTree(inputSequence);
            HelperFunctionToCheckBlackHeightOfNodes(redBlackTree.Root);
        }

        [Test]
        public void Insert_AfterInsert_LeftNodesAreSmallerAndRightNodesAreBigger([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createTree(inputSequence);
            HelperFunctionToCheckValueOfNodesChildren(redBlackTree.Root);
        }

        /*************  TEST METHODS FOR GetEnumerator  **************/

        [Test]
        public void GetEnumerator_WhenTreeWithNodes_ReturnsExpectededElementsInOrder([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            createTree(inputSequence);
            // We must make sure that the sequence we compare the enumerator sequence with only the distinct values, and sorted, from inputSequence
            var distinctInputs = inputSequence.Distinct().ToArray();
            Array.Sort(distinctInputs);
            var elementsInTree = new List<int>();

            // ACT
            var enumerator = redBlackTree.GetEnumerator();  // calls the GetEnumerator() of the tree

            // iterate as long as there are elements in the tree
            while (enumerator.MoveNext())
            {
                elementsInTree.Add(enumerator.Current); // save current element to list
            }

            // ASSERT
            CollectionAssert.AreEqual(distinctInputs, elementsInTree);
        }

        [Test]
        public void GetEnumerator_WhenEmptyTree_ReturnsEmptyCollection()
        {
            // ARRANGE
            var emptyCollection = new List<int>();

            // ACT
            var enumerator = redBlackTree.GetEnumerator();
            while (enumerator.MoveNext())
            {
                emptyCollection.Add(enumerator.Current);
            }

            // ASSERT
            Assert.That(emptyCollection, Is.Empty);
        }

        [Test]
        public void Insert_WhenEmptyTree_RootIsNotNull([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            createTree(inputSequence);

            // ACT + ASSERT
            Assert.That(redBlackTree.Root, Is.Not.Null);
        }

        /*************  HELPER METHODS FOR TESTS  **************/
        private void createTree(int[] inputSequence) { foreach (int element in inputSequence) { redBlackTree.Insert(element); } }

        public void HelperFunctionToCheckChildrenOfRedNodes(RBNode<int> root)
        {
            if (root.Left != null) { HelperFunctionToCheckChildrenOfRedNodes(root.Left); }
            if (root.Colour == Colour.Red)
            {
                Assert.That(root.Left.Colour, Is.EqualTo(Colour.Black));
                Assert.That(root.Right.Colour, Is.EqualTo(Colour.Black));
            }
            if (root.Right != null) { HelperFunctionToCheckChildrenOfRedNodes(root.Right); }
        }
        public void HelperFunctionToCheckBlackHeightOfNodes(RBNode<int> root)
        {
            if (root.Left != null) { HelperFunctionToCheckBlackHeightOfNodes(root.Left); }
            int res1 = HelperFunctionToCheckBlackHeightOfLeftPath(root, 0);
            int res2 = HelperFunctionToCheckBlackHeightOfRightPath(root, 0);

            Assert.That(res1, Is.EqualTo(res2));

            if (root.Right != null) { HelperFunctionToCheckBlackHeightOfNodes(root.Right); }
        }
        public int HelperFunctionToCheckBlackHeightOfLeftPath(RBNode<int> root, int count)
        {
            if (root.Colour == Colour.Black) { count++; }
            if (root.Left != null) { return HelperFunctionToCheckBlackHeightOfLeftPath(root.Left, count); }
            else { return count; }
        }
        public int HelperFunctionToCheckBlackHeightOfRightPath(RBNode<int> root, int count)
        {
            if (root.Colour == Colour.Black) { count++; }
            if (root.Left != null) { return HelperFunctionToCheckBlackHeightOfRightPath(root.Left, count); }
            else { return count; }
        }

        public void HelperFunctionToCheckValueOfNodesChildren(RBNode<int> root)
        {
            bool leftIsSmaller = false;
            bool rightIsBigger = false;

            if (root.Left != RBTree<int>.Nil) { HelperFunctionToCheckValueOfNodesChildren(root.Left); }

            if (root.Left == RBTree<int>.Nil || root.Data.CompareTo(root.Left.Data) > 0) { leftIsSmaller = true; }
            if (root.Right == RBTree<int>.Nil || root.Data.CompareTo(root.Right.Data) < 0) { rightIsBigger = true; }

            Assert.That(leftIsSmaller, Is.EqualTo(rightIsBigger));

            if (root.Right != RBTree<int>.Nil) { HelperFunctionToCheckValueOfNodesChildren(root.Right); }
        }
    }
}
