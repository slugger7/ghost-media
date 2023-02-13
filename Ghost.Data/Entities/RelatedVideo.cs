namespace Ghost.Data
{
    public class RelatedVideo
    {
        public int Id { get; set; }
        public Video Video { get; set; }
        public Video RelatedTo { get; set; }
    }
}