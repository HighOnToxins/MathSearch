
using MathSearch.Expression;

namespace MathSearch.Expressions;

[Precedence(0)]
[IsNotSimple]
public sealed class VariableExpression: AtomExpression<string> {

    public VariableExpression(string value) : base(value) {
    }

    public override MathExpression Clone() => new VariableExpression(Value);

    protected override MathType ComputeType(MathSystem context) => MathType.Universe;
}
