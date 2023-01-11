
using MathSearch.Expression;

namespace MathSearch.Expressions.Propersitions;

public sealed class DisjunctionExpression: GroupExpression {

    public override HashSet<MathExpression> Children { get; }

    public DisjunctionExpression(params MathExpression[] children) {
        Children = new(children);
    }

    public DisjunctionExpression(IEnumerable<MathExpression> children) {
        Children = new(children);
    }

    public DisjunctionExpression(HashSet<MathExpression> children) {
        Children = children;
    }

    public override bool Condition(IEnumerable<MathExpression> children, Context context) {
        return children.All(e => e.DetermineType(CreateSubContext(context, children.Where(e2 => !e2.Equals(e)))) == ExpressionType.Boolean);
    }

    public override bool TrySimplifyChildren(in IEnumerable<MathExpression> children, Context context, out IEnumerable<MathExpression> result) {
        result = children
            .Where(e => e is not BooleanExpression booleanExpression || !booleanExpression.Value)
            .SelectMany(e => e.Extract<DisjunctionExpression>());
        return true;
    }

    public override bool TryCompute(in IEnumerable<MathExpression> children, out MathExpression? result) {
        if(children.All(e => e is BooleanExpression booleanExpression && !booleanExpression.Value)) {
            result = new BooleanExpression(false);
            return true;
        } else if(children.Any(e => e is BooleanExpression booleanExpression && booleanExpression.Value)) {
            result = new BooleanExpression(true);
            return true;
        }

        result = null;
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

    public override bool TryDetermineType(in IEnumerable<ExpressionType> simplifiedChild, Context context, out ExpressionType result) {
        result = ExpressionType.Boolean;
        return true;
    }

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> children) =>
        new DisjunctionExpression(children);

    public override void AddToContext(Context context) {
        foreach(MathExpression child in Children) {
            //TODO: Add not expressions to context
            //TODO: Add not equals to context

            context.AddReplacement(child, new BooleanExpression(false));
            //child.AddInverseToContext(context); //TODO: Add inverse to context.
        }
    }

    public override Context CreateSubContext(Context context, IEnumerable<MathExpression> expressions) {
        Context result = context.Clone();

        foreach(MathExpression expression in expressions) {
            expression.AddToContext(result);
        }

        return result;
    }
}
