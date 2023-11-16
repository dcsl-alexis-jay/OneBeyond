namespace OneBeyondApi.Model
{
    public class BorrowerFine
	{
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid BorrowerId { get; set; }
        public DateTime LoanEndDate { get; set; }
		public decimal Fine { get; set; }
	}
}
