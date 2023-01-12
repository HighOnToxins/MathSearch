
using MathSearch.Expression;
using MathSearch.Expressions.Basics;

namespace MathSearch.Expressions;

public abstract class OperatorExpression : MathExpression {

    public virtual IReadOnlyCollection<MathExpression> Children { get;}

    public override int ChildCount => Children.Count;

    public OperatorExpression(IReadOnlyCollection<MathExpression> children) {
        Children = children;
    }

    public OperatorExpression() {
        Children = Array.Empty<MathExpression>();
    }

    public override IEnumerable<MathExpression> GetChildren() => Children;

    public override MathExpression Simplify(Context? context = null) {
        context ??= new();
        return context.Simplify(EvaluateSimplification(context));
    }

    private MathExpression EvaluateSimplification(Context context) {

        IEnumerable<MathExpression> children = Children;

        if(ConditionIsMet(children, context)) {
            children = SimplifyChildren(children);

            if(TrySimplify(children, context, out MathExpression? resultDown) && resultDown != null) {
                return resultDown;
            }

            context.AddContext(AddToContext(children));
        }

        children = children.Select((e, i) => e.Simplify(context.CreateSubContext(i)));

        //List<MathExpression> result = new();
        //foreach(MathExpression child in children) {
        //    result.Add(child.Simplify(context.CreateSubContext(child)));
        //}
        //children = result;
        
        if(ConditionIsMet(children, context)) {
            children = SimplifyChildren(children);

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

    protected abstract bool ConditionIsMet(IEnumerable<MathExpression> children, Context context);

    protected abstract bool TrySimplify(IEnumerable<MathExpression> children, Context context, out MathExpression? result);

    public override MathType DetermineType(Context? context = null) {
        context ??= new();
        return EvaluateType(context).Intersect(context.DetermineType(this));
    }

    protected MathType EvaluateType(Context context) {
        if(ConditionIsMet(Children, context)) {
            return ComputeType(Children, context);
        } else {
            return MathType.Universe;
        }
    }

    protected abstract MathType ComputeType(IEnumerable<MathExpression> children, Context context);

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> children);

    public override MathExpression Clone() => CreateInstance(Children.Select(e => e.Clone()));
}
