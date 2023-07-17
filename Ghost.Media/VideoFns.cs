using FFMpegCore;
using FFMpegCore.Enums;
using Ghost.Dtos;

namespace Ghost.Media
{
    public static class VideoFns
    {
        public static VideoMetaDataDto? GetVideoInformation(string path)
        {
            var mediaInfo = FFProbe.Analyse(path);
            if (mediaInfo.PrimaryVideoStream == null) return default;
            var fileInfo = new FileInfo(path);
            return new VideoMetaDataDto
            {
                Duration = mediaInfo.Duration,
                Format = mediaInfo.Format.FormatName,
                FormatLong = mediaInfo.Format.FormatLongName,
                Width = mediaInfo.PrimaryVideoStream.Width,
                Height = mediaInfo.PrimaryVideoStream.Height,
                Created = fileInfo.CreationTimeUtc,
                Size = fileInfo.Length
            };
        }

        public static async Task<bool> CreateSubVideoAsync(string inputPath, string outputPath, TimeSpan start, TimeSpan end)
        {
            return await FFMpeg.SubVideoAsync(inputPath, outputPath, start, end);
        }

        public static async Task ConvertVideo(string inputPath, string outputPath, int? constantRateFactor = null, int? variableBitrate = null)
        {
            await FFMpegArguments
                .FromFileInput(inputPath)
                .OutputToFile(outputPath, false, options =>
                {
                    options
                    .WithVideoCodec(VideoCodec.LibX264)
                    .WithAudioCodec(AudioCodec.Aac)
                    .WithFastStart();

                    if (constantRateFactor.HasValue)
                    {
                        options.WithConstantRateFactor(constantRateFactor.Value);
                    }

                    if (variableBitrate.HasValue)
                    {
                        options.WithVariableBitrate(variableBitrate.Value);
                    }
                })
                .ProcessAsynchronously();
        }
    }
}