using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class TernaryExpression: OperatorExpression {

    private static T GetLeftOperand<T>(IEnumerable<T> operands) => operands.First();
    private static T GetMiddleOperand<T>(IEnumerable<T> operands) => operands.ElementAt(1);
    private static T GetRightOperand<T>(IEnumerable<T> operands) => operands.ElementAt(2);

    public MathExpression LeftOperand { get => GetLeftOperand(operands); }

    public MathExpression MiddleOperand { get => GetMiddleOperand(operands); }

    public MathExpression RightOperand { get => GetRightOperand(operands); }

    public override int OperandCount => 3;

    public TernaryExpression(MathExpression leftOperand, MathExpression middleOperand, MathExpression rightOperand) : base(leftOperand, middleOperand, rightOperand) { }

    protected abstract bool Condition(MathExpression leftOperand, MathExpression middleOperand, MathExpression rightOperand, MathSystem context);

    protected override bool ConditionIsMet(IEnumerable<MathExpression> operands, MathSystem context) =>
        Condition(GetLeftOperand(operands), GetMiddleOperand(operands), GetRightOperand(operands), context);

    protected abstract bool TrySimplify(MathExpression leftOperand, MathExpression middleOperand, MathExpression rightOperand, MathSystem context, out MathExpression? result);

    protected override bool TrySimplify(ref IEnumerable<MathExpression> operands, MathSystem context, out MathExpression? result) =>
        TrySimplify(GetLeftOperand(operands), GetMiddleOperand(operands), GetRightOperand(operands), context, out result);

    protected abstract MathExpression CreateInstance(MathExpression leftOperand, MathExpression middleOperand, MathExpression rightOperand);

    protected override MathExpression CreateInstance(IEnumerable<MathExpression> operands) =>
        CreateInstance(GetLeftOperand(operands), GetMiddleOperand(operands), GetRightOperand(operands));

}
