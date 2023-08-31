using ContactAppWeb.Data;
using ContactAppWeb.Models;
using Bogus; // Import the Faker library

public static class DataSeeder
{
    public static void SeedInitialContacts(DataContext context)
    {
        // Check if there are existing contacts in the database and delete 
        if (context.ContactModels.Any())
        {
            // Remove existing contacts
            context.ContactModels.RemoveRange(context.ContactModels);
            context.SaveChanges();
        }

        // Check if there are no existing contacts in the database, then populate the database with new contacts
        if (!context.ContactModels.Any())
        {
            // Create a new instance of the Faker class
            var faker = new Faker();

            // Generate and add 20 new contact instances to the database
            for (int i = 1; i <= 20; i++)
            {
                // Generate a random 10-digit phone number as a string
                string phoneNumber = faker.Phone.PhoneNumber("##########");


                // Create a new contact instance with dynamically generated data
                var contact = new ContactModel
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    PhoneNumber = phoneNumber,
                    Email = faker.Internet.Email()
                };

                // Add the new contact instance to the database context
                context.ContactModels.Add(contact);
            }

            // Save changes to the database to persist the new contacts
            context.SaveChanges();
        }
    }
}
