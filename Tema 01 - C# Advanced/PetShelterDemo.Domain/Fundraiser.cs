namespace PetShelterDemo.Domain
{
    public class Fundraiser : INamedEntity
    {

        private readonly List<Person> donors = new List<Person>();
        private int totalDonationsInRON = 0;

        public string Name { get; } // foundraiser's title
        public string Description { get; }
        public int DonationTarget { get; }

        public Fundraiser(string title, string description, int donationTarget)
        {
            Name = title;
            Description = description;
            DonationTarget = donationTarget;
        }

        public void AddDonation(Person donor, int amountInRON)
        {
            donors.Add(donor);
            totalDonationsInRON += amountInRON;
        }

        public IReadOnlyList<Person> GetDonors()
        {
            return donors.AsReadOnly();
        }

        public int GetTotalDonationsInRON()
        {
            return totalDonationsInRON;
        }
    }
}