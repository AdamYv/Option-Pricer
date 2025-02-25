# Methode Monte Carlo 

La méthode de Monte Carlo est une approche numérique pour évaluer le prix des options en simulant des trajectoires possibles du prix de l'actif sous-jacent.

### **Principe de Monte Carlo**

1. Simuler un grand nombre de trajectoires du prix de l'actif sous-jacent en utilisant un modèle de type :
   $$
   S_T = S_0 \exp\left(\left(r - \frac{\sigma^2}{2}\right)T + \sigma \sqrt{T} Z\right)
   $$
   Où \( Z \) est une variable aléatoire suivant une loi normale centrée réduite.

2. Calculer le payoff de l'option pour chaque trajectoire :
   - Pour un call : $\max(S_T - K, 0)$
   - Pour un put :  $\max(K - S_T, 0)$

3. Moyenner les payoffs et actualiser à la valeur présente :
   $$
   C \approx e^{-rT} \cdot \frac{1}{N} \sum_{i=1}^N \max(S_T^{(i)} - K, 0)
   $$
   $$
   P \approx e^{-rT} \cdot \frac{1}{N} \sum_{i=1}^N \max(K - S_T^{(i)}, 0)
   $$
   Où $N$  est le nombre de simulations.