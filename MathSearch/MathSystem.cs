using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using System.Collections;
using System.Linq.Expressions;

namespace MathSearch;

public sealed class MathSystem: IEnumerable {

    private HashSet<MathExpression> expressions;

    public IReadOnlySet<MathExpression> Expressions => expressions;

    public bool IsInconsistent { get => expressions.Contains(new BooleanExpression(false)); }

    public MathSystem(params MathExpression[] expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public MathSystem(IEnumerable<MathExpression> expressions) {
        this.expressions = new HashSet<MathExpression>(expressions);
    }

    public MathSystem(MathSystem system) {
        expressions = new HashSet<MathExpression>(system.Expressions);
    }

    public void Add(MathExpression expression) {
        expressions.Add(expression);
    }

    public void Add(IEnumerable<MathExpression> expressions) {
        foreach(MathExpression expression in expressions) {
            this.expressions.Add(expression);
        }
    }

    public void Simplify() {
        List<MathExpression> newExpressions = expressions
            .SelectMany(e => e.Extract<ConjunctionExpression>())
            .ToList();

        //extract all type information
        int i = 0;
        foreach(MathExpression expression in expressions) {
            newExpressions.AddRange(expression
                .Extract()
                .Select(e => new InExpression(e.Clone(), e.EvaluateType(GetContextForChild(i, expressions)))));
            i++;
        }

        //simplify all expressions
        for(int j = 0; j < newExpressions.Count; j++) {
            newExpressions[j] = newExpressions[j].Simplify(GetContextForChild(j, newExpressions));
        }

        expressions = new(newExpressions);
        expressions.Remove(new BooleanExpression(true));
        expressions.Remove(new EmptyExpression());
    }

    private static MathSystem GetContextForChild(int index, IEnumerable<MathExpression> expressions) {
        List<MathExpression> temp = expressions.ToList();
        if(index < temp.Count) temp.RemoveAt(index);
        return new MathSystem() { temp };
    }

    public MathExpression Determine(MathExpression expression) {
        return Simplify(expression.Simplify(this));
    }

    internal MathExpression Simplify(MathExpression expression) {

        //contains the expression
        if(expressions.Contains(expression)) {
            return new BooleanExpression(true);
        }

        //contains the notted expression
        bool containsNot = expressions
            .OfType<NotExpression>()
            .Select(e => e.Child)
            .Contains(expression);

        if(containsNot) {
            return new BooleanExpression(false);
        }

        //contains simplified equality
        IEnumerable<MathExpression> replacement = GetEqualitiesOf(expression)
            .Where(e => e.IsSimple());

        if(replacement.Count() >= 1) {
            return replacement.First();
        }

        return expression.Clone();
    }

    public IEnumerable<MathExpression> GetEqualitiesOf(MathExpression expression) =>
        expressions
            .OfType<EqualsExpression>()
            .Where(e => e.Children.Contains(expression))
            .SelectMany(e => e.Children);

    public bool TryDetermineEquality(out bool result, params MathExpression[] expressions) =>
        TryDetermineEquality(out result, expressions.AsEnumerable());

    public bool TryDetermineEquality(out bool result, IEnumerable<MathExpression> expressions) {

        if(expressions.Count() <= 1) {
            result = true;
            return true;
        }

        MathExpression comparer = expressions.First();
        bool allAreEqual = true;
        for(int i = 1; i < expressions.Count() & allAreEqual; i++) {
            //Determine not equal by type or context
            if(     TypesAreDisjoint(expressions, comparer, i) ||
                    AreNotEqual(expressions, comparer, i)) {
                result = false;
                return true;
            }

            //determine simplified expressions
            if(comparer.IsSimple() && expressions.ElementAt(i).IsSimple()) {
                result = comparer.Equals(expressions.ElementAt(i));
                return true;
            }

            //if the expressions are not equal, it can not be determined.
            if(!comparer.Equals(expressions.ElementAt(i))) {
                result = default;
                return false;
            }
        }

        result = true;
        return true;
    }

    private bool AreNotEqual(IEnumerable<MathExpression> expressions, MathExpression comparer, int i) {
        return TryGetEquality(out bool equals, comparer, expressions.ElementAt(i)) && !equals;
    }

    private bool TypesAreDisjoint(IEnumerable<MathExpression> expressions, MathExpression comparer, int i) {
        return !comparer.EvaluateType(this).Overlaps(expressions.ElementAt(i).EvaluateType(this));
    }

    private bool TryGetEquality(out bool result, params MathExpression[] expressions) {

        bool containsEquality = this.expressions
            .OfType<EqualsExpression>()
            .Any(e => expressions.All(e2 => e.Children.Contains(e2)));

        if(containsEquality) {
            result = true;
            return true;
        }

        bool containsNotEquality = this.expressions
            .OfType<NotExpression>()
            .Select(e => e.Child)
            .OfType<EqualsExpression>()
            .Any(e => expressions.All(e2 => e.Children.Contains(e2)));

        if(containsNotEquality) {
            result = false;
            return true;
        }

        result = default;
        return false;
    }

    public MathType DetermineTypeOf(MathExpression expression) {
        return GetTypeOf(expression).IntersectWith(expression.EvaluateType());
    }

    internal MathType GetTypeOf(MathExpression expression) {
        IEnumerable<MathType> types = expressions
            .OfType<InExpression>()
            .Where(i => i.LeftChild.Equals(expression) && i.RightChild is TypeExpression)
            .Select(i => i.RightChild is TypeExpression typeExpression ? typeExpression.Value : MathType.Universe);

        MathType type = MathType.Universe;
        foreach(MathType t in types) {
            type = type.IntersectWith(t);
        }

        return type;
    }

    IEnumerator IEnumerable.GetEnumerator() => expressions.GetEnumerator();

}
