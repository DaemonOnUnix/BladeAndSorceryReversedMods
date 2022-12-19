using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x020000A2 RID: 162
	public static class SymbolExtensions
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		public static MethodInfo GetMethodInfo(Expression<Action> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00010EDC File Offset: 0x0000F0DC
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
