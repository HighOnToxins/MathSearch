
using MathSearch.Expression;
using MathSearch.Expressions.Basics;

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
        return context.Simplify(EvaluateSimplification(context));
    }

    private MathExpression EvaluateSimplification(Context context) {

        IEnumerable<MathExpression> children = Children;

        if(Condition(Children, context)) {
            children = SimplifyChildren(Children);

            if(TrySimplify(children, context, out MathExpression? resultDown) && resultDown != null) {
                return resultDown;
            }

            context.AddContext(AddToContext(children));
        }


        children = children.Select(e => e.Simplify(context.CreateSubContext(e)));

        if(Condition(Children, context)) {
            children = SimplifyChildren(Children);

            if(TrySimplify(children, context, out MathExpression? resultUp) && resultUp != null) {
                return resultUp;
            }
        }

        if(!children.Any() || children.Contains(new EmptyExpression())) {
            return new EmptyExpression();
        } else {
            return CreateInstance(children);
        }
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
            return ComputeType(Children, context);
        } else {
            return MathType.Universe;
        }
    }

    protected abstract MathType ComputeType(IEnumerable<MathExpression> children, Context context);

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> children);

    public override MathExpression Clone() => CreateInstance(Children);
}
