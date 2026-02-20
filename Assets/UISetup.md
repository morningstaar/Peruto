# UI Setup (Lioruto)

## Panneaux nécessaires
1. **Panel_Regles_1**
   - Titre + texte règles
   - Bouton **JOUER / CONTINUER** (appelle `GameManager.ContinuerRegles()`)
2. **Panel_Regles_2**
   - Deux encarts d’explication
   - Bouton **PRÊT** (appelle `GameManager.ContinuerRegles()`)
3. **HUD**
   - Texte score (lié à `TirMascotte.texteScore`)
   - Texte tirs (lié à `GameManager.texteTirs`)
4. **Panel_Question**
   - Texte **QUESTION X** (lié à `GameManager.texteNumeroQuestion`)
   - Texte question (lié à `GameManager.texteQuestion`)
   - 4 boutons réponses (liés à `AnswerButton` + `GameManager`)
   - (Option) bouton **CONTINUER** si `confirmerAuClic=false`
5. **Panel_Pause**
   - Titre "Pause" + texte
   - Boutons **Quitter** (`GameManager.Quitter()`), **Recommencer** (option), **Annuler** (`GameManager.Reprendre()`)
6. **Panel_Fin**
   - Texte résultat (lié à `GameManager.texteFin`)
   - Boutons **Rejouer** (option) / **Quitter**

## Liaison rapide
- `GameManager` sur un objet vide.
- Références à assigner:
  - `panelRegles1`, `panelRegles2`, `panelQuestion`, `panelPause`, `panelFin`
  - `texteQuestion`, `texteNumeroQuestion`, `textesReponses[4]`, `fondsReponses[4]`
  - `texteTirs`, `texteFin`
  - `tirMascotte`, `mascotteAnimator`, `ambiancePublic`

## Conseils UX (selon maquettes)
- **Canvas World Space** devant le joueur.
- Fond bleu translucide + bordures lumineuses.
- Boutons verts pour actions principales.
- Texte blanc / cyan.

## Notes
- Le flow est déjà géré dans `GameManager`.
- Les questions sont à remplir dans la liste `questions` de `GameManager`.
