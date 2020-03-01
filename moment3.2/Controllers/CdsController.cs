using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moment3._2.Data;
using moment3._2.Models;

namespace moment3._2.Controllers
{
    public class CdsController : Controller
    {
        private readonly CdContext _context;

        public CdsController(CdContext context)
        {
            _context = context;
        }

        // GET: Cds
        public async Task<IActionResult> Index(string searchString, int id)
        {
            
            var cdContext = _context.Cds.Include(c => c.Artist);

            var cds = from m in _context.Cds
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                cds = cds.Where(s => s.Name.Contains(searchString));
               foreach(var t in cdContext)
                {
                    var x = t.ArtistId;
                    if(x == id)
                    {
                        ViewData["test"] = t.Name;

                    }
                }
            

                return View(await cds.ToListAsync());
            }

            cds = _context.Cds.Include(c => c.Artist);



            var newdate = _context.Cds.Include(x => x.ReleaseDate);

            ViewData["Dates"] = newdate;
            return View(await cdContext.ToListAsync());
        }
        /*Search */

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Cds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

         
            var cd = await _context.Cds
                .Include(c => c.Artist).Include(c => c.Tracks)
                .FirstOrDefaultAsync(m => m.CdId == id);
            string date = cd.ReleaseDate.ToString("yyyy-MM-dd");
            ViewData["Date"] =date;


            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // GET: Cds/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name");
            return View();
        }

        // POST: Cds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CdId,Name,ReleaseDate,Avalable,ArtistId")] Cd cd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name", cd.ArtistId);
            return View(cd);
        }

        // GET: Cds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.Cds.FindAsync(id);
            if (cd == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name", cd.ArtistId);
            return View(cd);
        }

        // POST: Cds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CdId,Name,ReleaseDate,Avalable,ArtistId")] Cd cd)
        {
            if (id != cd.CdId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CdExists(cd.CdId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name", cd.ArtistId);
            return View(cd);
        }

        // GET: Cds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cd = await _context.Cds
                .Include(c => c.Artist)
                .FirstOrDefaultAsync(m => m.CdId == id);
            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // POST: Cds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cd = await _context.Cds.FindAsync(id);
            _context.Cds.Remove(cd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CdExists(int id)
        {
            return _context.Cds.Any(e => e.CdId == id);
        }
    }
}
