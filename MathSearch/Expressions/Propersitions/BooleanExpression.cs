
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

[Independent(MathType.Boolean)]
public sealed class BooleanExpression: AtomExpression<bool> {
    public BooleanExpression(bool value) : base(value) {
    }

    public override MathExpression Clone() =>
        new BooleanExpression(Value);
}
