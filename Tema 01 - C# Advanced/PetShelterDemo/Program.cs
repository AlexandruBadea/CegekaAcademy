﻿//// See https://aka.ms/new-console-template for more information
//// Syntactic sugar: Starting with .Net 6, Program.cs only contains the code that is in the Main method.
//// This means we no longer need to write the following code, but the compiler still creates the Program class with the Main method:
//// namespace PetShelterDemo
//// {
////    internal class Program
////    {
////        static void Main(string[] args)
////        { actual code here }
////    }
//// }

using PetShelterDemo.DAL;
using PetShelterDemo.Domain;

var shelter = new PetShelter();

Console.WriteLine("Hello, Welcome the the Pet Shelter!");

var exit = false;
try
{
    while (!exit)
    {
        PresentOptions(
            "Here's what you can do.. ",
            new Dictionary<string, Action>
            {
                { "Register a newly rescued pet", RegisterPet },
                { "Donate", Donate },
                { "See current donations total", SeeDonations },
                { "See our residents\n", SeePets },

                { "Create a new fundraiser", CreateFundraiser },
                { "See all fundraisers", GetAllFoundraisers },
                { "Donate to a fundraiser\n", DonateToFundraiser },

                { "Break our database connection\n", BreakDatabaseConnection },
      
                { "Leave:(", Leave }
            }
        );
    }
}
catch (Exception e)
{
    Console.WriteLine($"Unfortunately we ran into an issue: {e.Message}.");
    Console.WriteLine("Please try again later.");
}

// Fundraiser

 void CreateFundraiser()
{
    var title = ReadString("Title?");
    var description = ReadString("Description?");
    Console.WriteLine("Donation target?");
    var donationTarget = ReadInteger();

    var fundraiser = new Fundraiser(title, description, donationTarget);

    shelter.RegisterFundraiser(fundraiser);

    Console.WriteLine($"Fundraiser '{title}' created with a donation target of {donationTarget} RON.");
}

void GetAllFoundraisers()
{

    var foundraisers = shelter.GetAllFundraisers();

    var foundraisersOptions = new Dictionary<string, Action>();

    foreach (var foundraiser in foundraisers)
    {
        foundraisersOptions.Add($"{foundraiser.Name} -> {foundraiser.GetTotalDonationsInRON()}RON out of {foundraiser.DonationTarget}RON",
            () => SeeFundraiserDetailsByName(foundraiser.Name));
    }

    PresentOptions("Foundraisers: ", foundraisersOptions);
}

void SeeFundraiserDetailsByName(string title)
{
    var fundraiser = shelter.GetFundraiserByTitle(title);
    Console.WriteLine($"A few words about {fundraiser.Name}: {fundraiser.Description}");
}


void DonateToFundraiser()
{

    var fundraiserTitle = ReadString("Foundraiser title");

    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();

    Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();
    var donor = new Person(name, id);

    var amountInRON = ReadInteger();

    var fundraiser = shelter.GetFundraiserByTitle(fundraiserTitle);
    if (fundraiser == null)
    {
        Console.WriteLine($"Fundraiser '{fundraiserTitle}' not found.");
    }
    else
    {
        fundraiser.AddDonation(donor, amountInRON);
        Console.WriteLine($"Donation of {amountInRON} RON added to fundraiser '{fundraiserTitle}'.");
    }
}



// Pet shelter

void RegisterPet()
{
    var name = ReadString("Name?");
    var description = ReadString("Description?");

    var pet = new Pet(name, description);

    shelter.RegisterPet(pet);
}

void Donate()
{
    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();

    Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();
    var person = new Person(name, id);

    Console.WriteLine("How much would you like to donate? (RON)");
    var amountInRon = ReadInteger();
    shelter.Donate(person, amountInRon);
}

void SeeDonations()
{
    Console.WriteLine($"Our current donation total is {shelter.GetTotalDonationsInRON()}RON");
    Console.WriteLine("Special thanks to our donors:");
    var donors = shelter.GetAllDonors();
    foreach (var donor in donors)
    {
        Console.WriteLine(donor.Name);
    }
}

void SeePets()
{

    var pets = shelter.GetAllPets();

    var petOptions = new Dictionary<string, Action>();
    foreach (var pet in pets)
    {
        petOptions.Add(pet.Name, () => SeePetDetailsByName(pet.Name));
    }

    PresentOptions("We got..", petOptions);
}

void SeePetDetailsByName(string name)
{
    var pet = shelter.GetByName(name);
    Console.WriteLine($"A few words about {pet.Name}: {pet.Description}");
}

void BreakDatabaseConnection()
{
    Database.ConnectionIsDown = true;
}

void Leave()
{
    Console.WriteLine("Good bye!");
    exit = true;
}

void PresentOptions(string header, IDictionary<string, Action> options)
{

    Console.WriteLine(header);

    for (var index = 0; index < options.Count; index++)
    {
        Console.WriteLine(index + 1 + ". " + options.ElementAt(index).Key);
    }

    var userInput = ReadInteger(options.Count);

    options.ElementAt(userInput - 1).Value();
}

string ReadString(string? header = null)
{
    if (header != null) Console.WriteLine(header);

    var value = Console.ReadLine();
    Console.WriteLine("");
    return value;
}

int ReadInteger(int maxValue = int.MaxValue, string? header = null)
{
    if (header != null) Console.WriteLine(header);

    var isUserInputValid = int.TryParse(Console.ReadLine(), out var userInput);
    if (!isUserInputValid || userInput > maxValue)
    {
        Console.WriteLine("Invalid input");
        Console.WriteLine("");
        return ReadInteger(maxValue, header);
    }

    Console.WriteLine("");
    return userInput;
}