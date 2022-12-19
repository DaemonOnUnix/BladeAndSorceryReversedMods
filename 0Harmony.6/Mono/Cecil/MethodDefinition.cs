using System;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000142 RID: 322
	public sealed class MethodDefinition : MethodReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, ICustomDebugInformationProvider
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x00024209 File Offset: 0x00022409
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x00024211 File Offset: 0x00022411
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.Name)
				{
					throw new InvalidOperationException();
				}
				base.Name = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00024236 File Offset: 0x00022436
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0002423E File Offset: 0x0002243E
		public MethodAttributes Attributes
		{
			get
			{
				return (MethodAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (MethodAttributes)this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0002425E File Offset: 0x0002245E
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x00024266 File Offset: 0x00022466
		public MethodImplAttributes ImplAttributes
		{
			get
			{
				return (MethodImplAttributes)this.impl_attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (MethodImplAttributes)this.impl_attributes)
				{
					throw new InvalidOperationException();
				}
				this.impl_attributes = (ushort)value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x00024286 File Offset: 0x00022486
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x000242C4 File Offset: 0x000224C4
		public MethodSemanticsAttributes SemanticsAttributes
		{
			get
			{
				if (this.sem_attrs_ready)
				{
					return this.sem_attrs;
				}
				if (base.HasImage)
				{
					this.ReadSemantics();
					return this.sem_attrs;
				}
				this.sem_attrs = MethodSemanticsAttributes.None;
				this.sem_attrs_ready = true;
				return this.sem_attrs;
			}
			set
			{
				this.sem_attrs = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x000242CD File Offset: 0x000224CD
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x00020F72 File Offset: 0x0001F172
		internal new MethodDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (MethodDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000242DC File Offset: 0x000224DC
		internal void ReadSemantics()
		{
			if (this.sem_attrs_ready)
			{
				return;
			}
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				return;
			}
			if (!module.HasImage)
			{
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (!this.sem_attrs_ready)
				{
					module.Read<MethodDefinition>(this, delegate(MethodDefinition method, MetadataReader reader)
					{
						reader.ReadAllSemantics(method);
					});
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00024368 File Offset: 0x00022568
		public bool HasSecurityDeclarations
		{
			get
			{
				if (this.security_declarations != null)
				{
					return this.security_declarations.Count > 0;
				}
				return this.GetHasSecurityDeclarations(this.Module);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0002438D File Offset: 0x0002258D
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.Module);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x000243AB File Offset: 0x000225AB
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

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x000243D0 File Offset: 0x000225D0
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x000243EE File Offset: 0x000225EE
		public int RVA
		{
			get
			{
				return (int)this.rva;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x000243F8 File Offset: 0x000225F8
		public bool HasBody
		{
			get
			{
				return (this.attributes & 1024) == 0 && (this.attributes & 8192) == 0 && (this.impl_attributes & 4096) == 0 && (this.impl_attributes & 1) == 0 && (this.impl_attributes & 4) == 0 && (this.impl_attributes & 3) == 0;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00024450 File Offset: 0x00022650
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x000244D0 File Offset: 0x000226D0
		public MethodBody Body
		{
			get
			{
				MethodBody methodBody = this.body;
				if (methodBody != null)
				{
					return methodBody;
				}
				if (!this.HasBody)
				{
					return null;
				}
				if (base.HasImage && this.rva != 0U)
				{
					return this.Module.Read<MethodDefinition, MethodBody>(ref this.body, this, (MethodDefinition method, MetadataReader reader) => reader.ReadMethodBody(method));
				}
				Interlocked.CompareExchange<MethodBody>(ref this.body, new MethodBody(this), null);
				return this.body;
			}
			set
			{
				ModuleDefinition module = this.Module;
				if (module == null)
				{
					this.body = value;
					return;
				}
				object syncRoot = module.SyncRoot;
				lock (syncRoot)
				{
					this.body = value;
					if (value == null)
					{
						this.debug_info = null;
					}
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x00024530 File Offset: 0x00022730
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x0002455E File Offset: 0x0002275E
		public MethodDebugInformation DebugInformation
		{
			get
			{
				Mixin.Read(this.Body);
				if (this.debug_info == null)
				{
					Interlocked.CompareExchange<MethodDebugInformation>(ref this.debug_info, new MethodDebugInformation(this), null);
				}
				return this.debug_info;
			}
			set
			{
				this.debug_info = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00024567 File Offset: 0x00022767
		public bool HasPInvokeInfo
		{
			get
			{
				return this.pinvoke != null || this.IsPInvokeImpl;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0002457C File Offset: 0x0002277C
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x000245DB File Offset: 0x000227DB
		public PInvokeInfo PInvokeInfo
		{
			get
			{
				if (this.pinvoke != null)
				{
					return this.pinvoke;
				}
				if (base.HasImage && this.IsPInvokeImpl)
				{
					return this.Module.Read<MethodDefinition, PInvokeInfo>(ref this.pinvoke, this, (MethodDefinition method, MetadataReader reader) => reader.ReadPInvokeInfo(method));
				}
				return null;
			}
			set
			{
				this.IsPInvokeImpl = true;
				this.pinvoke = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x000245EC File Offset: 0x000227EC
		public bool HasOverrides
		{
			get
			{
				if (this.overrides != null)
				{
					return this.overrides.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<MethodDefinition, bool>(this, (MethodDefinition method, MetadataReader reader) => reader.HasOverrides(method));
				}
				return false;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00024648 File Offset: 0x00022848
		public Collection<MethodReference> Overrides
		{
			get
			{
				if (this.overrides != null)
				{
					return this.overrides;
				}
				if (base.HasImage)
				{
					return this.Module.Read<MethodDefinition, Collection<MethodReference>>(ref this.overrides, this, (MethodDefinition method, MetadataReader reader) => reader.ReadOverrides(method));
				}
				Interlocked.CompareExchange<Collection<MethodReference>>(ref this.overrides, new Collection<MethodReference>(), null);
				return this.overrides;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x000246B6 File Offset: 0x000228B6
		public override bool HasGenericParameters
		{
			get
			{
				if (this.generic_parameters != null)
				{
					return this.generic_parameters.Count > 0;
				}
				return this.GetHasGenericParameters(this.Module);
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000246DB File Offset: 0x000228DB
		public override Collection<GenericParameter> GenericParameters
		{
			get
			{
				return this.generic_parameters ?? this.GetGenericParameters(ref this.generic_parameters, this.Module);
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x000246F9 File Offset: 0x000228F9
		public bool HasCustomDebugInformations
		{
			get
			{
				Mixin.Read(this.Body);
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00024714 File Offset: 0x00022914
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				Mixin.Read(this.Body);
				if (this.custom_infos == null)
				{
					Interlocked.CompareExchange<Collection<CustomDebugInformation>>(ref this.custom_infos, new Collection<CustomDebugInformation>(), null);
				}
				return this.custom_infos;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00024741 File Offset: 0x00022941
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x00024750 File Offset: 0x00022950
		public bool IsCompilerControlled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 0U, value);
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00024766 File Offset: 0x00022966
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x00024775 File Offset: 0x00022975
		public bool IsPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 1U, value);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0002478B File Offset: 0x0002298B
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0002479A File Offset: 0x0002299A
		public bool IsFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 2U, value);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x000247B0 File Offset: 0x000229B0
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x000247BF File Offset: 0x000229BF
		public bool IsAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 3U, value);
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x000247D5 File Offset: 0x000229D5
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x000247E4 File Offset: 0x000229E4
		public bool IsFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 4U, value);
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x000247FA File Offset: 0x000229FA
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x00024809 File Offset: 0x00022A09
		public bool IsFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 5U, value);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0002481F File Offset: 0x00022A1F
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x0002482E File Offset: 0x00022A2E
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 6U, value);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00024844 File Offset: 0x00022A44
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x00024853 File Offset: 0x00022A53
		public bool IsStatic
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00024869 File Offset: 0x00022A69
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x00024878 File Offset: 0x00022A78
		public bool IsFinal
		{
			get
			{
				return this.attributes.GetAttributes(32);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0002488E File Offset: 0x00022A8E
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x0002489D File Offset: 0x00022A9D
		public bool IsVirtual
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x000248B3 File Offset: 0x00022AB3
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x000248C5 File Offset: 0x00022AC5
		public bool IsHideBySig
		{
			get
			{
				return this.attributes.GetAttributes(128);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x000248DE File Offset: 0x00022ADE
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x000248F1 File Offset: 0x00022AF1
		public bool IsReuseSlot
		{
			get
			{
				return this.attributes.GetMaskedAttributes(256, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(256, 0U, value);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002490B File Offset: 0x00022B0B
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00024922 File Offset: 0x00022B22
		public bool IsNewSlot
		{
			get
			{
				return this.attributes.GetMaskedAttributes(256, 256U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(256, 256U, value);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00024940 File Offset: 0x00022B40
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00024952 File Offset: 0x00022B52
		public bool IsCheckAccessOnOverride
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

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0002496B File Offset: 0x00022B6B
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0002497D File Offset: 0x00022B7D
		public bool IsAbstract
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

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00024996 File Offset: 0x00022B96
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x000249A8 File Offset: 0x00022BA8
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(2048);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2048, value);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x000249C1 File Offset: 0x00022BC1
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x000249D3 File Offset: 0x00022BD3
		public bool IsPInvokeImpl
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x000249EC File Offset: 0x00022BEC
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x000249FA File Offset: 0x00022BFA
		public bool IsUnmanagedExport
		{
			get
			{
				return this.attributes.GetAttributes(8);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00024A0F File Offset: 0x00022C0F
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x00024A21 File Offset: 0x00022C21
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(4096);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x00024A3A File Offset: 0x00022C3A
		// (set) Token: 0x06000979 RID: 2425 RVA: 0x00024A4C File Offset: 0x00022C4C
		public bool HasSecurity
		{
			get
			{
				return this.attributes.GetAttributes(16384);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16384, value);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x00024A65 File Offset: 0x00022C65
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x00024A74 File Offset: 0x00022C74
		public bool IsIL
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(3, 0U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(3, 0U, value);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x00024A8A File Offset: 0x00022C8A
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x00024A99 File Offset: 0x00022C99
		public bool IsNative
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(3, 1U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(3, 1U, value);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x00024AAF File Offset: 0x00022CAF
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x00024ABE File Offset: 0x00022CBE
		public bool IsRuntime
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(3, 3U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(3, 3U, value);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x00024AD4 File Offset: 0x00022CD4
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x00024AE3 File Offset: 0x00022CE3
		public bool IsUnmanaged
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(4, 4U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(4, 4U, value);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x00024AF9 File Offset: 0x00022CF9
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x00024B08 File Offset: 0x00022D08
		public bool IsManaged
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(4, 0U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(4, 0U, value);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x00024B1E File Offset: 0x00022D1E
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x00024B2D File Offset: 0x00022D2D
		public bool IsForwardRef
		{
			get
			{
				return this.impl_attributes.GetAttributes(16);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x00024B43 File Offset: 0x00022D43
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x00024B55 File Offset: 0x00022D55
		public bool IsPreserveSig
		{
			get
			{
				return this.impl_attributes.GetAttributes(128);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x00024B6E File Offset: 0x00022D6E
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x00024B80 File Offset: 0x00022D80
		public bool IsInternalCall
		{
			get
			{
				return this.impl_attributes.GetAttributes(4096);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x00024B99 File Offset: 0x00022D99
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x00024BA8 File Offset: 0x00022DA8
		public bool IsSynchronized
		{
			get
			{
				return this.impl_attributes.GetAttributes(32);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00024BBE File Offset: 0x00022DBE
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x00024BCC File Offset: 0x00022DCC
		public bool NoInlining
		{
			get
			{
				return this.impl_attributes.GetAttributes(8);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00024BE1 File Offset: 0x00022DE1
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x00024BF0 File Offset: 0x00022DF0
		public bool NoOptimization
		{
			get
			{
				return this.impl_attributes.GetAttributes(64);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00024C06 File Offset: 0x00022E06
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x00024C18 File Offset: 0x00022E18
		public bool AggressiveInlining
		{
			get
			{
				return this.impl_attributes.GetAttributes(256);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(256, value);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x00024C31 File Offset: 0x00022E31
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x00024C3A File Offset: 0x00022E3A
		public bool IsSetter
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Setter);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Setter, value);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00024C44 File Offset: 0x00022E44
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x00024C4D File Offset: 0x00022E4D
		public bool IsGetter
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Getter);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Getter, value);
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00024C57 File Offset: 0x00022E57
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x00024C60 File Offset: 0x00022E60
		public bool IsOther
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Other);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Other, value);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00024C6A File Offset: 0x00022E6A
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x00024C73 File Offset: 0x00022E73
		public bool IsAddOn
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.AddOn);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.AddOn, value);
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x00024C7D File Offset: 0x00022E7D
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x00024C87 File Offset: 0x00022E87
		public bool IsRemoveOn
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.RemoveOn);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.RemoveOn, value);
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00024C92 File Offset: 0x00022E92
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x00024C9C File Offset: 0x00022E9C
		public bool IsFire
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Fire);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Fire, value);
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x000207D4 File Offset: 0x0001E9D4
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x000207E1 File Offset: 0x0001E9E1
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00024CA7 File Offset: 0x00022EA7
		public bool IsConstructor
		{
			get
			{
				return this.IsRuntimeSpecialName && this.IsSpecialName && (this.Name == ".cctor" || this.Name == ".ctor");
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00024CDF File Offset: 0x00022EDF
		internal MethodDefinition()
		{
			this.token = new MetadataToken(TokenType.Method);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00024CF7 File Offset: 0x00022EF7
		public MethodDefinition(string name, MethodAttributes attributes, TypeReference returnType)
			: base(name, returnType)
		{
			this.attributes = (ushort)attributes;
			this.HasThis = !this.IsStatic;
			this.token = new MetadataToken(TokenType.Method);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override MethodDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000371 RID: 881
		private ushort attributes;

		// Token: 0x04000372 RID: 882
		private ushort impl_attributes;

		// Token: 0x04000373 RID: 883
		internal volatile bool sem_attrs_ready;

		// Token: 0x04000374 RID: 884
		internal MethodSemanticsAttributes sem_attrs;

		// Token: 0x04000375 RID: 885
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000376 RID: 886
		private Collection<SecurityDeclaration> security_declarations;

		// Token: 0x04000377 RID: 887
		internal uint rva;

		// Token: 0x04000378 RID: 888
		internal PInvokeInfo pinvoke;

		// Token: 0x04000379 RID: 889
		private Collection<MethodReference> overrides;

		// Token: 0x0400037A RID: 890
		internal MethodBody body;

		// Token: 0x0400037B RID: 891
		internal MethodDebugInformation debug_info;

		// Token: 0x0400037C RID: 892
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
