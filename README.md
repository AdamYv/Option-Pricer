* * *

# 📈 Option Pricer

## Goal

A desktop option pricer (C# / WPF) for European, American and Asian options, with their Greeks.

## Models

| Option | Pricing method | Greeks |
|---|---|---|
| European | Black-Scholes closed form | Closed-form formulas |
| American | Cox-Ross-Rubinstein binomial tree with continuous dividend yield (number of steps set in the UI) | Delta, Gamma and Theta read from the first nodes of the tree (Hull); Vega and Rho by central finite differences |
| Asian (arithmetic average) | Parallel Monte Carlo simulation of geometric Brownian motion (`Parallel.For`, 252 steps per year), spot and volatility from the Alpha Vantage API | Central finite differences with common random numbers |

Greeks returned: $\Delta$, $\Gamma$, $\Theta$ (per year), $\nu$ (Vega, for a change of 1 = 100 % in volatility) and $\rho$ (for a change of 1 = 100 % in the rate).

For the Asian option, every simulated path has its own random generator derived from a seed, so a price is reproducible and the bumped prices used for the Greeks reuse exactly the same paths. This keeps the finite differences stable.

## Negative rates

The risk-free rate can be zero or negative in the three engines. With a negative rate, the American call can be worth exercising early even without dividends. The binomial tree rejects parameters that would give a probability outside [0, 1] (increase the number of steps or the volatility).

## Alpha Vantage API key

The Asian option tab downloads prices from [Alpha Vantage](https://www.alphavantage.co/support/#api-key) (free key). The key is not stored in the code: set the `ALPHAVANTAGE_API_KEY` environment variable, or put the key alone in `MyOptionPricer/api_key.env` (ignored by git, copied next to the executable at build time).

## Tests

The `MyOptionPricer.Tests` project (xUnit) compiles the pricing engines without the WPF UI, so the tests run on any OS:

```
dotnet test MyOptionPricer.Tests
```

They check, among other things:
- Black-Scholes prices against the example from Hull's book (call 4.76, put 0.81) and put-call parity;
- closed-form European Greeks against finite differences;
- American call without dividend = European call, and American put = 6.09 for S = K = 100, r = 5 %, σ = 20 %, T = 1;
- signs and ranges of the American and Asian Greeks, and reproducibility of the Monte Carlo price;
- negative rates: put-call parity (European, Asian), no early exercise of the American put, early exercise of the American call, put-call symmetry.

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
