
using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class AtomExpression: MathExpression {

    #region Properties

    public override int Precedence => 0;

    public override int OperandCount => 0;

    public IConvertible Value { get; private init; }

    #endregion

    public AtomExpression(IConvertible value) {
        Value = value;
    }

    #region Helper Methods

    public override IEnumerable<MathType> TypeOfOperands(MathType typeOfThis) { yield break; }

    public override IEnumerable<MathExpression> GetOperands() { yield break; }

    public override MathExpression Simplify(MathSystem? context = null) => Clone();

    public override MathExpression Factorize<E>() => Clone();

    public override MathExpression Distribute<E>() => Clone();

    #endregion

    #region Equality

    public override bool Equals(MathExpression? other) =>
        other is AtomExpression atomOther &&
        atomOther.GetType().IsEquivalentTo(GetType()) &&
        atomOther.Value.Equals(Value);

    public override int GetHashCode() =>
        GetType().Name.GetHashCode() ^ Value.GetHashCode();

    #endregion
}

public abstract class AtomExpression<T>: AtomExpression where T : IConvertible {

    public new T Value {
        get {
            if(base.Value is T tValue) {
                return tValue;
            } else {
                throw new InvalidDataException();
            }
        }
    }

    protected AtomExpression(T value) : base(value) {
    }

}