using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class UnaryExpression: OperatorExpression {

    private static T GetOperand<T>(IEnumerable<T> operands) => operands.First();

    public MathExpression Operand { get => GetOperand(operands); }

    public override int OperandCount => 1;

    public UnaryExpression(MathExpression operand) : base(operand) { }

    protected override bool ConditionIsMet(IEnumerable<MathExpression> operands, MathSystem context) =>
        ConditionIsMet(GetOperand(operands), context);

    protected abstract bool ConditionIsMet(MathExpression operand, MathSystem context);

    protected abstract bool TrySimplify(MathExpression operand, MathSystem context, out MathExpression? result);

    protected override bool TrySimplify(ref IEnumerable<MathExpression> operand, MathSystem context, out MathExpression? result) =>
        TrySimplify(GetOperand(operand), context, out result);

    protected abstract MathExpression CreateInstance(MathExpression operand);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> operands) =>
        CreateInstance(GetOperand(operands));

}