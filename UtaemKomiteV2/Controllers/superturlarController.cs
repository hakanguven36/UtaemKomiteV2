using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UtaemKomiteV2.Araclar;
using UtaemKomiteV2.Models;

namespace UtaemKomiteV2.Controllers
{
	[Yetki("superadmin")]
    public class superturlarController : Controller
    {
        private readonly MyContext _context;

        public superturlarController(MyContext context)
        {
            _context = context;
        }

        // GET: superturlar
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tur.ToListAsync());
        }

        // GET: superturlar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tur = await _context.Tur
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tur == null)
            {
                return NotFound();
            }

            return View(tur);
        }

        // GET: superturlar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: superturlar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,isim")] Tur tur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tur);
        }

        // GET: superturlar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tur = await _context.Tur.FindAsync(id);
            if (tur == null)
            {
                return NotFound();
            }
            return View(tur);
        }

        // POST: superturlar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,isim")] Tur tur)
        {
            if (id != tur.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurExists(tur.ID))
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
            return View(tur);
        }

        // GET: superturlar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tur = await _context.Tur
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tur == null)
            {
                return NotFound();
            }

            return View(tur);
        }

        // POST: superturlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tur = await _context.Tur.FindAsync(id);
            _context.Tur.Remove(tur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurExists(int id)
        {
            return _context.Tur.Any(e => e.ID == id);
        }
    }
}
