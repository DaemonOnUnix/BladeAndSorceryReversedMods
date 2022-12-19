using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200010A RID: 266
	public sealed class EventDefinition : EventReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00020654 File Offset: 0x0001E854
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0002065C File Offset: 0x0001E85C
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00020665 File Offset: 0x0001E865
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x00020682 File Offset: 0x0001E882
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0002068B File Offset: 0x0001E88B
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x000206A8 File Offset: 0x0001E8A8
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x000206B1 File Offset: 0x0001E8B1
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x000206CE File Offset: 0x0001E8CE
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000206D7 File Offset: 0x0001E8D7
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x00020704 File Offset: 0x0001E904
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0002073B File Offset: 0x0001E93B
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00020760 File Offset: 0x0001E960
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0002077E File Offset: 0x0001E97E
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00020790 File Offset: 0x0001E990
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x000207A9 File Offset: 0x0001E9A9
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x000207BB File Offset: 0x0001E9BB
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x000207D4 File Offset: 0x0001E9D4
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x000207E1 File Offset: 0x0001E9E1
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000207EA File Offset: 0x0001E9EA
		public EventDefinition(string name, EventAttributes attributes, TypeReference eventType)
			: base(name, eventType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Event);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002080C File Offset: 0x0001EA0C
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

		// Token: 0x06000736 RID: 1846 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override EventDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040002BC RID: 700
		private ushort attributes;

		// Token: 0x040002BD RID: 701
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040002BE RID: 702
		internal MethodDefinition add_method;

		// Token: 0x040002BF RID: 703
		internal MethodDefinition invoke_method;

		// Token: 0x040002C0 RID: 704
		internal MethodDefinition remove_method;

		// Token: 0x040002C1 RID: 705
		internal Collection<MethodDefinition> other_methods;
	}
}
