
using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

[Simple]
public sealed class TypeExpression: AtomExpression<MathType> {
    public override MathType Type => MathType.Set;

    public TypeExpression(MathType value) : base(value) {
    }

    //TODO: simplify Boolean type to setExpression "{false, true}"

    public override MathExpression Clone() =>
        new TypeExpression(Value);

}
