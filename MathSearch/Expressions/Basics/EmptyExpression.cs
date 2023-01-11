
using MathSearch.Expression;

namespace MathSearch.Expressions.Basics;

public sealed class EmptyExpression: MathExpression {
    public override int ChildCount => 0;

    public override MathExpression Clone() => new EmptyExpression();

    public override MathType DetermineType(Context? context = null) => MathType.Nothing;

    public override IEnumerable<MathExpression> GetChildren() => Array.Empty<MathExpression>();

    public override MathExpression Simplify(Context? context = null) => Clone();

    protected override IEnumerable<MathExpression> AddToContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();
}
