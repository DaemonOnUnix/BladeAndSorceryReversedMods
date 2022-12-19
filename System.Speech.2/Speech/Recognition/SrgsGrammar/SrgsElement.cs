using System;
using System.Diagnostics;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000075 RID: 117
	[DebuggerDisplay("SrgsElement Children:[{_items.Count}]")]
	[DebuggerTypeProxy(typeof(SrgsElement.SrgsElementDebugDisplay))]
	[Serializable]
	public abstract class SrgsElement : MarshalByRefObject, IElement
	{
		// Token: 0x060003B7 RID: 951
		internal abstract void WriteSrgs(XmlWriter writer);

		// Token: 0x060003B8 RID: 952
		internal abstract string DebuggerDisplayString();

		// Token: 0x060003B9 RID: 953 RVA: 0x0000F798 File Offset: 0x0000D998
		internal virtual void Validate(SrgsGrammar grammar)
		{
			foreach (SrgsElement srgsElement in this.Children)
			{
				srgsElement.Validate(grammar);
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElement.PostParse(IElement parent)
		{
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000F7C5 File Offset: 0x0000D9C5
		internal virtual SrgsElement[] Children
		{
			get
			{
				return new SrgsElement[0];
			}
		}

		// Token: 0x0200017E RID: 382
		internal class SrgsElementDebugDisplay
		{
			// Token: 0x06000B54 RID: 2900 RVA: 0x0002D600 File Offset: 0x0002B800
			public SrgsElementDebugDisplay(SrgsElement element)
			{
				this._elements = element.Children;
			}

			// Token: 0x17000204 RID: 516
			// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0002D614 File Offset: 0x0002B814
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public SrgsElement[] AKeys
			{
				get
				{
					return this._elements;
				}
			}

			// Token: 0x040008B1 RID: 2225
			private SrgsElement[] _elements;
		}
	}
}
