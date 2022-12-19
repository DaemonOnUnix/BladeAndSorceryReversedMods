using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Speech.Internal
{
	// Token: 0x02000017 RID: 23
	internal abstract class RedBackList : IEnumerable
	{
		// Token: 0x06000053 RID: 83 RVA: 0x000055E1 File Offset: 0x000045E1
		internal RedBackList()
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000055EC File Offset: 0x000045EC
		internal void Add(object key)
		{
			RedBackList.TreeNode treeNode = new RedBackList.TreeNode(key);
			treeNode.IsRed = true;
			this.InsertNode(this._root, treeNode);
			this.FixUpInsertion(treeNode);
			this._root = this.FindRoot(treeNode);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000562C File Offset: 0x0000462C
		internal void Remove(object key)
		{
			RedBackList.TreeNode treeNode = this.FindItem(this._root, key);
			if (treeNode == null)
			{
				throw new KeyNotFoundException();
			}
			RedBackList.TreeNode treeNode2 = RedBackList.DeleteNode(treeNode);
			RedBackList.FixUpRemoval(treeNode2);
			if (treeNode2 != this._root)
			{
				this._root = this.FindRoot(this._root);
				return;
			}
			if (this._root.Left != null)
			{
				this._root = this.FindRoot(this._root.Left);
				return;
			}
			if (this._root.Right != null)
			{
				this._root = this.FindRoot(this._root.Right);
				return;
			}
			this._root = null;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000056CA File Offset: 0x000046CA
		public IEnumerator GetEnumerator()
		{
			return new RedBackList.MyEnumerator(this._root);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000056D7 File Offset: 0x000046D7
		internal bool IsEmpty
		{
			get
			{
				return this._root == null;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000056E2 File Offset: 0x000046E2
		internal bool CountIsOne
		{
			get
			{
				return this._root != null && this._root.Left == null && this._root.Right == null;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00005709 File Offset: 0x00004709
		internal bool ContainsMoreThanOneItem
		{
			get
			{
				return this._root != null && (this._root.Right != null || this._root.Left != null);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00005735 File Offset: 0x00004735
		internal object First
		{
			get
			{
				if (this._root == null)
				{
					return null;
				}
				return RedBackList.FindMinSubTree(this._root).Key;
			}
		}

		// Token: 0x0600005B RID: 91
		protected abstract int CompareTo(object object1, object object2);

		// Token: 0x0600005C RID: 92 RVA: 0x00005751 File Offset: 0x00004751
		private static RedBackList.TreeNode GetUncle(RedBackList.TreeNode node)
		{
			if (node.Parent == node.Parent.Parent.Left)
			{
				return node.Parent.Parent.Right;
			}
			return node.Parent.Parent.Left;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000578C File Offset: 0x0000478C
		private static RedBackList.TreeNode GetSibling(RedBackList.TreeNode node, RedBackList.TreeNode parent)
		{
			if (node == parent.Left)
			{
				return parent.Right;
			}
			return parent.Left;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000057A4 File Offset: 0x000047A4
		private static RedBackList.NodeColor GetColor(RedBackList.TreeNode node)
		{
			if (node == null)
			{
				return RedBackList.NodeColor.BLACK;
			}
			if (!node.IsRed)
			{
				return RedBackList.NodeColor.BLACK;
			}
			return RedBackList.NodeColor.RED;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000057B6 File Offset: 0x000047B6
		private static void SetColor(RedBackList.TreeNode node, RedBackList.NodeColor color)
		{
			if (node != null)
			{
				node.IsRed = color == RedBackList.NodeColor.RED;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000057C8 File Offset: 0x000047C8
		private static void TakeParent(RedBackList.TreeNode node, RedBackList.TreeNode newNode)
		{
			if (node.Parent == null)
			{
				if (newNode != null)
				{
					newNode.Parent = null;
					return;
				}
				return;
			}
			else
			{
				if (node.Parent.Left == node)
				{
					node.Parent.Left = newNode;
					return;
				}
				if (node.Parent.Right == node)
				{
					node.Parent.Right = newNode;
					return;
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005824 File Offset: 0x00004824
		private static RedBackList.TreeNode RotateLeft(RedBackList.TreeNode node)
		{
			RedBackList.TreeNode right = node.Right;
			node.Right = right.Left;
			RedBackList.TakeParent(node, right);
			right.Left = node;
			return right;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005854 File Offset: 0x00004854
		private static RedBackList.TreeNode RotateRight(RedBackList.TreeNode node)
		{
			RedBackList.TreeNode left = node.Left;
			node.Left = left.Right;
			RedBackList.TakeParent(node, left);
			left.Right = node;
			return left;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005883 File Offset: 0x00004883
		private static RedBackList.TreeNode FindMinSubTree(RedBackList.TreeNode node)
		{
			while (node.Left != null)
			{
				node = node.Left;
			}
			return node;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005898 File Offset: 0x00004898
		private static RedBackList.TreeNode FindSuccessor(RedBackList.TreeNode node)
		{
			if (node.Right != null)
			{
				return RedBackList.FindMinSubTree(node.Right);
			}
			while (node.Parent != null && node.Parent.Left != node)
			{
				node = node.Parent;
			}
			if (node.Parent != null)
			{
				return node.Parent;
			}
			return null;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000058EC File Offset: 0x000048EC
		private static RedBackList.TreeNode DeleteNode(RedBackList.TreeNode node)
		{
			if (node.Right == null)
			{
				RedBackList.TakeParent(node, node.Left);
				return node;
			}
			if (node.Left == null)
			{
				RedBackList.TakeParent(node, node.Right);
				return node;
			}
			RedBackList.TreeNode treeNode = RedBackList.FindSuccessor(node);
			node.CopyNode(treeNode);
			RedBackList.TakeParent(treeNode, treeNode.Right);
			return treeNode;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005940 File Offset: 0x00004940
		private RedBackList.TreeNode InsertNode(RedBackList.TreeNode node, RedBackList.TreeNode newNode)
		{
			if (node == null)
			{
				return newNode;
			}
			int num = this.CompareTo(newNode.Key, node.Key);
			if (num < 0)
			{
				node.Left = this.InsertNode(node.Left, newNode);
			}
			else
			{
				node.Right = this.InsertNode(node.Right, newNode);
			}
			return node;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005994 File Offset: 0x00004994
		private RedBackList.TreeNode FindItem(RedBackList.TreeNode node, object key)
		{
			if (node == null)
			{
				return null;
			}
			int num = this.CompareTo(key, node.Key);
			if (num == 0)
			{
				return node;
			}
			if (num < 0)
			{
				return this.FindItem(node.Left, key);
			}
			return this.FindItem(node.Right, key);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000059D8 File Offset: 0x000049D8
		private RedBackList.TreeNode FindRoot(RedBackList.TreeNode node)
		{
			while (node.Parent != null)
			{
				node = node.Parent;
			}
			return node;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000059ED File Offset: 0x000049ED
		private void FixUpInsertion(RedBackList.TreeNode node)
		{
			this.FixInsertCase1(node);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000059F6 File Offset: 0x000049F6
		private void FixInsertCase1(RedBackList.TreeNode node)
		{
			if (node.Parent == null)
			{
				node.IsRed = false;
				return;
			}
			this.FixInsertCase2(node);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005A10 File Offset: 0x00004A10
		private void FixInsertCase2(RedBackList.TreeNode node)
		{
			if (RedBackList.GetColor(node.Parent) == RedBackList.NodeColor.BLACK)
			{
				return;
			}
			RedBackList.TreeNode uncle = RedBackList.GetUncle(node);
			if (RedBackList.GetColor(uncle) == RedBackList.NodeColor.RED)
			{
				RedBackList.SetColor(node.Parent, RedBackList.NodeColor.BLACK);
				RedBackList.SetColor(uncle, RedBackList.NodeColor.BLACK);
				RedBackList.SetColor(node.Parent.Parent, RedBackList.NodeColor.RED);
				this.FixInsertCase1(node.Parent.Parent);
				return;
			}
			this.FixInsertCase3(node);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005A78 File Offset: 0x00004A78
		private void FixInsertCase3(RedBackList.TreeNode node)
		{
			if (node == node.Parent.Right && node.Parent == node.Parent.Parent.Left)
			{
				RedBackList.RotateLeft(node.Parent);
				node = node.Left;
			}
			else if (node == node.Parent.Left && node.Parent == node.Parent.Parent.Right)
			{
				RedBackList.RotateRight(node.Parent);
				node = node.Right;
			}
			this.FixInsertCase4(node);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005B04 File Offset: 0x00004B04
		private void FixInsertCase4(RedBackList.TreeNode node)
		{
			RedBackList.SetColor(node.Parent, RedBackList.NodeColor.BLACK);
			RedBackList.SetColor(node.Parent.Parent, RedBackList.NodeColor.RED);
			if (node == node.Parent.Left)
			{
				RedBackList.RotateRight(node.Parent.Parent);
				return;
			}
			RedBackList.RotateLeft(node.Parent.Parent);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005B60 File Offset: 0x00004B60
		private static void FixUpRemoval(RedBackList.TreeNode node)
		{
			RedBackList.TreeNode treeNode = ((node.Left == null) ? node.Right : node.Left);
			if (RedBackList.GetColor(node) == RedBackList.NodeColor.BLACK)
			{
				if (RedBackList.GetColor(treeNode) == RedBackList.NodeColor.RED)
				{
					RedBackList.SetColor(treeNode, RedBackList.NodeColor.BLACK);
					return;
				}
				if (node.Parent == null)
				{
					return;
				}
				RedBackList.FixRemovalCase2(RedBackList.GetSibling(treeNode, node.Parent));
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005BB7 File Offset: 0x00004BB7
		private static void FixRemovalCase1(RedBackList.TreeNode node)
		{
			if (node.Parent == null)
			{
				return;
			}
			RedBackList.FixRemovalCase2(RedBackList.GetSibling(node, node.Parent));
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005BD4 File Offset: 0x00004BD4
		private static void FixRemovalCase2(RedBackList.TreeNode sibling)
		{
			if (RedBackList.GetColor(sibling) == RedBackList.NodeColor.RED)
			{
				RedBackList.TreeNode parent = sibling.Parent;
				RedBackList.SetColor(parent, RedBackList.NodeColor.RED);
				RedBackList.SetColor(sibling, RedBackList.NodeColor.BLACK);
				if (sibling == parent.Right)
				{
					RedBackList.RotateLeft(sibling.Parent);
					sibling = parent.Right;
				}
				else
				{
					RedBackList.RotateRight(sibling.Parent);
					sibling = parent.Left;
				}
			}
			RedBackList.FixRemovalCase3(sibling);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005C38 File Offset: 0x00004C38
		private static void FixRemovalCase3(RedBackList.TreeNode sibling)
		{
			if (RedBackList.GetColor(sibling.Parent) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling.Left) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling.Right) == RedBackList.NodeColor.BLACK)
			{
				RedBackList.SetColor(sibling, RedBackList.NodeColor.RED);
				RedBackList.FixRemovalCase1(sibling.Parent);
				return;
			}
			RedBackList.FixRemovalCase4(sibling);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005C90 File Offset: 0x00004C90
		private static void FixRemovalCase4(RedBackList.TreeNode sibling)
		{
			if (RedBackList.GetColor(sibling.Parent) == RedBackList.NodeColor.RED && RedBackList.GetColor(sibling) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling.Left) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling.Right) == RedBackList.NodeColor.BLACK)
			{
				RedBackList.SetColor(sibling, RedBackList.NodeColor.RED);
				RedBackList.SetColor(sibling.Parent, RedBackList.NodeColor.BLACK);
				return;
			}
			RedBackList.FixRemovalCase5(sibling);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005CE8 File Offset: 0x00004CE8
		private static void FixRemovalCase5(RedBackList.TreeNode sibling)
		{
			if (sibling == sibling.Parent.Right && RedBackList.GetColor(sibling) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling.Left) == RedBackList.NodeColor.RED && RedBackList.GetColor(sibling.Right) == RedBackList.NodeColor.BLACK)
			{
				RedBackList.SetColor(sibling, RedBackList.NodeColor.RED);
				RedBackList.SetColor(sibling.Left, RedBackList.NodeColor.BLACK);
				RedBackList.RotateRight(sibling);
				sibling = sibling.Parent;
			}
			else if (sibling == sibling.Parent.Left && RedBackList.GetColor(sibling) == RedBackList.NodeColor.BLACK && RedBackList.GetColor(sibling.Right) == RedBackList.NodeColor.RED && RedBackList.GetColor(sibling.Left) == RedBackList.NodeColor.BLACK)
			{
				RedBackList.SetColor(sibling, RedBackList.NodeColor.RED);
				RedBackList.SetColor(sibling.Right, RedBackList.NodeColor.BLACK);
				RedBackList.RotateLeft(sibling);
				sibling = sibling.Parent;
			}
			RedBackList.FixRemovalCase6(sibling);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005DA4 File Offset: 0x00004DA4
		private static void FixRemovalCase6(RedBackList.TreeNode sibling)
		{
			RedBackList.SetColor(sibling, RedBackList.GetColor(sibling.Parent));
			RedBackList.SetColor(sibling.Parent, RedBackList.NodeColor.BLACK);
			if (sibling == sibling.Parent.Right)
			{
				RedBackList.SetColor(sibling.Right, RedBackList.NodeColor.BLACK);
				RedBackList.RotateLeft(sibling.Parent);
				return;
			}
			RedBackList.SetColor(sibling.Left, RedBackList.NodeColor.BLACK);
			RedBackList.RotateRight(sibling.Parent);
		}

		// Token: 0x04000093 RID: 147
		private RedBackList.TreeNode _root;

		// Token: 0x02000018 RID: 24
		private class MyEnumerator : IEnumerator
		{
			// Token: 0x06000075 RID: 117 RVA: 0x00005E0D File Offset: 0x00004E0D
			internal MyEnumerator(RedBackList.TreeNode node)
			{
				this._root = node;
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000076 RID: 118 RVA: 0x00005E1C File Offset: 0x00004E1C
			public object Current
			{
				get
				{
					if (this._node == null)
					{
						throw new InvalidOperationException();
					}
					return this._node.Key;
				}
			}

			// Token: 0x06000077 RID: 119 RVA: 0x00005E38 File Offset: 0x00004E38
			public bool MoveNext()
			{
				if (!this._moved)
				{
					this._node = ((this._root != null) ? RedBackList.FindMinSubTree(this._root) : null);
					this._moved = true;
				}
				else
				{
					this._node = ((this._node != null) ? RedBackList.FindSuccessor(this._node) : null);
				}
				return this._node != null;
			}

			// Token: 0x06000078 RID: 120 RVA: 0x00005E9A File Offset: 0x00004E9A
			public void Reset()
			{
				this._moved = false;
				this._node = null;
			}

			// Token: 0x04000094 RID: 148
			private RedBackList.TreeNode _node;

			// Token: 0x04000095 RID: 149
			private RedBackList.TreeNode _root;

			// Token: 0x04000096 RID: 150
			private bool _moved;
		}

		// Token: 0x02000019 RID: 25
		private class TreeNode
		{
			// Token: 0x06000079 RID: 121 RVA: 0x00005EAA File Offset: 0x00004EAA
			internal TreeNode(object key)
			{
				this._key = key;
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600007A RID: 122 RVA: 0x00005EB9 File Offset: 0x00004EB9
			// (set) Token: 0x0600007B RID: 123 RVA: 0x00005EC1 File Offset: 0x00004EC1
			internal RedBackList.TreeNode Left
			{
				get
				{
					return this._leftChild;
				}
				set
				{
					this._leftChild = value;
					if (this._leftChild != null)
					{
						this._leftChild._parent = this;
					}
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600007C RID: 124 RVA: 0x00005EDE File Offset: 0x00004EDE
			// (set) Token: 0x0600007D RID: 125 RVA: 0x00005EE6 File Offset: 0x00004EE6
			internal RedBackList.TreeNode Right
			{
				get
				{
					return this._rightChild;
				}
				set
				{
					this._rightChild = value;
					if (this._rightChild != null)
					{
						this._rightChild._parent = this;
					}
				}
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600007E RID: 126 RVA: 0x00005F03 File Offset: 0x00004F03
			// (set) Token: 0x0600007F RID: 127 RVA: 0x00005F0B File Offset: 0x00004F0B
			internal RedBackList.TreeNode Parent
			{
				get
				{
					return this._parent;
				}
				set
				{
					this._parent = value;
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000080 RID: 128 RVA: 0x00005F14 File Offset: 0x00004F14
			// (set) Token: 0x06000081 RID: 129 RVA: 0x00005F1C File Offset: 0x00004F1C
			internal bool IsRed
			{
				get
				{
					return this._isRed;
				}
				set
				{
					this._isRed = value;
				}
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000082 RID: 130 RVA: 0x00005F25 File Offset: 0x00004F25
			internal object Key
			{
				get
				{
					return this._key;
				}
			}

			// Token: 0x06000083 RID: 131 RVA: 0x00005F2D File Offset: 0x00004F2D
			internal void CopyNode(RedBackList.TreeNode from)
			{
				this._key = from._key;
			}

			// Token: 0x04000097 RID: 151
			private object _key;

			// Token: 0x04000098 RID: 152
			private bool _isRed;

			// Token: 0x04000099 RID: 153
			private RedBackList.TreeNode _leftChild;

			// Token: 0x0400009A RID: 154
			private RedBackList.TreeNode _rightChild;

			// Token: 0x0400009B RID: 155
			private RedBackList.TreeNode _parent;
		}

		// Token: 0x0200001A RID: 26
		private enum NodeColor
		{
			// Token: 0x0400009D RID: 157
			BLACK,
			// Token: 0x0400009E RID: 158
			RED
		}
	}
}
