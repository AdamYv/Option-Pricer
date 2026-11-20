using MyOptionPricer;
using Xunit;

public class AmericanOptionTests
{
    [Fact]
    public void CallWithoutDividendMatchesBlackScholes()
    {
        // Sans dividende, l'exercice anticipé d'un call n'est jamais optimal
        double eu = new EuropOpt(100, 100, 0.05, 0.2, 1).CallPrice();
        double am = new AmericanOption(100, 100, 1, 0.2, 0.05, 1000, 0).CallPrice();
        Assert.Equal(eu, am, 2);
    }

    [Fact]
    public void PutIsWorthAtLeastEuropeanPut()
    {
        double eu = new EuropOpt(100, 100, 0.05, 0.2, 1).PutPrice();
        double am = new AmericanOption(100, 100, 1, 0.2, 0.05, 1000, 0).PutPrice();
        Assert.True(am > eu, $"américaine {am} <= européenne {eu}");
    }

    [Fact]
    public void PutMatchesReferenceValue()
    {
        // Put américain S=K=100, r=5 %, sigma=20 %, T=1 : environ 6,09 (arbre CRR convergé)
        double am = new AmericanOption(100, 100, 1, 0.2, 0.05, 2000, 0).PutPrice();
        Assert.Equal(6.09, am, 2);
    }

    [Fact]
    public void DeepInTheMoneyPutIsAtLeastIntrinsicValue()
    {
        double am = new AmericanOption(50, 100, 1, 0.2, 0.05, 500, 0).PutPrice();
        Assert.True(am >= 50 - 1e-9);
    }
}

public class AmericanGreeksTests
{
    [Fact]
    public void CallGreeksWithoutDividendMatchEuropeanClosedForm()
    {
        var closed = new Greeks(100, 100, 0.05, 0.2, 1).ComputeGreeks(true);
        var tree = new AmericanOption(100, 100, 1, 0.2, 0.05, 1000, 0).ComputeGreeks(true);

        Assert.Equal(closed.Delta, tree.Delta, 2);
        Assert.Equal(closed.Gamma, tree.Gamma, 3);
        Assert.InRange(tree.Vega, closed.Vega - 0.5, closed.Vega + 0.5);
        Assert.InRange(tree.Rho, closed.Rho - 0.5, closed.Rho + 0.5);
        Assert.InRange(tree.Theta, closed.Theta - 0.5, closed.Theta + 0.5);
    }

    [Fact]
    public void PutGreeksHaveExpectedSigns()
    {
        var g = new AmericanOption(100, 100, 1, 0.2, 0.05, 1000, 0).ComputeGreeks(false);
        Assert.InRange(g.Delta, -1, 0);
        Assert.True(g.Gamma > 0);
        Assert.True(g.Vega > 0);
        Assert.True(g.Rho < 0);
        Assert.True(g.Theta < 0);
    }
}

public class AmericanGammaStabilityTests
{
    [Fact]
    public void GammaIsStableWithFewSteps()
    {
        // Avec 200 étapes, un choc de 1 % sur le spot déplace la grille et donne
        // une Gamma fausse ; la Gamma lue sur l'arbre reste proche de Black-Scholes.
        var closed = new Greeks(100, 100, 0.05, 0.2, 1).ComputeGreeks(true);
        var tree = new AmericanOption(100, 100, 1, 0.2, 0.05, 200, 0).ComputeGreeks(true);
        Assert.Equal(closed.Gamma, tree.Gamma, 3);
    }

    [Fact]
    public void SingleStepTreeFallsBackToFiniteDifferences()
    {
        var g = new AmericanOption(100, 100, 1, 0.2, 0.05, 1, 0).ComputeGreeks(false);
        Assert.False(double.IsNaN(g.Delta));
    }
}
