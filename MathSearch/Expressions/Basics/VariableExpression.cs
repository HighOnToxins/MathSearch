
using MathSearch.Expression;

namespace MathSearch.Expressions;

[Precedence(0)]
[IsNotSimple]
public sealed class VariableExpression: AtomExpression<string> {
    public VariableExpression(string value) : base(value) {
    }

    public override MathExpression Clone() => new VariableExpression(Value);

    internal override MathType DetermineTypeBasedOn(MathSystem context) => MathType.Universe;
}
