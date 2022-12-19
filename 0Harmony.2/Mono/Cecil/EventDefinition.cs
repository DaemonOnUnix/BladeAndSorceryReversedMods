using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001FC RID: 508
	public sealed class EventDefinition : EventReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x000264FC File Offset: 0x000246FC
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x00026504 File Offset: 0x00024704
		public EventAttributes Attributes
		{
			get
			{
				return (EventAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0002650D File Offset: 0x0002470D
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0002652A File Offset: 0x0002472A
		public MethodDefinition AddMethod
		{
			get
			{
				if (this.add_method != null)
				{
					return this.add_method;
				}
				this.InitializeMethods();
				return this.add_method;
			}
			set
			{
				this.add_method = value;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00026533 File Offset: 0x00024733
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x00026550 File Offset: 0x00024750
		public MethodDefinition InvokeMethod
		{
			get
			{
				if (this.invoke_method != null)
				{
					return this.invoke_method;
				}
				this.InitializeMethods();
				return this.invoke_method;
			}
			set
			{
				this.invoke_method = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00026559 File Offset: 0x00024759
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x00026576 File Offset: 0x00024776
		public MethodDefinition RemoveMethod
		{
			get
			{
				if (this.remove_method != null)
				{
					return this.remove_method;
				}
				this.InitializeMethods();
				return this.remove_method;
			}
			set
			{
				this.remove_method = value;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002657F File Offset: 0x0002477F
		public bool HasOtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods.Count > 0;
				}
				this.InitializeMethods();
				return !this.other_methods.IsNullOrEmpty<MethodDefinition>();
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x000265AC File Offset: 0x000247AC
		public Collection<MethodDefinition> OtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				this.InitializeMethods();
				if (this.other_methods == null)
				{
					Interlocked.CompareExchange<Collection<MethodDefinition>>(ref this.other_methods, new Collection<MethodDefinition>(), null);
				}
				return this.other_methods;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000265E3 File Offset: 0x000247E3
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00026608 File Offset: 0x00024808
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00026626 File Offset: 0x00024826
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x00026638 File Offset: 0x00024838
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00026651 File Offset: 0x00024851
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x00026663 File Offset: 0x00024863
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0002667C File Offset: 0x0002487C
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00026689 File Offset: 0x00024889
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00026692 File Offset: 0x00024892
		public EventDefinition(string name, EventAttributes attributes, TypeReference eventType)
			: base(name, eventType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Event);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x000266B4 File Offset: 0x000248B4
		private void InitializeMethods()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (this.add_method == null && this.invoke_method == null && this.remove_method == null)
				{
					if (module.HasImage())
					{
						module.Read<EventDefinition>(this, delegate(EventDefinition @event, MetadataReader reader)
						{
							reader.ReadMethods(@event);
						});
					}
				}
			}
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00017E2C File Offset: 0x0001602C
		public override EventDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040002EE RID: 750
		private ushort attributes;

		// Token: 0x040002EF RID: 751
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040002F0 RID: 752
		internal MethodDefinition add_method;

		// Token: 0x040002F1 RID: 753
		internal MethodDefinition invoke_method;

		// Token: 0x040002F2 RID: 754
		internal MethodDefinition remove_method;

		// Token: 0x040002F3 RID: 755
		internal Collection<MethodDefinition> other_methods;
	}
}
