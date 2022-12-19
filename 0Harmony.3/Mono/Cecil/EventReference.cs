using System;

namespace Mono.Cecil
{
	// Token: 0x0200010C RID: 268
	public abstract class EventReference : MemberReference
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x000208B1 File Offset: 0x0001EAB1
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x000208B9 File Offset: 0x0001EAB9
		public TypeReference EventType
		{
			get
			{
				return this.event_type;
			}
			set
			{
				this.event_type = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x000208C2 File Offset: 0x0001EAC2
		public override string FullName
		{
			get
			{
				return this.event_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000208DF File Offset: 0x0001EADF
		protected EventReference(string name, TypeReference eventType)
			: base(name)
		{
			Mixin.CheckType(eventType, Mixin.Argument.eventType);
			this.event_type = eventType;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000208F7 File Offset: 0x0001EAF7
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x0600073F RID: 1855
		public new abstract EventDefinition Resolve();

		// Token: 0x040002C4 RID: 708
		private TypeReference event_type;
	}
}
