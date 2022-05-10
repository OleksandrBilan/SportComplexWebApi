namespace WebApi.Models.CoachInfo
{
    public class IndividualCoach
    {
        public int Id { get; set; }

        public Coach CoachInfo { get; set; }

        public decimal PricePerHour { get; set; }
    }
}
