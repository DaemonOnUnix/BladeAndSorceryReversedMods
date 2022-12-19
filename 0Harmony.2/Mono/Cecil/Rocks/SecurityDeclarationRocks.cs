using System;
using System.Security;
using System.Security.Permissions;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000421 RID: 1057
	internal static class SecurityDeclarationRocks
	{
		// Token: 0x06001649 RID: 5705 RVA: 0x00047DCC File Offset: 0x00045FCC
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

		// Token: 0x0600164A RID: 5706 RVA: 0x00047DFC File Offset: 0x00045FFC
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
			if (!(name == "XML"))
			{
				if (!(name == "Name"))
				{
					throw new NotImplementedException(customAttributeNamedArgument.Name);
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

		// Token: 0x0600164B RID: 5707 RVA: 0x00047EC8 File Offset: 0x000460C8
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

		// Token: 0x0600164C RID: 5708 RVA: 0x00047F2C File Offset: 0x0004612C
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

		// Token: 0x0600164D RID: 5709 RVA: 0x00047F78 File Offset: 0x00046178
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

		// Token: 0x0600164E RID: 5710 RVA: 0x00047F98 File Offset: 0x00046198
		private static void CompleteSecurityAttributeFields(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			Type type = security_attribute.GetType();
			foreach (CustomAttributeNamedArgument customAttributeNamedArgument in attribute.Fields)
			{
				type.GetField(customAttributeNamedArgument.Name).SetValue(security_attribute, customAttributeNamedArgument.Argument.Value);
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00048010 File Offset: 0x00046210
		private static void CompleteSecurityAttributeProperties(SecurityAttribute security_attribute, SecurityAttribute attribute)
		{
			Type type = security_attribute.GetType();
			foreach (CustomAttributeNamedArgument customAttributeNamedArgument in attribute.Properties)
			{
				type.GetProperty(customAttributeNamedArgument.Name).SetValue(security_attribute, customAttributeNamedArgument.Argument.Value, null);
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00048088 File Offset: 0x00046288
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

		// Token: 0x06001651 RID: 5713 RVA: 0x000480E0 File Offset: 0x000462E0
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
