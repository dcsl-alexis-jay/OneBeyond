using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using System.Collections;

namespace OneBeyondApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OnLoanController : ControllerBase
	{
		private readonly ILogger<OnLoanController> _logger;
		private readonly IBorrowerRepository _borrowerRepository;

		public OnLoanController(ILogger<OnLoanController> logger, IBorrowerRepository borrowerRepository)
		{
			_logger = logger;
			_borrowerRepository = borrowerRepository;
		}

		[HttpGet]
		[Route("GetOnLoan")]
		public List<BorrowerBook> GetAllOnLoan()
		{
			return _borrowerRepository.GetAllOnLoan();
		}

		[HttpPut]
		[Route("ReturnBook")]
		public bool ReturnBook(Guid borrowerId, Guid bookId, decimal fine)
		{
			return _borrowerRepository.RemoveBorrower(borrowerId, bookId, fine);
		}

		[HttpPut]
		[Route("ReserveBook")]
		public bool ReserveBook(BorrowerReserve borrowerReserve)
		{
			return _borrowerRepository.ReserveBook(borrowerReserve);
		}
	}
}