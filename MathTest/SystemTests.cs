﻿
using MathSearch;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions;
using MathSearch.Expression;
using MathSearch.Expressions.Sets;
using MathSearch.Expressions.Basics;

namespace MathTest;

public sealed class SystemTests {

    [Test]
    public void SystemUsesSimplestEquation() {

        MathSystem system = new() {
            new EqualsExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new VariableExpression("b")
                ),
                new BooleanExpression(true)
            ),
            new EqualsExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new NotExpression(new VariableExpression("b"))
                ),
                new BooleanExpression(false)
            ),
        };

        HashSet<MathExpression> expected = new() {
            new EqualsExpression(
                new ConjunctionExpression(
                    new VariableExpression("a"),
                    new VariableExpression("b")
                ),
                new BooleanExpression(true)
            ),
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expected));

    }

    [Test]
    public void SystemExtractsConjunction() {

        MathSystem system = new() {
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new VariableExpression("c"),
            new ConjunctionExpression(
                new VariableExpression("a"),
                new VariableExpression("b")
            )
        };

        HashSet<MathExpression> expect = new() {
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new VariableExpression("c"),
            new VariableExpression("a"),
            new VariableExpression("b")
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expect));

    }

    [Test]
    public void SystemRemovesTrue() {

        MathSystem system = new() {
            new BooleanExpression(true)
        };

        HashSet<MathExpression> expect = new() {
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expect));

    }

    [Test]
    public void SystemRemovesEmptyExpression() {

        MathSystem system = new() {
            new EmptyExpression()
        };

        HashSet<MathExpression> expect = new() {
        };

        system.Simplify();

        Assert.That(system.Expressions, Is.EqualTo(expect));

    }

    [Test]
    public void CheckInconsistencyWorks() {

        MathSystem system = new() {
            new VariableExpression("a"),
            new NotExpression(new VariableExpression("a"))
        };

        bool expect = true;

        system.Simplify();

        Assert.That(system.IsInconsistent, Is.EqualTo(expect));

    }

    [Test]
    public void ADoesNotDisapear() {

        MathSystem system = new() {
            new VariableExpression("a"),
        };

        MathExpression expect = new VariableExpression("a");

        system.Simplify();

        Assert.That(system.Expressions.First(), Is.EqualTo(expect));

    }

    [Test]
    public void SystemRemoveAnyTrue() {

        MathSystem system = new() {
            new BooleanExpression(true),
        };

        bool expect = false;

        system.Simplify();

        Assert.That(system.Expressions.Any(), Is.EqualTo(expect));
    }

}