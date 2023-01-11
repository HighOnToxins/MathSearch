using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;

namespace MathTest;

public class Tests {

    [Test]
    public void TypeOfConjunctionIsBoolean() {

        MathExpression expression = new ConjunctionExpression(
            new DisjunctionExpression(
                new BooleanExpression(false),
                new BooleanExpression(true)
            ),
            new BooleanExpression(true)
        );

        MathType type = expression.DetermineType();

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

        MathType type = expression.DetermineType();

        Assert.That(type, Is.EqualTo(MathType.Universe));
    }


    [Test]
    public void SimplificationOfConjunctionIsFalse() {

        MathExpression expression = new ConjunctionExpression(
            new BooleanExpression(false)
        );

        MathExpression result = expression.Simplify();

        Assert.That(result, Is.EqualTo(new BooleanExpression(false)));
    }

    [Test]
    public void CheckUnsimplifyableConjunctionWorks() {

        MathExpression expression = new ConjunctionExpression(
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            ),
            new ConjunctionExpression(
                new BooleanExpression(false),
                new BooleanExpression(true)
            ),
            new BooleanExpression(true)
        );

        MathExpression expected = new ConjunctionExpression(
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            ),
            new BooleanExpression(false),
            new BooleanExpression(true)
        );

        MathExpression result = expression.Simplify();


        Assert.That(result, Is.EqualTo(expected));
    }


    [Test]
    public void CheckSimplifyableConjunction() {
        MathSystem system = new(
            new DisjunctionExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new VariableExpression("b")
                ),
                new VariableExpression("a"),
                new ConjunctionExpression(
                    new BooleanExpression(false),
                    new BooleanExpression(true)
                ),
                new BooleanExpression(false)
            ),
            new InExpression(new VariableExpression("a"), new TypeExpression(MathType.Boolean)),
            new InExpression(new VariableExpression("b"), new TypeExpression(MathType.Boolean))
        );

        MathExpression expected = new VariableExpression("a");

        MathSystem result = (MathSystem) system.Simplify();

        Assert.That(result.Children.ElementAt(0), Is.EqualTo(expected));
    }
}