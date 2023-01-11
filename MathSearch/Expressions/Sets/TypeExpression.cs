
using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

[Independent(ExpressionType.Set)]
public sealed class TypeExpression: AtomExpression<ExpressionType> {
    public TypeExpression(ExpressionType value) : base(value) {
    }

    public override MathExpression Clone() =>
        new TypeExpression(Value);

}
