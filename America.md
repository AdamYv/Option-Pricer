# Modèles pour les Options Américaines 🇺🇸

Les options américaines peuvent être exercées à tout moment avant leur expiration, ce qui rend leur évaluation plus complexe que celle des options européennes. Voici les modèles et méthodes couramment utilisés pour évaluer les options américaines :

---

### **1. Modèle de Black-Scholes avec Ajustements**

#### **Contexte**
Le modèle de Black-Scholes est conçu pour les options européennes, mais il peut être adapté pour approximer le prix des options américaines dans certains cas.

#### **Ajustements**
- **Options Américaines sur Actions sans Dividende** : Pour les options d'achat (calls) sur des actions ne versant pas de dividendes, il n'est jamais optimal d'exercer l'option avant l'expiration. Ainsi, le prix d'une option américaine est identique à celui d'une option européenne, et Black-Scholes peut être utilisé directement.
- **Options Américaines sur Actions avec Dividende** : Pour les options de vente (puts) ou les options sur des actions versant des dividendes, des ajustements sont nécessaires. Des méthodes comme **Black's Approximation** ou **Roll-Geske-Whaley** peuvent être utilisées pour estimer le prix.

#### **Limites**
- Ces ajustements ne sont pas toujours précis, surtout pour des options avec des dividendes complexes ou des volatilités stochastiques.

---

### **2. Modèle Binomial (Arbre Binomial)**

#### **Contexte**
Le modèle binomial est une méthode numérique flexible et largement utilisée pour évaluer les options américaines. Il modélise l'évolution du prix de l'actif sous-jacent comme un arbre binomial, où à chaque étape, le prix peut monter ou descendre.

#### **Étapes**
1. **Construction de l'Arbre** :
   - Diviser la durée de vie de l'option en \( n \) intervalles de temps.
   - Calculer les mouvements possibles du prix de l'actif à chaque nœud :
     $$
     u = e^{\sigma \sqrt{\Delta t}}, \quad d = \frac{1}{u}
     $$
     Où \( \sigma \) est la volatilité et \( \Delta t = T/n \).

2. **Calcul des Probabilités** :
   - Calculer la probabilité risque-neutre \( p \) :
     $$
     p = \frac{e^{r \Delta t} - d}{u - d}
     $$

3. **Évaluation des Options** :
   - Commencer par les nœuds finaux (à l'expiration) et calculer le payoff.
   - Remonter l'arbre en actualisant les valeurs et en vérifiant à chaque nœud si l'exercice anticipé est optimal :
     $$
     V = \max(\text{Payoff}, e^{-r \Delta t} [p V_u + (1-p) V_d])
     $$

#### **Avantages**
- Prend en compte l'exercice anticipé.
- Flexible pour les options avec dividendes ou autres caractéristiques complexes.

#### **Exemple d'Application**
- Évaluer une option américaine sur une action avec dividendes en utilisant un arbre binomial à 100 étapes.

---

### **3. Modèle Trinomial (Arbre Trinomial)**

#### **Contexte**
Le modèle trinomial est une extension du modèle binomial où le prix de l'actif peut monter, descendre ou rester stable à chaque étape. Cela permet une meilleure précision et une convergence plus rapide vers le prix réel.

#### **Étapes**
4. **Construction de l'Arbre** :
   - Définir trois mouvements possibles : \( u \), \( d \), et \( m \) (stable).
   - Calculer les probabilités risque-neutres pour chaque mouvement.

5. **Évaluation des Options** :
   - Similaire au modèle binomial, mais avec un nœud supplémentaire à chaque étape.

#### **Avantages**
- Plus précis et converge plus rapidement que le modèle binomial.
- Adapté aux options américaines avec des caractéristiques complexes.

---

### **4. Méthode de Monte Carlo avec Exercice Anticipé**

#### **Contexte**
La méthode de Monte Carlo est généralement utilisée pour les options européennes, mais elle peut être adaptée pour les options américaines en utilisant des techniques comme **Longstaff-Schwartz** (Least Squares Monte Carlo, LSM).

#### **Étapes**
6. **Simuler des Trajectoires** :
   - Simuler \( N \) trajectoires du prix de l'actif sous-jacent.

7. **Évaluer l'Exercice Anticipé** :
   - À chaque étape, utiliser une régression (par exemple, des polynômes) pour estimer la valeur de continuation (valeur de ne pas exercer l'option).
   - Comparer la valeur de continuation avec le payoff immédiat pour décider d'exercer ou non.

8. **Calculer le Prix** :
   - Moyenner les payoffs actualisés sur toutes les trajectoires.

#### **Avantages**
- Adapté aux options américaines avec des caractéristiques complexes (exotiques, plusieurs sources d'incertitude).
- Flexible et puissant pour des modèles stochastiques.

#### **Exemple d'Application**
- Évaluer une option américaine sur un panier d'actions avec des dividendes stochastiques.

---

### **5. Modèles d'Équations aux Dérivées Partielles (EDP)**

#### **Contexte**
Les options américaines peuvent être évaluées en résolvant des équations aux dérivées partielles (EDP) avec des conditions aux limites spécifiques.

#### **Formulation**
- L'équation de Black-Scholes est modifiée pour inclure une condition de libre frontière (free boundary) qui représente le moment optimal d'exercice :
  $$
  \frac{\partial V}{\partial t} + \frac{1}{2} \sigma^2 S^2 \frac{\partial^2 V}{\partial S^2} + r S \frac{\partial V}{\partial S} - r V \leq 0
  $$
  Avec :
  $$
  V(S, t) \geq \max(S - K, 0) \quad \text{(pour un call)}.
  $$

#### **Méthodes de Résolution**
- **Différences Finies** : Discrétiser l'EDP et résoudre numériquement.
- **Éléments Finis** : Pour des problèmes plus complexes.

#### **Avantages**
- Précision élevée.
- Adapté aux options américaines avec des conditions complexes.

---

### **6. Modèles Analytiques Approchés**

#### **Contexte**
Certains modèles analytiques approchés, comme **Barone-Adesi et Whaley**, fournissent des formules fermées pour approximer le prix des options américaines.

#### **Formule de Barone-Adesi et Whaley**
- Pour les options américaines sur des actions avec dividendes, ce modèle fournit une approximation rapide et précise.

#### **Avantages**
- Rapide à calculer.
- Utile pour des estimations approximatives.

---

### **Conclusion**

- **Modèle Binomial/Trinomial** : Méthodes numériques flexibles et précises pour les options américaines.
- **Monte Carlo (LSM)** : Adapté aux options américaines exotiques ou complexes.
- **EDP** : Méthode précise mais plus complexe à implémenter.
- **Modèles Analytiques Approchés** : Utiles pour des estimations rapides.


Le choix du modèle dépend de la complexité de l'option, de la précision requise, et des ressources disponibles. Pour un pricer d'options américaines, le modèle binomial ou la méthode de Monte Carlo avec LSM sont souvent les choix les plus populaires. 🚀