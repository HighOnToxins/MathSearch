
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
        return children.All(e => e.DetermineType(CreateSubContext(context, children.Where(e2 => !e2.Equals(e)))) == MathType.Boolean);
    }

    public override IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children) =>
        children
            .Where(e => e is not BooleanExpression booleanExpression || !booleanExpression.Value)
            .SelectMany(e => e.Extract<DisjunctionExpression>());

    public override bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result) {
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

    public override MathType EvaluateType(IEnumerable<MathType> childTypes, Context context) =>
        MathType.Boolean;

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
