# League of Smurfs

A small Windows account manager for **League of Legends** and **Valorant**, built **for fun**.

> **Not recommended for real use.**  
> This is an experimental / learning project. Do not use it in production, do not trust it with important accounts, and do not expect serious support. You use it **at your own risk** (Riot client automation, local password storage, etc.).

## What’s new in 2.0

Version **2.0** rebuilds the UI as a **WPF** app (still .NET Framework 4.7.2) with a more compact, modern shell:

- Custom rounded window, themed title bar (**League Of Smurfs** / **Agents Of Smurfs**)
- Unified **L / V slider** (hover previews, click applies)
- Smooth account list scrolling and open / card animations
- Animated galaxy background (Valorant mode applies a blue tint to icons + background)
- Account editor with Riot-like name/tag validation, `#` focus jump, and password reveal
- Separate encrypted API keys per mode (`api.key` / `valorant.key`)

## What it does

League of Smurfs lets you:

- **Manage multiple Riot accounts** (add, edit, delete)
- **Store credentials locally** (AES-encrypted) under `%AppData%\.los\`
- Switch between **League (L)** and **Valorant (V)** modes
- **Launch** League or Valorant through the Riot Client with the matching `--launch-product` argument
- Attempt **automatic login** on the selected account (keyboard simulation)

### League mode

- Title: **League Of Smurfs**
- Shows summoner level and **Solo / Flex** ranks via the **Riot API**
- Purple theme

### Valorant mode

- Title: **Agents Of Smurfs**
- Shows **Agent** info with **current RR**, **peak rank**, and Valorant rank colors
- Cobalt / navy theme (UI surfaces + tinted icons / background)
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
- Visual Studio (to build from source)
- Riot Client installed (for launch / auto-login)

## Download

Prefer the latest GitHub Release asset when you want a prebuilt binary:

- [Releases](https://github.com/cout-Haru-kun/LeagueOfSmurfs/releases)

### Release `v2.0.0`

Download `LeagueOfSmurfs-v2.0.0-win-x86.zip`, extract, then run `LeagueOfSmurfs.exe`.

SHA256:

```text
BF17DC84339F324070744E36FB7357282DF8ADAC36CFFE2070AF675126B4CBE0
```

[VirusTotal report](https://www.virustotal.com/gui/file/bf17dc84339f324070744e36fb7357282df8adac36cffe2070af675126b4cbe0) for this zip (re-upload after packaging fix if the previous hash differs).

PowerShell:

```powershell
Get-FileHash .\LeagueOfSmurfs-v2.0.0-win-x86.zip -Algorithm SHA256
```

The zip includes `LeagueOfSmurfs.exe` **and** required dependency DLLs — extract the whole folder before running.

## Build

Open the `LeagueOfSmurfs` project in Visual Studio, restore NuGet packages if needed, then build **Release**.

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

Check the [VirusTotal scan for v2.0.0](https://www.virustotal.com/gui/file/bf17dc84339f324070744e36fb7357282df8adac36cffe2070af675126b4cbe0) before trusting a downloaded build.

The source is public in this repository — prefer building from Visual Studio instead of downloading a prebuilt `.exe`.

### If Defender quarantines the file

1. Verify the hash above (for that release zip)
2. Restore from Windows Security quarantine if needed
3. Or build from source (recommended)

Do not disable Defender globally just for this.

## License / vibe

Personal project for tinkering.  
**Not recommended for daily serious use.**
