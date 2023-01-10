
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public class AndExpression: GroupExpression {

    public override HashSet<MathExpression> Children { get; }

    public AndExpression(params MathExpression[] children) {
        Children = new(children);
    }

    public AndExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    public override bool Condition(in IEnumerable<MathExpression> children, Context context) {
        return children.All(e => context.DetermineType(e) == ExpressionType.Boolean);
    }

    public override bool TrySimplifyChildren(in IEnumerable<MathExpression> children, Context context, out IEnumerable<MathExpression> result) {
        result = children.Where(e => e is not BooleanExpression booleanExpression || !booleanExpression.Value);
        return true;
    }

    public override bool TrySimplifyDown(in IEnumerable<MathExpression> children, Context context, out MathExpression? result) {
        foreach(MathExpression child in Children) {
            context.AddReplacement(child, new BooleanExpression(true));
            child.AddToContext(ref context);
        }

        result = null;
        return false;
    }

    public override bool TrySimplifyUp(in IEnumerable<MathExpression> simplifiedChild, Context context, out MathExpression? result) {
        result = null;
        return false;
    }

    public override bool TryCompute(in IEnumerable<MathExpression> children, out MathExpression? result) {
        if(children.Any(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        }else if(children.All(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        }

        result = null;
        return false;
    }

    public override bool TryDetermineType(in IEnumerable<MathExpression> simplifiedChild, Context context, out ExpressionType result) {
        result = ExpressionType.Boolean;
        return true;
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren) =>
        new AndExpression(Children);

    public override void AddToContext(ref Context context) {}
}
