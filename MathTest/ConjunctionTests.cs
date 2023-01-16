using MathSearch;
using MathSearch.Expression;
using MathSearch.Expressions;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;

namespace MathTest;

public class ConjunctionTests {

    [Test]
    public void DisjunctionGroupsWithConjunction() {

        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new InExpression(new VariableExpression("c"), MathType.Boolean),
        };

        MathExpression determinant = new DisjunctionExpression(
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            ),
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("c")
            )
        );

        MathExpression expected = new ConjunctionExpression(
            new VariableExpression("a"),
            new DisjunctionExpression(
                new VariableExpression("b"),
                new VariableExpression("c")
            )
        );

        MathExpression result = system.Determine(determinant);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ConjunctionGroupsWithDisjunction() {

        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new InExpression(new VariableExpression("c"), MathType.Boolean),
        };

        MathExpression determinant = new ConjunctionExpression(
            new DisjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            ),
            new DisjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("c")
            )
        );

        MathExpression expected = new DisjunctionExpression(
            new VariableExpression("a"),
            new ConjunctionExpression(
                new VariableExpression("b"),
                new VariableExpression("c")
            )
        );

        MathExpression result = system.Determine(determinant);

        Assert.That(result, Is.EqualTo(expected));
    }

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