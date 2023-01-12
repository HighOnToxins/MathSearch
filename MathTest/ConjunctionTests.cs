using MathSearch;
using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;

namespace MathTest;

public class ConjunctionTests {

    [Test]
    public void AAndAIsA() {

        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
        };

        MathExpression determinant = new ConjunctionExpression(
            new VariableExpression("a"),
            new VariableExpression("a")
        );

        MathExpression expected = new VariableExpression("a");

        MathExpression result = system.Determine(determinant);

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
    public void ConjunctionSimplifiesToFalse() {

        MathExpression expression = new ConjunctionExpression(
            new BooleanExpression(false)
        );

        MathExpression result = expression.Simplify();

        Assert.That(result, Is.EqualTo(new BooleanExpression(false)));
    }

    [Test]
    public void ConjunctionDoesNotSimplify() {

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
    public void ConjunctionSimplifies() {
        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
        };

        MathExpression determinant = new DisjunctionExpression(
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
        );

        MathExpression expected = new VariableExpression("a");

        MathExpression result = system.Determine(determinant);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void DifferentlyOrderedConjunctionsAreEqual() {

        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
        };

        MathExpression conjunction1 = new ConjunctionExpression(new VariableExpression("a"), new VariableExpression("b"));
        MathExpression conjunction2 = new ConjunctionExpression(new VariableExpression("b"), new VariableExpression("a"));

        MathExpression determined1 = system.Determine(conjunction1);
        MathExpression determined2 = system.Determine(conjunction2);

        bool result1 = conjunction1.Equals(conjunction2);
        bool result2 = determined1.Equals(determined2);

        Assert.That(result1, Is.EqualTo(false));
        Assert.That(result2, Is.EqualTo(true));
    }

}