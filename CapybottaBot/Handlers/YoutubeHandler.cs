using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using static CapybottaBot.Utils;

namespace CapybottaBot.Handlers;

using YoutubeExplode;

/// <summary>
/// Singleton class that handles dealing with the Youtube class
/// </summary>
public class YoutubeHandler
{
    private static YoutubeHandler? instance;

    public static YoutubeHandler Instance => instance ??= new YoutubeHandler();

    public YoutubeClient YtClient { get; private set; }

    private YoutubeHandler()
    {
        YtClient = new YoutubeClient();
    }

    /// <summary>
    /// Asynchronously gets the stream for a given youtube video
    /// </summary>
    /// <param name="id">Either a link or the youtube id</param>
    public async Task<AudioOnlyStreamInfo> GetAudioStream(string id)
    {
        var videoId = VideoId.Parse(id);
        _ =this.LogInfo($"Attempting to grab stream info for video {videoId}");
        var streamManifest = await YtClient.Videos.Streams.GetManifestAsync(videoId);
        var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        return (AudioOnlyStreamInfo) audioStreamInfo;
    }
}