using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Speech.Internal
{
	// Token: 0x02000094 RID: 148
	internal abstract class RedBackList : IEnumerable
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal RedBackList()
		{
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00013858 File Offset: 0x00011A58
		internal void Add(object key)
		{
			RedBackList.TreeNode treeNode = new RedBackList.TreeNode(key);
			treeNode.IsRed = true;
			this.InsertNode(this._root, treeNode);
			this.FixUpInsertion(treeNode);
			this._root = this.FindRoot(treeNode);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00013898 File Offset: 0x00011A98
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

		// Token: 0x060004D1 RID: 1233 RVA: 0x00013936 File Offset: 0x00011B36
		public IEnumerator GetEnumerator()
		{
			return new RedBackList.MyEnumerator(this._root);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00013943 File Offset: 0x00011B43
		internal bool IsEmpty
		{
			get
			{
				return this._root == null;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0001394E File Offset: 0x00011B4E
		internal bool CountIsOne
		{
			get
			{
				return this._root != null && this._root.Left == null && this._root.Right == null;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00013975 File Offset: 0x00011B75
		internal bool ContainsMoreThanOneItem
		{
			get
			{
				return this._root != null && (this._root.Right != null || this._root.Left != null);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001399E File Offset: 0x00011B9E
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

		// Token: 0x060004D6 RID: 1238
		protected abstract int CompareTo(object object1, object object2);

		// Token: 0x060004D7 RID: 1239 RVA: 0x000139BA File Offset: 0x00011BBA
		private static RedBackList.TreeNode GetUncle(RedBackList.TreeNode node)
		{
			if (node.Parent == node.Parent.Parent.Left)
			{
				return node.Parent.Parent.Right;
			}
			return node.Parent.Parent.Left;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000139F5 File Offset: 0x00011BF5
		private static RedBackList.TreeNode GetSibling(RedBackList.TreeNode node, RedBackList.TreeNode parent)
		{
			if (node == parent.Left)
			{
				return parent.Right;
			}
			return parent.Left;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00013A0D File Offset: 0x00011C0D
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

		// Token: 0x060004DA RID: 1242 RVA: 0x00013A1F File Offset: 0x00011C1F
		private static void SetColor(RedBackList.TreeNode node, RedBackList.NodeColor color)
		{
			if (node != null)
			{
				node.IsRed = color == RedBackList.NodeColor.RED;
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00013A30 File Offset: 0x00011C30
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

		// Token: 0x060004DC RID: 1244 RVA: 0x00013A8C File Offset: 0x00011C8C
		private static RedBackList.TreeNode RotateLeft(RedBackList.TreeNode node)
		{
			RedBackList.TreeNode right = node.Right;
			node.Right = right.Left;
			RedBackList.TakeParent(node, right);
			right.Left = node;
			return right;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00013ABC File Offset: 0x00011CBC
		private static RedBackList.TreeNode RotateRight(RedBackList.TreeNode node)
		{
			RedBackList.TreeNode left = node.Left;
			node.Left = left.Right;
			RedBackList.TakeParent(node, left);
			left.Right = node;
			return left;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00013AEB File Offset: 0x00011CEB
		private static RedBackList.TreeNode FindMinSubTree(RedBackList.TreeNode node)
		{
			while (node.Left != null)
			{
				node = node.Left;
			}
			return node;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00013B00 File Offset: 0x00011D00
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x00013B54 File Offset: 0x00011D54
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

		// Token: 0x060004E1 RID: 1249 RVA: 0x00013BA8 File Offset: 0x00011DA8
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

		// Token: 0x060004E2 RID: 1250 RVA: 0x00013BFC File Offset: 0x00011DFC
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

		// Token: 0x060004E3 RID: 1251 RVA: 0x00013C40 File Offset: 0x00011E40
		private RedBackList.TreeNode FindRoot(RedBackList.TreeNode node)
		{
			while (node.Parent != null)
			{
				node = node.Parent;
			}
			return node;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00013C55 File Offset: 0x00011E55
		private void FixUpInsertion(RedBackList.TreeNode node)
		{
			this.FixInsertCase1(node);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00013C5E File Offset: 0x00011E5E
		private void FixInsertCase1(RedBackList.TreeNode node)
		{
			if (node.Parent == null)
			{
				node.IsRed = false;
				return;
			}
			this.FixInsertCase2(node);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00013C78 File Offset: 0x00011E78
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

		// Token: 0x060004E7 RID: 1255 RVA: 0x00013CE0 File Offset: 0x00011EE0
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

		// Token: 0x060004E8 RID: 1256 RVA: 0x00013D6C File Offset: 0x00011F6C
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

		// Token: 0x060004E9 RID: 1257 RVA: 0x00013DC8 File Offset: 0x00011FC8
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

		// Token: 0x060004EA RID: 1258 RVA: 0x00013E1F File Offset: 0x0001201F
		private static void FixRemovalCase1(RedBackList.TreeNode node)
		{
			if (node.Parent == null)
			{
				return;
			}
			RedBackList.FixRemovalCase2(RedBackList.GetSibling(node, node.Parent));
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00013E3C File Offset: 0x0001203C
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

		// Token: 0x060004EC RID: 1260 RVA: 0x00013EA0 File Offset: 0x000120A0
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

		// Token: 0x060004ED RID: 1261 RVA: 0x00013EF8 File Offset: 0x000120F8
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

		// Token: 0x060004EE RID: 1262 RVA: 0x00013F50 File Offset: 0x00012150
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

		// Token: 0x060004EF RID: 1263 RVA: 0x0001400C File Offset: 0x0001220C
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

		// Token: 0x0400043D RID: 1085
		private RedBackList.TreeNode _root;

		// Token: 0x0200018A RID: 394
		private class MyEnumerator : IEnumerator
		{
			// Token: 0x06000B75 RID: 2933 RVA: 0x0002DA95 File Offset: 0x0002BC95
			internal MyEnumerator(RedBackList.TreeNode node)
			{
				this._root = node;
			}

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0002DAA4 File Offset: 0x0002BCA4
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

			// Token: 0x06000B77 RID: 2935 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
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

			// Token: 0x06000B78 RID: 2936 RVA: 0x0002DB1F File Offset: 0x0002BD1F
			public void Reset()
			{
				this._moved = false;
				this._node = null;
			}

			// Token: 0x0400091E RID: 2334
			private RedBackList.TreeNode _node;

			// Token: 0x0400091F RID: 2335
			private RedBackList.TreeNode _root;

			// Token: 0x04000920 RID: 2336
			private bool _moved;
		}

		// Token: 0x0200018B RID: 395
		private class TreeNode
		{
			// Token: 0x06000B79 RID: 2937 RVA: 0x0002DB2F File Offset: 0x0002BD2F
			internal TreeNode(object key)
			{
				this._key = key;
			}

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0002DB3E File Offset: 0x0002BD3E
			// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0002DB46 File Offset: 0x0002BD46
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

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002DB63 File Offset: 0x0002BD63
			// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0002DB6B File Offset: 0x0002BD6B
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

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0002DB88 File Offset: 0x0002BD88
			// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0002DB90 File Offset: 0x0002BD90
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

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0002DB99 File Offset: 0x0002BD99
			// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0002DBA1 File Offset: 0x0002BDA1
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

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0002DBAA File Offset: 0x0002BDAA
			internal object Key
			{
				get
				{
					return this._key;
				}
			}

			// Token: 0x06000B83 RID: 2947 RVA: 0x0002DBB2 File Offset: 0x0002BDB2
			internal void CopyNode(RedBackList.TreeNode from)
			{
				this._key = from._key;
			}

			// Token: 0x04000921 RID: 2337
			private object _key;

			// Token: 0x04000922 RID: 2338
			private bool _isRed;

			// Token: 0x04000923 RID: 2339
			private RedBackList.TreeNode _leftChild;

			// Token: 0x04000924 RID: 2340
			private RedBackList.TreeNode _rightChild;

			// Token: 0x04000925 RID: 2341
			private RedBackList.TreeNode _parent;
		}

		// Token: 0x0200018C RID: 396
		private enum NodeColor
		{
			// Token: 0x04000927 RID: 2343
			BLACK,
			// Token: 0x04000928 RID: 2344
			RED
		}
	}
}
