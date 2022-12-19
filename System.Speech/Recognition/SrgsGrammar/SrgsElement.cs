using System;
using System.Diagnostics;
using System.Speech.Internal.SrgsParser;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000113 RID: 275
	[DebuggerDisplay("SrgsElement Children:[{_items.Count}]")]
	[DebuggerTypeProxy(typeof(SrgsElement.SrgsElementDebugDisplay))]
	[Serializable]
	public abstract class SrgsElement : MarshalByRefObject, IElement
	{
		// Token: 0x060006FD RID: 1789
		internal abstract void WriteSrgs(XmlWriter writer);

		// Token: 0x060006FE RID: 1790
		internal abstract string DebuggerDisplayString();

		// Token: 0x060006FF RID: 1791 RVA: 0x0001FEE0 File Offset: 0x0001EEE0
		internal virtual void Validate(SrgsGrammar grammar)
		{
			foreach (SrgsElement srgsElement in this.Children)
			{
				srgsElement.Validate(grammar);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001FF0D File Offset: 0x0001EF0D
		void IElement.PostParse(IElement parent)
		{
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001FF0F File Offset: 0x0001EF0F
		internal virtual SrgsElement[] Children
		{
			get
			{
				return new SrgsElement[0];
			}
		}

		// Token: 0x02000114 RID: 276
		internal class SrgsElementDebugDisplay
		{
			// Token: 0x06000702 RID: 1794 RVA: 0x0001FF17 File Offset: 0x0001EF17
			public SrgsElementDebugDisplay(SrgsElement element)
			{
				this._elements = element.Children;
			}

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001FF2B File Offset: 0x0001EF2B
			[DebuggerBrowsable(3)]
			public SrgsElement[] AKeys
			{
				get
				{
					return this._elements;
				}
			}

			// Token: 0x0400054D RID: 1357
			private SrgsElement[] _elements;
		}
	}
}
