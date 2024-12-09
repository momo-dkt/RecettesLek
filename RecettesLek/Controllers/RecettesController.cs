using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecettesLek.Data;
using RecettesLek.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RecettesLek.Controllers
{
    public class RecettesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public RecettesController(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Recettes
        public async Task<IActionResult> Index()
        {
            //return View(await _context._recette.ToListAsync());
           

            var memoryStream = new MemoryStream();
            var modeleOutput =  _context._recette.Select(x => new ModeleVueRecetteOutput
            {
                RecetteID = x.RecetteId,
                Nom = x.Nom,
                Description = x.Description,
                ComposantsPrincipaux = x.ComposantsPrincipaux,
                Image = x.Image,
                Mimetype = x.Mimetype,
                
            });

            
            /*
            var modele = _context._recette.Select(x => new ModeleVueRecette
            {
                RecetteID = x.RecetteID,
                Nom = x.Nom,
                Description = x.Description,
                ComposantsPrincipaux = x.ComposantsPrincipaux,
                Image = new FormFile(new MemoryStream(x.Image), 0, x.Image.Length, "name", "fileName"),
            });
            */

            return View(modeleOutput);
        }

       

        // GET: Recettes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recette = await _context._recette
                .FirstOrDefaultAsync(m => m.RecetteId == id);
            if (recette == null)
            {
                return NotFound();
            }

            return View(recette);
        }

        // GET: Recettes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recettes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecetteID,Nom,Description,ComposantsPrincipaux,Image")] ModeleVueRecette recetteInput)
        {
            if (ModelState.IsValid)
            {
                string idUtilisateur = await RecupererIdUtilisteurCourant();
                Utilisateur utilisateur = await GetCurrentUserAsync();
                using (var memoryStream = new MemoryStream())
                {
                    // Obtenir le nom d'origine du fichier
                    var fileName = recetteInput.Image.FileName;

                    // Extraire l'extension
                    var extension = "image/"+Path.GetExtension(fileName).Remove(0,1);

                    await recetteInput.Image.CopyToAsync(memoryStream); // Copie le contenu du fichier dans le MemoryStream
                    byte[] image = memoryStream.ToArray(); // Convertit le MemoryStream en tableau de bytes
                    
                    Recette recette = new Recette() { RecetteId = recetteInput.RecetteID, Nom = recetteInput.Nom, Description = recetteInput.Description,ComposantsPrincipaux=recetteInput.ComposantsPrincipaux ,Image = image ,Mimetype=extension };
                    RecetteUtilisateur recetteUtilisateur = new RecetteUtilisateur() {  UtilisateurID=utilisateur.Id , RecetteId = recette.RecetteId ,User=utilisateur , Recette=recette };
                    _context.Add(recette);
                    _context.Add(recetteUtilisateur);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recetteInput);
        }

        // GET: Recettes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recette = await _context._recette.FindAsync(id);
            if (recette == null)
            {
                return NotFound();
            }
            return View(recette);
        }

        // POST: Recettes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecetteID,Nom,Description,ComposantsPrincipaux,Image")] Recette recette)
        {
            if (id != recette.RecetteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recette);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetteExists(recette.RecetteId))
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
            return View(recette);
        }

        // GET: Recettes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recette = await _context._recette
                .FirstOrDefaultAsync(m => m.RecetteId == id);
            if (recette == null)
            {
                return NotFound();
            }

            return View(recette);
        }

        // POST: Recettes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recette = await _context._recette.FindAsync(id);
            if (recette != null)
            {
                _context._recette.Remove(recette);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetteExists(int id)
        {
            return _context._recette.Any(e => e.RecetteId == id);
        }

        private Task<Utilisateur> GetCurrentUserAsync() =>
          _userManager.GetUserAsync(HttpContext.User);

        public async Task<string> RecupererIdUtilisteurCourant() {
            Utilisateur utilisateur = await GetCurrentUserAsync();
            return utilisateur?.Id;
        }

        public void Calcul() {
            int a = 1 + 1;
        }

        public void Calcul2()
        {
            int a = 1 + 1;
        }
    }
}
