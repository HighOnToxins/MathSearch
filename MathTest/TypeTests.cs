
using MathSearch.Expression;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expressions.Sets;
using MathSearch;
using MathSearch.Expressions.Basics;

namespace MathTest; 

public sealed class TypeTests {

    [Test]
    public void BooleanTypeSimplifiesToSet() {

        MathExpression determinant = new TypeExpression(MathType.Boolean);

        MathExpression expected = new SetExpression(new BooleanExpression(false), new BooleanExpression(true));

        MathExpression result = determinant.Simplify();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void TypeOfConjunctionIsBoolean() {

        MathExpression expression = new ConjunctionExpression(
            new DisjunctionExpression(
                new BooleanExpression(false),
                new BooleanExpression(true)
            ),
            new BooleanExpression(true)
        );

        MathType type = expression.EvaluateType();

        Assert.That(type, Is.EqualTo(MathType.Boolean));
    }

    [Test]
    public void TypeOfVariableIsUniverse() {

        MathExpression expression = new ConjunctionExpression(
            new ConjunctionExpression(
                new VariableExpression("a"),
                new BooleanExpression(false)
            ),
            new BooleanExpression(true)
        );

        MathType type = expression.EvaluateType();

        Assert.That(type, Is.EqualTo(MathType.Universe));
    }

    [Test]
    public void TypeImpliesEquality() {

        MathExpression expression1 = new SetExpression(new VariableExpression("a"));
        MathExpression expression2 = new ConjunctionExpression(new VariableExpression("a"));

        MathSystem system = new();

        if(system.TryDetermineEquality(out bool result, expression1, expression2)) {
            Assert.That(result, Is.False);
        } else {
            Assert.Fail();
        }

    }

    [Test]
    public void EqualityImpliesType() {

        MathSystem system = new() {
            new EqualsExpression(
                new VariableExpression("a"),
                new BooleanExpression(true)
            )
        };

        MathType expected = MathType.Boolean;

        MathType result = system.DetermineTypeOf(new VariableExpression("a"));

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void TopLevelOfSystemImpliesType() {

        MathSystem system = new() {
            new DisjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            ),
        };

        MathType expected = MathType.Boolean;

        MathType result = system.DetermineTypeOf(new VariableExpression("a"));

        Assert.That(result, Is.EqualTo(expected));

    }

}