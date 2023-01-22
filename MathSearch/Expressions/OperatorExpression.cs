
using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class OperatorExpression: MathExpression {

    #region Properties

    protected readonly IReadOnlyList<MathExpression> operands;

    public override int OperandCount => operands.Count;

    #endregion

    #region Constructors

    public OperatorExpression(IEnumerable<MathExpression> operands) {
        this.operands = new List<MathExpression>(operands);
    }

    public OperatorExpression(params MathExpression[] operands) {
        this.operands = operands;

        //TODO: Add message to exception.
        if(this.operands.Count != OperandCount) {
            throw new InvalidDataException();
        }
    }

    #endregion

    #region Simplify Methods

    public override MathExpression Simplify(MathSystem? context = null) {
        context ??= new();
        return context.Simplify(GetSimplification(context));
    }

    private MathExpression GetSimplification(MathSystem context) {
        IEnumerable<MathExpression> operands = this.operands;

        if(TryConditionedSimplify(ref operands, context, out MathExpression? resultDown) && resultDown != null) return resultDown;

        operands = operands.Select(e => e.Simplify(GetContextFor(e).CombineWith(context)));

        if(TryConditionedSimplify(ref operands, context, out MathExpression? resultUp) && resultUp != null) return resultUp;

        return Clone();
    }

    private bool TryConditionedSimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result) {
        result = null;
        return ConditionIsMet(operands, context) && TrySimplify(ref operands, context, out result);
    }

    protected abstract bool TrySimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result);

    #endregion

    #region Type Methods

    public override MathType GetMathType(MathSystem? context = null) {
        context ??= new();
        if(ConditionIsMet(operands, context)) {
            return ComputeType(context).IntersectWith(context.TypeOf(this));
        } else {
            return base.GetMathType(context);
        }
    }

    protected abstract MathType ComputeType(MathSystem context);

    #endregion

    #region Helper Methods

    public override IEnumerable<MathExpression> GetOperands() => operands;

    protected abstract bool ConditionIsMet(IEnumerable<MathExpression> operands, MathSystem context);

    public override MathExpression Factorize<E>() {
        throw new NotImplementedException();
    }

    public override MathExpression Distribute<E>() {
        throw new NotImplementedException();
    }

    #endregion

    #region Instancing

    protected abstract MathExpression CreateInstance(IEnumerable<MathExpression> children);

    public override MathExpression Clone() => CreateInstance(operands.Select(e => e.Clone()));

    #endregion

}
