using MyOptionPricer;
using Xunit;

public class EuropeanOptionTests
{
    // Exemple de Hull (Options, Futures and Other Derivatives) : S=42, K=40, r=10 %, sigma=20 %, T=0,5
    [Fact]
    public void PricesMatchHullExample()
    {
        var opt = new EuropOpt(42, 40, 0.10, 0.20, 0.5);
        Assert.Equal(4.76, opt.CallPrice(), 2);
        Assert.Equal(0.81, opt.PutPrice(), 2);
    }

    [Fact]
    public void PutCallParityHolds()
    {
        var opt = new EuropOpt(100, 95, 0.03, 0.25, 2);
        double parity = 100 - 95 * Math.Exp(-0.03 * 2);
        Assert.Equal(parity, opt.CallPrice() - opt.PutPrice(), 10);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ClosedFormGreeksMatchFiniteDifferences(bool isCall)
    {
        const double S = 100, K = 105, r = 0.04, sigma = 0.3, T = 0.75;
        var closed = new Greeks(S, K, r, sigma, T).ComputeGreeks(isCall);

        double Price(double s, double v, double rate, double t)
        {
            var o = new EuropOpt(s, K, rate, v, t);
            return isCall ? o.CallPrice() : o.PutPrice();
        }

        var fd = FiniteDifferenceGreeks.Compute(Price, S, sigma, r, T);

        Assert.Equal(closed.Delta, fd.Delta, 3);
        Assert.Equal(closed.Gamma, fd.Gamma, 3);
        Assert.Equal(closed.Vega, fd.Vega, 1);
        Assert.Equal(closed.Rho, fd.Rho, 1);
        Assert.Equal(closed.Theta, fd.Theta, 1);
    }

    [Fact]
    public void ThetaAndVegaAreNotSwapped()
    {
        // Pour un call ATM : Vega > 0 et Theta < 0
        var g = new Greeks(100, 100, 0.05, 0.2, 1).ComputeGreeks(true);
        Assert.True(g.Vega > 0);
        Assert.True(g.Theta < 0);
        Assert.Equal(37.52, g.Vega, 2);   // S * phi(d1) * sqrt(T)
        Assert.Equal(-6.41, g.Theta, 2);  // par année
    }
}
