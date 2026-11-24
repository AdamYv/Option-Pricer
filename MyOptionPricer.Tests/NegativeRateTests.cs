using MyOptionPricer;
using Xunit;

/// <summary>Taux sans risque négatifs sur les trois moteurs de pricing.</summary>
public class NegativeRateTests
{
    [Theory]
    [InlineData(-0.005)]
    [InlineData(-0.02)]
    public void EuropeanPutCallParityHolds(double r)
    {
        var opt = new EuropOpt(100, 95, r, 0.25, 2);
        double parity = 100 - 95 * Math.Exp(-r * 2);
        Assert.Equal(parity, opt.CallPrice() - opt.PutPrice(), 10);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EuropeanGreeksMatchFiniteDifferences(bool isCall)
    {
        const double S = 100, K = 105, r = -0.01, sigma = 0.3, T = 0.75;
        var closed = new Greeks(S, K, r, sigma, T).ComputeGreeks(isCall);
        var fd = FiniteDifferenceGreeks.Compute((s, v, rate, t) =>
        {
            var o = new EuropOpt(s, K, rate, v, t);
            return isCall ? o.CallPrice() : o.PutPrice();
        }, S, sigma, r, T);

        Assert.Equal(closed.Delta, fd.Delta, 3);
        Assert.Equal(closed.Gamma, fd.Gamma, 3);
        Assert.Equal(closed.Vega, fd.Vega, 1);
        Assert.Equal(closed.Rho, fd.Rho, 1);
        Assert.Equal(closed.Theta, fd.Theta, 1);
    }

    [Fact]
    public void AmericanPutWithoutDividendMatchesBlackScholes()
    {
        // Avec r < 0, recevoir le strike plus tôt n'a pas d'intérêt : pas d'exercice anticipé du put
        double eu = new EuropOpt(100, 100, -0.02, 0.2, 1).PutPrice();
        double am = new AmericanOption(100, 100, 1, 0.2, -0.02, 1000, 0).PutPrice();
        Assert.InRange(am, eu - 0.005, eu + 0.005); // erreur de discrétisation de l'arbre (environ 0,002)
    }

    [Fact]
    public void AmericanCallIsWorthMoreThanEuropeanCall()
    {
        // Avec r < 0, l'exercice anticipé du call devient optimal même sans dividende
        double eu = new EuropOpt(100, 100, -0.05, 0.2, 1).CallPrice();
        double am = new AmericanOption(100, 100, 1, 0.2, -0.05, 1000, 0).CallPrice();
        Assert.True(am > eu + 0.01, $"américaine {am}, européenne {eu}");
    }

    [Fact]
    public void AmericanPutCallSymmetryHolds()
    {
        // Symétrie de McDonald-Schroder : C(S, K, r, q) = P(K, S, q, r)
        double call = new AmericanOption(100, 95, 1, 0.2, -0.03, 1000, 0).CallPrice();
        double put = new AmericanOption(95, 100, 1, 0.2, 0, 1000, -0.03).PutPrice();
        Assert.Equal(call, put, 6);
    }

    [Fact]
    public void UnstableTreeIsRejected()
    {
        // r - q très négatif devant la volatilité, avec une seule étape
        var opt = new AmericanOption(100, 100, 1, 0.05, -0.20, 1, 0);
        Assert.Throws<ArgumentException>(() => opt.PutPrice());
    }

    [Fact]
    public void AsianPutCallParityHolds()
    {
        const double S = 100, K = 100, r = -0.02, T = 1;
        const int n = 252;
        var opt = new AsianOpt(S, K, r, 0.2, T, n, 100000, seed: 42);

        // Espérance de la moyenne arithmétique : S/n * somme des exp(r t_i)
        double mean = 0;
        for (int i = 1; i <= n; i++) mean += S * Math.Exp(r * T * i / n) / n;
        double parity = Math.Exp(-r * T) * (mean - K);

        Assert.InRange(opt.CallPrice() - opt.PutPrice(), parity - 0.15, parity + 0.15);
    }

    [Fact]
    public void AsianGreeksHaveExpectedSigns()
    {
        var g = new AsianOpt(100, 100, -0.02, 0.2, 1, 252, 20000, seed: 42).ComputeGreeks(true);
        Assert.InRange(g.Delta, 0, 1);
        Assert.True(g.Gamma > 0);
        Assert.True(g.Vega > 0);
        Assert.True(g.Rho > 0);
    }
}
