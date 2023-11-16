using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;
using System.Net;

namespace OneBeyondApi.DataAccess
{
    public class BorrowerRepository : IBorrowerRepository
    {
        public BorrowerRepository()
        {
        }
        public List<Borrower> GetBorrowers()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Borrowers
                    .ToList();
                return list;
            }
        }

		public List<BorrowerBook> GetAllOnLoan()
		{
			using (var context = new LibraryContext())
			{
				var list = context.Catalogue
					.Include(x => x.Book)
					.Include(x => x.OnLoanTo)
					.AsQueryable();

				var booksOnLoan = list.Where(c => c.LoanEndDate <= DateTime.Now).Select(s => new BorrowerBook() { BorrowerName = s.OnLoanTo!.Name, BookName = s.Book.Name });

				return booksOnLoan.ToList();
			}
		}

		public bool ReserveBook(BorrowerReserve borrowerReserve)
		{
			using (var context = new LibraryContext())
			{
				var list = context.Catalogue
					.Include(x => x.Book)
					.Include(x => x.OnLoanTo)
					.AsQueryable();

				var bookToReserve = list.Where(c => c.Book.Id == borrowerReserve.BookId && c.OnLoanTo != null).FirstOrDefault();

				if (bookToReserve != null)
				{
					bookToReserve.Reserved = borrowerReserve.Borrower;

					context.Catalogue.Update(bookToReserve);
					context.SaveChanges();

					return true;

				}

				return false;
			}
		}


		public Guid AddBorrower(Borrower borrower)
        {
            using (var context = new LibraryContext())
            {
                context.Borrowers.Add(borrower);
                context.SaveChanges();
                return borrower.Id;
            }
        }

		public bool RemoveBorrower(Guid borrowerId, Guid bookId, decimal fine)
		{
			using (var context = new LibraryContext())
			{
				var list = context.Catalogue
					.Include(x => x.Book)
					.Include(x => x.OnLoanTo)
					.AsQueryable();

				var bookBorrowed = list.Where(c => c.Book.Id == bookId && c.OnLoanTo!.Id == borrowerId).FirstOrDefault();

				if (bookBorrowed != null)
				{
					if (DateTime.Now > bookBorrowed.LoanEndDate)
					{
						var borrowerFine = new BorrowerFine() {

							BorrowerId = bookBorrowed.OnLoanTo!.Id,
							BookId = bookBorrowed.Book.Id,
							LoanEndDate = bookBorrowed.LoanEndDate.Value,
							Fine = fine
						};

						context.BorrowerFines.Add(borrowerFine);
					}

					bookBorrowed.OnLoanTo = null;

					context.Catalogue.Update(bookBorrowed);
					context.SaveChanges();

					return true;

				}

				return false;
			}
		}
	}
}
