namespace WebApi.Models.GroupTrainingSubscription
{
    public class SubscriptionType
    {
        public int Id { get; set; }

        public int AvailableTrainingsCount { get; set; }

        public decimal Price { get; set; }

        public SportSection SportSection { get; set; }
    }
}
