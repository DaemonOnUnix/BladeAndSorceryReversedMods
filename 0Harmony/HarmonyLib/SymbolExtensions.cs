using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000192 RID: 402
	public static class SymbolExtensions
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x000169C4 File Offset: 0x00014BC4
		public static MethodInfo GetMethodInfo(Expression<Action> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000169C4 File Offset: 0x00014BC4
		public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000169C4 File Offset: 0x00014BC4
		public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000169CC File Offset: 0x00014BCC
		public static MethodInfo GetMethodInfo(LambdaExpression expression)
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
			if (methodCallExpression == null)
			{
				throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
			}
			MethodInfo method = methodCallExpression.Method;
			if (method == null)
			{
				throw new Exception(string.Format("Cannot find method for expression {0}", expression));
			}
			return method;
		}
	}
}
