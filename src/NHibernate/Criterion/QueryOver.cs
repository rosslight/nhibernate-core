
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NHibernate.Impl;
using NHibernate.SqlCommand;

namespace NHibernate.Criterion
{

	/// <summary>
	/// Implementation of the <see cref="IQueryOver&lt;T&gt;"/> interface
	/// </summary>
	[Serializable]
	public class QueryOver<T> : IQueryOver<T>
	{

		private ICriteria		_criteria;
		private CriteriaImpl	_impl;

		public QueryOver()
		{
			_impl = new CriteriaImpl(typeof(T), null);
			_criteria = _impl;
		}

		public QueryOver(CriteriaImpl impl)
		{
			_impl = impl;
			_criteria = impl;
		}

		public QueryOver(CriteriaImpl rootImpl, ICriteria criteria)
		{
			_impl = rootImpl;
			_criteria = criteria;
		}

		public ICriteria UnderlyingCriteria
		{
			get { return _criteria; }
		}

		public QueryOver<T> And(Expression<Func<T, bool>> expression)
		{
			return Add(expression);
		}

		public QueryOver<T> And(Expression<Func<bool>> expression)
		{
			return Add(expression);
		}

		public QueryOver<T> Where(Expression<Func<T, bool>> expression)
		{
			return Add(expression);
		}

		public QueryOver<T> Where(Expression<Func<bool>> expression)
		{
			return Add(expression);
		}

		public QueryOver<T> OrderBy(Expression<Func<T, object>> path, Func<string, Order> orderDelegate)
		{
			return AddOrder(path, orderDelegate);
		}

		public QueryOver<T> OrderBy(Expression<Func<object>> path, Func<string, Order> orderDelegate)
		{
			return AddOrder(path, orderDelegate);
		}

		public QueryOver<T> ThenBy(Expression<Func<T, object>> path, Func<string, Order> orderDelegate)
		{
			return AddOrder(path, orderDelegate);
		}

		public QueryOver<T> ThenBy(Expression<Func<object>> path, Func<string, Order> orderDelegate)
		{
			return AddOrder(path, orderDelegate);
		}

		public QueryOver<U> JoinQueryOver<U>(Expression<Func<T, U>> path)
		{
			return new QueryOver<U>(_impl,
				_criteria.CreateCriteria(
					ExpressionProcessor.FindMemberExpression(path.Body)));
		}

		public QueryOver<U> JoinQueryOver<U>(Expression<Func<T, IEnumerable<U>>> path)
		{
			return new QueryOver<U>(_impl,
				_criteria.CreateCriteria(
					ExpressionProcessor.FindMemberExpression(path.Body)));
		}

		public QueryOver<T> Join(Expression<Func<T, object>> path, Expression<Func<object>> alias)
		{
			return AddAlias(
				ExpressionProcessor.FindMemberExpression(path.Body),
				ExpressionProcessor.FindMemberExpression(alias.Body),
				JoinType.InnerJoin);
		}

		public IList<T> List()
		{
			return _criteria.List<T>();
		}

		/// <summary>
		/// Get an executable instance of <c>IQueryOver&lt;T&gt;</c>,
		/// to actually run the query.</summary>
		public IQueryOver<T> GetExecutableQueryOver(ISession session)
		{
			_impl.Session = session.GetSessionImplementation();
			return this;
		}

		private QueryOver<T> AddAlias(string path, string alias, JoinType joinType)
		{
			_criteria.CreateAlias(path, alias, joinType);
			return this;
		}

		private QueryOver<T> Add(Expression<Func<T, bool>> expression)
		{
			_criteria.Add(ExpressionProcessor.ProcessExpression<T>(expression));
			return this;
		}

		private QueryOver<T> Add(Expression<Func<bool>> expression)
		{
			_criteria.Add(ExpressionProcessor.ProcessExpression(expression));
			return this;
		}

		private QueryOver<T> AddOrder(Expression<Func<T, object>> path, Func<string, Order> orderDelegate)
		{
			_criteria.AddOrder(ExpressionProcessor.ProcessOrder<T>(path, orderDelegate));
			return this;
		}

		private QueryOver<T> AddOrder(Expression<Func<object>> path, Func<string, Order> orderDelegate)
		{
			_criteria.AddOrder(ExpressionProcessor.ProcessOrder(path, orderDelegate));
			return this;
		}


		IQueryOver<T> IQueryOver<T>.And(Expression<Func<T, bool>> expression)
		{ return And(expression); }

		IQueryOver<T> IQueryOver<T>.And(Expression<Func<bool>> expression)
		{ return And(expression); }

		IQueryOver<T> IQueryOver<T>.Where(Expression<Func<T, bool>> expression)
		{ return Where(expression); }

		IQueryOver<T> IQueryOver<T>.Where(Expression<Func<bool>> expression)
		{ return Where(expression); }

		IQueryOver<T> IQueryOver<T>.OrderBy(Expression<Func<T, object>> path, Func<string, Order> orderDelegate)
		{ return OrderBy(path, orderDelegate); }

		IQueryOver<T> IQueryOver<T>.OrderBy(Expression<Func<object>> path, Func<string, Order> orderDelegate)
		{ return OrderBy(path, orderDelegate); }

		IQueryOver<T> IQueryOver<T>.ThenBy(Expression<Func<T, object>> path, Func<string, Order> orderDelegate)
		{ return ThenBy(path, orderDelegate); }

		IQueryOver<T> IQueryOver<T>.ThenBy(Expression<Func<object>> path, Func<string, Order> orderDelegate)
		{ return ThenBy(path, orderDelegate); }

		IQueryOver<U> IQueryOver<T>.JoinQueryOver<U>(Expression<Func<T, U>> path)
		{ return JoinQueryOver(path); }

		IQueryOver<U> IQueryOver<T>.JoinQueryOver<U>(Expression<Func<T, IEnumerable<U>>> path)
		{ return JoinQueryOver(path); }

		IQueryOver<T> IQueryOver<T>.Join(Expression<Func<T, object>> path, Expression<Func<object>> alias)
		{ return Join(path, alias); }

	}

}