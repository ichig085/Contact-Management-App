using ContactAppWeb.Data;
using ContactAppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList; // Import the required namespace for IPagedList

namespace ContactAppWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly DataContext _db;

        public ContactController(DataContext db)
        {
            _db = db;  // Injected DataContext instance for database operations
        }

        // GET: /Contact/Index
        public IActionResult Index(string sortBy, int? page)
        {
            var contacts = _db.ContactModels.AsQueryable();

            if (sortBy == "FirstName")
            {
                // Retrieve all contacts from the database and order by first name 
                contacts = _db.ContactModels.OrderBy(c => c.FirstName);
            }
            else if (sortBy == "LastName")
            {
                // Retrieve all contacts from the database and order by last name 
                contacts = _db.ContactModels.OrderBy(c => c.LastName);
            }
            else
            {
                // Retrieve all contacts from the database and order by first name 
                contacts = _db.ContactModels.OrderBy(c => c.FirstName);
            }

            // Create a paged list of contacts based on the current page and page size
            var pageNumber = page ?? 1;

            // Number of contacts per page
            var pageSize = 10;

            // Create a paged list of contacts based on the current page and page size
            var pagedList = contacts.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: /Contact/SearchResults
        public IActionResult SearchResults(string searchUserInput, int? page)
        {
            ViewBag.SearchUserInput = searchUserInput;

            // Query to retrieve contacts from the database
            var contacts = from c in _db.ContactModels
                           select c;

            // Check if search input is provided
            if (!string.IsNullOrEmpty(searchUserInput))
            {
                // Filter contacts based on case-insensitive search input
                contacts = contacts.Where(c =>
                    c.FirstName.ToLower().Contains(searchUserInput.ToLower()) ||
                    c.LastName.ToLower().Contains(searchUserInput.ToLower()) ||
                    c.Email.ToLower().Contains(searchUserInput.ToLower()) ||
                    c.PhoneNumber.ToLower().Contains(searchUserInput.ToLower()));

                // Order the filtered contacts by first name
                contacts = contacts.OrderBy(c => c.FirstName);
            }
            else
            {
                return View("NoResultsFound");
            }

            int pageSize = 10; // Number of items per page
            int pageNumber = page ?? 1; // Default to page 1 if no page parameter is provided

            // Create a paged list of contacts based on the current page and page size
            var pagedList = contacts.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: /Contact/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Contact/Create
        [HttpPost]
        public IActionResult Create(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                // Add the new contact to the database and redirect to the "Index" action
                _db.ContactModels.Add(contact);
                _db.SaveChanges();
                TempData["success"] = "Contact created successfully.";
                return RedirectToAction("Index");
            }
            // If model validation fails, return to the "Create" view with the invalid contact data
            return View(contact);
        }

        // GET: /Contact/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the contact with the provided ID from the database
            var contactFromDb = _db.ContactModels.Find(id);

            if (contactFromDb == null)
            {
                return NotFound();
            }
            return View(contactFromDb);
        }

        // POST: /Contact/Edit
        [HttpPost]
        public IActionResult Edit(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                // Update the contact in the database and redirect to the "Index" action
                _db.ContactModels.Update(contact);
                _db.SaveChanges();
                TempData["success"] = "Contact updated successfully.";
                return RedirectToAction("Index");
            }
            // If model validation fails, return to the "Edit" view with the invalid contact data
            return View(contact);
        }

        // GET: /Contact/Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the contact with the provided ID from the database
            var contactFromDb = _db.ContactModels.Find(id);

            if (contactFromDb == null)
            {
                return NotFound();
            }

            return View(contactFromDb);
        }

        // POST: /Contact/DeleteContact
        [HttpPost]
        public IActionResult DeleteContact(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the contact with the provided ID from the database
            var contactFromDb = _db.ContactModels.Find(id);

            if (contactFromDb == null)
            {
                return NotFound();
            }

            // Delete the contact from the database and redirect to the "Index" action
            _db.ContactModels.Remove(contactFromDb);
            _db.SaveChanges();
            TempData["delete"] = "Contact deleted successfully.";

            // Redirect to the "Index" action
            return RedirectToAction("Index");
        }

        // GET: /Contact/ResetContactsData
        public IActionResult ResetContactsData()
        {
            // Reset the contact data by reseeding with initial contacts
            DataSeeder.SeedInitialContacts(_db);

            // Redirect to the "Index" action to show the list of contacts after resetting
            return RedirectToAction("Index");
        }
    }
}