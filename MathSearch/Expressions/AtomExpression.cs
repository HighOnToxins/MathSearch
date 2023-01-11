
using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class AtomExpression: MathExpression {

    public override int ChildCount => 0;

    public abstract MathType Type {get;}

    public IConvertible Value { get; private init; }

    public AtomExpression(IConvertible value) {
        Value = value;
    }

    public override IEnumerable<MathExpression> GetChildren() {
        yield break;
    }


    public override void AddToContext(Context context) { }

    public override Context CreateSubContext(Context context, IEnumerable<MathExpression> expressions) {
        return context;
    }

    public override MathExpression Simplify(Context? context = null) {
        if(context != null) {
            return context.ReplaceEquality(this);
        } else {
            return Clone();
        }
    }

    public override MathType DetermineType(Context? context = null) {
        context ??= new();
        return Type.Intersect(context.DetermineType(this));
    }

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