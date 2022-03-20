using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Tests.TestHelper
{
    [ExcludeFromCodeCoverage]
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression)).Result;
        }
    }

    [ExcludeFromCodeCoverage]
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }


        IQueryProvider IQueryable.Provider
        {
            get { return new TestAsyncQueryProvider<T>(this); }
        }
        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }

    [ExcludeFromCodeCoverage]
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T Current
        {
            get { return _inner.Current; }
        }
        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync()
        {
            return ValueTask.FromResult(_inner.MoveNext());
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }
    }

    public static class MoqExtensions
    {
        public static Mock<IQueryable<TEntity>> BuildMock<TEntity>(this IQueryable<TEntity> data) where TEntity : class
        {
            var mock = new Mock<IQueryable<TEntity>>();
            var enumerable = new TestAsyncEnumerable<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.ConfigureQueryableCalls(enumerable, data);
            return mock;
        }

        public static Mock<DbSet<TEntity>> BuildMockDbSet<TEntity>(this IQueryable<TEntity> data) where TEntity : class
        {
            var mock = new Mock<DbSet<TEntity>>();
            var enumerable = new TestAsyncEnumerable<TEntity>(data);
            mock.As<IAsyncEnumerable<TEntity>>().ConfigureAsyncEnumerableCalls(enumerable);
            mock.As<IQueryable<TEntity>>().ConfigureQueryableCalls(enumerable, data);
            mock.ConfigureDbSetCalls();
            return mock;
        }

        private static void ConfigureDbSetCalls<TEntity>(this Mock<DbSet<TEntity>> mock)
            where TEntity : class
        {
            mock.Setup(m => m.AsQueryable()).Returns(mock.Object);
            mock.Setup(m => m.AsAsyncEnumerable()).Returns(mock.Object);
        }

        private static void ConfigureQueryableCalls<TEntity>(
            this Mock<IQueryable<TEntity>> mock,
            IQueryProvider queryProvider,
            IQueryable<TEntity> data) where TEntity : class
        {
            mock.Setup(m => m.Provider).Returns(queryProvider);
            mock.Setup(m => m.Expression).Returns(data?.Expression);
            mock.Setup(m => m.ElementType).Returns(data?.ElementType);
            mock.Setup(m => m.GetEnumerator()).Returns(() => data?.GetEnumerator());
        }

        private static void ConfigureAsyncEnumerableCalls<TEntity>(
            this Mock<IAsyncEnumerable<TEntity>> mock,
            IAsyncEnumerable<TEntity> enumerable)
        {
            mock.Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(() => enumerable.GetAsyncEnumerator());
        }
    }
}
