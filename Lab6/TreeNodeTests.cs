using NUnit.Framework;
using HashTableLiterature.DataStructures;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class TreeNodeTests
    {
        [Test]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            string key = "TestKey";
            string value = "TestValue";

            // Act
            var node = new TreeNode<string, string>(key, value);

            // Assert
            Assert.That(node.Key, Is.EqualTo(key));
            Assert.That(node.Value, Is.EqualTo(value));
            Assert.That(node.Left, Is.Null);
            Assert.That(node.Right, Is.Null);
            Assert.That(node.Height, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_WithNullKey_SetsKeyToNull()
        {
            // Arrange & Act
            var node = new TreeNode<string, string>(null, "TestValue");

            // Assert
            Assert.That(node.Key, Is.Null);
            Assert.That(node.Value, Is.EqualTo("TestValue"));
            Assert.That(node.Height, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_WithNullValue_SetsValueToNull()
        {
            // Arrange & Act
            var node = new TreeNode<string, string>("TestKey", null);

            // Assert
            Assert.That(node.Key, Is.EqualTo("TestKey"));
            Assert.That(node.Value, Is.Null);
            Assert.That(node.Height, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_WithBothNullParameters_SetsBothToNull()
        {
            // Arrange & Act
            var node = new TreeNode<string, string>(null, null);

            // Assert
            Assert.That(node.Key, Is.Null);
            Assert.That(node.Value, Is.Null);
            Assert.That(node.Height, Is.EqualTo(1));
        }

        [Test]
        public void Properties_CanBeModified()
        {
            // Arrange
            var node = new TreeNode<string, string>("OriginalKey", "OriginalValue");
            var leftChild = new TreeNode<string, string>("LeftKey", "LeftValue");
            var rightChild = new TreeNode<string, string>("RightKey", "RightValue");

            // Act
            node.Key = "ModifiedKey";
            node.Value = "ModifiedValue";
            node.Left = leftChild;
            node.Right = rightChild;
            node.Height = 5;

            // Assert
            Assert.That(node.Key, Is.EqualTo("ModifiedKey"));
            Assert.That(node.Value, Is.EqualTo("ModifiedValue"));
            Assert.That(node.Left, Is.EqualTo(leftChild));
            Assert.That(node.Right, Is.EqualTo(rightChild));
            Assert.That(node.Height, Is.EqualTo(5));
        }

        [Test]
        public void Left_CanBeSetToNull()
        {
            // Arrange
            var node = new TreeNode<string, string>("Key", "Value");
            var leftChild = new TreeNode<string, string>("LeftKey", "LeftValue");
            node.Left = leftChild;

            // Act
            node.Left = null;

            // Assert
            Assert.That(node.Left, Is.Null);
        }

        [Test]
        public void Right_CanBeSetToNull()
        {
            // Arrange
            var node = new TreeNode<string, string>("Key", "Value");
            var rightChild = new TreeNode<string, string>("RightKey", "RightValue");
            node.Right = rightChild;

            // Act
            node.Right = null;

            // Assert
            Assert.That(node.Right, Is.Null);
        }

        [Test]
        public void Height_CanBeSetToZero()
        {
            // Arrange
            var node = new TreeNode<string, string>("Key", "Value");

            // Act
            node.Height = 0;

            // Assert
            Assert.That(node.Height, Is.EqualTo(0));
        }

        [Test]
        public void Height_CanBeSetToNegativeValue()
        {
            // Arrange
            var node = new TreeNode<string, string>("Key", "Value");

            // Act
            node.Height = -1;

            // Assert
            Assert.That(node.Height, Is.EqualTo(-1));
        }

        [Test]
        public void Constructor_WithIntegerTypes_WorksCorrectly()
        {
            // Arrange & Act
            var node = new TreeNode<int, int>(42, 100);

            // Assert
            Assert.That(node.Key, Is.EqualTo(42));
            Assert.That(node.Value, Is.EqualTo(100));
            Assert.That(node.Height, Is.EqualTo(1));
            Assert.That(node.Left, Is.Null);
            Assert.That(node.Right, Is.Null);
        }

        [Test]
        public void Constructor_WithComplexTypes_WorksCorrectly()
        {
            // Arrange
            var keyObject = new { Id = 1, Name = "Test" };
            var valueObject = new { Data = "TestData", Count = 5 };

            // Act
            var node = new TreeNode<object, object>(keyObject, valueObject);

            // Assert
            Assert.That(node.Key, Is.EqualTo(keyObject));
            Assert.That(node.Value, Is.EqualTo(valueObject));
            Assert.That(node.Height, Is.EqualTo(1));
        }

        [Test]
        public void TreeStructure_CanBeBuilt()
        {
            // Arrange
            var root = new TreeNode<string, string>("Root", "RootValue");
            var leftChild = new TreeNode<string, string>("Left", "LeftValue");
            var rightChild = new TreeNode<string, string>("Right", "RightValue");
            var leftGrandChild = new TreeNode<string, string>("LeftLeft", "LeftLeftValue");

            // Act
            root.Left = leftChild;
            root.Right = rightChild;
            leftChild.Left = leftGrandChild;
            root.Height = 3;
            leftChild.Height = 2;

            // Assert
            Assert.That(root.Left, Is.EqualTo(leftChild));
            Assert.That(root.Right, Is.EqualTo(rightChild));
            Assert.That(leftChild.Left, Is.EqualTo(leftGrandChild));
            Assert.That(root.Height, Is.EqualTo(3));
            Assert.That(leftChild.Height, Is.EqualTo(2));
        }

        [Test]
        public void Properties_IndependentOfEachOther()
        {
            // Arrange
            var node1 = new TreeNode<string, string>("Key1", "Value1");
            var node2 = new TreeNode<string, string>("Key2", "Value2");

            // Act
            node1.Height = 10;
            node2.Height = 20;

            // Assert
            Assert.That(node1.Height, Is.EqualTo(10));
            Assert.That(node2.Height, Is.EqualTo(20));
            Assert.That(node1.Key, Is.Not.EqualTo(node2.Key));
            Assert.That(node1.Value, Is.Not.EqualTo(node2.Value));
        }

        [Test]
        public void EmptyString_CanBeUsedAsKeyAndValue()
        {
            // Arrange & Act
            var node = new TreeNode<string, string>("", "");

            // Assert
            Assert.That(node.Key, Is.EqualTo(""));
            Assert.That(node.Value, Is.EqualTo(""));
            Assert.That(node.Height, Is.EqualTo(1));
        }
    }
}