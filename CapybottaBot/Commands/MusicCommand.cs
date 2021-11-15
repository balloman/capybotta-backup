using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CapybottaBot.Handlers;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using static CapybottaBot.Utils;

namespace CapybottaBot.Commands;

public class MusicCommand : ModuleBase<SocketCommandContext>
{
    [Command("play", RunMode = RunMode.Async)]
    [Summary("Attempts to play a song.")]
    // ReSharper disable once UnusedMember.Global
    public async Task PlayAsync(string link)
    {
        _ = this.LogInfo($"Attempting to run command {Context.Message} from channel {Context.Channel.Name}");
        var vc = GetVcFromUser(Context.User, Context.Guild);
        if (vc is null)
        {
            _ = Context.Channel.SendMessageAsync("Capybotta cannot find the channel you are in :/");
            return;
        }

        _ = Context.Channel.SendMessageAsync($"Hopping in to {Context.Channel.Name}");
        var streamInfo = await YoutubeHandler.Instance.GetAudioStream(link);
        var audioClient = await vc.ConnectAsync();
        var audioOutStream = audioClient.CreatePCMStream(AudioApplication.Music);
        var ffmpeg = CreateFfmpeg() ?? throw new InvalidOperationException();
        Task ffmpegOutputTask = ffmpeg.StandardOutput.BaseStream.CopyToAsync(audioOutStream);
        var progress = new Progress<double>();
        progress.ProgressChanged += (_, d) => this.LogInfo($"Progress: {d}");
        await YoutubeHandler.Instance.YtClient.Videos.Streams.CopyToAsync(streamInfo, ffmpeg.StandardInput.BaseStream, progress);
        ffmpeg.StandardInput.BaseStream.Close();
        ffmpegOutputTask.Wait();
        await vc.DisconnectAsync();
    }

}