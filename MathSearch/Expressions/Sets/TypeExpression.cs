
using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

[Independent(MathType.Set)]
public sealed class TypeExpression: AtomExpression<MathType> {
    public TypeExpression(MathType value) : base(value) {
    }

    public override MathExpression Clone() =>
        new TypeExpression(Value);

}
