using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moment3._2.Data;
using moment3._2.Models;

namespace moment3._2.Controllers
{
    public class RentsController : Controller
    {
        private readonly CdContext _context;

        public RentsController(CdContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            var cdContext = _context.Rents.Include(r => r.Cd);
            return View(await cdContext.ToListAsync());
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Cd)
                .FirstOrDefaultAsync(m => m.RentId == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }
  

        // GET: Rents/Create
        public IActionResult Create()
        {

            var idAndName = _context.Cds
               .Where(x => x.Avalable == true)
               .Select(x => new { x.CdId, x.Name });
      
            ViewBag.cdId = new SelectList(idAndName, "CdId", "Name"); 
         
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentId,Name,RentDate,cdId")] Rent rent)
        {
  

            if (ModelState.IsValid)
            {
               Cd result = (from p in _context.Cds
                             where p.CdId == rent.cdId
                             select p).SingleOrDefault();
                result.Avalable = false;

                _context.SaveChanges();

                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.cdId = new SelectList(_context.Cds, "CdId", "Name");
          
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            var idAndName = _context.Cds
          .Where(x => x.Avalable == true)
          .Select(x => new { x.CdId, x.Name });



            ViewBag.cdId = new SelectList(idAndName, "CdId", "Name");
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentId,Name,RentDate,cdId")] Rent rent)
        {
            if (id != rent.RentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.RentId))
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
            ViewData["cdId"] = new SelectList(_context.Cds, "CdId", "CdId", rent.cdId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

 


            var rent = await _context.Rents
                .Include(r => r.Cd)
                .FirstOrDefaultAsync(m => m.RentId == id);

            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var rent = await _context.Rents.FindAsync(id);
           
            _context.Rents.Remove(rent);
     
                Cd results = (from p in _context.Cds
                             where p.CdId == rent.cdId
                             select p).SingleOrDefault();
                results.Avalable = true;

                _context.SaveChanges();
       
         

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.RentId == id);
        }
    }
}
