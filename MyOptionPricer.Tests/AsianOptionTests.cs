using MyOptionPricer;
using Xunit;

public class AsianOptionTests
{
    private static AsianOpt Create(int sims = 20000, int seed = 42) =>
        new AsianOpt(100, 100, 0.05, 0.2, 1, 252, sims, seed: seed);

    [Fact]
    public void SameSeedGivesSamePrice()
    {
        Assert.Equal(Create().CallPrice(), Create().CallPrice());
    }

    [Fact]
    public void ArithmeticAsianCallIsCheaperThanEuropeanCall()
    {
        // La moyenne lisse la volatilité : l'asiatique vaut moins que l'européenne
        double asian = Create().CallPrice();
        double european = new EuropOpt(100, 100, 0.05, 0.2, 1).CallPrice();
        Assert.True(asian < european, $"asiatique {asian} >= européenne {european}");
        // Valeur de référence d'un call asiatique arithmétique (S=K=100, r=5 %, sigma=20 %, T=1,
        // 252 dates de constatation) : environ 5,78 avec 200 000 trajectoires
        Assert.InRange(asian, 5.6, 5.95);
    }

    [Fact]
    public void CallGreeksHaveExpectedSignsAndRanges()
    {
        var g = Create().ComputeGreeks(true);
        var eu = new Greeks(100, 100, 0.05, 0.2, 1).ComputeGreeks(true);

        Assert.InRange(g.Delta, 0, eu.Delta);   // moins sensible que l'européenne
        Assert.True(g.Gamma > 0);
        Assert.InRange(g.Vega, 0, eu.Vega);
        Assert.True(g.Rho > 0);
        Assert.True(g.Theta < 0);
    }

    [Fact]
    public void PutDeltaIsNegative()
    {
        var g = Create().ComputeGreeks(false);
        Assert.InRange(g.Delta, -1, 0);
        Assert.True(g.Rho < 0);
    }
}
