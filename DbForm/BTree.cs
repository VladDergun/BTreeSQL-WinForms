using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using DbForm.Models;
using System.Linq;

namespace DbForm
{
    public class BTreeNode
    {
        public int T { get; set; }
        public List<SampleModel> Keys { get; set; }
        public List<BTreeNode> Children { get; set; }
        public bool IsLeaf { get; set; }
        public int KeysCount => Keys.Count;

        public int numberOfComparisons = 0;


        public BTreeNode(int t, bool isLeaf)
        {
            T = t;
            IsLeaf = isLeaf;
            Keys = new List<SampleModel>();
            Children = new List<BTreeNode>();
        }
        #region Insert key
        public void InsertNonFull(SampleModel key)
        {
            int i = KeysCount - 1;
            if (IsLeaf)
            {
                while (i >= 0 && key.Id < Keys[i].Id)
                {
                    i--;
                }
                Keys.Insert(i + 1, key);
            }
            else
            {
                while (i >= 0 && key.Id < Keys[i].Id)
                {
                    i--;
                }

                i++;
                if (Children[i].KeysCount == 2 * T - 1)
                {
                    SplitChild(i);
                    if (key.Id > Keys[i].Id)
                    {
                        i++;
                    }
                }
                Children[i].InsertNonFull(key);
            }
        }

        public void SplitChild(int index)
        {
            BTreeNode fullChild = Children[index];
            BTreeNode newChild = new BTreeNode(T, fullChild.IsLeaf);
            Keys.Insert(index, fullChild.Keys[T - 1]);

            newChild.Keys.AddRange(fullChild.Keys.GetRange(T, T - 1));
            fullChild.Keys.RemoveRange(T - 1, T);

            if (!fullChild.IsLeaf)
            {
                newChild.Children.AddRange(fullChild.Children.GetRange(T, T));
                fullChild.Children.RemoveRange(T, T);
            }

            Children.Insert(index + 1, newChild);
        }
        #endregion
        #region Delete key
        private int FindKey(BTreeNode node, int key)
        {
            int idx = 0;
            while (idx < node.KeysCount && node.Keys[idx].Id < key)
                idx++;
            return idx;
        }

        public void Delete(BTreeNode node, int key)
        {
            int idx = FindKey(node, key);

            // Case 1: The key is in a leaf node
            if (node.IsLeaf)
            {
                if (idx < node.KeysCount && node.Keys[idx].Id == key)
                {
                    node.Keys.RemoveAt(idx);
                }
                return;
            }

            // Case 2: The key is in an internal node
            if (idx < node.KeysCount && node.Keys[idx].Id == key)
            {
                if (node.Children[idx].KeysCount >= T)
                {
                    // If the child that precedes the key has enough keys, get the predecessor
                    BTreeNode predNode = node.Children[idx];
                    int predKey = GetPredecessor(predNode);
                    node.Keys[idx].Id = predKey;
                    Delete(predNode, predKey);
                }
                else if (node.Children[idx + 1].KeysCount >= T)
                {
                    // If the next child has enough keys, get the successor
                    BTreeNode succNode = node.Children[idx + 1];
                    int succKey = GetSuccessor(succNode);
                    node.Keys[idx].Id = succKey;
                    Delete(succNode, succKey);
                }
                else
                {
                    // Merge the children
                    Merge(node, idx);
                    Delete(node.Children[idx], key);
                }
            }
            else
            {
                if (node.Children[idx].KeysCount < T)
                {
                    // If the child has less than t keys, try to fix it
                    FixChild(node, idx);
                }
                idx = FindKey(node, key);
                Delete(node.Children[idx], key);
            }
        }

        // Merge two children of an internal node
        private void Merge(BTreeNode node, int idx)
        {
            BTreeNode child1 = node.Children[idx];
            BTreeNode child2 = node.Children[idx + 1];

            // Move the key from node to child1
            child1.Keys.Add(node.Keys[idx]);

            // Append all keys and children from child2 to child1
            child1.Keys.AddRange(child2.Keys);
            child1.Children.AddRange(child2.Children);

            node.Keys.RemoveAt(idx);
            node.Children.RemoveAt(idx + 1);
        }

        // Fix a child that has fewer than t keys
        private void FixChild(BTreeNode node, int idx)
        {
            if (idx > 0 && node.Children[idx - 1].KeysCount >= T)
            {
                // Borrow from the left child
                BorrowFromLeft(node, idx);
            }
            else if (idx < node.KeysCount && node.Children[idx + 1].KeysCount >= T)
            {
                // Borrow from the right child
                BorrowFromRight(node, idx);
            }
            else
            {
                // Merge the children
                if (idx < node.KeysCount)
                    Merge(node, idx);
                else
                    Merge(node, idx - 1);
            }
        }

        // Borrow a key from the left sibling
        private void BorrowFromLeft(BTreeNode node, int idx)
        {
            BTreeNode child = node.Children[idx];
            BTreeNode sibling = node.Children[idx - 1];

            // Move a key from node to child
            child.Keys.Insert(0, node.Keys[idx - 1]);

            // Move a child from sibling to node
            if (!sibling.IsLeaf)
            {
                child.Children.Insert(0, sibling.Children[sibling.KeysCount]);
                sibling.Children.RemoveAt(sibling.KeysCount);
            }

            node.Keys[idx - 1] = sibling.Keys[sibling.KeysCount - 1];
            sibling.Keys.RemoveAt(sibling.KeysCount - 1);
        }

