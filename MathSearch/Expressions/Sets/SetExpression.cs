using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

public sealed class SetExpression: OperatorExpression {

    public override HashSet<MathExpression> Children { get; }

    public SetExpression(params MathExpression[] children) {
        Children = new(children);
    }

    public SetExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> simplifiedChild, Context context) =>
        MathType.Set;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren) =>
        new SetExpression(Children);

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, Context context) {
        return false;
    }

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children;

    protected override bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {

        //simplify empty to TypeExpression of nothing

        result = null;
        return false;
    }

    protected override IEnumerable<MathExpression> AddToContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();
}
