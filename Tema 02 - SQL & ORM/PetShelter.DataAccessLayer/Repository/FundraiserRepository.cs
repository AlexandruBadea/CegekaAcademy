using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;

namespace PetShelter.DataAccessLayer.Repository;

public class FundraiserRepository : BaseRepository<Fundraiser>, IFundraiserRepository
{

    public FundraiserRepository(PetShelterContext context) : base(context) { }

    public async Task<decimal> GetCurrentRaisedAmount(int fundraiserId)
    {
    
        var fundraiser = await _context.Set<Fundraiser>()
            .Include(f => f.Donations) // includ si donatiile acestui fundraiser
            .FirstOrDefaultAsync(f => f.Id == fundraiserId); // cautam dupa id

        if (fundraiser == null)
        {
            return 0;
        }

        decimal totalAmountInRON = fundraiser.Donations.Sum(d => d.Amount);

        return totalAmountInRON;
    }

}