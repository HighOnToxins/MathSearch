
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

        //down
        if(context.TryDetermineReplacement(this, out MathExpression? replacement) && replacement != null) {
            return replacement;
        }

        IEnumerable<MathExpression> children = Children;

        if(!Condition(children, context)) {
            return Clone();
        }

        if(TrySimplifyChildren(children, context, out IEnumerable<MathExpression> result1)) {
            children = result1;
        }

        if(TryCompute(children, out MathExpression? result3) && result3 != null) {
            return result3;
        }

        if(TrySimplifyDown(children, context, out MathExpression? result2) && result2 != null) {
            return result2;
        }

        //TODO: DETERMINE SUB CONTEXT PROPERLY?
        //children
        IEnumerable<MathExpression> simplifiedChildren = children.Select(e => e.Simplify(CreateSubContext(context, children.Where(e2 => !e2.Equals(e)))));

        //up
        if(!Condition(simplifiedChildren, context)) {
            return CreateInstance(simplifiedChildren);
        }

        if(TrySimplifyChildren(simplifiedChildren, context, out IEnumerable<MathExpression> result4)) {
            simplifiedChildren = result4;
        }

        if(TryCompute(simplifiedChildren, out MathExpression? result6) && result6 != null) {
            return result6;
        }

        if(TrySimplifyUp(simplifiedChildren, context, out MathExpression? result5) && result5 != null) {
            return result5;
        }

        return CreateInstance(simplifiedChildren);
    }


    //TODO: Restructure.

    public abstract bool Condition(IEnumerable<MathExpression> children, Context context);

    public abstract bool TrySimplifyChildren(in IEnumerable<MathExpression> children, Context context, out IEnumerable<MathExpression> result);

    public abstract bool TrySimplifyDown(in IEnumerable<MathExpression> children, Context context, out MathExpression? result);

    public abstract bool TrySimplifyUp(in IEnumerable<MathExpression> simplifiedChildren, Context context, out MathExpression? result);

    public override ExpressionType DetermineType(Context? context = null) {
        context ??= new();

        //TODO: Check for the actual output, and determine which of the type are the smallest
        //TODO: Maybe change to simply check for if expression in set is equal to true

        if(context.TryDetermineType(this, out ExpressionType type)) {
            return type;
        }

        if(Condition(Children, context) && TryDetermineType(
                Children.Select(e => e.DetermineType(CreateSubContext(context, Children.Where(e2 => !e2.Equals(e))))), 
                context, out ExpressionType result)) {
            return result;
        } else {
            return ExpressionType.Universe;
        }
    }

    public abstract bool TryDetermineType(in IEnumerable<ExpressionType> childTypes, Context context, out ExpressionType result);

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> simplifiedChildren);

    public override MathExpression Clone() => CreateInstance(Children);

    public abstract bool TryCompute(in IEnumerable<MathExpression> children, out MathExpression? result);
}
