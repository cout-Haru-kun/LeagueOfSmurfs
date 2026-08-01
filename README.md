# League of Smurfs

Petit gestionnaire de comptes League of Legends fait **pour le fun**.

> **Déconseillé à l’utilisation.**  
> Ce projet est expérimental / pédagogique. Ne l’utilise pas en production, ne lui fais pas confiance pour stocker des comptes importants, et n’attends pas un support sérieux. Tu l’utilises **à tes risques et périls** (automatisation du client Riot, stockage local de mots de passe, etc.).

## À quoi ça sert ?

League of Smurfs est une appli Windows (WinForms) qui permet de :

- **Gérer plusieurs comptes** LoL (ajout, édition, suppression)
- **Sauvegarder localement** les identifiants (chiffrés) dans `%AppData%\.los\`
- **Voir le niveau / les rangs** (solo & flex) via l’API Riot, si une clé est fournie
- **Lancer le client Riot** et tenter une connexion automatique au compte sélectionné (simulation clavier)

Sans clé API, tu peux quand même ajouter / éditer / lancer des comptes, mais **sans mise à jour live** des infos invocateur / ranked.

## Clé API Riot (requis pour le mode complet)

Pour que l’app fonctionne **entièrement** (vérification Riot ID, niveau, rangs, refresh) :

1. Crée / récupère une clé sur [developer.riotgames.com](https://developer.riotgames.com/)
2. Colle-la dans le champ prévu dans le menu
3. Clique sur le bouton de refresh de la clé (pastille verte = OK)

Notes :

- Une clé de développement Riot expire en général au bout de **24 h**
- Sans clé valide, les requêtes Riot sont désactivées
- La clé est stockée localement dans `%AppData%\.los\api.key`

## Fonctionnalités liées à l’API

Avec une clé valide :

- Recherche d’un compte via **Riot ID** (`nom#tag`)
- Récupération du **niveau** et des **ranks** (solo / flex)
- Refresh des comptes déjà sauvegardés

## Prérequis

- Windows
- .NET Framework 4.7.2
- Visual Studio (pour compiler)
- Client Riot installé (pour le lancement / login auto)

## Compilation

Ouvre la solution / le projet `LeagueOfSmurfs` dans Visual Studio, restaure les packages NuGet si besoin, puis compile en **Debug** ou **Release**.

## Avertissements

- L’automatisation du login envoie des frappes clavier au client Riot : ne touche pas au clavier / à la souris pendant la séquence
- Les comptes et secrets restent sur ta machine ; ce n’est **pas** un coffre-fort
- Respecte les conditions d’utilisation de Riot Games ; ce repo n’est affilié à Riot d’aucune façon

## Antivirus / faux positifs

Windows Defender (et d’autres AV) peuvent **bloquer ou supprimer** `LeagueOfSmurfs.exe`.

C’est **attendu** et en général un **faux positif**, parce que l’app fait des choses que les heuristiques associent souvent à du malware :

- simulation clavier (`SendKeys`)
- prise de focus d’une autre fenêtre
- accès au presse-papiers
- fermeture de processus Riot/League
- exécutable **non signé**, téléchargé depuis internet

Le code source est public dans ce dépôt : tu peux compiler toi-même depuis Visual Studio pour éviter le téléchargement d’un `.exe` prébuild.

### Vérifier le zip de la release `v1.0.0`

SHA256 de `LeagueOfSmurfs-v1.0.0-win-x86.zip` :

```text
22472FE0E627BDABE0331D14D84F9E866C849AC3F25F86ACF1316FF632BEC454
```

PowerShell :

```powershell
Get-FileHash .\LeagueOfSmurfs-v1.0.0-win-x86.zip -Algorithm SHA256
```

### Si Defender bloque le fichier

1. Vérifie le hash ci-dessus
2. Restaure le fichier depuis la protection Windows / quarantaine si besoin
3. Ou compile depuis les sources (recommandé)

Ne désactive pas Defender globalement juste pour ça.

## Licence / esprit du projet

Projet perso, pour s’amuser et bricoler.  
**Pas recommandé pour un usage réel au quotidien.**
