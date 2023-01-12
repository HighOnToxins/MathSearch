
using MathSearch.Expression;
using System.Collections.Immutable;

namespace MathSearch.Expressions.Propersitions;

[Precedence(3)]
public sealed class DisjunctionExpression: OperatorExpression {

    public DisjunctionExpression(params MathExpression[] children) : base(children) { }

    public DisjunctionExpression(IEnumerable<MathExpression> children) : base(children) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) => 
        children.All(e => MathType.Boolean.Contains(e, context));

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) =>
        children
            .OrderBy(e => e)
            .Where(e => e is not BooleanExpression booleanExpression || booleanExpression.Value)
            .SelectMany(e => e.Extract<DisjunctionExpression>());

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) {
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

    protected override MathType ComputeType(IEnumerable<MathExpression> children, MathSystem context) => MathType.Boolean;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new DisjunctionExpression(children);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => children.Select(e => new NotExpression(e));
}
