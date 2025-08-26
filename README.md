## What is SteamDisc
DiscSteam is an application that allows you to connect your Discord bot to your family Steam library. Every time someone purchases a new game, the bot will send a message to the specified Discord channel.
![image](https://github.com/user-attachments/assets/1ebd02a0-cde8-44be-b2b4-771a40b5455e)

## How it works
Once your Steam account is connected to Discord, the application runs in the background. Every 5 minutes, it sends a request to the Steam API to check for new games in the library. If new games are detected, the bot sends a message to the designated Discord channel. If there are no updates, the application does nothing.

<br><b>Note</b>: Occasionally, you may need to update the Steam API access token, as it expires after a certain period of time.

## How to setup
1. Create a Discord bot in the [Developer Portal](https://discord.com/developers/applications) and invite it to your server.
2. Enable Developer Mode in Discord, right-click your channel, and copy its ID.
<br>![image](https://github.com/user-attachments/assets/1c353614-07fc-46e0-b111-02e7842b8e9b).
3. Get a [Steam API](https://steamcommunity.com/dev/apikey) key from Steam API Key Registration.
5. Retrieve your Steam access token from the [Steam Points Summary](https://store.steampowered.com/pointssummary/ajaxgetasyncconfig).
6. Run the app, input your bot token, channel ID, Steam API key, and access token, and start the bot.
7. If any input is invalid, the app will notify you and prompt you to re-enter the correct information.
8. If all inputs are correct, a confirmation message will display in the command prompt.
![image](https://github.com/user-attachments/assets/6e9b27b5-1e71-4191-a6d4-a6f13ccfe366)

## Or... You can just use docker
1. In the config.json file, update your tokens.
2. In the directory of the project, run "docker build -t [name_do_you_want] .".
3. After, you run "docker run [name_do_you_want]".
<br><b>Note</b>: The default time to start fetching your games is set to 1739588400 (UNIX time), which is <b>Sat Feb 15 2025 03:00:00 GMT+0000</b>. To change this, go to config/config.json and update the "LastGameAdquiredTime" property with your desired UNIX timestamp.
