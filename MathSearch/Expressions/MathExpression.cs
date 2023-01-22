using MathSearch.Expression;

namespace MathSearch.Expressions;

//TODO: add ternary if operator
//TODO: add binary-forall operator
//TODO: add binary-exists operator

public abstract class MathExpression: ICloneable, IComparable<MathExpression>, IEquatable<MathExpression> {

    #region Properties

    public abstract int OperandCount { get; }

    public abstract int Precedence { get; }

    #endregion

    public virtual MathExpression CleanUp() => Clone();

    public abstract MathExpression Simplify(MathSystem? context = null);

    public virtual MathType GetMathType(MathSystem? context = null) => MathType.Universe;

    #region Helper Methods

    public MathSystem GetContextFor(MathExpression child) =>
        new(GetOperands().Except(new MathExpression[] { child })); 

    public abstract IEnumerable<MathExpression> GetOperands();

    public abstract IEnumerable<MathType> TypeOfOperands(MathType typeOfThis);

    public bool IsSimple() {
        if(GetMathType() != MathType.Universe) {
            return false;
        } else {
            return GetOperands().All(e => e.IsSimple());
        }
    }

    public IEnumerable<MathExpression> ExtractOperandsOf<E>() where E : MathExpression {
        if(this is E) {
            return GetOperands().SelectMany(c => c.ExtractOperandsOf<E>());
        } else {
            return new[] { Clone() };
        }
    }

    public abstract MathExpression Factorize<E>() where E : OperatorExpression;

    public abstract MathExpression Distribute<E>() where E : OperatorExpression;

    #endregion

    #region Clone Methods

    public abstract MathExpression Clone();

    object ICloneable.Clone() => Clone();

    #endregion

    #region Equality Methods

    public override bool Equals(object? obj) =>
        Equals(obj as MathExpression);

    public virtual bool Equals(MathExpression? other) {
        if(other == null || !other.GetType().IsEquivalentTo(GetType())) {
            return false;
        }

        MathExpression[] children = GetOperands().ToArray();
        MathExpression[] otherChildren = other.GetOperands().ToArray();

        if(children.Length != otherChildren.Length) {
            return false;
        }

        for(int i = 0; i < children.Length; i++) {
            if(!children[i].Equals(otherChildren[i])) {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() {
        int result = GetType().Name.GetHashCode();

        unchecked {
            foreach(MathExpression child in GetOperands()) {
                result ^= child.GetHashCode();
            }
        }

        return result;
    }

    public int CompareTo(MathExpression? other) {
        if(other != null) {
            return Compare(this, other);
        } else {
            return -1;
        }
    }

    public static int Compare(MathExpression expression1, MathExpression expression2) {

        //compare values
        if(expression1 is AtomExpression atom1 && expression2 is AtomExpression atom2) {
            return (atom1.Value.ToString() ?? "").CompareTo(atom2.Value.ToString());
        }

        //compare precedence
        if(!expression1.GetType().IsEquivalentTo(expression2.GetType())) {
            int result = expression2.Precedence - expression1.Precedence;
            return result != 0 ? result : expression2.GetHashCode() - expression1.GetHashCode();
        }

        //compare children
        if(expression1.OperandCount > 0 && expression2.OperandCount > 0) {
            return CompareChildrenOf(expression1, expression2);
        }

        return 0;
    }

    private static int CompareChildrenOf(MathExpression operator1, MathExpression operator2) {

        MathExpression[] children1 = operator1.GetOperands().ToArray();
        MathExpression[] children2 = operator2.GetOperands().ToArray();

        for(int i = 0; i < operator1.OperandCount && i < operator2.OperandCount; i++) {
            int result = Compare(children1[i], children2[i]);

            if(result != 0) {
                return result;
            }
        }

        return operator2.OperandCount - operator1.OperandCount;
    }

    #endregion

}