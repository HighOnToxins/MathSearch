using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class MathExpression: ICloneable, IComparable<MathExpression>, IEquatable<MathExpression> {

    public abstract int ChildCount { get; }

    public abstract IEnumerable<MathExpression> GetChildren();

    public abstract MathExpression Simplify(MathSystem? context = null);

    public abstract MathType DetermineType(MathSystem? context = null);

    public abstract MathExpression Clone();

    object ICloneable.Clone() => Clone();

    public bool IsSimple() {
        if(Attribute.GetCustomAttribute(GetType(), typeof(IsNotSimpleAttribute)) != null) {
            return false;
        } else {
            return GetChildren().All(e => e.IsSimple());
        }
    }

    public MathSystem? GetContextForChild(int index, IEnumerable<MathExpression>? children = null, MathSystem? context = null) {
        children ??= GetChildren();

        List<MathExpression> temp = AsContext(children).ToList();
        if(temp.Count > 0) temp.RemoveAt(index);

        context ??= new();
        return new MathSystem(context) { temp };
    }

    protected abstract IEnumerable<MathExpression> AsContext(IEnumerable<MathExpression> children);

    public IEnumerable<MathExpression> Extract<E>() where E : MathExpression {
        if(this is E) {
            return GetChildren().SelectMany(c => c.Extract<E>());
        } else {
            return new[] { Clone() };
        }
    }

    public abstract bool TryGroup<E>(out MathExpression? result) where E : OperatorExpression;

    public virtual bool Equals(MathExpression? other) {
        if(other == null || !other.GetType().IsEquivalentTo(GetType())) {
            return false;
        }

        MathExpression[] children = GetChildren().ToArray();
        MathExpression[] otherChildren = other.GetChildren().ToArray();

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

    public override bool Equals(object? obj) {
        return Equals(obj as MathExpression);
    }

    public override int GetHashCode() {
        int result = GetType().GetHashCode();

        unchecked {
            foreach(MathExpression child in GetChildren()) {
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
            int result = expression2.GetPrecedence() - expression1.GetPrecedence();
            return result != 0 ? result : expression2.GetHashCode() - expression1.GetHashCode();
        }

        //compare children
        if(expression1.ChildCount > 0 && expression2.ChildCount > 0) {
            return CompareChildrenOf(expression1, expression2);
        }

        return 0;
    }

    private int GetPrecedence() {
        if(Attribute.GetCustomAttribute(GetType(), typeof(PrecedenceAttribute)) is PrecedenceAttribute attrib) {
            return attrib.Value;
        } else {
            return GetHashCode();
        }
    }

    private static int CompareChildrenOf(MathExpression operator1, MathExpression operator2) {

        MathExpression[] children1 = operator1.GetChildren().ToArray();
        MathExpression[] children2 = operator2.GetChildren().ToArray();

        for(int i = 0; i < operator1.ChildCount && i < operator2.ChildCount; i++) {
            int result = Compare(children1[i], children2[i]);

            if(result != 0) {
                return result;
            }
        }

        return operator2.ChildCount - operator1.ChildCount;
    }
}