using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CapybottaBot
{
    public static class Utils
    {
        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            const string someString = "Hello World";
            const int someInt = 3;
            return Task.CompletedTask;
        }

        public static Task LogInfo(this object obj, string msg)
        {
            Log(new LogMessage(LogSeverity.Info,obj.GetType().Name, msg));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the token from the properties.json file
        /// </summary>
        /// <returns>the token</returns>
        /// <exception cref="NullReferenceException">Thrown if </exception>
        public static async Task<string> GetToken()
        {
            const string fileName = "properties.json";
            await using var openStream = File.OpenRead(fileName);
            var propertiesNode = JsonNode.Parse(openStream);
            if (propertiesNode is null) throw new NullReferenceException("Properties.json incorrectly configured...");
            var token = propertiesNode["token"]!.GetValue<string>();
            return token;
        }
        
        /// <summary>
        /// Asynchronously gets the voice channel that the user is currently in
        /// </summary>
        /// <param name="user">the user's voice channel</param>
        /// <param name="guild">the guild to check for a user in</param>
        /// <returns></returns>
        public static IAudioChannel? GetVcFromUser(SocketUser user, SocketGuild guild)
        {
            IReadOnlyCollection<SocketVoiceChannel>? channels = guild.VoiceChannels;
            return channels.FirstOrDefault(voiceChannel => voiceChannel.Users.Contains(user));
        }

        /// <summary>
        /// Starts an ffmpeg process and returns a reference to it
        /// </summary>
        /// <returns>Process object</returns>
        public static Process? CreateFfmpeg()
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = $"ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i - -ac 2 -f s16le -ar 48000 -",
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            });
        }
    }
}
