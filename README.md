# League of Smurfs

A small Windows account manager for **League of Legends** and **Valorant**, built **for fun**.

> **Not recommended for real use.**  
> This is an experimental / learning project. Do not use it in production, do not trust it with important accounts, and do not expect serious support. You use it **at your own risk** (Riot client automation, local password storage, etc.).

## What it does

League of Smurfs is a WinForms app that lets you:

- **Manage multiple Riot accounts** (add, edit, delete)
- **Store credentials locally** (AES-encrypted) under `%AppData%\.los\`
- Switch between **League (L)** and **Valorant (V)** modes
- **Launch** League or Valorant through the Riot Client with the matching `--launch-product` argument
- Attempt **automatic login** on the selected account (keyboard simulation)

### League mode

- Shows summoner level and **Solo / Flex** ranks via the **Riot API**
- Purple theme

### Valorant mode

- Shows **Agent** info with **current RR**, **peak rank**, and Valorant rank colors
- Blue theme (UI surfaces + tinted icons / background)
- Rank data via **[HenrikDev](https://api.henrikdev.xyz/dashboard/)** (Riot’s public API does not expose personal competitive RR)

Without a valid API key for the current mode, you can still add / edit / launch accounts, but **live profile / rank refresh is disabled**.

## API keys

Each mode has its **own encrypted key file**:

| Mode | Key source | File |
|------|------------|------|
| League | [developer.riotgames.com](https://developer.riotgames.com/) | `%AppData%\.los\api.key` |
| Valorant | [HenrikDev dashboard](https://api.henrikdev.xyz/dashboard/) | `%AppData%\.los\valorant.key` |

1. Paste the key for the active mode in the top bar field (shown as `*`)
2. Click the refresh button next to it (green dot = OK)
3. Use the eye button to reveal / hide the key

Notes:

- Riot **development** keys usually expire after **24 hours**
- Keys are stored encrypted on disk; switching L/V does not re-validate a key that was already checked
- Emptying the field clears that mode’s saved key

## Requirements

- Windows
- .NET Framework 4.7.2
- Visual Studio (to build)
- Riot Client installed (for launch / auto-login)

## Build

Open the `LeagueOfSmurfs` project in Visual Studio, restore NuGet packages if needed, then build **Debug** or **Release**.

Release output:

```text
LeagueOfSmurfs\bin\Release\LeagueOfSmurfs.exe
```

## Warnings

- Auto-login sends keystrokes to the Riot Client — do not use the keyboard / mouse during the sequence
- Accounts and secrets stay on your machine; this is **not** a password vault
- Follow Riot Games’ Terms of Service; this repo is **not** affiliated with Riot

## Antivirus / false positives

Windows Defender (and other AVs) may **block or delete** `LeagueOfSmurfs.exe`.

That is **common** and usually a **false positive**, because the app does things heuristics often associate with malware:

- keyboard simulation (`SendKeys`)
- focusing another window
- clipboard access
- closing Riot / League / Valorant processes
- **unsigned** executable downloaded from the internet

The source is public in this repository — prefer building from Visual Studio instead of downloading a prebuilt `.exe`.

### Release `v1.0.0` zip checksum

SHA256 of `LeagueOfSmurfs-v1.0.0-win-x86.zip`:

```text
22472FE0E627BDABE0331D14D84F9E866C849AC3F25F86ACF1316FF632BEC454
```

PowerShell:

```powershell
Get-FileHash .\LeagueOfSmurfs-v1.0.0-win-x86.zip -Algorithm SHA256
```

### If Defender quarantines the file

1. Verify the hash above (for that release zip)
2. Restore from Windows Security quarantine if needed
3. Or build from source (recommended)

Do not disable Defender globally just for this.

## License / vibe

Personal project for tinkering.  
**Not recommended for daily serious use.**
