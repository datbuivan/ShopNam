using QLBanQuanAoNam.Models;

namespace QLBanQuanAoNam.Repository
{
	public class LoaiSpRepository : ILoaiSpRepository
	{
		private readonly QlshopNamContext _contact;
		public LoaiSpRepository(QlshopNamContext contact)
		{
			_contact = contact;
		}
		public IEnumerable<LoaiSp> GetAllLoaiSp()
		{
			return _contact.LoaiSps;
		}
	}
}
