
using MathSearch.Expression;

namespace MathSearch.Expressions.Basics;

public sealed class EmptyExpression: MathExpression, INonSimpleExpression {
    public override int ChildCount => 0;

    public override int Precedence => -1;

    public override MathExpression Clone() => new EmptyExpression();

    public override IEnumerable<MathExpression> GetChildren() => Array.Empty<MathExpression>();

    public override bool TryGroup<E>(out MathExpression? result) {
        result = null;
        return false;
    }

    public override MathExpression Simplify(MathSystem? context = null) => Clone();

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();

    protected override MathType ComputeType(MathSystem context) => MathType.Nothing;
}
