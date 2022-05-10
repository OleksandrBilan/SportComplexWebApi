namespace WebApi.Models.GroupTrainingSubscription
{
    public class SportSection
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SportType SportType { get; set; }
    }
}
