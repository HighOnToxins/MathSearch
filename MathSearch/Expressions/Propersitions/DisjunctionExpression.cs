
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
        return children.All(e => MathType.Boolean.Contains(e, context));
    }

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) =>
        children
            .Where(e => e is not BooleanExpression booleanExpression || booleanExpression.Value)
            .SelectMany(e => e.Extract<DisjunctionExpression>());

    protected override bool TrySimplify(IEnumerable<MathExpression> children, Context context, out MathExpression? result) {
        if(children.Count() == 1) {
            result = children.First();
            return true;
        }else if(children.All(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        } else if(children.Any(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        }

        result = null;
        return false;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> children, Context context) => MathType.Boolean;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new DisjunctionExpression(children);

    protected override IEnumerable<MathExpression> AddToContext(IEnumerable<MathExpression> children) => children.Select(e => new NotExpression(e));
}
