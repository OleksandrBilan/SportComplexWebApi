namespace WebApi.ApiModels.GroupTrainingSubscription
{
    public class SubscriptionTypeApiModel
    {
        public int Id { get; set; }

        public int SportSectionId { get; set; }

        public int AvailableTrainingsCount { get; set; }

        public decimal Price { get; set; }
    }
}
