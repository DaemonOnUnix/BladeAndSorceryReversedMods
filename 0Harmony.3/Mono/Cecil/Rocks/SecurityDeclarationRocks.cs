using System;
using System.Security;
using System.Security.Permissions;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200032B RID: 811
	internal static class SecurityDeclarationRocks
	{
		// Token: 0x060012DA RID: 4826 RVA: 0x0003FE84 File Offset: 0x0003E084
		public static PermissionSet ToPermissionSet(this SecurityDeclaration self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			PermissionSet permissionSet;
			if (SecurityDeclarationRocks.TryProcessPermissionSetAttribute(self, out permissionSet))
			{
				return permissionSet;
			}
			return SecurityDeclarationRocks.CreatePermissionSet(self);
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0003FEB4 File Offset: 0x0003E0B4
		private static bool TryProcessPermissionSetAttribute(SecurityDeclaration declaration, out PermissionSet set)
		{
			set = null;
			if (!declaration.HasSecurityAttributes && declaration.SecurityAttributes.Count != 1)
			{
				return false;
			}
			SecurityAttribute securityAttribute = declaration.SecurityAttributes[0];
			if (!securityAttribute.AttributeType.IsTypeOf("System.Security.Permissions", "PermissionSetAttribute"))
			{
				return false;
			}
			PermissionSetAttribute permissionSetAttribute = new PermissionSetAttribute((SecurityAction)declaration.Action);
			CustomAttributeNamedArgument customAttributeNamedArgument = securityAttribute.Properties[0];
			string text = (string)customAttributeNamedArgument.Argument.Value;
			string name = customAttributeNamedArgument.Name;
			if (name != null)
			{
				if (!(name == "XML"))
				{
					if (!(name == "Name"))
					{
						goto IL_AD;
					}
					permissionSetAttribute.Name = text;
				}
				else
				{
					permissionSetAttribute.XML = text;
				}
				set = permissionSetAttribute.CreatePermissionSet();
				return true;
			}
			IL_AD:
			throw new NotImplementedException(customAttributeNamedArgument.Name);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0003FF84 File Offset: 0x0003E184
		private static PermissionSet CreatePermissionSet(SecurityDeclaration declaration)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			foreach (SecurityAttribute securityAttribute in declaration.SecurityAttributes)
			{
				IPermission permission = SecurityDeclarationRocks.CreatePermission(declaration, securityAttribute);
				permissionSet.AddPermission(permission);
			}
			return permissionSet;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0003FFE8 File Offset: 0x0003E1E8
		private static IPermission CreatePermission(SecurityDeclaration declaration, SecurityAttribute attribute)
		{
			Type type = Type.GetType(attribute.AttributeType.FullName);
			if (type == null)
			{
				throw new ArgumentException("attribute");
			}
			SecurityAttribute securityAttribute = SecurityDeclarationRocks.CreateSecurityAttribute(type, declaration);
			if (securityAttribute == null)
			{
				throw new InvalidOperationException();
			}
			SecurityDeclarationRocks.CompleteSecurityAttribute(securityAttribute, attribute);
			return securityAttribute.CreatePermission();
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00040034 File Offset: 0x0003E234
		private static void CompleteSecurityAttribute(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			if (attribute.HasFields)
			{
				SecurityDeclarationRocks.CompleteSecurityAttributeFields(security_attribute, attribute);
			}
			if (attribute.HasProperties)
			{
				SecurityDeclarationRocks.CompleteSecurityAttributeProperties(security_attribute, attribute);
			}
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00040054 File Offset: 0x0003E254
		private static void CompleteSecurityAttributeFields(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			Type type = security_attribute.GetType();
			foreach (CustomAttributeNamedArgument customAttributeNamedArgument in attribute.Fields)
			{
				type.GetField(customAttributeNamedArgument.Name).SetValue(security_attribute, customAttributeNamedArgument.Argument.Value);
			}
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x000400CC File Offset: 0x0003E2CC
		private static void CompleteSecurityAttributeProperties(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			Type type = security_attribute.GetType();
			foreach (CustomAttributeNamedArgument customAttributeNamedArgument in attribute.Properties)
			{
				type.GetProperty(customAttributeNamedArgument.Name).SetValue(security_attribute, customAttributeNamedArgument.Argument.Value, null);
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00040144 File Offset: 0x0003E344
		private static SecurityAttribute CreateSecurityAttribute(Type attribute_type, SecurityDeclaration declaration)
		{
			SecurityAttribute securityAttribute;
			try
			{
				securityAttribute = (SecurityAttribute)Activator.CreateInstance(attribute_type, new object[] { (SecurityAction)declaration.Action });
			}
			catch (MissingMethodException)
			{
				securityAttribute = (SecurityAttribute)Activator.CreateInstance(attribute_type, new object[0]);
			}
			return securityAttribute;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0004019C File Offset: 0x0003E39C
		public static SecurityDeclaration ToSecurityDeclaration(this PermissionSet self, SecurityAction action, ModuleDefinition module)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (module == null)
			{
				throw new ArgumentNullException("module");
			}
			SecurityDeclaration securityDeclaration = new SecurityDeclaration(action);
			SecurityAttribute securityAttribute = new SecurityAttribute(module.TypeSystem.LookupType("System.Security.Permissions", "PermissionSetAttribute"));
			securityAttribute.Properties.Add(new CustomAttributeNamedArgument("XML", new CustomAttributeArgument(module.TypeSystem.String, self.ToXml().ToString())));
			securityDeclaration.SecurityAttributes.Add(securityAttribute);
			return securityDeclaration;
		}
	}
}
