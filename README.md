# smpp-2-telegram

The main purpose of this project is to provide an ability to receive and send sms into the telegram account

## Version 0.0.1

The application can connect to SMPP host, receive incoming SMS and send it to Telegram Chat.

## Prerequsits

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

### TelegramConverstaionConfiguration

- ChatId - chat id as signed long. It can be a private chat with the bot or any group where the bot was added.
- ThreadId - optional signed long id if threads are enabled in the current chat
