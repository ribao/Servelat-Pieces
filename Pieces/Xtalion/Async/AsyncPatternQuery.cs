﻿using System;
using System.Linq.Expressions;

namespace Xtalion.Async
{
	public sealed class AsyncPatternQuery<TResult> : AsyncCall<TResult>
	{
		readonly ApmInvocation invocation;

		public AsyncPatternQuery(Expression<Func<TResult>> expression)
		{
			var call = (MethodCallExpression) expression.Body;

			invocation = new ApmInvocation(call.GetTarget(), call)
			{
				Completed = (sender, args) =>
				{
					Result = (TResult) invocation.Result;
					Exception = invocation.Exception;

					SignalCompleted();
				}
			};
		}

		public override void Execute()
		{
			invocation.Invoke();
		}
	}
}
