## `🎮 Loly - Tools`
<img src="/Ressources/logo.ico" width="100" alt=""/>

A set of several small tools for League Of Legends.
*If you want to support the project, please leave a __star__ on the Github directory*

### Works with the latest `14.8` update of League of Legends ([Patch Notes : 14.8](https://na.leagueoflegends.com/en-us/news/game-updates/patch-14-8-notes/))
![Loly - Tools](/Ressources/mainMenu.png)

## `❗ Usage`
You must install .NET Desktop Runtime 6 to use this tool.<br>
No need to install this if you have a more recent version installed :<br>
.NET Desktop Runtime 6 -> [Official x64 installer (Microsoft)](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.27-windows-x64-installer)<br>

You can download the latest version of **LolyTools** [here](https://github.com/devRael1/LolyTools/releases/download/2.2.1/Loly.Tools.-.2.2.1.exe).

## `⚙️ Features`
- [x] Interactive Menu in Console
- [x] Multi-region support (DE, FR, ES, JP, CN...)
- [x] Lobby Revealer/Shower (Ranked/Draft/Normal games)
- [x] Auto Accept match (Auto accept matchmaking)
- [x] Auto Pick and Ban multiple champions (By roles => Top, Jungle, Mid, ADC, Support)
- [x] Import / Export League of Legends settings for all accounts
- [x] Auto send logs when an error occurs (Discord Webhook)
- [x] Auto Chat (Auto send messages when champ select start)
- [x] Get U.GG stats of your team (SoloQ & DuoQ / FlexQ...)
- [x] Advanced Logs system (Real Time & Persistent)
- [x] Use JSON file to save your settings
- [x] Very low CPU and RAM usage
- [x] Auto update (Github API)

More features coming soon...

## `📷 Screenshots`
<details>
<summary>Click here to see all screenshots...</summary>

### Updater Menu
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/updater.png" alt="devRael1">
</details>

### Main Menu
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/mainMenu.png" alt="devRael1">
</details>

### Tools Menu
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/toolsMenu.png" alt="devRael1">
</details>

### Settings Menu
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/settingsMenu.png" alt="devRael1">
</details>

### Logs Menu
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/logsMenu.png" alt="devRael1">
</details>

### Credits Menu
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/creditsMenu.png" alt="devRael1">
</details>

#### Discord Webhook Example (Auto send logs when an error occurs)
<details>
<summary>See screenshot...</summary>
<img src="/Ressources/discordWebhook.png" alt="devRael1">
</details>

</details>

## `❓ Bugs report / Suggestions`
If you want to report a bug or suggest a feature, you can open an issue [here](https://github.com/devRael1/LolyTools/issues) or contact me on Discord.<br>
My Discord: `1043813027205619804`

## `🧾 Todo List`
### V3.0.0 COMING SOON
======================== V3.0.0 ========================
- [x] Replace OP.GG stats sytem to add a new one (U.GG with all API's)
- [ ] Add more information in LobbyRevealer tool (API's : HistoricRanks / GetPlayerOverallRanking / FetchMatchSummaries...)
- [x] Review the menu system and optimize it (Code optimization / lisibility)
- [x] Add system to save all settings (all Keybinds, Colors, chat options...) of the Game
- [x] Add a system to auto-send logs to the developer (Discord Webhook) when an error occurs (use a setting to enable/disable)
- [x] Upgrade the error management system in the app (use `AppDomain.CurrentDomain.UnhandledException` for each error
- [x] Add a second option of champion pick in case of ban of the main champion ([#5 Suggestion](https://github.com/devRael1/LolyTools/issues/5))
- [x] Add an option in PicknBan menu to show the list of champions by roles (Top, Jungle, Mid, ADC, Support, Default) and by Pick and or Ban
- [ ] Update screenshots in `README.md` with Release V3.0.0
- [x] Add a screenshot exemple for auto send logs (Discord Webhook) system
- [x] Refactor the code of `Settings.cs` to make it more readable and optimized (create new class `Settings` in Class folder)
- [x] Change all lisibility of code to improve code quality
- [x] Update system with Version of the app (delete global variable in `Global.cs`)

## `📝 License`
Copyright © 2024 devRael<br>
This project is MIT licensed.<br>
This is not an official Riot Games product. It's not affiliated with or endorsed by Riot Games Inc.
