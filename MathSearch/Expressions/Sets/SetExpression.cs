using MathSearch.Expression;
using System.Collections.Immutable;

namespace MathSearch.Expressions.Sets;

public sealed class SetExpression: OperatorExpression {

    public IReadOnlyList<MathExpression> Children => children;

    public SetExpression(params MathExpression[] children) : base(children) { }

    public SetExpression(IEnumerable<MathExpression> children) : base(children) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) => true;

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children.ToImmutableSortedSet();

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) {
        
        //TODO: Simplify empty set to nothing type.
        
        result = null;
        return false;
    }

    protected override MathType ComputeType(IEnumerable<MathExpression> simplifiedChild, MathSystem context) => MathType.Set;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new SetExpression(children);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();
}
