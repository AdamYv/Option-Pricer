* * *

# 📈 Option Pricer

## Goal

A desktop option pricer (C# / WPF) for European, American and Asian options, with their Greeks.

## Models

| Option | Pricing method | Greeks |
|---|---|---|
| European | Black-Scholes closed form | Closed-form formulas |
| American | Cox-Ross-Rubinstein binomial tree with continuous dividend yield (number of steps set in the UI) | Central finite differences on the tree |
| Asian (arithmetic average) | Parallel Monte Carlo simulation of geometric Brownian motion (`Parallel.For`, 252 steps per year), spot and volatility from the Alpha Vantage API | Central finite differences with common random numbers |

Greeks returned: $\Delta$, $\Gamma$, $\Theta$ (per year), $\nu$ (Vega, for a change of 1 = 100 % in volatility) and $\rho$ (for a change of 1 = 100 % in the rate).

For the Asian option, every simulated path has its own random generator derived from a seed, so a price is reproducible and the bumped prices used for the Greeks reuse exactly the same paths. This keeps the finite differences stable.

## Tests

The `MyOptionPricer.Tests` project (xUnit) compiles the pricing engines without the WPF UI, so the tests run on any OS:

```
dotnet test MyOptionPricer.Tests
```

They check, among other things:
- Black-Scholes prices against the example from Hull's book (call 4.76, put 0.81) and put-call parity;
- closed-form European Greeks against finite differences;
- American call without dividend = European call, and American put = 6.09 for S = K = 100, r = 5 %, σ = 20 %, T = 1;
- signs and ranges of the American and Asian Greeks, and reproducibility of the Monte Carlo price.

## Stack

- C#
- WPF
- .NET 8.0
- MathNet.Numerics (normal distribution)
- xUnit (tests)
- API: [Alpha Vantage](https://www.alphavantage.co)

## Resources

### Web & Personal Notes

- Example without API: [Site de F. Longin](https://www.longin.fr/Ressources_Outils/Pricers/Actions/Call_put_standards_dividendes_continus/options_standards_actions_calls_puts_pricer.php)
- Monte Carlo
- The Greeks
- American Option Model
- Black-Scholes Model

### Books

- [OPTIONS, FUTURES, AND OTHER DERIVATIVES - John C. Hull](http://lib.ysu.am/disciplines_bk/2b66030e0dd4c77b2bda437f6c1e5e66.pdf)
