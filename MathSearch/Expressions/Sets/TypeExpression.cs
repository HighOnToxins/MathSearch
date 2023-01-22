
using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

public sealed class TypeExpression: AtomExpression<MathType> {

    public TypeExpression(MathType value) : base(value) {
    }

    public override MathExpression Clone() => new TypeExpression(Value);

    public override MathType GetMathType(MathSystem? context = null) => MathType.Set;
}
