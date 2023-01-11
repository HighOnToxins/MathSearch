using MathSearch.Expression;

namespace MathSearch.Expressions.Sets;

public sealed class SetExpression: GroupExpression {

    public override HashSet<MathExpression> Children { get; }

    public SetExpression(params MathExpression[] children) {
        Children = new(children);
    }

    public SetExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    public override bool TryDetermineType(in IEnumerable<ExpressionType> simplifiedChild, Context context, out ExpressionType result) {
        result = ExpressionType.Set;
        return true;
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren) =>
        new SetExpression(Children);

    public override bool Condition(IEnumerable<MathExpression> children, Context context) {
        return false;
    }

    public override bool TryCompute(in IEnumerable<MathExpression> children, out MathExpression? result) {
        result = null;
        return false;
    }

    public override bool TrySimplifyChildren(in IEnumerable<MathExpression> children, Context context, out IEnumerable<MathExpression> result) {
        result = Array.Empty<MathExpression>();
        return false;
    }

    public override bool TrySimplifyDown(in IEnumerable<MathExpression> children, Context context, out MathExpression? result) {
        result = null;
        return false;
    }

    public override bool TrySimplifyUp(in IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {
        result = null;
        return false;
    }

    public override void AddToContext(Context context) { }

    public override Context CreateSubContext(Context context, IEnumerable<MathExpression> expressions) {
        return context;
    }
}
