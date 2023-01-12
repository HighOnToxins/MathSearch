﻿
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public class ConjunctionExpression: OperatorExpression {

    public override IReadOnlySet<MathExpression> Children { get; }

    public ConjunctionExpression(params MathExpression[] children) {
        Children = new HashSet<MathExpression>(children);
    }

    public ConjunctionExpression(IEnumerable<MathExpression> children) {
        Children = new HashSet<MathExpression>(children);
    }

    public ConjunctionExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) {
        return children.All(e => MathType.Boolean.Contains(e, context));
    }

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) =>
        children
            .Where(e => e is not BooleanExpression booleanExpression || !booleanExpression.Value)
            .SelectMany(e => e.Extract<ConjunctionExpression>());

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) {
        if(children.Count() == 1) {
            result = children.First();
            return true;
        } else if(children.All(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        } else if(children.Any(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        }

        result = null;
        return false;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> children, MathSystem context) => MathType.Boolean;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new ConjunctionExpression(children);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => children;
}
