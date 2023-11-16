using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IBorrowerRepository
    {
        public List<Borrower> GetBorrowers();
		public List<BorrowerBook> GetAllOnLoan();
		public bool ReserveBook(BorrowerReserve borrowerReserve);
		public Guid AddBorrower(Borrower borrower);
		public bool RemoveBorrower(Guid borrowerId, Guid bookId, decimal fine);
	}
}
