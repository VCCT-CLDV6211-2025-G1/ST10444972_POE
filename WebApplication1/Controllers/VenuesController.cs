using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobService _blobService;
        private readonly ILogger<VenuesController> _logger;

        public VenuesController(ApplicationDbContext context, BlobService blobService, ILogger<VenuesController> logger)
        {
            _context = context;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venues
                .ToListAsync();
            return View(venues);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,VenueName,Location,Capacity,ImageFile,Status")] Venue venue)
        {
            _logger.LogInformation("Create venue action started. Venue Name: {VenueName}", venue.VenueName);
            
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model state is valid");
                    
                    if (venue.ImageFile != null)
                    {
                        _logger.LogInformation("Attempting to upload image. File name: {FileName}, Size: {Size}", 
                            venue.ImageFile.FileName, venue.ImageFile.Length);
                        
                        try
                        {
                            venue.ImageUrl = await _blobService.UploadImageAsync(venue.ImageFile);
                            _logger.LogInformation("Image uploaded successfully. URL: {ImageUrl}", venue.ImageUrl);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Image upload failed");
                            ModelState.AddModelError("ImageFile", $"Image upload failed: {ex.Message}");
                            return View(venue);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No image file provided");
                    }
                    
                    venue.CreatedDate = DateTime.UtcNow;
                    venue.LastModified = DateTime.UtcNow;
                    venue.Status = VenueStatus.Active; // Set default status for new venues
                    
                    _logger.LogInformation("Attempting to save venue to database");
                    _context.Add(venue);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Venue saved successfully. VenueId: {VenueId}", venue.VenueId);
                    
                    return RedirectToAction(nameof(Index));
                }
                
                _logger.LogWarning("Model state is invalid: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return View(venue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venue");
                ModelState.AddModelError("", $"Error creating venue: {ex.Message}");
                return View(venue);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,ImageFile,ImageUrl,Status")] Venue venue)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (venue.ImageFile != null)
                    {
                        try
                        {
                            // Delete old image if it exists
                            if (!string.IsNullOrEmpty(venue.ImageUrl))
                            {
                                await _blobService.DeleteImageAsync(venue.ImageUrl);
                            }
                            // Upload new image
                            venue.ImageUrl = await _blobService.UploadImageAsync(venue.ImageFile);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ImageFile", $"Image upload failed: {ex.Message}");
                            return View(venue);
                        }
                    }

                    venue.LastModified = DateTime.UtcNow;
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError("", $"Error updating venue: {ex.Message}");
                }
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var venue = await _context.Venues.FindAsync(id);
                if (venue != null)
                {
                    if (!string.IsNullOrEmpty(venue.ImageUrl))
                    {
                        await _blobService.DeleteImageAsync(venue.ImageUrl);
                    }
                    _context.Venues.Remove(venue);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting venue");
                ModelState.AddModelError("", $"Error deleting venue: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
