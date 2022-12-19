using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200021F RID: 543
	internal struct ImportGenericContext
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00028064 File Offset: 0x00026264
		public bool IsEmpty
		{
			get
			{
				return this.stack == null;
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002806F File Offset: 0x0002626F
		public ImportGenericContext(IGenericParameterProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.stack = null;
			this.Push(provider);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002808D File Offset: 0x0002628D
		public void Push(IGenericParameterProvider provider)
		{
			if (this.stack == null)
			{
				this.stack = new Collection<IGenericParameterProvider>(1) { provider };
				return;
			}
			this.stack.Add(provider);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000280B7 File Offset: 0x000262B7
		public void Pop()
		{
			this.stack.RemoveAt(this.stack.Count - 1);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000280D4 File Offset: 0x000262D4
		public TypeReference MethodParameter(string method, int position)
		{
			for (int i = this.stack.Count - 1; i >= 0; i--)
			{
				MethodReference methodReference = this.stack[i] as MethodReference;
				if (methodReference != null && !(method != this.NormalizeMethodName(methodReference)))
				{
					return methodReference.GenericParameters[position];
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002812F File Offset: 0x0002632F
		public string NormalizeMethodName(MethodReference method)
		{
			return method.DeclaringType.GetElementType().FullName + "." + method.Name;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00028154 File Offset: 0x00026354
		public TypeReference TypeParameter(string type, int position)
		{
			for (int i = this.stack.Count - 1; i >= 0; i--)
			{
				TypeReference typeReference = ImportGenericContext.GenericTypeFor(this.stack[i]);
				if (!(typeReference.FullName != type))
				{
					return typeReference.GenericParameters[position];
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000281AC File Offset: 0x000263AC
		private static TypeReference GenericTypeFor(IGenericParameterProvider context)
		{
			TypeReference typeReference = context as TypeReference;
			if (typeReference != null)
			{
				return typeReference.GetElementType();
			}
			MethodReference methodReference = context as MethodReference;
			if (methodReference != null)
			{
				return methodReference.DeclaringType.GetElementType();
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000281E8 File Offset: 0x000263E8
		public static ImportGenericContext For(IGenericParameterProvider context)
		{
			if (context == null)
			{
				return default(ImportGenericContext);
			}
			return new ImportGenericContext(context);
		}

		// Token: 0x04000342 RID: 834
		private Collection<IGenericParameterProvider> stack;
	}
}
