namespace Ghost.Data.Enums
{
    public static class JobStatus
    {
        //TODO: find better way of using string enums
        public static readonly string NotStarted = "NotStarted";
        public static readonly string InProgress = "InProgress";
        public static readonly string Completed = "Completed";
        public static readonly string Error = "Error";
    }
}