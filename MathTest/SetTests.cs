
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expressions.Sets;
using MathSearch.Expressions.Basics;

namespace MathTest;

internal sealed class SetTests {

    [Test]
    public void CheckDifferentlyOrderedSetsAreEqual() {

        MathExpression set1 = new SetExpression(new VariableExpression("a"), new VariableExpression("b"));
        MathExpression set2 = new SetExpression(new VariableExpression("b"), new VariableExpression("a"));

        MathExpression determined1 = set1.Simplify();
        MathExpression determined2 = set2.Simplify();

        bool result1 = set1.Equals(set2);
        bool result2 = determined1.Equals(determined2);

        Assert.That(result1, Is.EqualTo(false));
        Assert.That(result2, Is.EqualTo(true));
    }

    [Test]
    public void CheckIfInSetBecomesEqualities() {

        MathExpression expression = new InExpression(
            new VariableExpression("a"), 
            new SetExpression(
                new VariableExpression("b"),
                new VariableExpression("c"),
                new VariableExpression("d")
            )
        );

        MathExpression expected = new ConjunctionExpression(
            new EqualsExpression(new VariableExpression("a"), new VariableExpression("b")),
            new EqualsExpression(new VariableExpression("a"), new VariableExpression("c")),
            new EqualsExpression(new VariableExpression("a"), new VariableExpression("d"))
        );

        MathExpression result = expression.Simplify();

        Assert.That(result, Is.EqualTo(expected));

    }

}