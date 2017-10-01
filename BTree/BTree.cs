/******************************************************************************
 *  Copyright 2002-2016, Robert Sedgewick and Kevin Wayne.
 *
 *  This file is part of algs4.jar, which accompanies the textbook
 *
 *      Algorithms, 4th edition by Robert Sedgewick and Kevin Wayne,
 *      Addison-Wesley Professional, 2011, ISBN 0-321-57351-X.
 *      http://algs4.cs.princeton.edu
 *
 *
 *  algs4.jar is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  algs4.jar is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with algs4.jar.  If not, see http://www.gnu.org/licenses.
 ******************************************************************************/

using System;
using System.Text;

namespace BTree
{
    public class BTree<Key, Value> where Key : IComparable<Key>
    {
        static readonly int maxChildrenPerNode = 4;

        Node root;
        int height;
        int numberOfPairs;

        sealed class Node
        {
            public int numberOfChildren;
            public Entry[] children = new Entry[maxChildrenPerNode];

            public Node(int numberOfChildren)
            {
                this.numberOfChildren = numberOfChildren;
            }
        }

        sealed class Entry
        {
            public Key key;
            public Object val;
            public Node next;

            public Entry(Key key, Object val, Node next)
            {
                this.key = key;
                this.val = val;
                this.next = next;
            }
        }

        public BTree()
        {
            root = new Node(0);
        }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public int Size()
        {
            return numberOfPairs;
        }

        public int Height()
        {
            return height;
        }

        public Value Get(Key key)
        {
            if (key == null)
            {
                throw new ArgumentException("argument to get() is null");
            }
            return Search(root, key, height);
        }

        Value Search(Node node, Key key, int _height)
        {
            Entry[] children = node.children;

            if (_height == 0)
            {
                for (int j = 0; j < node.numberOfChildren; j++)
                {
                    if (Eq(key, children[j].key))
                    {
                        return (Value)children[j].val;
                    }
                }
            }
            else
            {
                for (int j = 0; j < node.numberOfChildren; j++)
                {
                    if (j + 1 == node.numberOfChildren || Less(key, children[j + 1].key))
                    {
                        return Search(children[j].next, key, _height - 1);
                    }
                }
            }
            // TODO: For value types, this will return the zero value.
            return default(Value);
        }

        public void Put(Key key, Value val)
        {
            if (key == null)
            {
                throw new ArgumentException("argument key to put() is null");
            }

            Node n = Insert(root, key, val, height);
            numberOfPairs++;
            if (n == null) return;

            Node split = new Node(2);
            split.children[0] = new Entry(root.children[0].key, null, root);
            split.children[1] = new Entry(n.children[0].key, null, n);

            root = split;
            height++;
        }

        Node Insert(Node _root, Key key, Value val, int _height)
        {
            int j;
            Entry entry = new Entry(key, val, null);

            // External node
            if (_height == 0)
            {
                for (j = 0; j < _root.numberOfChildren; j++)
                {
                    if (Less(key, _root.children[j].key)) break;
                }
            }
            // Internal node
            else
            {
                for (j = 0; j < _root.numberOfChildren; j++)
                {
                    if ((j + 1 == _root.numberOfChildren) || Less(key, _root.children[j + 1].key))
                    {
                        Node n = Insert(_root.children[j++].next, key, val, _height - 1);
                        if (n == null) return null;

                        entry.key = n.children[0].key;
                        entry.next = n;
                        break;
                    }
                }
            }

            for (int i = _root.numberOfChildren; i > j; i--)
            {
                _root.children[i] = _root.children[i - 1];
            }

            _root.children[j] = entry;
            _root.numberOfChildren++;

            if (_root.numberOfChildren < maxChildrenPerNode) return null;
            return Split(_root);
        }

        Node Split(Node n)
        {
            int splitChildrenPerNode = maxChildrenPerNode / 2;

            Node t = new Node(splitChildrenPerNode);
            n.numberOfChildren = splitChildrenPerNode;

            for (int j = 0; j < splitChildrenPerNode; j++)
            {
                t.children[j] = n.children[splitChildrenPerNode + j];
            }
            return t;
        }

        public override string ToString()
        {
            return ToString(root, height, "") + "\n";
        }

        string ToString(Node _root, int _height, string indent)
        {
            StringBuilder sb = new StringBuilder();
            Entry[] children = _root.children;

            if (_height == 0)
            {
                for (int j = 0; j < _root.numberOfChildren; j++)
                {
                    sb.Append(indent + children[j].key + " " + children[j].val + "\n");
                }
            }
            else
            {
                for (int j = 0; j < _root.numberOfChildren; j++)
                {
                    if (j > 0)
                    {
                        sb.Append(indent + "(" + children[j].key + ")\n");
                    }
                    sb.Append(ToString(children[j].next, _height - 1, indent + "\t"));
                }
            }
            return sb.ToString();
        }

        bool Less(Key lhs, Key rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        bool Eq(Key lhs, Key rhs)
        {
            return lhs.Equals(rhs);
        }
    }
}

