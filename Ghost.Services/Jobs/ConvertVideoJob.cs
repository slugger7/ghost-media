using Ghost.Data;
using Ghost.Dtos;
using Ghost.Exceptions;
using Ghost.Media;
using Ghost.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Services.Jobs
{
    public class ConvertVideoJob
    {
        private readonly DbContextOptions<GhostContext> contextOptions;
        private int Id;
        private ConvertRequestDto convertRequest;

        public ConvertVideoJob(int id, ConvertRequestDto convertRequestDto, DbContextOptions<GhostContext> contextOptions)
        {
            this.Id = id;
            this.convertRequest = convertRequestDto;
            this.contextOptions = contextOptions;
        }

        public async void Run()
        {
            using (var context = new GhostContext(contextOptions))
            {
                Console.WriteLine("Starting thread to convert video");
                var video = VideoRepository.FindById(context, Id, new List<string> { "LibraryPath" });
                if (video == null) throw new NullReferenceException("Video was not found to convert");

                var root = Path.GetDirectoryName(video.Path) ?? "";
                var newPath = Path.Combine(root, convertRequest.Title + ".mp4");

                if (!convertRequest.Overwrite && File.Exists(newPath))
                {
                    throw new FileExistsException();
                }

                await VideoFns.ConvertVideo(video.Path, newPath);

                var newVideoInfo = VideoFns.GetVideoInformation(newPath);
                if (newVideoInfo == null) throw new NullReferenceException("Could not find video info");

                // if overwirite make sure not to create another entity
                var newVideoEntity = await VideoRepository.CreateVideo(context, newPath, newVideoInfo, video.LibraryPath);

                // ImageService.GenerateThumbnailForVideo(new GenerateImageRequestDto
                // {
                //     VideoId = newVideoEntity.Id
                // });

                // copy actors
                // copy genres

                // rehidrate video before relating
                await VideoRepository.RelateVideo(context, video.Id, newVideoEntity.Id);
                await VideoRepository.RelateVideo(context, newVideoEntity.Id, video.Id);

                // Optional: create thread
                // Optional: create jobs entity
                Console.WriteLine("Finished converting video in thread");
            }
        }
    }
}