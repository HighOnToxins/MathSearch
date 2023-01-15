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
                .Select(e => new InExpression(e.Clone(), e.DetermineTypeBasedOn(GetContextForChild(i, expressions)))));
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
        return expression.Simplify(this);
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
            MathExpression comparer2 = expressions.ElementAt(i);

            //Determine not equal by type or context
            if(TypesAreDisjoint(comparer, comparer2) || AreNotEqual(comparer, comparer2)) {
                result = false;
                return true;
            }

            //determine simplified expressions
            if(comparer.IsSimple() && comparer2.IsSimple()) {
                result = comparer.Equals(expressions.ElementAt(i));
                return true;
            }

            //if the expressions are not equal, it can not be determined.
            if(!comparer.Equals(comparer2)) {
                result = default;
                return false;
            }
        }

        result = true;
        return true;
    }

    private bool AreNotEqual(MathExpression expression1, MathExpression expression2) {
        return TryGetEquality(out bool equals, expression1, expression2) && !equals;
    }

    private bool TypesAreDisjoint(MathExpression expression1, MathExpression expression2) {
        return !DetermineType(expression1).Overlaps(DetermineType(expression2));
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

    public MathType DetermineType(MathExpression expression) => expression.DetermineTypeBasedOn(this);

    internal MathType GetTypeOf(MathExpression expression) {
        IEnumerable<MathType> types = expressions
            .OfType<InExpression>()
            .Where(i => i.LeftChild.Equals(expression) && i.RightChild is TypeExpression)
            .Select(i => i.RightChild is TypeExpression typeExpression ? typeExpression.Value : MathType.Universe);

        IEnumerable<MathType> types2 = GetEqualitiesOf(expression).Select(e => e.DetermineType());

        MathType type3 = expressions.Contains(expression) ? MathType.Boolean : MathType.Universe;

        MathType type = MathType.Universe;
        foreach(MathType t in types.Concat(types2).Concat(new MathType[] { type3 })) {
            type = type.IntersectWith(t);
        }

        return type;
    }

    IEnumerator IEnumerable.GetEnumerator() => expressions.GetEnumerator();

}
