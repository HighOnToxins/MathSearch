
using MathSearch.Expression;
using MathSearch.Expressions.Basics;

namespace MathSearch.Expressions;

public abstract class OperatorExpression : MathExpression {

    public IReadOnlyList<MathExpression> Children { get;}

    public override int ChildCount => Children.Count;

    public OperatorExpression(IEnumerable<MathExpression> children) {
        Children = new List<MathExpression>(children);
    }

    public OperatorExpression(params MathExpression[] children) {
        Children = children;
    }

    public override IEnumerable<MathExpression> GetChildren() => Children;

    public override MathExpression Simplify(MathSystem? context = null) {
        context ??= new();
        return context.Simplify(EvaluateSimplification(context));
    }

    private MathExpression EvaluateSimplification(MathSystem context) {

        IEnumerable<MathExpression> children = Children;

        if(ConditionIsMet(children, context)) {
            children = SimplifyChildren(children);

            if(TrySimplify(children, context, out MathExpression? resultDown) && resultDown != null) {
                return resultDown;
            }
        }

        //children = children.Select((e, i) => e.Simplify(GetContextForChild(i, children, context)));

        List<MathExpression> result = new();
        int i = 0;
        foreach(MathExpression child in children) {
            result.Add(child.Simplify(GetContextForChild(i, children, context)));
            i++;
        }
        children = result;

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

    protected abstract bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context);

    protected abstract bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result);

    public override MathType DetermineType(MathSystem? context = null) {
        context ??= new();
        return EvaluateType(context).Intersect(context.EvaluateTypeOf(this));
    }

    protected MathType EvaluateType(MathSystem context) {
        if(ConditionIsMet(Children, context)) {
            return ComputeType(Children, context);
        } else {
            return MathType.Universe;
        }
    }

    protected abstract MathType ComputeType(IEnumerable<MathExpression> children, MathSystem context);

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> children);

    public override MathExpression Clone() => CreateInstance(Children.Select(e => e.Clone()));
}
