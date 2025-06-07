using System;
using System.Collections.Generic;
using HashTableLiterature.Interfaces;
using HashTableLiterature.DataStructures;

namespace HashTableLiterature.DataStructures
{
    public class AVLTree : IBalancedTree<string, string>
    {
        private TreeNode<string, string> root;

        public void Insert(string key, string value)
        {
            root = Insert(root, key, value);
        }

        public string Search(string key)
        {
            return Search(root, key);
        }

        public bool Delete(string key)
        {
            if (Search(key) == null)
                return false;

            root = Delete(root, key);
            return true;
        }

        public List<string> GetAllKeys()
        {
            var keys = new List<string>();
            InorderTraversal(root, keys);
            return keys;
        }

        public int Count()
        {
            return Count(root);
        }

        private TreeNode<string, string> Insert(TreeNode<string, string> node, string key, string value)
        {
            if (node == null)
                return new TreeNode<string, string>(key, value);

            if (string.Compare(key, node.Key) < 0)
                node.Left = Insert(node.Left, key, value);
            else if (string.Compare(key, node.Key) > 0)
                node.Right = Insert(node.Right, key, value);
            else
            {
                node.Value = value;
                return node;
            }

            UpdateHeight(node);
            return Balance(node, key);
        }

        private string Search(TreeNode<string, string> node, string key)
        {
            if (node == null)
                return null;

            if (string.Compare(key, node.Key) == 0)
                return node.Value;
            else if (string.Compare(key, node.Key) < 0)
                return Search(node.Left, key);
            else
                return Search(node.Right, key);
        }

        private TreeNode<string, string> Delete(TreeNode<string, string> root, string key)
        {
            if (root == null)
                return root;

            if (string.Compare(key, root.Key) < 0)
                root.Left = Delete(root.Left, key);
            else if (string.Compare(key, root.Key) > 0)
                root.Right = Delete(root.Right, key);
            else
            {
                if (root.Left == null || root.Right == null)
                {
                    var temp = root.Left ?? root.Right;
                    if (temp == null)
                    {
                        temp = root;
                        root = null;
                    }
                    else
                        root = temp;
                }
                else
                {
                    var temp = GetMinValueNode(root.Right);
                    root.Key = temp.Key;
                    root.Value = temp.Value;
                    root.Right = Delete(root.Right, temp.Key);
                }
            }

            if (root == null)
                return root;

            UpdateHeight(root);
            return BalanceAfterDeletion(root);
        }

        private int GetHeight(TreeNode<string, string> node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(TreeNode<string, string> node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private void UpdateHeight(TreeNode<string, string> node)
        {
            if (node != null)
            {
                node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            }
        }

        private TreeNode<string, string> RotateRight(TreeNode<string, string> y)
        {
            var x = y.Left;
            var T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        private TreeNode<string, string> RotateLeft(TreeNode<string, string> x)
        {
            var y = x.Right;
            var T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }

        private TreeNode<string, string> Balance(TreeNode<string, string> node, string key)
        {
            int balance = GetBalance(node);

            if (balance > 1 && string.Compare(key, node.Left.Key) < 0)
                return RotateRight(node);

            if (balance < -1 && string.Compare(key, node.Right.Key) > 0)
                return RotateLeft(node);

            if (balance > 1 && string.Compare(key, node.Left.Key) > 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            if (balance < -1 && string.Compare(key, node.Right.Key) < 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private TreeNode<string, string> BalanceAfterDeletion(TreeNode<string, string> root)
        {
            int balance = GetBalance(root);

            if (balance > 1 && GetBalance(root.Left) >= 0)
                return RotateRight(root);

            if (balance > 1 && GetBalance(root.Left) < 0)
            {
                root.Left = RotateLeft(root.Left);
                return RotateRight(root);
            }

            if (balance < -1 && GetBalance(root.Right) <= 0)
                return RotateLeft(root);

            if (balance < -1 && GetBalance(root.Right) > 0)
            {
                root.Right = RotateRight(root.Right);
                return RotateLeft(root);
            }

            return root;
        }

        private TreeNode<string, string> GetMinValueNode(TreeNode<string, string> node)
        {
            var current = node;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

        private void InorderTraversal(TreeNode<string, string> node, List<string> keys)
        {
            if (node != null)
            {
                InorderTraversal(node.Left, keys);
                keys.Add(node.Key);
                InorderTraversal(node.Right, keys);
            }
        }

        private int Count(TreeNode<string, string> node)
        {
            if (node == null)
                return 0;
            return 1 + Count(node.Left) + Count(node.Right);
        }
    }
}