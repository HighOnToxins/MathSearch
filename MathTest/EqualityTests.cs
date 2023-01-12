﻿
using MathSearch.Expression;
using MathSearch.Expressions.Basics;
using MathSearch.Expressions.Propersitions;
using MathSearch.Expressions.Sets;
using MathSearch.Expressions;
using MathSearch;

namespace MathTest; 

public sealed class EqualityTests {

    [Test]
    public void DetermineVariablesAreEqualToThemselves() {
        MathExpression determinant = new EqualsExpression(new VariableExpression("a"), new VariableExpression("a"));
        MathExpression expected = new BooleanExpression(true);

        MathExpression result = determinant.Simplify();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void DeterminedThatUndefinedVariablesAreUnsimplifiable() {
        MathExpression determinant = new EqualsExpression(new VariableExpression("a"), new VariableExpression("b"));
        MathExpression expected = new EqualsExpression(new VariableExpression("a"), new VariableExpression("b"));

        MathExpression result = determinant.Simplify();

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void DetermineEqualityBasedOnContext() {

        MathSystem system = new(){
            new InExpression(new VariableExpression("a"), MathType.Boolean),
            new InExpression(new VariableExpression("b"), MathType.Boolean),
            new InExpression(new VariableExpression("c"), MathType.Boolean),
        };

        MathExpression determinant = new ConjunctionExpression(
            new EqualsExpression(new VariableExpression("a"), new VariableExpression("b")),
            new EqualsExpression(new VariableExpression("c"), new VariableExpression("b")),
            new VariableExpression("a"),
            new VariableExpression("b"),
            new VariableExpression("c")
        );

        MathExpression expected = new ConjunctionExpression(
            new VariableExpression("a"),
            new VariableExpression("b"),
            new VariableExpression("c")
        );

        MathExpression result = system.Determine(determinant);

        Assert.That(result, Is.EqualTo(expected));
    }

}