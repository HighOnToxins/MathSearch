using MathSearch.Expression;

namespace MathSearch.Expressions;

public abstract class MathExpression: ICloneable, IComparable<MathExpression>, IEquatable<MathExpression> {

    public abstract int ChildCount { get; }

    public abstract IEnumerable<MathExpression> GetChildren();

    public abstract MathExpression Simplify(Context? context = null);

    public abstract MathType DetermineType(Context? context = null);

    public abstract MathExpression Clone();

    object ICloneable.Clone() => Clone();

    protected abstract IEnumerable<MathExpression> AddToContext(IEnumerable<MathExpression> children);

    public IEnumerable<MathExpression> Extract<T>() where T : MathExpression{
        if(this is T) {
            return GetChildren().SelectMany(c => c.Extract<T>());
        } else {
            return new[] { Clone() };
        }
    }

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

        if(expression1 is AtomExpression atom1 && expression2 is AtomExpression atom2) {
            return (atom1.Value.ToString() ?? "").CompareTo(atom2.Value.ToString());
        }

        if(expression1.GetType().IsEquivalentTo(expression2.GetType()) &&
                expression1.ChildCount > 0 &&
                expression2.ChildCount > 0) {
            return CompareChildrenOf(expression1, expression2);
        }

        return 0;
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

        return 0;
    }
}