using System;

namespace Mono.Cecil
{
	// Token: 0x020001FE RID: 510
	public abstract class EventReference : MemberReference
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00026759 File Offset: 0x00024959
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x00026761 File Offset: 0x00024961
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

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0002676A File Offset: 0x0002496A
		public override string FullName
		{
			get
			{
				return this.event_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00026787 File Offset: 0x00024987
		protected EventReference(string name, TypeReference eventType)
			: base(name)
		{
			Mixin.CheckType(eventType, Mixin.Argument.eventType);
			this.event_type = eventType;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002679F File Offset: 0x0002499F
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000A77 RID: 2679
		public new abstract EventDefinition Resolve();

		// Token: 0x040002F6 RID: 758
		private TypeReference event_type;
	}
}
