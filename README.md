# TBot++

_Brought to you by Zeus._

TBot++ is a control-plane extension built on top of the original TBot project.

It keeps the original TBot runtime model, worker logic, and `ogamed` integration, but adds a much richer operator experience for:

- persistent configuration
- live supervision
- runtime control
- browser-session bootstrap
- remote control-plane usage

This repository still includes the original runtime and legacy web pieces. TBot++ does not replace that core. It gives it a more complete UI and a clearer operational model.

On Windows, the intended production split is:

- `TBot++.exe` starts the backend in `--web --api-only` mode and opens the UI in the default browser
- `TBot.exe` keeps the classic standalone TBot behavior

## What TBot++ adds

- a real multi-screen UI instead of a minimal helper page
- a clear split between saved configuration and live runtime control
- a full `Instances` editor instead of editing JSON blindly
- a modular `Cockpit` for live monitoring and runtime actions
- an intelligent setup assistant for new or incomplete instances
- explicit manual-login and browser-session login flows
- an advanced sleep flow with dedicated before-sleep expedition batching
- safer device/session handling
- better remote control support for VPS or dedicated-host deployments
- multilingual UI, integrated themes, and customizable cockpit layout

## Core idea

If you already know TBot, the main mental model is:

- `Instances` = persistent configuration
- `Cockpit` = live runtime supervision and temporary control
- `Logs` = raw event stream

That split is the heart of TBot++.

## Main screens

### Cockpit

Cockpit is the live operations view.

Use it to:

- monitor one selected instance in real time
- switch focus between configured instances
- inspect attacks, empire state, celestials, queue, feedback, timers, and logs
- start or stop runtime features temporarily
- start or stop full instances from `Instance Header`

### Instances

Instances is the persistent configuration view.

Use it to:

- create and select profiles
- run the setup assistant
- choose login mode
- edit credentials, proxy, relay access, and device identity
- edit bot sections such as `General`, `Sleep`, `Defender`, `Expeditions`, `Farm`, `Harvest`, `Discovery`, `Colonize`, and `Brain`
- compare persisted config and effective config

If a setting must survive reloads and future runs, it belongs here.

### Logs

Logs is the consolidated backend log view.

Use it to:

- diagnose startup issues
- follow worker actions
- correlate UI state with backend behavior

## Login modes

TBot++ supports two explicit login strategies.

### Manual login

Use it when you want the classic TBot approach:

- universe
- email
- password
- language
- optional proxy and relay settings

### Browser session

Use it when you want to bootstrap a session from a real browser login and import `ogamed` storage.

This mode is built around:

- `DeviceConf.Name` as the storage identity
- imported `cookies` / `fingerprint` / optional `account`
- support for SSO providers such as `Gameforge`, `Google`, and `Facebook` once the flow reaches universe selection

In this mode, email and password can stay empty.

## Quick Settings

The top navbar `Settings` button is the fast connection panel for the control plane.

Use it to:

- keep same-origin mode for local usage
- set a remote backend URL
- choose auth mode (`Disabled`, `Token`, `Basic`)
- reconnect quickly without leaving the current screen

It changes the UI target and auth context, not your instance JSON content.

## Device identity and safety

`ogamed` storage is keyed by device name. Reusing the same device name across unrelated instances can silently collide with another stored session.

TBot++ improves this with:

- device diagnostics in the UI
- collision severity feedback
- suggested unique device names
- explicit override only when reuse is intentional

## Remote control support

TBot++ can be used as a real remote control plane for a TBot instance hosted elsewhere.

Typical pattern:

- run TBot on a VPS or dedicated host
- expose the backend deliberately
- connect to it from the UI by setting the backend URL and auth mode in `Settings`

For non-local deployments, authentication should be configured deliberately.

## Typical usage

### Same-host usage

Run TBot in web mode and use the UI on the same machine.

### Remote usage

Run TBot on a remote host, set its URL and auth mode in the UI, and operate it from another machine.

### Browser bootstrap usage

Choose browser-session login in the setup assistant, launch the local SessionBridge companion, sign in through the browser, then let TBot++ import the resulting `ogamed` storage.

### Control-plane-only usage

If you want configuration and remote-control access before starting workers, run TBot in control-plane mode:

- `TBot.exe --web --api-only`
- `dotnet TBot.dll --web --api-only`

In this mode, instances are not auto-started. Start the one you want from `Cockpit > Instance Header`.

