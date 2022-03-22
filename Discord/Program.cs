using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Ports;

namespace ChatBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        static void Main(string[] args) =>
            new Program().MainAsync().GetAwaiter().GetResult();

        /* Any exception here leads to crash program. */
        public async Task MainAsync()
        {
            // Create instans for client and commands
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
            });
            _client.Log += Log;

            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = false,
            });
            _commands.Log += Log;

            // Add all commands
            await InitCommands();

            // Start client
            var token = "OTEyNDI0OTU0MTAyOTY4Mzkw.YZvv9Q.CVRbQMhP6Cor8HS-fCv1YxzXyIU";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // UART section
            UART.Init();

            // Tests
            //...

            // Block task until the program is closed
            await Task.Delay(Timeout.Infinite);
        }     

        private Task Log(LogMessage msg)
        {
            switch (msg.Severity) 
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message} {msg.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
        
        private async Task InitCommands()
        {
            // Add Modules manually
            await _commands.AddModuleAsync<InfoModule>(_services);
            await _commands.AddModuleAsync<UARTModule>(_services);
            await _commands.AddModuleAsync<RGBModule>(_services);
            await _commands.AddModuleAsync<RGB2Module>(_services);
            // Note that the first one is 'Modules' (plural) and the second is 'Module' (singular).

            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            var msg = arg as SocketUserMessage;
            if (msg == null) 
                return;

            // Bot do not respond to itself or other bots
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) 
                return;

            // Position where command prefix ends from the start
            int pos = 0;
            // '#' - prefix command character
            if (msg.HasCharPrefix('#', ref pos) /* || msg.HasMentionPrefix(_client.CurrentUser, ref pos) */)
            {
                // Create a Command Context.
                var context = new SocketCommandContext(_client, msg);

                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed successfully).
                var result = await _commands.ExecuteAsync(context, pos, _services);
            }
        }
    }



}
