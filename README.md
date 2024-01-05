# smpp-2-telegram

The main purpose of this project is to provide an ability to receive and send sms into the telegram account

## Version 0.0.1

The application can connect to SMPP host, receive incoming SMS and send it to Telegram Chat.

## Prerequisites

Before you start you need working SMPP server and created Telegram Bot which will send you incoming SMS.

## Settings

### SmppChannelConfiguration

- ChannelId - SMPP Channel
- Host - IP address or DNS name of SMPP Server
- Port - port of the SMPP Server
- SystemId - login
- Password - password

### TelegramBotConfiguration

- Name - any string
- Token - the token from @BotFather
- Owner - account name of creator without '@'

### TelegramConversationConfiguration

- ChatId - chat id as signed long. It can be a private chat with the bot or any group where the bot was added.
- ThreadId - optional signed long id if threads are enabled in the current chat

## Run in Docker

In src folder

``` bash
docker build -t smpp-2-telegram .
```

Then create compose yaml

``` yaml
version: "3.9"

name: "smpp-2-telegram"
      
services:
  apphost:
    container_name: smpp-2-telegram
    image: smpp-2-telegram:latest
    hostname: apphost
    restart: unless-stopped
    environment:
      - SmppChannelConfiguration__ChannelId=1
      - SmppChannelConfiguration__Host=localhost
      - SmppChannelConfiguration__Port=1234
      - SmppChannelConfiguration__SystemId=login
      - SmppChannelConfiguration__Password=password
      - TelegramBotConfiguration__Name=incoming sms
      - TelegramBotConfiguration__Token=Token:Token
      - TelegramBotConfiguration__Owner=owner
      - TelegramConversationConfiguration__ChatId=123456
    network_mode: "host"
```

Then up the compose

``` bash
docker compose up -d
```
