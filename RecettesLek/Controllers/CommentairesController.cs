﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecettesLek.Data;

namespace RecettesLek.Controllers
{
    public class CommentairesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentairesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Commentaires
        public async Task<IActionResult> Index()
        {
            return View(await _context._commentaire.ToListAsync());
        }

        // GET: Commentaires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentaire = await _context._commentaire
                .FirstOrDefaultAsync(m => m.CommentaireId == id);
            if (commentaire == null)
            {
                return NotFound();
            }

            return View(commentaire);
        }

        // GET: Commentaires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Commentaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCommentaire,IdRecette,IdUtilisateur,contenuCommentaire")] Commentaire commentaire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentaire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commentaire);
        }

        // GET: Commentaires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentaire = await _context._commentaire.FindAsync(id);
            if (commentaire == null)
            {
                return NotFound();
            }
            return View(commentaire);
        }

        // POST: Commentaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCommentaire,IdRecette,IdUtilisateur,contenuCommentaire")] Commentaire commentaire)
        {
            if (id != commentaire.CommentaireId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentaire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentaireExists(commentaire.CommentaireId))
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
            return View(commentaire);
        }

        // GET: Commentaires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentaire = await _context._commentaire
                .FirstOrDefaultAsync(m => m.CommentaireId == id);
            if (commentaire == null)
            {
                return NotFound();
            }

            return View(commentaire);
        }

        // POST: Commentaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentaire = await _context._commentaire.FindAsync(id);
            if (commentaire != null)
            {
                _context._commentaire.Remove(commentaire);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentaireExists(int id)
        {
            return _context._commentaire.Any(e => e.CommentaireId == id);
        }
    }
}
