using ContactAppWeb.Data; // Import the necessary namespace for DataContext

namespace ContactAppWeb
{
    // A background service to reset the database at regular intervals
    public class DatabaseResetService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        // Constructor to inject the IServiceProvider
        public DatabaseResetService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Override the ExecuteAsync method to define the background task logic
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Run the background task until cancellation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                // Create a scope to access database services
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Get the DataContext from the scope
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                    // Reset the database by reseeding with initial contacts
                    DataSeeder.SeedInitialContacts(dbContext);
                }

                // Delay the background task for 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}