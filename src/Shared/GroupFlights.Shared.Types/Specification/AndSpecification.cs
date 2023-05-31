using System.Linq.Expressions;

namespace GroupFlights.Shared.Types.Specification;

public class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T>[] _specifications;

    public AndSpecification(params Specification<T>[] specifications)
    {
        _specifications = specifications ?? throw new ArgumentNullException(nameof(specifications));
    }
    
    protected override Expression<Func<T, bool>> AsPredicateExpression()
    {
        Expression<Func<T, bool>> resultingExpression = null;

        foreach (var specification in _specifications)
        {
            if (resultingExpression is null)
            {
                resultingExpression = specification;
                continue;
            }

            resultingExpression = Combine(resultingExpression, specification);
        }

        return resultingExpression;
    }

    private Expression<Func<T, bool>> Combine(Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
    {
        var parameter = Expression.Parameter(typeof (T));

        var leftVisitor = new ReplaceExpressionVisitor(leftExpression.Parameters[0], parameter);
        var left = leftVisitor.Visit(leftExpression.Body);

        var rightVisitor = new ReplaceExpressionVisitor(rightExpression.Parameters[0], parameter);
        var right = rightVisitor.Visit(rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.And(left, right), parameter);
    }

    private class ReplaceExpressionVisitor
        : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
}