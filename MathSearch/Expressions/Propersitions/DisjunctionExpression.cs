
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public sealed class DisjunctionExpression: OperatorExpression {

    public override IReadOnlySet<MathExpression> Children { get; }

    public DisjunctionExpression(params MathExpression[] children) {
        Children = new HashSet<MathExpression>(children);
    }

    public DisjunctionExpression(IEnumerable<MathExpression> children) {
        Children = new HashSet<MathExpression>(children);
    }

    public DisjunctionExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, Context context) {
        return children.All(e => e.DetermineType(context) == MathType.Boolean);
    }

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) =>
        children
            .Where(e => e is not BooleanExpression booleanExpression || !booleanExpression.Value)
            .SelectMany(e => e.Extract<DisjunctionExpression>());

    protected override bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {
        if(simplifiedChildren.All(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        } else if(simplifiedChildren.Any(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        }

        result = null;
        return false;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> childTypes, Context context) => MathType.Boolean;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        new DisjunctionExpression(children);

    protected override IEnumerable<MathExpression> AddToContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();
}
