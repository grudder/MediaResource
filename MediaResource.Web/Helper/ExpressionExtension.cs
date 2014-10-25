using System;
using System.Linq.Expressions;

namespace MediaResource.Web.Helper
{
	/// <summary>
	/// Expression 类的扩展方法。
	/// </summary>
	public static class ExpressionExtension
	{
		/// <summary>
		/// 两个布尔表达式相与。
		/// </summary>
		/// <typeparam name="T">类型。</typeparam>
		/// <param name="expr1">布尔表达式1。</param>
		/// <param name="expr2">布尔表达式2。</param>
		/// <returns>相与之后的布尔表达式。</returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
		}

		/// <summary>
		/// 两个布尔表达式相或。
		/// </summary>
		/// <typeparam name="T">类型。</typeparam>
		/// <param name="expr1">布尔表达式1。</param>
		/// <param name="expr2">布尔表达式2。</param>
		/// <returns>相或之后的布尔表达式。</returns>
		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
			return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
		}
	}
}