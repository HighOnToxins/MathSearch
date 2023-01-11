
using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class GroupExpression : MathExpression {

    public virtual IReadOnlyCollection<MathExpression> Children { get;}

    public override int ChildCount => Children.Count;

    public GroupExpression(IReadOnlyCollection<MathExpression> children) {
        Children = children;
    }

    public GroupExpression() {
        Children = Array.Empty<MathExpression>();
    }

    public override IEnumerable<MathExpression> GetChildren() => Children;

    public override MathExpression Simplify(Context? context = null) {
        context ??= new();
        return context.ReplaceEquality(EvaluateSimplification(context));
    }

    private MathExpression EvaluateSimplification(Context context) {
        if(!Condition(Children, context)) {
            return CreateInstance(Children);
        }

        IEnumerable<MathExpression> simplifiedChildren = SimplifyChildren(Children);

        if(TrySimplify(simplifiedChildren, context, out MathExpression? resultDown) && resultDown != null) {
            return resultDown;
        }

        //TODO: DETERMINE SUB CONTEXT PROPERLY?
        simplifiedChildren = SimplifyChildren(simplifiedChildren
            .Select(e => e.Simplify(CreateSubContext(context, simplifiedChildren.Where(e2 => !e2.Equals(e))))));

        if(TrySimplify(simplifiedChildren, context, out MathExpression? resultUp) && resultUp != null) {
            return resultUp;
        }

        return CreateInstance(simplifiedChildren);
    }

    protected abstract IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children);

    protected abstract bool Condition(IEnumerable<MathExpression> children, Context context);

    protected abstract bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result);

    public override MathType DetermineType(Context? context = null) {
        context ??= new();
        return EvaluateType(context).Intersect(context.DetermineType(this));
    }

    protected MathType EvaluateType(Context context) {
        if(Condition(Children, context)) {
            return ComputeType(
                Children.Select(e => e.DetermineType(CreateSubContext(context, Children.Where(e2 => !e2.Equals(e))))),
                context);
        } else {
            return MathType.Universe;
        }
    }

    protected abstract MathType ComputeType(IEnumerable<MathType> childTypes, Context context);

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren);

    public override MathExpression Clone() => CreateInstance(Children);
}
