using System;

namespace WebApi.ApiModels
{
    public class GroupTrainingDto
    {
        public int Id { get; set; }

        public int ReceiptId { get; set; }

        public int GroupId { get; set; }

        public DateTime StartDateTime { get; set; }
    }
}
