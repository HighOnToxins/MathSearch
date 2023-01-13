
using MathSearch.Expression;

namespace MathSearch.Expressions.Basics;

[Precedence(-1)]
[IsNotSimple]
public sealed class EmptyExpression: MathExpression {
    public override int ChildCount => 0;

    public override MathExpression Clone() => new EmptyExpression();

    public override MathType EvaluateType(MathSystem? context = null) => MathType.Nothing;

    public override IEnumerable<MathExpression> GetChildren() => Array.Empty<MathExpression>();

    public override bool TryGroup<E>(out MathExpression? result) {
        result = null;
        return false;
    }

    public override MathExpression Simplify(MathSystem? context = null) => Clone();

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();
}
