namespace RBTest
{
    using Lab1_RBTree;

    public class OrderedSetUnitTest
    {
        RBTree<int> orderedSet;
        RBTree<int> otherOrderedSet;

        [SetUp]
        public void Setup()
        {
            orderedSet = new RBTree<int>();
            otherOrderedSet = new RBTree<int>();
        }

        /*************  TEST METHODS FOR MINIMUM  **************/

        [Test]
        public void Minimum_InputIsEmpty_ReturnsNull()
        {
            Assert.That(orderedSet.Minimum(), Is.Null);
        }

        [Test]
        public void Minimum_InputIsNotEmpty_ReturnsMinimum([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);

            Assert.That(orderedSet.Minimum(), Is.EqualTo(inputSequence.Min()));
        }

        /*************  TEST METHODS FOR MAXIMUM  **************/

        [Test]
        public void Maximum_InputIsEmpty_ReturnsNull()
        {
            Assert.That(orderedSet.Maximum(), Is.Null);
        }

        [Test]
        public void Maximum_InputIsNotEmpty_ReturnsMaximum([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Maximum(), Is.EqualTo(inputSequence.Max()));
        }

        /*************  TEST METHODS FOR SUCCESSOR  **************/

        [Test]
        public void Successor_InputIsEmpty_ReturnsNull()
        {
            Assert.That(orderedSet.Successor(default), Is.Null);
        }
        [Test]
        public void Successor_SuccesorDoesNotExistButElementExists_ReturnsNull([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Successor(inputSequence.Max()), Is.Null);
        }
        [Test]
        public void Successor_SuccesorAndElementDoesNotExist_ReturnsNull([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Successor(inputSequence.Max() + 1), Is.Null);
        }
        // The below test implicitly test both the cases when an inputted element is part of the set and when it is not part of the set, depending on the input sequence
        // For a sequence of {1, 2, 3}, inputSequence.Max() - 1 = 2 which exists in the set, and 3 should be returned
        // For a sequence of {0, 1, 3}, inputSequence.Max() - 1 = 2 which does not exists in the set, but should still return 3
        [Test]
        public void Successor_SuccessorExists_ReturnsSuccessor([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Successor(inputSequence.Max() - 1), Is.EqualTo(inputSequence.Max()));
        }

        /*************  TEST METHODS FOR PREDECESSOR  **************/

        [Test]
        public void Predecessor_InputIsEmpty_ReturnsNull()
        {
            Assert.That(orderedSet.Predecessor(default), Is.Null);
        }
        [Test]
        public void Predecessor_PredecessorDoesNotExistButElementExists_ReturnsNull([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Predecessor(inputSequence.Min()), Is.Null);
        }
        [Test]
        public void Predecessor_PredecessorAndElementDoesNotExist_ReturnsNull([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Predecessor(inputSequence.Min() - 1), Is.Null);
        }
        // The below test implicitly test both the cases when an inputted element is part of the set and when it is not part of the set, depending on the input sequence
        // For a sequence of {1, 2, 3}, inputSequence.Min() + 1 = 2 which exists in the set, and 1 should be returned
        // For a sequence of {1, 3, 4}, inputSequence.Max() + 1 = 2 which does not exists in the set, but should still return 1
        [Test]
        public void Predecessor_PredecessorExists_ReturnsPredecessor([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);
            Assert.That(orderedSet.Predecessor(inputSequence.Min() + 1), Is.EqualTo(inputSequence.Min()));
        }

        /*************  TEST METHODS FOR UNIONWITH  **************/

        [Test]
        public void UnionWith_CurrentSetIsEmpty_ReturnsInputSet([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, otherOrderedSet);

            orderedSet.UnionWith(otherOrderedSet);
            Assert.That(orderedSet, Is.EqualTo(otherOrderedSet));
        }