        // Borrow a key from the right sibling
        private void BorrowFromRight(BTreeNode node, int idx)
        {
            BTreeNode child = node.Children[idx];
            BTreeNode sibling = node.Children[idx + 1];

            // Move a key from node to child
            child.Keys.Add(node.Keys[idx]);

            // Move a child from sibling to node
            if (!sibling.IsLeaf)
            {
                child.Children.Add(sibling.Children[0]);
                sibling.Children.RemoveAt(0);
            }

            node.Keys[idx] = sibling.Keys[0];
            sibling.Keys.RemoveAt(0);
        }

        // Get the predecessor (largest key in the left child)
        private int GetPredecessor(BTreeNode node)
        {
            while (!node.IsLeaf)
                node = node.Children[node.KeysCount];
            return node.Keys[node.KeysCount - 1].Id;
        }

        // Get the successor (smallest key in the right child)
        private int GetSuccessor(BTreeNode node)
        {
            while (!node.IsLeaf)
                node = node.Children[0];
            return node.Keys[0].Id;
        }
        #endregion
        #region Search Key
        public (int, bool, SampleModel) Search(int primaryKey, bool calculateComps)
        {
            int start = 0;
            int end = Keys.Count - 1;

            SampleModel exampleStructure = default;

            while (start <= end)
            {
                int mid = start + (end - start) / 2;
                if (calculateComps)
                    numberOfComparisons++;

                if (Keys[mid].Id == primaryKey)
                {
                    exampleStructure = Keys[mid];
                    return (mid, true, exampleStructure);
                }
                else if (Keys[mid].Id > primaryKey)
                {
                    end = mid - 1;
                }
                else
                {
                    start = mid + 1;
                }
            }

            return (start, false, exampleStructure);
        }




        #endregion

        public void ExportToDot(StreamWriter writer, int nodeId, ref int nextId)
        {
            writer.WriteLine($"  Node{nodeId} [label=\"{string.Join(", ", Keys.Select(k => $"{k.Id}: {k.Name}"))}\"]");

            for (int i = 0; i < Children.Count; i++)
            {
                int childId = nextId++;
                writer.WriteLine($"  Node{nodeId} -> Node{childId}");
                Children[i].ExportToDot(writer, childId, ref nextId);
            }
        }

    }

    public class BTree
    {
        public BTreeNode Root { get; set; }
        private int T { get; set; }
        public BTree(int t)
        {
            Root = new BTreeNode(t, true);
            T = t;
        }


        public void Insert(SampleModel key)
        {
            var existing = Search(key.Id, false);

            if (existing.Item1)
            {
                throw new InvalidOperationException("Key already exists");
            }
            if (Root.KeysCount == 2 * T - 1)
            {
                BTreeNode newRoot = new BTreeNode(T, false);
                newRoot.Children.Add(Root);
                newRoot.SplitChild(0);
                Root = newRoot;
            }
            Root.InsertNonFull(key);
        }

        public void Delete(int key)
        {
            Root.Delete(Root, key);

            if (Root.KeysCount == 0)
            {
                if (Root.IsLeaf)
                {
                    Root = null;
                }
                else
                {
                    Root = Root.Children[0];
                }
            }
        }
        public (bool, int, SampleModel) Search(int primaryKey, bool calculateComps)
        {
            BTreeNode node = Root;
            int numberOfComparisons = 0;
            while (node != null)
            {
                (int result, bool found, SampleModel exampleStructure) = node.Search(primaryKey, calculateComps);

                if (calculateComps)
                {
                    numberOfComparisons += node.numberOfComparisons;
                    node.numberOfComparisons = 0;
                }
                    
                if (found)
                {
                    return (true, numberOfComparisons, exampleStructure);
                }
                else if (node.IsLeaf)
                {
                    return (false, numberOfComparisons, new SampleModel());
                }
                else
                {
                    node = node.Children[result];
                }
            }
            return (false, numberOfComparisons, new SampleModel());
        }

        public void Update(int key, string value)
        {
            var existing = Search(key, false);
            if (!existing.Item1)
            {
                throw new InvalidOperationException("Key not found");
            }
            existing.Item3.Name = value;
        }

        public List<SampleModel> GetAll()
        {
            // Collect all nodes from the BTree
            List<BTreeNode> allNodes = new List<BTreeNode>();
            GetAllNodes(Root, allNodes);

            // Collect all keys from the nodes
            List<SampleModel> allKeys = new List<SampleModel>();
            foreach (var node in allNodes)
            {
                allKeys.AddRange(node.Keys);
            }

            // Sort the keys by Id
            allKeys.Sort((x, y) => x.Id.CompareTo(y.Id));

            return allKeys;
        }


        // Helper method to recursively collect nodes from the tree
        private void GetAllNodes(BTreeNode node, List<BTreeNode> allNodes)
        {
            if (node == null)
                return;

            allNodes.Add(node);  // Add the current node to the list

            foreach (var child in node.Children)  // Recursively collect child nodes
            {
                GetAllNodes(child, allNodes);
            }
        }


        public void ExportToDot(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("digraph BTree {");
                writer.WriteLine("  node [shape=record];");

                int nextId = 1;
                Root?.ExportToDot(writer, 0, ref nextId);

                writer.WriteLine("}");
            }
        }

        public static void SaveTree(BTree tree, string filePath)
        {
            string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            File.WriteAllText(filePath, json);
        }


        public static BTree LoadTree(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<BTree>(json,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
        }



    }
}
