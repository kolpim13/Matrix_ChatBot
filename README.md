# Matrix_ChatBot
 Discord bot to control RGB matrix via UART

Alow user to run Discord bot on a local machine. This bot can draw pictures chosen from the explorer space. Image shoud be 32x64p resolution.

Consist of two programs, driver for atmega2560 to drive an RGB matrix and discord bot.

All bot commands start from # symbol. They are divided into two groups, UART and RGB2.
UART Commands:
1)open [port name]
2)close []
3)get_ports []: get all available port names for this particulare machine.
RGB2 Commands:
1)pic [path to image]: convert image to the format sutable for driver and send it. Image size should be exactly the same or less then matrix size. 

To commit a command, type comand group and a command belonging to this group.
