# Modele Black-Sholes & les grecs 🇪🇺 

Le modèle de Black-Scholes est une formule mathématique utilisée pour évaluer le prix des options européennes. Les "Grecs" sont des mesures de sensibilité du prix de l'option par rapport à différents facteurs. Voici comment calculer les principaux Grecs dans le cadre du modèle de Black-Scholes :

### 1. **Delta (Δ)**
Le Delta mesure la sensibilité du prix de l'option par rapport à une variation du prix de l'actif sous-jacent.

- **Pour une option d'achat (Call) :**
  \[
  \Delta_C = N(d_1)
  \]
 
- **Pour une option de vente (Put) :**
  \[
  \Delta_P = N(d_1) - 1
  \]

Où \( N(d_1) \) est la fonction de répartition de la loi normale centrée réduite évaluée en \( d_1 \).

### 2. **Gamma (Γ)**
Le Gamma mesure la sensibilité du Delta par rapport à une variation du prix de l'actif sous-jacent. Il est le même pour les options d'achat et de vente.

\[
\Gamma = \frac{N'(d_1)}{S \sigma \sqrt{T}}
\]

Où \( N'(d_1) \) est la densité de probabilité de la loi normale centrée réduite évaluée en \( d_1 \), \( S \) est le prix de l'actif sous-jacent, \( \sigma \) est la volatilité, et \( T \) est le temps jusqu'à l'expiration.

### 3. **Theta (Θ)**
Le Theta mesure la sensibilité du prix de l'option par rapport au temps qui passe.

- **Pour une option d'achat (Call) :**
  \[
  \Theta_C = -\frac{S N'(d_1) \sigma}{2 \sqrt{T}} - r K e^{-rT} N(d_2)
  \]
 
- **Pour une option de vente (Put) :**
  \[
  \Theta_P = -\frac{S N'(d_1) \sigma}{2 \sqrt{T}} + r K e^{-rT} N(-d_2)
  \]

Où \( r \) est le taux d'intérêt sans risque et \( K \) est le prix d'exercice.

### 4. **Vega (ν)**
Le Vega mesure la sensibilité du prix de l'option par rapport à une variation de la volatilité de l'actif sous-jacent. Il est le même pour les options d'achat et de vente.

\[
\nu = S \sqrt{T} N'(d_1)
\]

### 5. **Rho (ρ)**
Le Rho mesure la sensibilité du prix de l'option par rapport à une variation du taux d'intérêt sans risque.

- **Pour une option d'achat (Call) :**
  \[
  \rho_C = K T e^{-rT} N(d_2)
  \]
 
- **Pour une option de vente (Put) :**
  \[
  \rho_P = -K T e^{-rT} N(-d_2)
  \]

### Calcul de \( d_1 \) et \( d_2 \)
Les termes \( d_1 \) et \( d_2 \) sont calculés comme suit :

\[
d_1 = \frac{\ln\left(\frac{S}{K}\right) + \left(r + \frac{\sigma^2}{2}\right)T}{\sigma \sqrt{T}}
\]

\[
d_2 = d_1 - \sigma \sqrt{T}
\]

### Résumé
- **Delta (Δ)** : Sensibilité par rapport au prix de l'actif sous-jacent.
- **Gamma (Γ)** : Sensibilité du Delta par rapport au prix de l'actif sous-jacent.
- **Theta (Θ)** : Sensibilité par rapport au temps qui passe.
- **Vega (ν)** : Sensibilité par rapport à la volatilité.
- **Rho (ρ)** : Sensibilité par rapport au taux d'intérêt.

Ces mesures sont essentielles pour la gestion des risques et la compréhension du comportement des options dans différentes conditions de marché.