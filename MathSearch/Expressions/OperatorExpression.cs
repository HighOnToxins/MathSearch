
using MathSearch.Expression;
using MathSearch.Expressions.Basics;

namespace MathSearch.Expressions;

public abstract class OperatorExpression: MathExpression {

    protected readonly IReadOnlyList<MathExpression> children;

    public override int ChildCount => children.Count;

    public OperatorExpression(IEnumerable<MathExpression> children) {
        this.children = new List<MathExpression>(children);
    }

    public OperatorExpression(params MathExpression[] children) {
        this.children = children;
    }

    public override IEnumerable<MathExpression> GetChildren() => children;

    public override MathExpression Simplify(MathSystem? context = null) {
        context ??= new();
        return context.Simplify(GetSimplification(context));
    }

    private MathExpression GetSimplification(MathSystem context) {

        IEnumerable<MathExpression> children = this.children;

        if(TryEvaluateSimplification(ref children, out MathExpression? resultDown, context) && resultDown != null) {
            return resultDown;
        }

        children = SimplifyChildren(children, context);

        if(TryEvaluateSimplification(ref children, out MathExpression? resultUp, context) && resultUp != null) {
            return resultUp;
        }

        if(children.Contains(new EmptyExpression())) {
            return new EmptyExpression();
        } else {
            return CreateInstance(children);
        }
    }

    private IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children, MathSystem context) {
        List<MathExpression> result = children.ToList();
        for(int i = 0; i < result.Count; i++) {
            result[i] = result[i].Simplify(GetContextForChild(i, result, context));
        }
        return result;
    }

    private bool TryEvaluateSimplification(ref IEnumerable<MathExpression> children, out MathExpression? result, MathSystem context) {
        if(ConditionIsMet(children, context)) {
            children = SimplifyChildren(children);

            if(TrySimplify(children, context, out MathExpression? resultDown) && resultDown != null) {
                result = resultDown;
                return true;
            }
        }

        result = null;
        return false;
    }

    protected abstract IEnumerable<MathExpression> SimplifyChildren(IEnumerable<MathExpression> children);

    protected abstract bool ConditionIsMet(IEnumerable<MathExpression> children, MathSystem context);

    protected abstract bool TrySimplify(IEnumerable<MathExpression> children, MathSystem context, out MathExpression? result);

    protected override MathType ComputeType(MathSystem context) {
        if(ConditionIsMet(children, context)) {
            return DetermineType(children, context);
        } else {
            return MathType.Universe;
        }
    }

    protected abstract MathType DetermineType(IEnumerable<MathExpression> children, MathSystem context);

    public override bool TryGroup<E>(out MathExpression? result) {

        IEnumerable<E> eChildren = GetChildren().OfType<E>();
        IEnumerable<MathExpression> nonEChildren = GetChildren().Except(eChildren);

        if(!eChildren.Any()) {
            result = null;
            return false;
        }

        IEnumerable<MathExpression> comparisons = eChildren.First().GetChildren();
        List<MathExpression> group = new();

        foreach(MathExpression comparer in comparisons) {

            bool isContained = true;
            foreach(MathExpression eChild in eChildren) {
                if(!eChild.GetChildren().Contains(comparer)) {
                    isContained = false;
                    break;
                }
            }

            if(isContained) {
                group.Add(comparer);
            }
        }

        if(!group.Any()) {
            result = null;
            return false;
        }

        IEnumerable<MathExpression> nonGroup = eChildren
            .SelectMany(e => e.GetChildren())
            .Except(group);

        if(!nonGroup.Any()) {
            result = null;
            return false;
        }

        MathExpression eInstance = eChildren.First().CreateInstance(group.Concat(new MathExpression[] { CreateInstance(nonGroup) }));

        if(nonEChildren.Any()) {
            result = CreateInstance(new MathExpression[] { eInstance }.Concat(nonEChildren));
        } else {
            result = eInstance;
        }
        return true;

    }

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> children);

    public override MathExpression Clone() => CreateInstance(children.Select(e => e.Clone()));
}
