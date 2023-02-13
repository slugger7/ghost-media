using Ghost.Data;

namespace Ghost.Exceptions
{
    public class VideoRelationException : Exception
    {
        public VideoRelationException(Video video, Video relatedVideo) : base($"Video {video.Id} is already related to {relatedVideo.Id}") { }
    }
}