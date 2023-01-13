
using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class AtomExpression: MathExpression {

    public override int ChildCount => 0;

    public abstract MathType Type { get; }

    public IConvertible Value { get; private init; }

    public AtomExpression(IConvertible value) {
        Value = value;
    }

    public override IEnumerable<MathExpression> GetChildren() {
        yield break;
    }

    public override MathExpression Simplify(MathSystem? context = null) {
        if(context != null) {
            return context.Simplify(this);
        } else {
            return Clone();
        }
    }

    public override MathType EvaluateType(MathSystem? context = null) {
        context ??= new();
        return Type.IntersectWith(context.GetTypeOf(this));
    }
    
    public override bool TryGroup<E>(out MathExpression? result) {
        result = null;
        return false;
    }

    protected override IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children) => Array.Empty<MathExpression>();

    public override bool Equals(MathExpression? other) =>
        other is AtomExpression atomOther &&
        atomOther.GetType().IsEquivalentTo(GetType()) &&
        atomOther.Value.Equals(Value);

    public override int GetHashCode() =>
        GetType().Name.GetHashCode() ^ Value.GetHashCode();

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