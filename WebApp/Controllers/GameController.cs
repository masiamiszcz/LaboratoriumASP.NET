using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.VideoGames;
using WebApp.Helpers;


namespace WebApp.Controllers
{
    [Authorize(Roles = "user")]
    public class GameController : Controller
    {
        private readonly VideoGamesDbContext _context;

        public GameController(VideoGamesDbContext context)
        {
            _context = context;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
        {
            int defaultPageSize = pageSize ?? 20; // Domyślna liczba elementów na stronę
            int currentPageNumber = pageNumber ?? 1; // Ustaw bieżący numer strony
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["PageSize"] = defaultPageSize;

            var games = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.GamePublishers)
                .ThenInclude(gp => gp.Publisher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(g => g.GameName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    games = games.OrderByDescending(g => g.GameName);
                    break;
                default:
                    games = games.OrderBy(g => g.GameName);
                    break;
            }

            int totalItems = await games.CountAsync();
            int maxPageNumber = (int)Math.Ceiling((double)totalItems / defaultPageSize);
            ViewData["PageNumber"] = currentPageNumber;
            ViewData["MaxPageNumber"] = maxPageNumber;

            return View(await PaginatedList<Game>.CreateAsync(games.AsNoTracking(), currentPageNumber, defaultPageSize));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchString, int? pageNumber, int? pageSize)
        {
            int defaultPageSize = pageSize ?? 20;
            int currentPageNumber = pageNumber ?? 1;
        
            ViewData["CurrentFilter"] = searchString;
            ViewData["PageSize"] = defaultPageSize;
        
            var sortedGameIds = new SortedSet<int>();
        
            if (!string.IsNullOrEmpty(searchString))
            {
                // Find publishers matching the search string
                var matchingPublisherIds = await _context.Publishers
                    .Where(p => p.PublisherName.Contains(searchString))
                    .Select(p => p.Id)
                    .ToListAsync();
        
                if (matchingPublisherIds.Any())
                {
                    // Find games associated with matching publishers
                    var publisherGameIds = await _context.GamePublishers
                        .Where(gp => matchingPublisherIds.Contains(gp.PublisherId ?? 0))
                        .Select(gp => gp.GameId ?? 0)
                        .ToListAsync();
        
                    foreach (var gameId in publisherGameIds)
                    {
                        sortedGameIds.Add(gameId);
                    }
                }
        
                // Find games matching the search string by name
                var matchingGames = await _context.Games
                    .Where(g => g.GameName.Contains(searchString))
                    .Select(g => g.Id)
                    .ToListAsync();
        
                foreach (var gameId in matchingGames)
                {
                    sortedGameIds.Add(gameId);
                }
            }
        
            // Retrieve the games based on the sortedGameIds
            var games = _context.Games
                .Include(g => g.Genre)
                .Include(g => g.GamePublishers).ThenInclude(gp => gp.Publisher)
                .Where(g => sortedGameIds.Contains(g.Id));
        
            // Pagination logic
            int totalItems = sortedGameIds.Count;
            int maxPageNumber = (int)Math.Ceiling((double)totalItems / defaultPageSize);
            ViewData["PageNumber"] = currentPageNumber;
            ViewData["MaxPageNumber"] = maxPageNumber;
        
            var paginatedGames = await PaginatedList<Game>.CreateAsync(games.AsNoTracking(), currentPageNumber, defaultPageSize);
        
            return View(paginatedGames);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName");
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameName,GenreId")] Game game)
        {
            if (ModelState.IsValid)
            {
                // Sprawdź, czy nazwa gry już istnieje
                if (_context.Games.Any(g => g.GameName == game.GameName))
                {
                    ModelState.AddModelError("GameName", "A game with this name already exists.");
                    ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", game.GenreId);
                    return View(game);
                }
                //znajdywanie i ustawianie ID, żeby możnabyło dodać nową grę (NIE WIEM DLACZEGO SQLITE NIE INKREMENTUJE A PRZYPISUJE ID = 0)
                var maxId = _context.Games.Max(g => g.Id);  
                game.Id = maxId + 1;  
                
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", game.GenreId);
            return View(game);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", game.GenreId);
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenreId,GameName")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "GenreName", game.GenreId);
            return View(game);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
