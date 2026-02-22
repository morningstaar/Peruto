# README - Foot Quiz VR

## Description
Foot Quiz VR est une application de simulation de gardien de but en réalité virtuelle développée sous Unity. Le projet combine des mécaniques de sport et un système de quiz interactif. Le joueur doit arrêter des tirs déclenchés par une mascotte animée et répondre à des questions sur le football pour progresser.

## Caractéristiques Techniques
- Moteur : Unity 2022.3.x
- Framework VR : XR Interaction Toolkit
- Mode de rendu : World Space UI pour l'immersion
- Langage : C#

## Installation et Configuration (Meta Link)
Pour exécuter le projet via Meta Quest Link (Câble ou Air Link) :

1. Plateforme de Build : Dans Build Settings, sélectionnez la plateforme Windows, Mac, Linux.
2. Configuration XR : Dans Project Settings > XR Plug-in Management, cochez l'option Oculus dans l'onglet Windows.
3. Sources Inconnues : Dans l'application Meta Quest Link sur PC, autorisez les sources inconnues dans les paramètres généraux.

## Fonctionnement du GameManager
Le script GameManager.cs est le cœur du projet et gère les éléments suivants :

- Gestion des états : Transition entre la phase de tir, la phase de quiz et le menu de pause.
- Recentrer du joueur : Au démarrage, le script identifie l'objet XR Origin (XR Rig) et force sa position au centre des buts selon les coordonnées définies dans l'éditeur.
- Interface Utilisateur : Création dynamique par code des Canvas de score, de règles et de quiz.
- Interaction : Gestion de l'activation des lasers (XR Ray Interactors) uniquement lors des phases de menu.

## Commandes
- Clavier : Touche P ou Echap pour mettre le jeu en pause.
- Manette Meta : Bouton Menu (Manette Gauche) pour la pause.
- Interaction : Utilisation des gâchettes pour valider les réponses du quiz via les rayons.

## Notes de Calibration
Si la position verticale est incorrecte (joueur trop haut) :
- Vérifiez que le composant XR Origin est réglé sur Tracking Origin Mode : Floor.
- Assurez-vous que l'objet XR Origin (XR Rig) est positionné à Y = 0 dans la scène Unity.