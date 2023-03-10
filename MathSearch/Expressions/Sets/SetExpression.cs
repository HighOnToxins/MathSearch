using MathSearch.Expression;
using MathSearch.Expressions.Basics;
using System.Collections.Immutable;

namespace MathSearch.Expressions.Sets;

public sealed class SetExpression: OperatorExpression {

    public override int Precedence => 0;

    public IReadOnlyList<MathExpression> Children => children;

    public SetExpression(params MathExpression[] children) : base(children) { }

    public SetExpression(IEnumerable<MathExpression> children) : base(children) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context) => true;

    protected override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) => children
        .Where(e => e is not EmptyExpression)
        .ToImmutableSortedSet();

    protected override bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result) {
        result = null;
        return false;
    }

    protected override MathType DetermineType(IEnumerable<MathExpression> simplifiedChild, MathSystem context) => MathType.Set;

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) => new SetExpression(children);

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();
}
