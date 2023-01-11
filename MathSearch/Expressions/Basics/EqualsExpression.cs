
using MathSearch.Expression;

namespace MathSearch.Expressions.Basics;

public sealed class EqualsExpression: GroupExpression {
    public override HashSet<MathExpression> Children { get; }

    public EqualsExpression(params MathExpression[] children) {
        Children = new(children);
    }

    public EqualsExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    public override bool Condition(IEnumerable<MathExpression> children, Context context) {
        throw new NotImplementedException();
    }

    public override bool TryCompute(in IEnumerable<MathExpression> children, out MathExpression? result) {
        throw new NotImplementedException();
    }

    public override bool TryDetermineType(in IEnumerable<ExpressionType> simplifiedChild, Context context, out ExpressionType result) {
        throw new NotImplementedException();
    }

    public override bool TrySimplifyChildren(in IEnumerable<MathExpression> children, Context context, out IEnumerable<MathExpression> result) {
        throw new NotImplementedException();
    }

    public override bool TrySimplifyDown(in IEnumerable<MathExpression> children, Context context, out MathExpression? result) {
        throw new NotImplementedException();
    }

    public override bool TrySimplifyUp(in IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {
        throw new NotImplementedException();
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren) {
        throw new NotImplementedException();
    }

    public override void AddToContext(Context context) {
        throw new NotImplementedException();
    }

    public override Context CreateSubContext(Context context, IEnumerable<MathExpression> expressions) {
        throw new NotImplementedException();
    }
}