        [Test]
        public void UnionWith_BothSetsNotEmpty_ReturnsUnion([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);

            int[] inputSequence2 = { 1, 4, 12, 15 };
            createSet(inputSequence2, otherOrderedSet);

            // Creating an array containing all elements from both input sequences, without duplicates. The resulting tree is compared against this array
            int[] array = inputSequence.Union(inputSequence2).ToArray();
            // Sort the array, since it will be compared to orderedSet in ascending order
            Array.Sort(array);

            orderedSet.UnionWith(otherOrderedSet);
            Assert.That(orderedSet, Is.EqualTo(array));
        }
        [Test]
        public void UnionWith_InputSetIsEmpty_ReturnsCurrentSet([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            createSet(inputSequence, orderedSet);

            // Sort the input sequence, since it will be compared to orderedSet in ascending order
            int[] newArray = inputSequence.Distinct().ToArray();
            Array.Sort(newArray);

            orderedSet.UnionWith(otherOrderedSet);
            Assert.That(orderedSet, Is.EqualTo(newArray));
        }
        [Test]
        public void UnionWith_BothSetsAreEmpty_ReturnsEmpty()
        {
            orderedSet.UnionWith(otherOrderedSet);
            Assert.That(orderedSet, Is.Empty);
        }

        /*************  TEST METHODS FOR COUNT PROPERTY  **************/

        [Test]
        // This method asserts that the Count property is 0 on object initialisation, when set is empty
        public void Count_WhenSetIsEmpty_ShouldReturnZero()
        {
            // ARRANGE

            // ACT + ASSERT
            Assert.That(orderedSet.Count, Is.EqualTo(0));
        }

        [Test]
        // This method asserts that Count holds the same value as number of elements inserted to the set
        // We use an input sequence, provides more flexibility
        public void Count_WhenLengthNumberElementsAreInserted_ShouldReturnSameNumberAsLength([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            var expectedCount = inputSequence.Distinct().Count();
            createSet(inputSequence, orderedSet);

            // ACT + ASSERT
            Assert.That(orderedSet.Count, Is.EqualTo(expectedCount));

        }

        /*************  TEST METHODS FOR SEARCH  **************/

        [Test]
        public void Search_WhenEmptySet_ShouldReturnFalse()
        {
            // ARRANGE
            var elementNotInEmptySet = 10;

            // ACT + ASSERT
            Assert.That(orderedSet.Search(elementNotInEmptySet), Is.False);
        }

        [Test]
        public void Search_WhenElementExistsInSet_ShouldReturnTrue([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            // Pick an element that exists in the sequence (all sequences in the inputSequence contain at least one element)
            var existingElement = inputSequence[0];
            createSet(inputSequence, orderedSet);

            // ACT + ASSERT
            Assert.That(orderedSet.Search(existingElement), Is.True);
        }

        [Test]
        public void Search_WhenElementDoesNotExist_ShouldReturnFalse([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            var noExistingElement = getElementThatDoesNotExistInSet(inputSequence);
            createSet(inputSequence, orderedSet);

            // ACT + ASSERT
            Assert.That(orderedSet.Search(noExistingElement), Is.False);
        }

        [Test]
        public void Search_WhenSearchingForMinimumBoundaryValue_ShouldReturnTrue([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE 
            var minElement = inputSequence.Min();
            createSet(inputSequence, orderedSet);

            // ACT + ASSERT
            // Should be true for the minimum element as well, boundary value
            Assert.That(orderedSet.Search(minElement), Is.True);
        }

        [Test]
        public void Search_WhenSearchingForMaximumBoundaryValue_ShouldReturnTrue([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE 
            var maxElement = inputSequence.Max();
            createSet(inputSequence, orderedSet);

            // ACT + ASSERT
            // Should be true for the minimum element as well, boundary value
            Assert.That(orderedSet.Search(maxElement), Is.True);
        }


        /*************  TEST METHODS FOR INSERT  **************/

        [Test]
        // We assert that it is possible to add elements to an empty set
        public void Insert_WhenSetIsEmpty_ShouldAddToSet()
        {
            // ARRANGE

            // ACT
            orderedSet.Insert(5);

            // ASSERT
            Assert.That(orderedSet.Count, Is.EqualTo(1));
        }

        [Test]
        public void Insert_NewElementToSet_ShouldAddToSet([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            createSet(inputSequence, orderedSet);
            var newElement = getElementThatDoesNotExistInSet(inputSequence);

            // ACT
            // Insert new element, element 100
            orderedSet.Insert(newElement);

            // ASSERT
            Assert.IsTrue(orderedSet.Contains(newElement));
        }

        [Test]
        public void Insert_DuplicateElement_ShouldNotBeAdded([ValueSource(typeof(TestInputSequences), nameof(TestInputSequences.InputSequences))] int[] inputSequence)
        {
            // ARRANGE
            var elementToInsert = getElementThatDoesNotExistInSet(inputSequence);
            createSet(inputSequence, orderedSet);
            var countBefore = orderedSet.Count;

            // ACT
            // Inserting two elements of the same value, duplicates. Only one should be inserted
            orderedSet.Insert(elementToInsert);
            orderedSet.Insert(elementToInsert);

            // ASSERT
            Assert.That(orderedSet.Count, Is.EqualTo(countBefore + 1));
        }

        /*************  HELPER METHODS FOR TESTS  **************/

        // Helper method for inserting elements of a sequence into a set
        private void createSet(int[] inputSequence, RBTree<int> set) { foreach (int element in inputSequence) { set.Insert(element); } }

        // Find a random element that does not exist in the input sequence
        private int getElementThatDoesNotExistInSet(int[] inputSequence)
        {
            int nonExistingElement;
            var random = new Random();

            // get a new randomised value as long as it exists in the inputSequence
            do
            {
                nonExistingElement = random.Next();
            } while (inputSequence.Contains<int>(nonExistingElement));

            return nonExistingElement;
        }
    }
}