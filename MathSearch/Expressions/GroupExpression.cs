
using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class GroupExpression : MathExpression {

    public virtual ICollection<MathExpression> Children { get;}

    public override int ChildCount => Children.Count;

    public GroupExpression(ICollection<MathExpression> children) {
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

    public abstract IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children);

    public abstract bool Condition(IEnumerable<MathExpression> children, Context context);

    public abstract bool TrySimplify(IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result);

    public override MathType DetermineType(Context? context = null) {
        context ??= new();

        //TODO: Create type structure.
        //TODO: Check for the actual output, and determine which of the type are the smallest
        //TODO: Maybe change to simply check for if expression in set is equal to true

        if(Condition(Children, context)) {
            return EvaluateType(
                Children.Select(e => e.DetermineType(CreateSubContext(context, Children.Where(e2 => !e2.Equals(e))))),
                context);
        } else {
            return context.DetermineType(this);
        }
    }

    public abstract MathType EvaluateType(IEnumerable<MathType> childTypes, Context context);

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren);

    public override MathExpression Clone() => CreateInstance(Children);
}
