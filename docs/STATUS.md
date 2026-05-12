```
//////////////////////////////////////////////
// XDCKIT STATUS: Active development   2026 //
//////////////////////////////////////////////
```

---

| | |
|---|---|
| **Project** | XDCKIT — Xbox Direct Connect Kit |
| **Status** | Active (library + XCETools host app) |
| **Original roadmap** | 2021–2022 (extended; major refactors through 2025–2026) |

---

### INFO

- **Started:** ~2016–2018 (exploration / prototypes)  
- **Current focus:** 2024–2026 — wire-accurate xbdm transport, XDevkit-shaped interop, discovery, file I/O, screenshots, consolefeatures RPC, and tooling hooks.  
- **Goal:** Emulate what the official XDevkit stack does over the wire, with clearer control and a single managed codebase you can extend.

### How it works

XDCKIT speaks the same **xbdm** text/binary protocol the debug monitor exposes on **TCP/730**. It does not wrap `xbdm.dll` on the PC; it **reimplements** the session framing (`200-`, `202-` multiline, `203-` binary, etc.) so your app owns timeouts, threading, and logging.

---

### Legends who contributed

- **ohhsodead** — early project contributions

### Code & inspiration (thank you)

| Source | Role |
|--------|------|
| **Yelo / Neighborhood-style tooling** | LAN workflows, quality-of-life patterns |
| **PeekPoker** | Debugging / value-finding ideas |
| **Accension Tool** | Debugging / value-finding ideas |
| **Community “consolefeatures” RPC plugins** | Remote-call patterns (no legacy plugin names in-tree) |

### Credits

- **Vodka Doc**

---

### Known issues / roadmap (symbol legend: `==` done · `*` in progress · `-=` almost there)

| Area | State |
|------|--------|
| **DmScreenShot / framebuffer capture** | `==` (xbdm `screenshot` + metadata + binary read) |
| **Console features (LEDs, temps, reboot, …)** | `==` / `*` (surface keeps growing with xbdm parity) |
| **Debugging (breakpoints, threads, context)** | `*` |
| **File system (dirlist, send/get, partial I/O)** | `==` / `*` (bulk paths + quoting hardening) |
| **Xenia Canary game-patches** | `==` (load `.patch.toml`, apply enabled rows via typed memory writes) — see [game-patches](https://github.com/xenia-canary/game-patches) |

---

### IDEAS

1. **Cheat Engine–style workflow** — XCE Tools + memory search / debugger panels (`*`).  
2. **Controller emulation** — `autoinput` path exists; higher-level UX `*`.  
3. **Live screenshot / streaming** — capture works; streaming UX `*`.  
4. **Async I/O end-to-end** — `*` (Task shims today; true `ReadAsync` pipeline later).

---

### Progress log (historical + new)

| When | What |
|------|------|
| 2020-04-26 06:33 | Polished code |
| 2020-09-15 09:00 | Polished code |
| 2020-09-17 09:00 | Polished code |
| 2020-12-05 03:00 | Polished + added code |
| 2020-12-08 13:00 | Removed code |
| 2020-12-30 22:44 | Added code |
| 2021-01-22 23:41 | Removed + added + polished |
| 2021-02-15 19:05 | Removed code |
| 2021-04-22 19:05 | Removed code |
| 2021-05-13 09:23 | Polished code |
| 2021-05-27 12:19 | Added code |
| 2021-05-31 09:15 | Polished — deprecated API cleanup |
| 2022-06-17 11:22 | Polished |
| 2025–2026 | XDCKIT split, xbdm parity pass, interop layer, discovery, screenshot, quoting, Xenia patch loader, `Notify(...)` API |
| 2026-05-12 | README refreshed to feature-list format (General / Debugging / FileSystem / XNotify / Xenia Patches), banner reinstated, per-instance timeouts + async wrappers documented |

---

*Last updated: May 2026*
