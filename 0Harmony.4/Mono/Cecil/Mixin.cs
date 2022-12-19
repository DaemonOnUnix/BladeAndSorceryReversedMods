using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;
using Mono.Security.Cryptography;

namespace Mono.Cecil
{
	// Token: 0x020001AA RID: 426
	internal static class Mixin
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x00018461 File Offset: 0x00016661
		public static bool IsNullOrEmpty<T>(this T[] self)
		{
			return self == null || self.Length == 0;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001846D File Offset: 0x0001666D
		public static bool IsNullOrEmpty<T>(this Collection<T> self)
		{
			return self == null || self.size == 0;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001847D File Offset: 0x0001667D
		public static T[] Resize<T>(this T[] self, int length)
		{
			Array.Resize<T>(ref self, length);
			return self;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00018488 File Offset: 0x00016688
		public static T[] Add<T>(this T[] self, T item)
		{
			if (self == null)
			{
				self = new T[] { item };
				return self;
			}
			self = self.Resize(self.Length + 1);
			self[self.Length - 1] = item;
			return self;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000184BC File Offset: 0x000166BC
		public static Version CheckVersion(Version version)
		{
			if (version == null)
			{
				return Mixin.ZeroVersion;
			}
			if (version.Build == -1)
			{
				return new Version(version.Major, version.Minor, 0, 0);
			}
			if (version.Revision == -1)
			{
				return new Version(version.Major, version.Minor, version.Build, 0);
			}
			return version;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00018518 File Offset: 0x00016718
		public static bool TryGetUniqueDocument(this MethodDebugInformation info, out Document document)
		{
			document = info.SequencePoints[0].Document;
			for (int i = 1; i < info.SequencePoints.Count; i++)
			{
				if (info.SequencePoints[i].Document != document)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00018568 File Offset: 0x00016768
		public static void ResolveConstant(this IConstantProvider self, ref object constant, ModuleDefinition module)
		{
			if (module == null)
			{
				constant = Mixin.NoValue;
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (constant == Mixin.NotResolved)
				{
					if (module.HasImage())
					{
						constant = module.Read<IConstantProvider, object>(self, (IConstantProvider provider, MetadataReader reader) => reader.ReadConstant(provider));
					}
					else
					{
						constant = Mixin.NoValue;
					}
				}
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000185F4 File Offset: 0x000167F4
		public static bool GetHasCustomAttributes(this ICustomAttributeProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ICustomAttributeProvider, bool>(self, (ICustomAttributeProvider provider, MetadataReader reader) => reader.HasCustomAttributes(provider));
			}
			return false;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00018628 File Offset: 0x00016828
		public static Collection<CustomAttribute> GetCustomAttributes(this ICustomAttributeProvider self, ref Collection<CustomAttribute> variable, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ICustomAttributeProvider, Collection<CustomAttribute>>(ref variable, self, (ICustomAttributeProvider provider, MetadataReader reader) => reader.ReadCustomAttributes(provider));
			}
			Interlocked.CompareExchange<Collection<CustomAttribute>>(ref variable, new Collection<CustomAttribute>(), null);
			return variable;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00018674 File Offset: 0x00016874
		public static bool ContainsGenericParameter(this IGenericInstance self)
		{
			Collection<TypeReference> genericArguments = self.GenericArguments;
			for (int i = 0; i < genericArguments.Count; i++)
			{
				if (genericArguments[i].ContainsGenericParameter)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000186AC File Offset: 0x000168AC
		public static void GenericInstanceFullName(this IGenericInstance self, StringBuilder builder)
		{
			builder.Append("<");
			Collection<TypeReference> genericArguments = self.GenericArguments;
			for (int i = 0; i < genericArguments.Count; i++)
			{
				if (i > 0)
				{
					builder.Append(",");
				}
				builder.Append(genericArguments[i].FullName);
			}
			builder.Append(">");
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001870C File Offset: 0x0001690C
		public static bool GetHasGenericParameters(this IGenericParameterProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IGenericParameterProvider, bool>(self, (IGenericParameterProvider provider, MetadataReader reader) => reader.HasGenericParameters(provider));
			}
			return false;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00018740 File Offset: 0x00016940
		public static Collection<GenericParameter> GetGenericParameters(this IGenericParameterProvider self, ref Collection<GenericParameter> collection, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IGenericParameterProvider, Collection<GenericParameter>>(ref collection, self, (IGenericParameterProvider provider, MetadataReader reader) => reader.ReadGenericParameters(provider));
			}
			Interlocked.CompareExchange<Collection<GenericParameter>>(ref collection, new GenericParameterCollection(self), null);
			return collection;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001878D File Offset: 0x0001698D
		public static bool GetHasMarshalInfo(this IMarshalInfoProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<IMarshalInfoProvider, bool>(self, (IMarshalInfoProvider provider, MetadataReader reader) => reader.HasMarshalInfo(provider));
			}
			return false;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000187BF File Offset: 0x000169BF
		public static MarshalInfo GetMarshalInfo(this IMarshalInfoProvider self, ref MarshalInfo variable, ModuleDefinition module)
		{
			if (!module.HasImage())
			{
				return null;
			}
			return module.Read<IMarshalInfoProvider, MarshalInfo>(ref variable, self, (IMarshalInfoProvider provider, MetadataReader reader) => reader.ReadMarshalInfo(provider));
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000187F2 File Offset: 0x000169F2
		public static bool GetAttributes(this uint self, uint attributes)
		{
			return (self & attributes) > 0U;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000187FA File Offset: 0x000169FA
		public static uint SetAttributes(this uint self, uint attributes, bool value)
		{
			if (value)
			{
				return self | attributes;
			}
			return self & ~attributes;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00018807 File Offset: 0x00016A07
		public static bool GetMaskedAttributes(this uint self, uint mask, uint attributes)
		{
			return (self & mask) == attributes;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001880F File Offset: 0x00016A0F
		public static uint SetMaskedAttributes(this uint self, uint mask, uint attributes, bool value)
		{
			if (value)
			{
				self &= ~mask;
				return self | attributes;
			}
			return self & ~(mask & attributes);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000187F2 File Offset: 0x000169F2
		public static bool GetAttributes(this ushort self, ushort attributes)
		{
			return (self & attributes) > 0;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00018824 File Offset: 0x00016A24
		public static ushort SetAttributes(this ushort self, ushort attributes, bool value)
		{
			if (value)
			{
				return self | attributes;
			}
			return self & ~attributes;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00018833 File Offset: 0x00016A33
		public static bool GetMaskedAttributes(this ushort self, ushort mask, uint attributes)
		{
			return (long)(self & mask) == (long)((ulong)attributes);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001883D File Offset: 0x00016A3D
		public static ushort SetMaskedAttributes(this ushort self, ushort mask, uint attributes, bool value)
		{
			if (value)
			{
				self &= ~mask;
				return (ushort)((uint)self | attributes);
			}
			return (ushort)((uint)self & ~((uint)mask & attributes));
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00018855 File Offset: 0x00016A55
		public static bool HasImplicitThis(this IMethodSignature self)
		{
			return self.HasThis && !self.ExplicitThis;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001886C File Offset: 0x00016A6C
		public static void MethodSignatureFullName(this IMethodSignature self, StringBuilder builder)
		{
			builder.Append("(");
			if (self.HasParameters)
			{
				Collection<ParameterDefinition> parameters = self.Parameters;
				for (int i = 0; i < parameters.Count; i++)
				{
					ParameterDefinition parameterDefinition = parameters[i];
					if (i > 0)
					{
						builder.Append(",");
					}
					if (parameterDefinition.ParameterType.IsSentinel)
					{
						builder.Append("...,");
					}
					builder.Append(parameterDefinition.ParameterType.FullName);
				}
			}
			builder.Append(")");
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000188F4 File Offset: 0x00016AF4
		public static void CheckModule(ModuleDefinition module)
		{
			if (module == null)
			{
				throw new ArgumentNullException(Mixin.Argument.module.ToString());
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001891C File Offset: 0x00016B1C
		public static bool TryGetAssemblyNameReference(this ModuleDefinition module, AssemblyNameReference name_reference, out AssemblyNameReference assembly_reference)
		{
			Collection<AssemblyNameReference> assemblyReferences = module.AssemblyReferences;
			for (int i = 0; i < assemblyReferences.Count; i++)
			{
				AssemblyNameReference assemblyNameReference = assemblyReferences[i];
				if (Mixin.Equals(name_reference, assemblyNameReference))
				{
					assembly_reference = assemblyNameReference;
					return true;
				}
			}
			assembly_reference = null;
			return false;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001895C File Offset: 0x00016B5C
		private static bool Equals(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00018997 File Offset: 0x00016B97
		private static bool Equals<T>(T a, T b) where T : class, IEquatable<T>
		{
			return a == b || (a != null && a.Equals(b));
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000189C0 File Offset: 0x00016BC0
		private static bool Equals(AssemblyNameReference a, AssemblyNameReference b)
		{
			return a == b || (!(a.Name != b.Name) && Mixin.Equals<Version>(a.Version, b.Version) && !(a.Culture != b.Culture) && Mixin.Equals(a.PublicKeyToken, b.PublicKeyToken));
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00018A28 File Offset: 0x00016C28
		public static ParameterDefinition GetParameter(this Mono.Cecil.Cil.MethodBody self, int index)
		{
			MethodDefinition method = self.method;
			if (method.HasThis)
			{
				if (index == 0)
				{
					return self.ThisParameter;
				}
				index--;
			}
			Collection<ParameterDefinition> parameters = method.Parameters;
			if (index < 0 || index >= parameters.size)
			{
				return null;
			}
			return parameters[index];
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00018A70 File Offset: 0x00016C70
		public static VariableDefinition GetVariable(this Mono.Cecil.Cil.MethodBody self, int index)
		{
			Collection<VariableDefinition> variables = self.Variables;
			if (index < 0 || index >= variables.size)
			{
				return null;
			}
			return variables[index];
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00018A9A File Offset: 0x00016C9A
		public static bool GetSemantics(this MethodDefinition self, MethodSemanticsAttributes semantics)
		{
			return (self.SemanticsAttributes & semantics) > MethodSemanticsAttributes.None;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00018AA7 File Offset: 0x00016CA7
		public static void SetSemantics(this MethodDefinition self, MethodSemanticsAttributes semantics, bool value)
		{
			if (value)
			{
				self.SemanticsAttributes |= semantics;
				return;
			}
			self.SemanticsAttributes &= ~semantics;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00018ACB File Offset: 0x00016CCB
		public static bool IsVarArg(this IMethodSignature self)
		{
			return self.CallingConvention == MethodCallingConvention.VarArg;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00018AD8 File Offset: 0x00016CD8
		public static int GetSentinelPosition(this IMethodSignature self)
		{
			if (!self.HasParameters)
			{
				return -1;
			}
			Collection<ParameterDefinition> parameters = self.Parameters;
			for (int i = 0; i < parameters.Count; i++)
			{
				if (parameters[i].ParameterType.IsSentinel)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00018B20 File Offset: 0x00016D20
		public static void CheckName(object name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(Mixin.Argument.name.ToString());
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00018B48 File Offset: 0x00016D48
		public static void CheckName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.name.ToString());
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00018B74 File Offset: 0x00016D74
		public static void CheckFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.fileName.ToString());
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00018BA0 File Offset: 0x00016DA0
		public static void CheckFullName(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				throw new ArgumentNullOrEmptyException(Mixin.Argument.fullName.ToString());
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00018BCC File Offset: 0x00016DCC
		public static void CheckStream(object stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(Mixin.Argument.stream.ToString());
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00018BF1 File Offset: 0x00016DF1
		public static void CheckWriteSeek(Stream stream)
		{
			if (!stream.CanWrite || !stream.CanSeek)
			{
				throw new ArgumentException("Stream must be writable and seekable.");
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00018C0E File Offset: 0x00016E0E
		public static void CheckReadSeek(Stream stream)
		{
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new ArgumentException("Stream must be readable and seekable.");
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00018C2C File Offset: 0x00016E2C
		public static void CheckType(object type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(Mixin.Argument.type.ToString());
			}
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00018C51 File Offset: 0x00016E51
		public static void CheckType(object type, Mixin.Argument argument)
		{
			if (type == null)
			{
				throw new ArgumentNullException(argument.ToString());
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00018C6C File Offset: 0x00016E6C
		public static void CheckField(object field)
		{
			if (field == null)
			{
				throw new ArgumentNullException(Mixin.Argument.field.ToString());
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00018C94 File Offset: 0x00016E94
		public static void CheckMethod(object method)
		{
			if (method == null)
			{
				throw new ArgumentNullException(Mixin.Argument.method.ToString());
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00018CBC File Offset: 0x00016EBC
		public static void CheckParameters(object parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(Mixin.Argument.parameters.ToString());
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00018CE4 File Offset: 0x00016EE4
		public static uint GetTimestamp()
		{
			return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00018D13 File Offset: 0x00016F13
		public static bool HasImage(this ModuleDefinition self)
		{
			return self != null && self.HasImage;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00018D20 File Offset: 0x00016F20
		public static string GetFileName(this Stream self)
		{
			FileStream fileStream = self as FileStream;
			if (fileStream == null)
			{
				return string.Empty;
			}
			return Path.GetFullPath(fileStream.Name);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00018D48 File Offset: 0x00016F48
		public static TargetRuntime ParseRuntime(this string self)
		{
			if (string.IsNullOrEmpty(self))
			{
				return TargetRuntime.Net_4_0;
			}
			switch (self[1])
			{
			case '1':
				if (self[3] != '0')
				{
					return TargetRuntime.Net_1_1;
				}
				return TargetRuntime.Net_1_0;
			case '2':
				return TargetRuntime.Net_2_0;
			}
			return TargetRuntime.Net_4_0;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00018D94 File Offset: 0x00016F94
		public static string RuntimeVersionString(this TargetRuntime runtime)
		{
			switch (runtime)
			{
			case TargetRuntime.Net_1_0:
				return "v1.0.3705";
			case TargetRuntime.Net_1_1:
				return "v1.1.4322";
			case TargetRuntime.Net_2_0:
				return "v2.0.50727";
			}
			return "v4.0.30319";
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00018DC5 File Offset: 0x00016FC5
		public static bool IsWindowsMetadata(this ModuleDefinition module)
		{
			return module.MetadataKind > MetadataKind.Ecma335;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00018DD0 File Offset: 0x00016FD0
		public static byte[] ReadAll(this Stream self)
		{
			MemoryStream memoryStream = new MemoryStream((int)self.Length);
			byte[] array = new byte[1024];
			int num;
			while ((num = self.Read(array, 0, array.Length)) != 0)
			{
				memoryStream.Write(array, 0, num);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00018105 File Offset: 0x00016305
		public static void Read(object o)
		{
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00018E15 File Offset: 0x00017015
		public static bool GetHasSecurityDeclarations(this ISecurityDeclarationProvider self, ModuleDefinition module)
		{
			if (module.HasImage())
			{
				return module.Read<ISecurityDeclarationProvider, bool>(self, (ISecurityDeclarationProvider provider, MetadataReader reader) => reader.HasSecurityDeclarations(provider));
			}
			return false;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00018E48 File Offset: 0x00017048
		public static Collection<SecurityDeclaration> GetSecurityDeclarations(this ISecurityDeclarationProvider self, ref Collection<SecurityDeclaration> variable, ModuleDefinition module)
		{
			if (module.HasImage)
			{
				return module.Read<ISecurityDeclarationProvider, Collection<SecurityDeclaration>>(ref variable, self, (ISecurityDeclarationProvider provider, MetadataReader reader) => reader.ReadSecurityDeclarations(provider));
			}
			Interlocked.CompareExchange<Collection<SecurityDeclaration>>(ref variable, new Collection<SecurityDeclaration>(), null);
			return variable;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00018E94 File Offset: 0x00017094
		public static TypeReference GetEnumUnderlyingType(this TypeDefinition self)
		{
			Collection<FieldDefinition> fields = self.Fields;
			for (int i = 0; i < fields.Count; i++)
			{
				FieldDefinition fieldDefinition = fields[i];
				if (!fieldDefinition.IsStatic)
				{
					return fieldDefinition.FieldType;
				}
			}
			throw new ArgumentException();
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00018ED8 File Offset: 0x000170D8
		public static TypeDefinition GetNestedType(this TypeDefinition self, string fullname)
		{
			if (!self.HasNestedTypes)
			{
				return null;
			}
			Collection<TypeDefinition> nestedTypes = self.NestedTypes;
			for (int i = 0; i < nestedTypes.Count; i++)
			{
				TypeDefinition typeDefinition = nestedTypes[i];
				if (typeDefinition.TypeFullName() == fullname)
				{
					return typeDefinition;
				}
			}
			return null;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00018F20 File Offset: 0x00017120
		public static bool IsPrimitive(this ElementType self)
		{
			return self - ElementType.Boolean <= 11 || self - ElementType.I <= 1;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00018F33 File Offset: 0x00017133
		public static string TypeFullName(this TypeReference self)
		{
			if (!string.IsNullOrEmpty(self.Namespace))
			{
				return self.Namespace + "." + self.Name;
			}
			return self.Name;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00018F5F File Offset: 0x0001715F
		public static bool IsTypeOf(this TypeReference self, string @namespace, string name)
		{
			return self.Name == name && self.Namespace == @namespace;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00018F80 File Offset: 0x00017180
		public static bool IsTypeSpecification(this TypeReference type)
		{
			ElementType etype = type.etype;
			switch (etype)
			{
			case ElementType.Ptr:
			case ElementType.ByRef:
			case ElementType.Var:
			case ElementType.Array:
			case ElementType.GenericInst:
			case ElementType.FnPtr:
			case ElementType.SzArray:
			case ElementType.MVar:
			case ElementType.CModReqD:
			case ElementType.CModOpt:
				break;
			case ElementType.ValueType:
			case ElementType.Class:
			case ElementType.TypedByRef:
			case (ElementType)23:
			case ElementType.I:
			case ElementType.U:
			case (ElementType)26:
			case ElementType.Object:
				return false;
			default:
				if (etype != ElementType.Sentinel && etype != ElementType.Pinned)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00018FF2 File Offset: 0x000171F2
		public static TypeDefinition CheckedResolve(this TypeReference self)
		{
			TypeDefinition typeDefinition = self.Resolve();
			if (typeDefinition == null)
			{
				throw new ResolutionException(self);
			}
			return typeDefinition;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00019004 File Offset: 0x00017204
		public static bool TryGetCoreLibraryReference(this ModuleDefinition module, out AssemblyNameReference reference)
		{
			Collection<AssemblyNameReference> assemblyReferences = module.AssemblyReferences;
			for (int i = 0; i < assemblyReferences.Count; i++)
			{
				reference = assemblyReferences[i];
				if (Mixin.IsCoreLibrary(reference))
				{
					return true;
				}
			}
			reference = null;
			return false;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00019044 File Offset: 0x00017244
		public static bool IsCoreLibrary(this ModuleDefinition module)
		{
			if (module.Assembly == null)
			{
				return false;
			}
			if (!Mixin.IsCoreLibrary(module.Assembly.Name))
			{
				return false;
			}
			if (module.HasImage)
			{
				if (module.Read<ModuleDefinition, bool>(module, (ModuleDefinition m, MetadataReader reader) => reader.image.GetTableLength(Table.AssemblyRef) > 0))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000190A2 File Offset: 0x000172A2
		public static void KnownValueType(this TypeReference type)
		{
			if (!type.IsDefinition)
			{
				type.IsValueType = true;
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000190B4 File Offset: 0x000172B4
		private static bool IsCoreLibrary(AssemblyNameReference reference)
		{
			string name = reference.Name;
			return name == "mscorlib" || name == "System.Runtime" || name == "System.Private.CoreLib" || name == "netstandard";
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000190FC File Offset: 0x000172FC
		public static ImageDebugHeaderEntry GetCodeViewEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.CodeView);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00019105 File Offset: 0x00017305
		public static ImageDebugHeaderEntry GetDeterministicEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.Deterministic);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00019110 File Offset: 0x00017310
		public static ImageDebugHeader AddDeterministicEntry(this ImageDebugHeader header)
		{
			ImageDebugHeaderEntry imageDebugHeaderEntry = new ImageDebugHeaderEntry(new ImageDebugDirectory
			{
				Type = ImageDebugType.Deterministic
			}, Empty<byte>.Array);
			if (header == null)
			{
				return new ImageDebugHeader(imageDebugHeaderEntry);
			}
			ImageDebugHeaderEntry[] array = new ImageDebugHeaderEntry[header.Entries.Length + 1];
			Array.Copy(header.Entries, array, header.Entries.Length);
			array[array.Length - 1] = imageDebugHeaderEntry;
			return new ImageDebugHeader(array);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00019176 File Offset: 0x00017376
		public static ImageDebugHeaderEntry GetEmbeddedPortablePdbEntry(this ImageDebugHeader header)
		{
			return header.GetEntry(ImageDebugType.EmbeddedPortablePdb);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00019180 File Offset: 0x00017380
		private static ImageDebugHeaderEntry GetEntry(this ImageDebugHeader header, ImageDebugType type)
		{
			if (!header.HasEntries)
			{
				return null;
			}
			for (int i = 0; i < header.Entries.Length; i++)
			{
				ImageDebugHeaderEntry imageDebugHeaderEntry = header.Entries[i];
				if (imageDebugHeaderEntry.Directory.Type == type)
				{
					return imageDebugHeaderEntry;
				}
			}
			return null;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000191C4 File Offset: 0x000173C4
		public static string GetPdbFileName(string assemblyFileName)
		{
			return Path.ChangeExtension(assemblyFileName, ".pdb");
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000191D1 File Offset: 0x000173D1
		public static string GetMdbFileName(string assemblyFileName)
		{
			return assemblyFileName + ".mdb";
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000191E0 File Offset: 0x000173E0
		public static bool IsPortablePdb(string fileName)
		{
			bool flag;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				flag = Mixin.IsPortablePdb(fileStream);
			}
			return flag;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001921C File Offset: 0x0001741C
		public static bool IsPortablePdb(Stream stream)
		{
			if (stream.Length < 4L)
			{
				return false;
			}
			long position = stream.Position;
			bool flag;
			try
			{
				flag = new BinaryReader(stream).ReadUInt32() == 1112167234U;
			}
			finally
			{
				stream.Position = position;
			}
			return flag;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001926C File Offset: 0x0001746C
		public static uint ReadCompressedUInt32(this byte[] data, ref int position)
		{
			uint num;
			if ((data[position] & 128) == 0)
			{
				num = (uint)data[position];
				position++;
			}
			else if ((data[position] & 64) == 0)
			{
				num = ((uint)data[position] & 4294967167U) << 8;
				num |= (uint)data[position + 1];
				position += 2;
			}
			else
			{
				num = ((uint)data[position] & 4294967103U) << 24;
				num |= (uint)((uint)data[position + 1] << 16);
				num |= (uint)((uint)data[position + 2] << 8);
				num |= (uint)data[position + 3];
				position += 4;
			}
			return num;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000192F0 File Offset: 0x000174F0
		public static MetadataToken GetMetadataToken(this CodedIndex self, uint data)
		{
			uint num;
			TokenType tokenType;
			switch (self)
			{
			case CodedIndex.TypeDefOrRef:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.TypeDef;
					break;
				case 1U:
					tokenType = TokenType.TypeRef;
					break;
				case 2U:
					tokenType = TokenType.TypeSpec;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.HasConstant:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.Field;
					break;
				case 1U:
					tokenType = TokenType.Param;
					break;
				case 2U:
					tokenType = TokenType.Property;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.HasCustomAttribute:
				num = data >> 5;
				switch (data & 31U)
				{
				case 0U:
					tokenType = TokenType.Method;
					break;
				case 1U:
					tokenType = TokenType.Field;
					break;
				case 2U:
					tokenType = TokenType.TypeRef;
					break;
				case 3U:
					tokenType = TokenType.TypeDef;
					break;
				case 4U:
					tokenType = TokenType.Param;
					break;
				case 5U:
					tokenType = TokenType.InterfaceImpl;
					break;
				case 6U:
					tokenType = TokenType.MemberRef;
					break;
				case 7U:
					tokenType = TokenType.Module;
					break;
				case 8U:
					tokenType = TokenType.Permission;
					break;
				case 9U:
					tokenType = TokenType.Property;
					break;
				case 10U:
					tokenType = TokenType.Event;
					break;
				case 11U:
					tokenType = TokenType.Signature;
					break;
				case 12U:
					tokenType = TokenType.ModuleRef;
					break;
				case 13U:
					tokenType = TokenType.TypeSpec;
					break;
				case 14U:
					tokenType = TokenType.Assembly;
					break;
				case 15U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 16U:
					tokenType = TokenType.File;
					break;
				case 17U:
					tokenType = TokenType.ExportedType;
					break;
				case 18U:
					tokenType = TokenType.ManifestResource;
					break;
				case 19U:
					tokenType = TokenType.GenericParam;
					break;
				case 20U:
					tokenType = TokenType.GenericParamConstraint;
					break;
				case 21U:
					tokenType = TokenType.MethodSpec;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.HasFieldMarshal:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_5BB;
					}
					tokenType = TokenType.Param;
				}
				else
				{
					tokenType = TokenType.Field;
				}
				break;
			}
			case CodedIndex.HasDeclSecurity:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.TypeDef;
					break;
				case 1U:
					tokenType = TokenType.Method;
					break;
				case 2U:
					tokenType = TokenType.Assembly;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.MemberRefParent:
				num = data >> 3;
				switch (data & 7U)
				{
				case 0U:
					tokenType = TokenType.TypeDef;
					break;
				case 1U:
					tokenType = TokenType.TypeRef;
					break;
				case 2U:
					tokenType = TokenType.ModuleRef;
					break;
				case 3U:
					tokenType = TokenType.Method;
					break;
				case 4U:
					tokenType = TokenType.TypeSpec;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.HasSemantics:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_5BB;
					}
					tokenType = TokenType.Property;
				}
				else
				{
					tokenType = TokenType.Event;
				}
				break;
			}
			case CodedIndex.MethodDefOrRef:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_5BB;
					}
					tokenType = TokenType.MemberRef;
				}
				else
				{
					tokenType = TokenType.Method;
				}
				break;
			}
			case CodedIndex.MemberForwarded:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_5BB;
					}
					tokenType = TokenType.Method;
				}
				else
				{
					tokenType = TokenType.Field;
				}
				break;
			}
			case CodedIndex.Implementation:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.File;
					break;
				case 1U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 2U:
					tokenType = TokenType.ExportedType;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.CustomAttributeType:
			{
				num = data >> 3;
				uint num2 = data & 7U;
				if (num2 != 2U)
				{
					if (num2 != 3U)
					{
						goto IL_5BB;
					}
					tokenType = TokenType.MemberRef;
				}
				else
				{
					tokenType = TokenType.Method;
				}
				break;
			}
			case CodedIndex.ResolutionScope:
				num = data >> 2;
				switch (data & 3U)
				{
				case 0U:
					tokenType = TokenType.Module;
					break;
				case 1U:
					tokenType = TokenType.ModuleRef;
					break;
				case 2U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 3U:
					tokenType = TokenType.TypeRef;
					break;
				default:
					goto IL_5BB;
				}
				break;
			case CodedIndex.TypeOrMethodDef:
			{
				num = data >> 1;
				uint num2 = data & 1U;
				if (num2 != 0U)
				{
					if (num2 != 1U)
					{
						goto IL_5BB;
					}
					tokenType = TokenType.Method;
				}
				else
				{
					tokenType = TokenType.TypeDef;
				}
				break;
			}
			case CodedIndex.HasCustomDebugInformation:
				num = data >> 5;
				switch (data & 31U)
				{
				case 0U:
					tokenType = TokenType.Method;
					break;
				case 1U:
					tokenType = TokenType.Field;
					break;
				case 2U:
					tokenType = TokenType.TypeRef;
					break;
				case 3U:
					tokenType = TokenType.TypeDef;
					break;
				case 4U:
					tokenType = TokenType.Param;
					break;
				case 5U:
					tokenType = TokenType.InterfaceImpl;
					break;
				case 6U:
					tokenType = TokenType.MemberRef;
					break;
				case 7U:
					tokenType = TokenType.Module;
					break;
				case 8U:
					tokenType = TokenType.Permission;
					break;
				case 9U:
					tokenType = TokenType.Property;
					break;
				case 10U:
					tokenType = TokenType.Event;
					break;
				case 11U:
					tokenType = TokenType.Signature;
					break;
				case 12U:
					tokenType = TokenType.ModuleRef;
					break;
				case 13U:
					tokenType = TokenType.TypeSpec;
					break;
				case 14U:
					tokenType = TokenType.Assembly;
					break;
				case 15U:
					tokenType = TokenType.AssemblyRef;
					break;
				case 16U:
					tokenType = TokenType.File;
					break;
				case 17U:
					tokenType = TokenType.ExportedType;
					break;
				case 18U:
					tokenType = TokenType.ManifestResource;
					break;
				case 19U:
					tokenType = TokenType.GenericParam;
					break;
				case 20U:
					tokenType = TokenType.GenericParamConstraint;
					break;
				case 21U:
					tokenType = TokenType.MethodSpec;
					break;
				case 22U:
					tokenType = TokenType.Document;
					break;
				case 23U:
					tokenType = TokenType.LocalScope;
					break;
				case 24U:
					tokenType = TokenType.LocalVariable;
					break;
				case 25U:
					tokenType = TokenType.LocalConstant;
					break;
				case 26U:
					tokenType = TokenType.ImportScope;
					break;
				default:
					goto IL_5BB;
				}
				break;
			default:
				goto IL_5BB;
			}
			return new MetadataToken(tokenType, num);
			IL_5BB:
			return MetadataToken.Zero;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000198C0 File Offset: 0x00017AC0
		public static uint CompressMetadataToken(this CodedIndex self, MetadataToken token)
		{
			uint num = 0U;
			if (token.RID == 0U)
			{
				return num;
			}
			switch (self)
			{
			case CodedIndex.TypeDefOrRef:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.TypeRef)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.TypeDef)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.TypeSpec)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.HasConstant:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Field)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Param)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.Property)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.HasCustomAttribute:
			{
				num = token.RID << 5;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.Event)
				{
					if (tokenType <= TokenType.Method)
					{
						if (tokenType <= TokenType.TypeRef)
						{
							if (tokenType == TokenType.Module)
							{
								return num | 7U;
							}
							if (tokenType == TokenType.TypeRef)
							{
								return num | 2U;
							}
						}
						else
						{
							if (tokenType == TokenType.TypeDef)
							{
								return num | 3U;
							}
							if (tokenType == TokenType.Field)
							{
								return num | 1U;
							}
							if (tokenType == TokenType.Method)
							{
								return num | 0U;
							}
						}
					}
					else if (tokenType <= TokenType.MemberRef)
					{
						if (tokenType == TokenType.Param)
						{
							return num | 4U;
						}
						if (tokenType == TokenType.InterfaceImpl)
						{
							return num | 5U;
						}
						if (tokenType == TokenType.MemberRef)
						{
							return num | 6U;
						}
					}
					else
					{
						if (tokenType == TokenType.Permission)
						{
							return num | 8U;
						}
						if (tokenType == TokenType.Signature)
						{
							return num | 11U;
						}
						if (tokenType == TokenType.Event)
						{
							return num | 10U;
						}
					}
				}
				else if (tokenType <= TokenType.AssemblyRef)
				{
					if (tokenType <= TokenType.ModuleRef)
					{
						if (tokenType == TokenType.Property)
						{
							return num | 9U;
						}
						if (tokenType == TokenType.ModuleRef)
						{
							return num | 12U;
						}
					}
					else
					{
						if (tokenType == TokenType.TypeSpec)
						{
							return num | 13U;
						}
						if (tokenType == TokenType.Assembly)
						{
							return num | 14U;
						}
						if (tokenType == TokenType.AssemblyRef)
						{
							return num | 15U;
						}
					}
				}
				else if (tokenType <= TokenType.ManifestResource)
				{
					if (tokenType == TokenType.File)
					{
						return num | 16U;
					}
					if (tokenType == TokenType.ExportedType)
					{
						return num | 17U;
					}
					if (tokenType == TokenType.ManifestResource)
					{
						return num | 18U;
					}
				}
				else
				{
					if (tokenType == TokenType.GenericParam)
					{
						return num | 19U;
					}
					if (tokenType == TokenType.MethodSpec)
					{
						return num | 21U;
					}
					if (tokenType == TokenType.GenericParamConstraint)
					{
						return num | 20U;
					}
				}
				break;
			}
			case CodedIndex.HasFieldMarshal:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Field)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Param)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.HasDeclSecurity:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.TypeDef)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Method)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.Assembly)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.MemberRefParent:
			{
				num = token.RID << 3;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.TypeDef)
				{
					if (tokenType == TokenType.TypeRef)
					{
						return num | 1U;
					}
					if (tokenType == TokenType.TypeDef)
					{
						return num | 0U;
					}
				}
				else
				{
					if (tokenType == TokenType.Method)
					{
						return num | 3U;
					}
					if (tokenType == TokenType.ModuleRef)
					{
						return num | 2U;
					}
					if (tokenType == TokenType.TypeSpec)
					{
						return num | 4U;
					}
				}
				break;
			}
			case CodedIndex.HasSemantics:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Event)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Property)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.MethodDefOrRef:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Method)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.MemberRef)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.MemberForwarded:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Field)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Method)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.Implementation:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.AssemblyRef)
				{
					return num | 1U;
				}
				if (tokenType == TokenType.File)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.ExportedType)
				{
					return num | 2U;
				}
				break;
			}
			case CodedIndex.CustomAttributeType:
			{
				num = token.RID << 3;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.Method)
				{
					return num | 2U;
				}
				if (tokenType == TokenType.MemberRef)
				{
					return num | 3U;
				}
				break;
			}
			case CodedIndex.ResolutionScope:
			{
				num = token.RID << 2;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.TypeRef)
				{
					if (tokenType == TokenType.Module)
					{
						return num | 0U;
					}
					if (tokenType == TokenType.TypeRef)
					{
						return num | 3U;
					}
				}
				else
				{
					if (tokenType == TokenType.ModuleRef)
					{
						return num | 1U;
					}
					if (tokenType == TokenType.AssemblyRef)
					{
						return num | 2U;
					}
				}
				break;
			}
			case CodedIndex.TypeOrMethodDef:
			{
				num = token.RID << 1;
				TokenType tokenType = token.TokenType;
				if (tokenType == TokenType.TypeDef)
				{
					return num | 0U;
				}
				if (tokenType == TokenType.Method)
				{
					return num | 1U;
				}
				break;
			}
			case CodedIndex.HasCustomDebugInformation:
			{
				num = token.RID << 5;
				TokenType tokenType = token.TokenType;
				if (tokenType <= TokenType.ModuleRef)
				{
					if (tokenType <= TokenType.Param)
					{
						if (tokenType <= TokenType.TypeDef)
						{
							if (tokenType == TokenType.Module)
							{
								return num | 7U;
							}
							if (tokenType == TokenType.TypeRef)
							{
								return num | 2U;
							}
							if (tokenType == TokenType.TypeDef)
							{
								return num | 3U;
							}
						}
						else
						{
							if (tokenType == TokenType.Field)
							{
								return num | 1U;
							}
							if (tokenType == TokenType.Method)
							{
								return num | 0U;
							}
							if (tokenType == TokenType.Param)
							{
								return num | 4U;
							}
						}
					}
					else if (tokenType <= TokenType.Permission)
					{
						if (tokenType == TokenType.InterfaceImpl)
						{
							return num | 5U;
						}
						if (tokenType == TokenType.MemberRef)
						{
							return num | 6U;
						}
						if (tokenType == TokenType.Permission)
						{
							return num | 8U;
						}
					}
					else if (tokenType <= TokenType.Event)
					{
						if (tokenType == TokenType.Signature)
						{
							return num | 11U;
						}
						if (tokenType == TokenType.Event)
						{
							return num | 10U;
						}
					}
					else
					{
						if (tokenType == TokenType.Property)
						{
							return num | 9U;
						}
						if (tokenType == TokenType.ModuleRef)
						{
							return num | 12U;
						}
					}
				}
				else if (tokenType <= TokenType.GenericParam)
				{
					if (tokenType <= TokenType.AssemblyRef)
					{
						if (tokenType == TokenType.TypeSpec)
						{
							return num | 13U;
						}
						if (tokenType == TokenType.Assembly)
						{
							return num | 14U;
						}
						if (tokenType == TokenType.AssemblyRef)
						{
							return num | 15U;
						}
					}
					else if (tokenType <= TokenType.ExportedType)
					{
						if (tokenType == TokenType.File)
						{
							return num | 16U;
						}
						if (tokenType == TokenType.ExportedType)
						{
							return num | 17U;
						}
					}
					else
					{
						if (tokenType == TokenType.ManifestResource)
						{
							return num | 18U;
						}
						if (tokenType == TokenType.GenericParam)
						{
							return num | 19U;
						}
					}
				}
				else if (tokenType <= TokenType.Document)
				{
					if (tokenType == TokenType.MethodSpec)
					{
						return num | 21U;
					}
					if (tokenType == TokenType.GenericParamConstraint)
					{
						return num | 20U;
					}
					if (tokenType == TokenType.Document)
					{
						return num | 22U;
					}
				}
				else if (tokenType <= TokenType.LocalVariable)
				{
					if (tokenType == TokenType.LocalScope)
					{
						return num | 23U;
					}
					if (tokenType == TokenType.LocalVariable)
					{
						return num | 24U;
					}
				}
				else
				{
					if (tokenType == TokenType.LocalConstant)
					{
						return num | 25U;
					}
					if (tokenType == TokenType.ImportScope)
					{
						return num | 26U;
					}
				}
				break;
			}
			}
			throw new ArgumentException();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00019FC4 File Offset: 0x000181C4
		public static int GetSize(this CodedIndex self, Func<Table, int> counter)
		{
			int num;
			Table[] array;
			switch (self)
			{
			case CodedIndex.TypeDefOrRef:
				num = 2;
				array = new Table[]
				{
					Table.TypeDef,
					Table.TypeRef,
					Table.TypeSpec
				};
				break;
			case CodedIndex.HasConstant:
				num = 2;
				array = new Table[]
				{
					Table.Field,
					Table.Param,
					Table.Property
				};
				break;
			case CodedIndex.HasCustomAttribute:
				num = 5;
				array = new Table[]
				{
					Table.Method,
					Table.Field,
					Table.TypeRef,
					Table.TypeDef,
					Table.Param,
					Table.InterfaceImpl,
					Table.MemberRef,
					Table.Module,
					Table.DeclSecurity,
					Table.Property,
					Table.Event,
					Table.StandAloneSig,
					Table.ModuleRef,
					Table.TypeSpec,
					Table.Assembly,
					Table.AssemblyRef,
					Table.File,
					Table.ExportedType,
					Table.ManifestResource,
					Table.GenericParam,
					Table.GenericParamConstraint,
					Table.MethodSpec
				};
				break;
			case CodedIndex.HasFieldMarshal:
				num = 1;
				array = new Table[]
				{
					Table.Field,
					Table.Param
				};
				break;
			case CodedIndex.HasDeclSecurity:
				num = 2;
				array = new Table[]
				{
					Table.TypeDef,
					Table.Method,
					Table.Assembly
				};
				break;
			case CodedIndex.MemberRefParent:
				num = 3;
				array = new Table[]
				{
					Table.TypeDef,
					Table.TypeRef,
					Table.ModuleRef,
					Table.Method,
					Table.TypeSpec
				};
				break;
			case CodedIndex.HasSemantics:
				num = 1;
				array = new Table[]
				{
					Table.Event,
					Table.Property
				};
				break;
			case CodedIndex.MethodDefOrRef:
				num = 1;
				array = new Table[]
				{
					Table.Method,
					Table.MemberRef
				};
				break;
			case CodedIndex.MemberForwarded:
				num = 1;
				array = new Table[]
				{
					Table.Field,
					Table.Method
				};
				break;
			case CodedIndex.Implementation:
				num = 2;
				array = new Table[]
				{
					Table.File,
					Table.AssemblyRef,
					Table.ExportedType
				};
				break;
			case CodedIndex.CustomAttributeType:
				num = 3;
				array = new Table[]
				{
					Table.Method,
					Table.MemberRef
				};
				break;
			case CodedIndex.ResolutionScope:
				num = 2;
				array = new Table[]
				{
					Table.Module,
					Table.ModuleRef,
					Table.AssemblyRef,
					Table.TypeRef
				};
				break;
			case CodedIndex.TypeOrMethodDef:
				num = 1;
				array = new Table[]
				{
					Table.TypeDef,
					Table.Method
				};
				break;
			case CodedIndex.HasCustomDebugInformation:
				num = 5;
				array = new Table[]
				{
					Table.Method,
					Table.Field,
					Table.TypeRef,
					Table.TypeDef,
					Table.Param,
					Table.InterfaceImpl,
					Table.MemberRef,
					Table.Module,
					Table.DeclSecurity,
					Table.Property,
					Table.Event,
					Table.StandAloneSig,
					Table.ModuleRef,
					Table.TypeSpec,
					Table.Assembly,
					Table.AssemblyRef,
					Table.File,
					Table.ExportedType,
					Table.ManifestResource,
					Table.GenericParam,
					Table.GenericParamConstraint,
					Table.MethodSpec,
					Table.Document,
					Table.LocalScope,
					Table.LocalVariable,
					Table.LocalConstant,
					Table.ImportScope
				};
				break;
			default:
				throw new ArgumentException();
			}
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num2 = Math.Max(counter(array[i]), num2);
			}
			if (num2 >= 1 << 16 - num)
			{
				return 4;
			}
			return 2;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001A29C File Offset: 0x0001849C
		public static RSA CreateRSA(this WriterParameters writer_parameters)
		{
			if (writer_parameters.StrongNameKeyBlob != null)
			{
				return CryptoConvert.FromCapiKeyBlob(writer_parameters.StrongNameKeyBlob);
			}
			string strongNameKeyContainer;
			byte[] array;
			if (writer_parameters.StrongNameKeyContainer != null)
			{
				strongNameKeyContainer = writer_parameters.StrongNameKeyContainer;
			}
			else if (!Mixin.TryGetKeyContainer(writer_parameters.StrongNameKeyPair, out array, out strongNameKeyContainer))
			{
				return CryptoConvert.FromCapiKeyBlob(array);
			}
			return new RSACryptoServiceProvider(new CspParameters
			{
				Flags = CspProviderFlags.UseMachineKeyStore,
				KeyContainerName = strongNameKeyContainer,
				KeyNumber = 2
			});
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001A308 File Offset: 0x00018508
		private static bool TryGetKeyContainer(ISerializable key_pair, out byte[] key, out string key_container)
		{
			SerializationInfo serializationInfo = new SerializationInfo(typeof(StrongNameKeyPair), new FormatterConverter());
			key_pair.GetObjectData(serializationInfo, default(StreamingContext));
			key = (byte[])serializationInfo.GetValue("_keyPairArray", typeof(byte[]));
			key_container = serializationInfo.GetString("_keyPairContainer");
			return key_container != null;
		}

		// Token: 0x04000236 RID: 566
		public static Version ZeroVersion = new Version(0, 0, 0, 0);

		// Token: 0x04000237 RID: 567
		public const int NotResolvedMarker = -2;

		// Token: 0x04000238 RID: 568
		public const int NoDataMarker = -1;

		// Token: 0x04000239 RID: 569
		internal static object NoValue = new object();

		// Token: 0x0400023A RID: 570
		internal static object NotResolved = new object();

		// Token: 0x0400023B RID: 571
		public const string mscorlib = "mscorlib";

		// Token: 0x0400023C RID: 572
		public const string system_runtime = "System.Runtime";

		// Token: 0x0400023D RID: 573
		public const string system_private_corelib = "System.Private.CoreLib";

		// Token: 0x0400023E RID: 574
		public const string netstandard = "netstandard";

		// Token: 0x0400023F RID: 575
		public const int TableCount = 58;

		// Token: 0x04000240 RID: 576
		public const int CodedIndexCount = 14;

		// Token: 0x020001AB RID: 427
		public enum Argument
		{
			// Token: 0x04000242 RID: 578
			name,
			// Token: 0x04000243 RID: 579
			fileName,
			// Token: 0x04000244 RID: 580
			fullName,
			// Token: 0x04000245 RID: 581
			stream,
			// Token: 0x04000246 RID: 582
			type,
			// Token: 0x04000247 RID: 583
			method,
			// Token: 0x04000248 RID: 584
			field,
			// Token: 0x04000249 RID: 585
			parameters,
			// Token: 0x0400024A RID: 586
			module,
			// Token: 0x0400024B RID: 587
			modifierType,
			// Token: 0x0400024C RID: 588
			eventType,
			// Token: 0x0400024D RID: 589
			fieldType,
			// Token: 0x0400024E RID: 590
			declaringType,
			// Token: 0x0400024F RID: 591
			returnType,
			// Token: 0x04000250 RID: 592
			propertyType,
			// Token: 0x04000251 RID: 593
			interfaceType,
			// Token: 0x04000252 RID: 594
			constraintType
		}
	}
}