This is also the default behavior of `TBot++.exe`.

If needed, browser auto-open can be disabled with `--no-browser`.

## Compared to the original TBot web surface

The old web surface mostly exposed settings, logs, and a simple OGame-facing page.

TBot++ goes further with:

- guided setup instead of raw JSON-first configuration
- a full screen-based UI instead of utility pages
- live supervision and runtime control from Cockpit
- clearer persistent configuration editing
- more intuitive instance selection and profile switching
- an advanced sleep-mode extension with before-sleep expedition planning around wake-up
- better support for browser-session bootstrap and remote control

## Compatibility philosophy

TBot++ is intentionally conservative with the base runtime.

The goal is:

- preserve normal CLI behavior
- preserve the original TBot worker model
- preserve the `ogamed` integration model
- add UI-side clarity and safer orchestration without turning the bot into a different backend

In short, TBot++ is a better operator surface for TBot, not a separate bot engine.

## Who this is for

TBot++ is especially useful if:

- you already know TBot but want a real operator UI
- you manage multiple instances
- you want cleaner configuration than manual JSON edits
- you want live visibility into queue, workers, timers, and runtime state
- you want to control a remote TBot backend from your own machine

---

## This fork: TBotPlus.Web

This fork adds `TBotPlus.Web/` (see `TBotPlus.sln`), an independent, lightweight Blazor Server dashboard for TBot. It does not depend on `TBot++`'s backend or Cockpit — it reads a TBot deployment's `settings.json`, `instance_settings.json`, and daily CSV logs straight off disk, so it works against a plain TBot install with no control-plane API involved.

### What it adds on top of the base TBot web surface

- **Instances view** — lists every configured instance and shows its `instance_settings.json` grouped by section (Credentials with the password masked, General, SleepMode, Defender, Expeditions, AutoFarm, AutoHarvest, AutoColonize, AutoDiscovery, ...).
- **Feature toggles** — flips the `Active`/`Enabled` flag of each feature (Defender and its SpyAttacker/SpyWatch sub-workers, Brain and its AutoMine/AutoResearch/Lifeform/AutoCargo/AutoDefence/AutoRepatriate/BuyOfferOfTheDay sub-workers, Expeditions, AutoFarm, AutoHarvest, AutoColonize, AutoDiscovery, Watchdog) directly from the browser, saved back to `instance_settings.json`. Also exposes the two newer flags added in this fork: AutoFarm's `FastFarmMode` and AutoDiscovery's `PauseWhenArtifactsAbove`. Every other field in the file stays read-only for now — see [Limitations](#limitations-tbotplusweb).
- **Logs view** — reads the daily `TBot{yyyyMMdd}.csv` log file, with text/level/sender filters and an optional live-tail poll.
- **Language selector** — a searchable dropdown in the top bar to pick the UI language. Defaults to English; the selection persists in the browser. Only English exists today, but the picker and the underlying locale-pack format (same shape as TBot's own custom-locale-pack contract: `Locale`, `Name`, `LocaleTag`, `Inherits`, `Translations`) are already in place for future translations.

### Project layout

```
TBotPlus.sln
TBotPlus.Web/            Blazor Server app (the actual project)
  Services/               File-reading/writing services (settings, logs, locale)
  Models/                 POCOs used by the pages/services
  Pages/                  Razor pages (Instances, Logs)
  Shared/                 Layout + reusable components (nav menu, language selector)
  sample-data/            Fixture settings.json / instance_settings.json / CSV for local dev
```

### Running locally

```
cd TBotPlus.Web
dotnet run
```

`appsettings.json` points `TBotPaths:BotOutputPath` at `sample-data/` by default, so it runs against fixtures out of the box. To point it at a real TBot deployment, change `BotOutputPath` (in `appsettings.json` or an environment override) to that deployment's folder — the one containing `settings.json`.

### Limitations (TBotPlus.Web) {#limitations-tbotplusweb}

- Only the boolean `Active`/`Enabled`-style feature flags are editable. All other fields are read-only.
- TBot's own `SettingsService` uses an in-process lock that doesn't coordinate with an external writer. If TBotPlus.Web and TBot happen to write `instance_settings.json` at the exact same instant, one write can be lost. TBot's `FileSystemWatcher` picks up the change on its next reload either way.
- No authentication — run this behind your own access control if exposing it beyond localhost.
