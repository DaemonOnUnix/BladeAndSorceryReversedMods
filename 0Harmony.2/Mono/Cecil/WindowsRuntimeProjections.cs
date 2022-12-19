﻿using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000281 RID: 641
	internal sealed class WindowsRuntimeProjections
	{
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00030E0C File Offset: 0x0002F00C
		private static Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> Projections
		{
			get
			{
				if (WindowsRuntimeProjections.projections != null)
				{
					return WindowsRuntimeProjections.projections;
				}
				Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> dictionary = new Dictionary<string, WindowsRuntimeProjections.ProjectionInfo>
				{
					{
						"AttributeTargets",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Metadata", "System", "AttributeTargets", "System.Runtime", false)
					},
					{
						"AttributeUsageAttribute",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Metadata", "System", "AttributeUsageAttribute", "System.Runtime", true)
					},
					{
						"Color",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI", "Windows.UI", "Color", "System.Runtime.WindowsRuntime", false)
					},
					{
						"CornerRadius",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "CornerRadius", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"DateTime",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "DateTimeOffset", "System.Runtime", false)
					},
					{
						"Duration",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "Duration", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"DurationType",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "DurationType", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"EventHandler`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "EventHandler`1", "System.Runtime", false)
					},
					{
						"EventRegistrationToken",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System.Runtime.InteropServices.WindowsRuntime", "EventRegistrationToken", "System.Runtime.InteropServices.WindowsRuntime", false)
					},
					{
						"GeneratorPosition",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Controls.Primitives", "Windows.UI.Xaml.Controls.Primitives", "GeneratorPosition", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"GridLength",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "GridLength", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"GridUnitType",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "GridUnitType", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"HResult",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Exception", "System.Runtime", false)
					},
					{
						"IBindableIterable",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections", "IEnumerable", "System.Runtime", false)
					},
					{
						"IBindableVector",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections", "IList", "System.Runtime", false)
					},
					{
						"IClosable",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "IDisposable", "System.Runtime", false)
					},
					{
						"ICommand",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Input", "System.Windows.Input", "ICommand", "System.ObjectModel", false)
					},
					{
						"IIterable`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IEnumerable`1", "System.Runtime", false)
					},
					{
						"IKeyValuePair`2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "KeyValuePair`2", "System.Runtime", false)
					},
					{
						"IMapView`2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IReadOnlyDictionary`2", "System.Runtime", false)
					},
					{
						"IMap`2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IDictionary`2", "System.Runtime", false)
					},
					{
						"INotifyCollectionChanged",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "INotifyCollectionChanged", "System.ObjectModel", false)
					},
					{
						"INotifyPropertyChanged",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "INotifyPropertyChanged", "System.ObjectModel", false)
					},
					{
						"IReference`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Nullable`1", "System.Runtime", false)
					},
					{
						"IVectorView`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IReadOnlyList`1", "System.Runtime", false)
					},
					{
						"IVector`1",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Collections", "System.Collections.Generic", "IList`1", "System.Runtime", false)
					},
					{
						"KeyTime",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "KeyTime", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"Matrix",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media", "Windows.UI.Xaml.Media", "Matrix", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"Matrix3D",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Media3D", "Windows.UI.Xaml.Media.Media3D", "Matrix3D", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"Matrix3x2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Matrix3x2", "System.Numerics.Vectors", false)
					},
					{
						"Matrix4x4",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Matrix4x4", "System.Numerics.Vectors", false)
					},
					{
						"NotifyCollectionChangedAction",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedAction", "System.ObjectModel", false)
					},
					{
						"NotifyCollectionChangedEventArgs",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedEventArgs", "System.ObjectModel", false)
					},
					{
						"NotifyCollectionChangedEventHandler",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System.Collections.Specialized", "NotifyCollectionChangedEventHandler", "System.ObjectModel", false)
					},
					{
						"Plane",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Plane", "System.Numerics.Vectors", false)
					},
					{
						"Point",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Point", "System.Runtime.WindowsRuntime", false)
					},
					{
						"PropertyChangedEventArgs",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "PropertyChangedEventArgs", "System.ObjectModel", false)
					},
					{
						"PropertyChangedEventHandler",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Data", "System.ComponentModel", "PropertyChangedEventHandler", "System.ObjectModel", false)
					},
					{
						"Quaternion",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Quaternion", "System.Numerics.Vectors", false)
					},
					{
						"Rect",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Rect", "System.Runtime.WindowsRuntime", false)
					},
					{
						"RepeatBehavior",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "RepeatBehavior", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"RepeatBehaviorType",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Media.Animation", "Windows.UI.Xaml.Media.Animation", "RepeatBehaviorType", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"Size",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "Windows.Foundation", "Size", "System.Runtime.WindowsRuntime", false)
					},
					{
						"Thickness",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml", "Windows.UI.Xaml", "Thickness", "System.Runtime.WindowsRuntime.UI.Xaml", false)
					},
					{
						"TimeSpan",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "TimeSpan", "System.Runtime", false)
					},
					{
						"TypeName",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.UI.Xaml.Interop", "System", "Type", "System.Runtime", false)
					},
					{
						"Uri",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation", "System", "Uri", "System.Runtime", false)
					},
					{
						"Vector2",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector2", "System.Numerics.Vectors", false)
					},
					{
						"Vector3",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector3", "System.Numerics.Vectors", false)
					},
					{
						"Vector4",
						new WindowsRuntimeProjections.ProjectionInfo("Windows.Foundation.Numerics", "System.Numerics", "Vector4", "System.Numerics.Vectors", false)
					}
				};
				Interlocked.CompareExchange<Dictionary<string, WindowsRuntimeProjections.ProjectionInfo>>(ref WindowsRuntimeProjections.projections, dictionary, null);
				return WindowsRuntimeProjections.projections;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00031578 File Offset: 0x0002F778
		private AssemblyNameReference[] VirtualReferences
		{
			get
			{
				if (this.virtual_references == null)
				{
					Mixin.Read(this.module.AssemblyReferences);
				}
				return this.virtual_references;
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00031598 File Offset: 0x0002F798
		public WindowsRuntimeProjections(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x000315C8 File Offset: 0x0002F7C8
		public static void Project(TypeDefinition type)
		{
			TypeDefinitionTreatment typeDefinitionTreatment = TypeDefinitionTreatment.None;
			MetadataKind metadataKind = type.Module.MetadataKind;
			Collection<MethodDefinition> collection = null;
			Collection<KeyValuePair<InterfaceImplementation, InterfaceImplementation>> collection2 = null;
			if (type.IsWindowsRuntime)
			{
				if (metadataKind == MetadataKind.WindowsMetadata)
				{
					typeDefinitionTreatment = WindowsRuntimeProjections.GetWellKnownTypeDefinitionTreatment(type);
					if (typeDefinitionTreatment != TypeDefinitionTreatment.None)
					{
						WindowsRuntimeProjections.ApplyProjection(type, new TypeDefinitionProjection(type, typeDefinitionTreatment, collection, collection2));
						return;
					}
					TypeReference baseType = type.BaseType;
					if (baseType != null && WindowsRuntimeProjections.IsAttribute(baseType))
					{
						typeDefinitionTreatment = TypeDefinitionTreatment.NormalAttribute;
					}
					else
					{
						typeDefinitionTreatment = WindowsRuntimeProjections.GenerateRedirectionInformation(type, out collection, out collection2);
					}
				}
				else if (metadataKind == MetadataKind.ManagedWindowsMetadata && WindowsRuntimeProjections.NeedsWindowsRuntimePrefix(type))
				{
					typeDefinitionTreatment = TypeDefinitionTreatment.PrefixWindowsRuntimeName;
				}
				if ((typeDefinitionTreatment == TypeDefinitionTreatment.PrefixWindowsRuntimeName || typeDefinitionTreatment == TypeDefinitionTreatment.NormalType) && !type.IsInterface && WindowsRuntimeProjections.HasAttribute(type, "Windows.UI.Xaml", "TreatAsAbstractComposableClassAttribute"))
				{
					typeDefinitionTreatment |= TypeDefinitionTreatment.Abstract;
				}
			}
			else if (metadataKind == MetadataKind.ManagedWindowsMetadata && WindowsRuntimeProjections.IsClrImplementationType(type))
			{
				typeDefinitionTreatment = TypeDefinitionTreatment.UnmangleWindowsRuntimeName;
			}
			if (typeDefinitionTreatment != TypeDefinitionTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(type, new TypeDefinitionProjection(type, typeDefinitionTreatment, collection, collection2));
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0003168C File Offset: 0x0002F88C
		private static TypeDefinitionTreatment GetWellKnownTypeDefinitionTreatment(TypeDefinition type)
		{
			WindowsRuntimeProjections.ProjectionInfo projectionInfo;
			if (!WindowsRuntimeProjections.Projections.TryGetValue(type.Name, out projectionInfo))
			{
				return TypeDefinitionTreatment.None;
			}
			TypeDefinitionTreatment typeDefinitionTreatment = (projectionInfo.Attribute ? TypeDefinitionTreatment.RedirectToClrAttribute : TypeDefinitionTreatment.RedirectToClrType);
			if (type.Namespace == projectionInfo.ClrNamespace)
			{
				return typeDefinitionTreatment;
			}
			if (type.Namespace == projectionInfo.WinRTNamespace)
			{
				return typeDefinitionTreatment | TypeDefinitionTreatment.Internal;
			}
			return TypeDefinitionTreatment.None;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x000316EC File Offset: 0x0002F8EC
		private static TypeDefinitionTreatment GenerateRedirectionInformation(TypeDefinition type, out Collection<MethodDefinition> redirectedMethods, out Collection<KeyValuePair<InterfaceImplementation, InterfaceImplementation>> redirectedInterfaces)
		{
			bool flag = false;
			redirectedMethods = null;
			redirectedInterfaces = null;
			using (Collection<InterfaceImplementation>.Enumerator enumerator = type.Interfaces.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (WindowsRuntimeProjections.IsRedirectedType(enumerator.Current.InterfaceType))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return TypeDefinitionTreatment.NormalType;
			}
			HashSet<TypeReference> hashSet = new HashSet<TypeReference>(new TypeReferenceEqualityComparer());
			redirectedMethods = new Collection<MethodDefinition>();
			redirectedInterfaces = new Collection<KeyValuePair<InterfaceImplementation, InterfaceImplementation>>();
			foreach (InterfaceImplementation interfaceImplementation in type.Interfaces)
			{
				TypeReference interfaceType = interfaceImplementation.InterfaceType;
				if (WindowsRuntimeProjections.IsRedirectedType(interfaceType))
				{
					hashSet.Add(interfaceType);
					WindowsRuntimeProjections.CollectImplementedInterfaces(interfaceType, hashSet);
				}
			}
			foreach (InterfaceImplementation interfaceImplementation2 in type.Interfaces)
			{
				TypeReference interfaceType2 = interfaceImplementation2.InterfaceType;
				if (WindowsRuntimeProjections.IsRedirectedType(interfaceImplementation2.InterfaceType))
				{
					TypeReference elementType = interfaceType2.GetElementType();
					TypeReference typeReference = new TypeReference(elementType.Namespace, elementType.Name, elementType.Module, elementType.Scope)
					{
						DeclaringType = elementType.DeclaringType,
						projection = elementType.projection
					};
					WindowsRuntimeProjections.RemoveProjection(typeReference);
					GenericInstanceType genericInstanceType = interfaceType2 as GenericInstanceType;
					if (genericInstanceType != null)
					{
						GenericInstanceType genericInstanceType2 = new GenericInstanceType(typeReference);
						foreach (TypeReference typeReference2 in genericInstanceType.GenericArguments)
						{
							genericInstanceType2.GenericArguments.Add(typeReference2);
						}
						typeReference = genericInstanceType2;
					}
					InterfaceImplementation interfaceImplementation3 = new InterfaceImplementation(typeReference);
					redirectedInterfaces.Add(new KeyValuePair<InterfaceImplementation, InterfaceImplementation>(interfaceImplementation2, interfaceImplementation3));
				}
			}
			if (!type.IsInterface)
			{
				foreach (TypeReference typeReference3 in hashSet)
				{
					WindowsRuntimeProjections.RedirectInterfaceMethods(typeReference3, redirectedMethods);
				}
			}
			return TypeDefinitionTreatment.RedirectImplementedMethods;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00031934 File Offset: 0x0002FB34
		private static void CollectImplementedInterfaces(TypeReference type, HashSet<TypeReference> results)
		{
			TypeResolver typeResolver = TypeResolver.For(type);
			foreach (InterfaceImplementation interfaceImplementation in type.Resolve().Interfaces)
			{
				TypeReference typeReference = typeResolver.Resolve(interfaceImplementation.InterfaceType);
				results.Add(typeReference);
				WindowsRuntimeProjections.CollectImplementedInterfaces(typeReference, results);
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x000319A8 File Offset: 0x0002FBA8
		private static void RedirectInterfaceMethods(TypeReference interfaceType, Collection<MethodDefinition> redirectedMethods)
		{
			TypeResolver typeResolver = TypeResolver.For(interfaceType);
			foreach (MethodDefinition methodDefinition in interfaceType.Resolve().Methods)
			{
				MethodDefinition methodDefinition2 = new MethodDefinition(methodDefinition.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask, typeResolver.Resolve(methodDefinition.ReturnType));
				methodDefinition2.ImplAttributes = MethodImplAttributes.CodeTypeMask;
				foreach (ParameterDefinition parameterDefinition in methodDefinition.Parameters)
				{
					methodDefinition2.Parameters.Add(new ParameterDefinition(parameterDefinition.Name, parameterDefinition.Attributes, typeResolver.Resolve(parameterDefinition.ParameterType)));
				}
				methodDefinition2.Overrides.Add(typeResolver.Resolve(methodDefinition));
				redirectedMethods.Add(methodDefinition2);
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00031AAC File Offset: 0x0002FCAC
		private static bool IsRedirectedType(TypeReference type)
		{
			TypeReferenceProjection typeReferenceProjection = type.GetElementType().projection as TypeReferenceProjection;
			return typeReferenceProjection != null && typeReferenceProjection.Treatment == TypeReferenceTreatment.UseProjectionInfo;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00031AD8 File Offset: 0x0002FCD8
		private static bool NeedsWindowsRuntimePrefix(TypeDefinition type)
		{
			if ((type.Attributes & (TypeAttributes.VisibilityMask | TypeAttributes.ClassSemanticMask)) != TypeAttributes.Public)
			{
				return false;
			}
			TypeReference baseType = type.BaseType;
			if (baseType == null || baseType.MetadataToken.TokenType != TokenType.TypeRef)
			{
				return false;
			}
			if (baseType.Namespace == "System")
			{
				string name = baseType.Name;
				if (name == "Attribute" || name == "MulticastDelegate" || name == "ValueType")
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00031B57 File Offset: 0x0002FD57
		public static bool IsClrImplementationType(TypeDefinition type)
		{
			return (type.Attributes & (TypeAttributes.VisibilityMask | TypeAttributes.SpecialName)) == TypeAttributes.SpecialName && type.Name.StartsWith("<CLR>");
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00031B80 File Offset: 0x0002FD80
		public static void ApplyProjection(TypeDefinition type, TypeDefinitionProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			TypeDefinitionTreatment treatment = projection.Treatment;
			switch (treatment & TypeDefinitionTreatment.KindMask)
			{
			case TypeDefinitionTreatment.NormalType:
				type.Attributes |= TypeAttributes.Import | TypeAttributes.WindowsRuntime;
				break;
			case TypeDefinitionTreatment.NormalAttribute:
				type.Attributes |= TypeAttributes.Sealed | TypeAttributes.WindowsRuntime;
				break;
			case TypeDefinitionTreatment.UnmangleWindowsRuntimeName:
				type.Attributes = (type.Attributes & ~TypeAttributes.SpecialName) | TypeAttributes.Public;
				type.Name = type.Name.Substring("<CLR>".Length);
				break;
			case TypeDefinitionTreatment.PrefixWindowsRuntimeName:
				type.Attributes = (type.Attributes & ~TypeAttributes.Public) | TypeAttributes.Import;
				type.Name = "<WinRT>" + type.Name;
				break;
			case TypeDefinitionTreatment.RedirectToClrType:
				type.Attributes = (type.Attributes & ~TypeAttributes.Public) | TypeAttributes.Import;
				break;
			case TypeDefinitionTreatment.RedirectToClrAttribute:
				type.Attributes &= ~TypeAttributes.Public;
				break;
			case TypeDefinitionTreatment.RedirectImplementedMethods:
				type.Attributes |= TypeAttributes.Import | TypeAttributes.WindowsRuntime;
				foreach (KeyValuePair<InterfaceImplementation, InterfaceImplementation> keyValuePair in projection.RedirectedInterfaces)
				{
					type.Interfaces.Add(keyValuePair.Value);
					foreach (CustomAttribute customAttribute in keyValuePair.Key.CustomAttributes)
					{
						keyValuePair.Value.CustomAttributes.Add(customAttribute);
					}
					keyValuePair.Key.CustomAttributes.Clear();
					foreach (MethodDefinition methodDefinition in type.Methods)
					{
						foreach (MethodReference methodReference in methodDefinition.Overrides)
						{
							if (TypeReferenceEqualityComparer.AreEqual(methodReference.DeclaringType, keyValuePair.Key.InterfaceType, TypeComparisonMode.Exact))
							{
								methodReference.DeclaringType = keyValuePair.Value.InterfaceType;
							}
						}
					}
				}
				foreach (MethodDefinition methodDefinition2 in projection.RedirectedMethods)
				{
					type.Methods.Add(methodDefinition2);
				}
				break;
			}
			if ((treatment & TypeDefinitionTreatment.Abstract) != TypeDefinitionTreatment.None)
			{
				type.Attributes |= TypeAttributes.Abstract;
			}
			if ((treatment & TypeDefinitionTreatment.Internal) != TypeDefinitionTreatment.None)
			{
				type.Attributes &= ~TypeAttributes.Public;
			}
			type.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00031EB8 File Offset: 0x000300B8
		public static TypeDefinitionProjection RemoveProjection(TypeDefinition type)
		{
			if (!type.IsWindowsRuntimeProjection)
			{
				return null;
			}
			TypeDefinitionProjection windowsRuntimeProjection = type.WindowsRuntimeProjection;
			type.WindowsRuntimeProjection = null;
			type.Attributes = windowsRuntimeProjection.Attributes;
			type.Name = windowsRuntimeProjection.Name;
			if (windowsRuntimeProjection.Treatment == TypeDefinitionTreatment.RedirectImplementedMethods)
			{
				foreach (MethodDefinition methodDefinition in windowsRuntimeProjection.RedirectedMethods)
				{
					type.Methods.Remove(methodDefinition);
				}
				foreach (KeyValuePair<InterfaceImplementation, InterfaceImplementation> keyValuePair in windowsRuntimeProjection.RedirectedInterfaces)
				{
					foreach (MethodDefinition methodDefinition2 in type.Methods)
					{
						foreach (MethodReference methodReference in methodDefinition2.Overrides)
						{
							if (TypeReferenceEqualityComparer.AreEqual(methodReference.DeclaringType, keyValuePair.Value.InterfaceType, TypeComparisonMode.Exact))
							{
								methodReference.DeclaringType = keyValuePair.Key.InterfaceType;
							}
						}
					}
					foreach (CustomAttribute customAttribute in keyValuePair.Value.CustomAttributes)
					{
						keyValuePair.Key.CustomAttributes.Add(customAttribute);
					}
					keyValuePair.Value.CustomAttributes.Clear();
					type.Interfaces.Remove(keyValuePair.Value);
				}
			}
			return windowsRuntimeProjection;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x000320EC File Offset: 0x000302EC
		public static void Project(TypeReference type)
		{
			WindowsRuntimeProjections.ProjectionInfo projectionInfo;
			TypeReferenceTreatment typeReferenceTreatment;
			if (WindowsRuntimeProjections.Projections.TryGetValue(type.Name, out projectionInfo) && projectionInfo.WinRTNamespace == type.Namespace)
			{
				typeReferenceTreatment = TypeReferenceTreatment.UseProjectionInfo;
			}
			else
			{
				typeReferenceTreatment = WindowsRuntimeProjections.GetSpecialTypeReferenceTreatment(type);
			}
			if (typeReferenceTreatment != TypeReferenceTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(type, new TypeReferenceProjection(type, typeReferenceTreatment));
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0003213B File Offset: 0x0003033B
		private static TypeReferenceTreatment GetSpecialTypeReferenceTreatment(TypeReference type)
		{
			if (type.Namespace == "System")
			{
				if (type.Name == "MulticastDelegate")
				{
					return TypeReferenceTreatment.SystemDelegate;
				}
				if (type.Name == "Attribute")
				{
					return TypeReferenceTreatment.SystemAttribute;
				}
			}
			return TypeReferenceTreatment.None;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00032178 File Offset: 0x00030378
		private static bool IsAttribute(TypeReference type)
		{
			return type.MetadataToken.TokenType == TokenType.TypeRef && type.Name == "Attribute" && type.Namespace == "System";
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x000321C0 File Offset: 0x000303C0
		private static bool IsEnum(TypeReference type)
		{
			return type.MetadataToken.TokenType == TokenType.TypeRef && type.Name == "Enum" && type.Namespace == "System";
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00032208 File Offset: 0x00030408
		public static void ApplyProjection(TypeReference type, TypeReferenceProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			TypeReferenceTreatment treatment = projection.Treatment;
			if (treatment - TypeReferenceTreatment.SystemDelegate > 1)
			{
				if (treatment == TypeReferenceTreatment.UseProjectionInfo)
				{
					WindowsRuntimeProjections.ProjectionInfo projectionInfo = WindowsRuntimeProjections.Projections[type.Name];
					type.Name = projectionInfo.ClrName;
					type.Namespace = projectionInfo.ClrNamespace;
					type.Scope = type.Module.Projections.GetAssemblyReference(projectionInfo.ClrAssembly);
				}
			}
			else
			{
				type.Scope = type.Module.Projections.GetAssemblyReference("System.Runtime");
			}
			type.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00032298 File Offset: 0x00030498
		public static TypeReferenceProjection RemoveProjection(TypeReference type)
		{
			if (!type.IsWindowsRuntimeProjection)
			{
				return null;
			}
			TypeReferenceProjection windowsRuntimeProjection = type.WindowsRuntimeProjection;
			type.WindowsRuntimeProjection = null;
			type.Name = windowsRuntimeProjection.Name;
			type.Namespace = windowsRuntimeProjection.Namespace;
			type.Scope = windowsRuntimeProjection.Scope;
			return windowsRuntimeProjection;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x000322E4 File Offset: 0x000304E4
		public static void Project(MethodDefinition method)
		{
			MethodDefinitionTreatment methodDefinitionTreatment = MethodDefinitionTreatment.None;
			bool flag = false;
			TypeDefinition declaringType = method.DeclaringType;
			if (declaringType.IsWindowsRuntime)
			{
				if (WindowsRuntimeProjections.IsClrImplementationType(declaringType))
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.None;
				}
				else if (declaringType.IsNested)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.None;
				}
				else if (declaringType.IsInterface)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.Runtime | MethodDefinitionTreatment.InternalCall;
				}
				else if (declaringType.Module.MetadataKind == MetadataKind.ManagedWindowsMetadata && !method.IsPublic)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.None;
				}
				else
				{
					flag = true;
					TypeReference baseType = declaringType.BaseType;
					if (baseType != null && baseType.MetadataToken.TokenType == TokenType.TypeRef)
					{
						TypeReferenceTreatment specialTypeReferenceTreatment = WindowsRuntimeProjections.GetSpecialTypeReferenceTreatment(baseType);
						if (specialTypeReferenceTreatment != TypeReferenceTreatment.SystemDelegate)
						{
							if (specialTypeReferenceTreatment == TypeReferenceTreatment.SystemAttribute)
							{
								methodDefinitionTreatment = MethodDefinitionTreatment.Runtime | MethodDefinitionTreatment.InternalCall;
								flag = false;
							}
						}
						else
						{
							methodDefinitionTreatment = MethodDefinitionTreatment.Public | MethodDefinitionTreatment.Runtime;
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				foreach (MethodReference methodReference in method.Overrides)
				{
					if (methodReference.MetadataToken.TokenType == TokenType.MemberRef && WindowsRuntimeProjections.ImplementsRedirectedInterface(methodReference))
					{
						flag2 = true;
					}
					else
					{
						flag3 = true;
					}
				}
				if (flag2 && !flag3)
				{
					methodDefinitionTreatment = MethodDefinitionTreatment.Private | MethodDefinitionTreatment.Runtime | MethodDefinitionTreatment.InternalCall;
					flag = false;
				}
			}
			if (flag)
			{
				methodDefinitionTreatment |= WindowsRuntimeProjections.GetMethodDefinitionTreatmentFromCustomAttributes(method);
			}
			if (methodDefinitionTreatment != MethodDefinitionTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(method, new MethodDefinitionProjection(method, methodDefinitionTreatment));
			}
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00032424 File Offset: 0x00030624
		private static MethodDefinitionTreatment GetMethodDefinitionTreatmentFromCustomAttributes(MethodDefinition method)
		{
			MethodDefinitionTreatment methodDefinitionTreatment = MethodDefinitionTreatment.None;
			foreach (CustomAttribute customAttribute in method.CustomAttributes)
			{
				TypeReference attributeType = customAttribute.AttributeType;
				if (!(attributeType.Namespace != "Windows.UI.Xaml"))
				{
					if (attributeType.Name == "TreatAsPublicMethodAttribute")
					{
						methodDefinitionTreatment |= MethodDefinitionTreatment.Public;
					}
					else if (attributeType.Name == "TreatAsAbstractMethodAttribute")
					{
						methodDefinitionTreatment |= MethodDefinitionTreatment.Abstract;
					}
				}
			}
			return methodDefinitionTreatment;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000324B8 File Offset: 0x000306B8
		public static void ApplyProjection(MethodDefinition method, MethodDefinitionProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			MethodDefinitionTreatment treatment = projection.Treatment;
			if ((treatment & MethodDefinitionTreatment.Abstract) != MethodDefinitionTreatment.None)
			{
				method.Attributes |= MethodAttributes.Abstract;
			}
			if ((treatment & MethodDefinitionTreatment.Private) != MethodDefinitionTreatment.None)
			{
				method.Attributes = (method.Attributes & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Private;
			}
			if ((treatment & MethodDefinitionTreatment.Public) != MethodDefinitionTreatment.None)
			{
				method.Attributes = (method.Attributes & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Public;
			}
			if ((treatment & MethodDefinitionTreatment.Runtime) != MethodDefinitionTreatment.None)
			{
				method.ImplAttributes |= MethodImplAttributes.CodeTypeMask;
			}
			if ((treatment & MethodDefinitionTreatment.InternalCall) != MethodDefinitionTreatment.None)
			{
				method.ImplAttributes |= MethodImplAttributes.InternalCall;
			}
			method.WindowsRuntimeProjection = projection;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0003254C File Offset: 0x0003074C
		public static MethodDefinitionProjection RemoveProjection(MethodDefinition method)
		{
			if (!method.IsWindowsRuntimeProjection)
			{
				return null;
			}
			MethodDefinitionProjection windowsRuntimeProjection = method.WindowsRuntimeProjection;
			method.WindowsRuntimeProjection = null;
			method.Attributes = windowsRuntimeProjection.Attributes;
			method.ImplAttributes = windowsRuntimeProjection.ImplAttributes;
			method.Name = windowsRuntimeProjection.Name;
			return windowsRuntimeProjection;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00032598 File Offset: 0x00030798
		public static void Project(FieldDefinition field)
		{
			FieldDefinitionTreatment fieldDefinitionTreatment = FieldDefinitionTreatment.None;
			TypeDefinition declaringType = field.DeclaringType;
			if (declaringType.Module.MetadataKind == MetadataKind.WindowsMetadata && field.IsRuntimeSpecialName && field.Name == "value__")
			{
				TypeReference baseType = declaringType.BaseType;
				if (baseType != null && WindowsRuntimeProjections.IsEnum(baseType))
				{
					fieldDefinitionTreatment = FieldDefinitionTreatment.Public;
				}
			}
			if (fieldDefinitionTreatment != FieldDefinitionTreatment.None)
			{
				WindowsRuntimeProjections.ApplyProjection(field, new FieldDefinitionProjection(field, fieldDefinitionTreatment));
			}
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x000325FA File Offset: 0x000307FA
		public static void ApplyProjection(FieldDefinition field, FieldDefinitionProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			if (projection.Treatment == FieldDefinitionTreatment.Public)
			{
				field.Attributes = (field.Attributes & ~FieldAttributes.FieldAccessMask) | FieldAttributes.Public;
			}
			field.WindowsRuntimeProjection = projection;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00032624 File Offset: 0x00030824
		public static FieldDefinitionProjection RemoveProjection(FieldDefinition field)
		{
			if (!field.IsWindowsRuntimeProjection)
			{
				return null;
			}
			FieldDefinitionProjection windowsRuntimeProjection = field.WindowsRuntimeProjection;
			field.WindowsRuntimeProjection = null;
			field.Attributes = windowsRuntimeProjection.Attributes;
			return windowsRuntimeProjection;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00032658 File Offset: 0x00030858
		private static bool ImplementsRedirectedInterface(MemberReference member)
		{
			TypeReference declaringType = member.DeclaringType;
			TokenType tokenType = declaringType.MetadataToken.TokenType;
			TypeReference typeReference;
			if (tokenType != TokenType.TypeRef)
			{
				if (tokenType != TokenType.TypeSpec)
				{
					return false;
				}
				if (!declaringType.IsGenericInstance)
				{
					return false;
				}
				typeReference = ((TypeSpecification)declaringType).ElementType;
				if (typeReference.MetadataType != MetadataType.Class || typeReference.MetadataToken.TokenType != TokenType.TypeRef)
				{
					return false;
				}
			}
			else
			{
				typeReference = declaringType;
			}
			TypeReferenceProjection typeReferenceProjection = WindowsRuntimeProjections.RemoveProjection(typeReference);
			bool flag = false;
			WindowsRuntimeProjections.ProjectionInfo projectionInfo;
			if (WindowsRuntimeProjections.Projections.TryGetValue(typeReference.Name, out projectionInfo) && typeReference.Namespace == projectionInfo.WinRTNamespace)
			{
				flag = true;
			}
			WindowsRuntimeProjections.ApplyProjection(typeReference, typeReferenceProjection);
			return flag;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003270C File Offset: 0x0003090C
		public void AddVirtualReferences(Collection<AssemblyNameReference> references)
		{
			AssemblyNameReference coreLibrary = WindowsRuntimeProjections.GetCoreLibrary(references);
			this.corlib_version = coreLibrary.Version;
			coreLibrary.Version = WindowsRuntimeProjections.version;
			if (this.virtual_references == null)
			{
				AssemblyNameReference[] assemblyReferences = WindowsRuntimeProjections.GetAssemblyReferences(coreLibrary);
				Interlocked.CompareExchange<AssemblyNameReference[]>(ref this.virtual_references, assemblyReferences, null);
			}
			foreach (AssemblyNameReference assemblyNameReference in this.virtual_references)
			{
				references.Add(assemblyNameReference);
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00032778 File Offset: 0x00030978
		public void RemoveVirtualReferences(Collection<AssemblyNameReference> references)
		{
			WindowsRuntimeProjections.GetCoreLibrary(references).Version = this.corlib_version;
			foreach (AssemblyNameReference assemblyNameReference in this.VirtualReferences)
			{
				references.Remove(assemblyNameReference);
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x000327B8 File Offset: 0x000309B8
		private static AssemblyNameReference[] GetAssemblyReferences(AssemblyNameReference corlib)
		{
			AssemblyNameReference assemblyNameReference = new AssemblyNameReference("System.Runtime", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference2 = new AssemblyNameReference("System.Runtime.InteropServices.WindowsRuntime", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference3 = new AssemblyNameReference("System.ObjectModel", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference4 = new AssemblyNameReference("System.Runtime.WindowsRuntime", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference5 = new AssemblyNameReference("System.Runtime.WindowsRuntime.UI.Xaml", WindowsRuntimeProjections.version);
			AssemblyNameReference assemblyNameReference6 = new AssemblyNameReference("System.Numerics.Vectors", WindowsRuntimeProjections.version);
			if (corlib.HasPublicKey)
			{
				assemblyNameReference4.PublicKey = (assemblyNameReference5.PublicKey = corlib.PublicKey);
				assemblyNameReference.PublicKey = (assemblyNameReference2.PublicKey = (assemblyNameReference3.PublicKey = (assemblyNameReference6.PublicKey = WindowsRuntimeProjections.contract_pk)));
			}
			else
			{
				assemblyNameReference4.PublicKeyToken = (assemblyNameReference5.PublicKeyToken = corlib.PublicKeyToken);
				assemblyNameReference.PublicKeyToken = (assemblyNameReference2.PublicKeyToken = (assemblyNameReference3.PublicKeyToken = (assemblyNameReference6.PublicKeyToken = WindowsRuntimeProjections.contract_pk_token)));
			}
			return new AssemblyNameReference[] { assemblyNameReference, assemblyNameReference2, assemblyNameReference3, assemblyNameReference4, assemblyNameReference5, assemblyNameReference6 };
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000328DC File Offset: 0x00030ADC
		private static AssemblyNameReference GetCoreLibrary(Collection<AssemblyNameReference> references)
		{
			foreach (AssemblyNameReference assemblyNameReference in references)
			{
				if (assemblyNameReference.Name == "mscorlib")
				{
					return assemblyNameReference;
				}
			}
			throw new BadImageFormatException("Missing mscorlib reference in AssemblyRef table.");
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00032948 File Offset: 0x00030B48
		private AssemblyNameReference GetAssemblyReference(string name)
		{
			foreach (AssemblyNameReference assemblyNameReference in this.VirtualReferences)
			{
				if (assemblyNameReference.Name == name)
				{
					return assemblyNameReference;
				}
			}
			throw new Exception();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00032984 File Offset: 0x00030B84
		public static void Project(ICustomAttributeProvider owner, CustomAttribute attribute)
		{
			if (!WindowsRuntimeProjections.IsWindowsAttributeUsageAttribute(owner, attribute))
			{
				return;
			}
			CustomAttributeValueTreatment customAttributeValueTreatment = CustomAttributeValueTreatment.None;
			TypeDefinition typeDefinition = (TypeDefinition)owner;
			if (typeDefinition.Namespace == "Windows.Foundation.Metadata")
			{
				if (typeDefinition.Name == "VersionAttribute")
				{
					customAttributeValueTreatment = CustomAttributeValueTreatment.VersionAttribute;
				}
				else if (typeDefinition.Name == "DeprecatedAttribute")
				{
					customAttributeValueTreatment = CustomAttributeValueTreatment.DeprecatedAttribute;
				}
			}
			if (customAttributeValueTreatment == CustomAttributeValueTreatment.None)
			{
				customAttributeValueTreatment = (WindowsRuntimeProjections.HasAttribute(typeDefinition, "Windows.Foundation.Metadata", "AllowMultipleAttribute") ? CustomAttributeValueTreatment.AllowMultiple : CustomAttributeValueTreatment.AllowSingle);
			}
			if (customAttributeValueTreatment != CustomAttributeValueTreatment.None)
			{
				AttributeTargets attributeTargets = (AttributeTargets)attribute.ConstructorArguments[0].Value;
				WindowsRuntimeProjections.ApplyProjection(attribute, new CustomAttributeValueProjection(attributeTargets, customAttributeValueTreatment));
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00032A24 File Offset: 0x00030C24
		private static bool IsWindowsAttributeUsageAttribute(ICustomAttributeProvider owner, CustomAttribute attribute)
		{
			if (owner.MetadataToken.TokenType != TokenType.TypeDef)
			{
				return false;
			}
			MethodReference constructor = attribute.Constructor;
			if (constructor.MetadataToken.TokenType != TokenType.MemberRef)
			{
				return false;
			}
			TypeReference declaringType = constructor.DeclaringType;
			return declaringType.MetadataToken.TokenType == TokenType.TypeRef && declaringType.Name == "AttributeUsageAttribute" && declaringType.Namespace == "System";
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00032AA8 File Offset: 0x00030CA8
		private static bool HasAttribute(TypeDefinition type, string @namespace, string name)
		{
			foreach (CustomAttribute customAttribute in type.CustomAttributes)
			{
				TypeReference attributeType = customAttribute.AttributeType;
				if (attributeType.Name == name && attributeType.Namespace == @namespace)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00032B1C File Offset: 0x00030D1C
		public static void ApplyProjection(CustomAttribute attribute, CustomAttributeValueProjection projection)
		{
			if (projection == null)
			{
				return;
			}
			bool flag;
			bool flag2;
			switch (projection.Treatment)
			{
			case CustomAttributeValueTreatment.AllowSingle:
				flag = false;
				flag2 = false;
				break;
			case CustomAttributeValueTreatment.AllowMultiple:
				flag = false;
				flag2 = true;
				break;
			case CustomAttributeValueTreatment.VersionAttribute:
			case CustomAttributeValueTreatment.DeprecatedAttribute:
				flag = true;
				flag2 = true;
				break;
			default:
				throw new ArgumentException();
			}
			AttributeTargets attributeTargets = (AttributeTargets)attribute.ConstructorArguments[0].Value;
			if (flag)
			{
				attributeTargets |= AttributeTargets.Constructor | AttributeTargets.Property;
			}
			attribute.ConstructorArguments[0] = new CustomAttributeArgument(attribute.ConstructorArguments[0].Type, attributeTargets);
			attribute.Properties.Add(new CustomAttributeNamedArgument("AllowMultiple", new CustomAttributeArgument(attribute.Module.TypeSystem.Boolean, flag2)));
			attribute.projection = projection;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00032BF0 File Offset: 0x00030DF0
		public static CustomAttributeValueProjection RemoveProjection(CustomAttribute attribute)
		{
			if (attribute.projection == null)
			{
				return null;
			}
			CustomAttributeValueProjection projection = attribute.projection;
			attribute.projection = null;
			attribute.ConstructorArguments[0] = new CustomAttributeArgument(attribute.ConstructorArguments[0].Type, projection.Targets);
			attribute.Properties.Clear();
			return projection;
		}

		// Token: 0x040005B4 RID: 1460
		private static readonly Version version = new Version(4, 0, 0, 0);

		// Token: 0x040005B5 RID: 1461
		private static readonly byte[] contract_pk_token = new byte[] { 176, 63, 95, 127, 17, 213, 10, 58 };

		// Token: 0x040005B6 RID: 1462
		private static readonly byte[] contract_pk = new byte[]
		{
			0, 36, 0, 0, 4, 128, 0, 0, 148, 0,
			0, 0, 6, 2, 0, 0, 0, 36, 0, 0,
			82, 83, 65, 49, 0, 4, 0, 0, 1, 0,
			1, 0, 7, 209, 250, 87, 196, 174, 217, 240,
			163, 46, 132, 170, 15, 174, 253, 13, 233, 232,
			253, 106, 236, 143, 135, 251, 3, 118, 108, 131,
			76, 153, 146, 30, 178, 59, 231, 154, 217, 213,
			220, 193, 221, 154, 210, 54, 19, 33, 2, 144,
			11, 114, 60, 249, 128, 149, 127, 196, 225, 119,
			16, 143, 198, 7, 119, 79, 41, 232, 50, 14,
			146, 234, 5, 236, 228, 232, 33, 192, 165, 239,
			232, 241, 100, 92, 76, 12, 147, 193, 171, 153,
			40, 93, 98, 44, 170, 101, 44, 29, 250, 214,
			61, 116, 93, 111, 45, 229, 241, 126, 94, 175,
			15, 196, 150, 61, 38, 28, 138, 18, 67, 101,
			24, 32, 109, 192, 147, 52, 77, 90, 210, 147
		};

		// Token: 0x040005B7 RID: 1463
		private static Dictionary<string, WindowsRuntimeProjections.ProjectionInfo> projections;

		// Token: 0x040005B8 RID: 1464
		private readonly ModuleDefinition module;

		// Token: 0x040005B9 RID: 1465
		private Version corlib_version = new Version(255, 255, 255, 255);

		// Token: 0x040005BA RID: 1466
		private AssemblyNameReference[] virtual_references;

		// Token: 0x02000282 RID: 642
		private struct ProjectionInfo
		{
			// Token: 0x06001019 RID: 4121 RVA: 0x00032C91 File Offset: 0x00030E91
			public ProjectionInfo(string winrt_namespace, string clr_namespace, string clr_name, string clr_assembly, bool attribute = false)
			{
				this.WinRTNamespace = winrt_namespace;
				this.ClrNamespace = clr_namespace;
				this.ClrName = clr_name;
				this.ClrAssembly = clr_assembly;
				this.Attribute = attribute;
			}

			// Token: 0x040005BB RID: 1467
			public readonly string WinRTNamespace;

			// Token: 0x040005BC RID: 1468
			public readonly string ClrNamespace;

			// Token: 0x040005BD RID: 1469
			public readonly string ClrName;

			// Token: 0x040005BE RID: 1470
			public readonly string ClrAssembly;

			// Token: 0x040005BF RID: 1471
			public readonly bool Attribute;
		}
	}
}
