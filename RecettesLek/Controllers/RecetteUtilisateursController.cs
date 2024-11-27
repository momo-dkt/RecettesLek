using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecettesLek.Data;
using RecettesLek.Models;

namespace RecettesLek.Controllers
{
    public class RecetteUtilisateursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public RecetteUtilisateursController(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        // GET: RecetteUtilisateurs
        public async Task<IActionResult> Index()
        {
            string IdUtilisateurCourant= await RecupererIdUtilisteurCourant();
            
            var recettesUtilisateursId =  await _context._recetteUtilisateur.Where(recette => recette.UtilisateurID.Equals(IdUtilisateurCourant)).Select(recette => recette.RecetteId).ToListAsync();
            var recettes_Utilisateur =   _context._recette.Where(recette => recettesUtilisateursId.Contains(recette.RecetteId)).Select(x => new ModeleVueRecetteOutput {

                RecetteID = x.RecetteId,
                Nom = x.Nom,
                Description = x.Description,
                ComposantsPrincipaux = x.ComposantsPrincipaux,
                Image = x.Image,
                Mimetype = x.Mimetype,

            });
           
            
            return View(recettes_Utilisateur);
        }


        public IActionResult Create()
        {
            return View();
        }

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
                    var extension = "image/" + Path.GetExtension(fileName).Remove(0, 1);

                    await recetteInput.Image.CopyToAsync(memoryStream); // Copie le contenu du fichier dans le MemoryStream
                    byte[] image = memoryStream.ToArray(); // Convertit le MemoryStream en tableau de bytes

                    Recette recette = new Recette() { RecetteId = recetteInput.RecetteID, Nom = recetteInput.Nom, Description = recetteInput.Description, ComposantsPrincipaux = recetteInput.ComposantsPrincipaux, Image = image, Mimetype = extension };
                    RecetteUtilisateur recetteUtilisateur = new RecetteUtilisateur() { UtilisateurID = utilisateur.Id, RecetteId = recette.RecetteId, User = utilisateur, Recette = recette };
                    _context.Add(recette);
                    _context.Add(recetteUtilisateur);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recetteInput);
            
        }
        // GET: RecetteUtilisateurs/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recetteUtilisateur = await _context._recetteUtilisateur
                .Include(r => r.Recette)
                .FirstOrDefaultAsync(m => m.UtilisateurID == id);
            if (recetteUtilisateur == null)
            {
                return NotFound();
            }

            return View(recetteUtilisateur);
        }

        

        

        // GET: RecetteUtilisateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            string idUtilisateur = await RecupererIdUtilisteurCourant();
            var recetteUtilisateur = await _context._recette.FindAsync(id);
            if (recetteUtilisateur == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ModeleVueRecetteEdit output= new ModeleVueRecetteEdit() { RecetteId=recetteUtilisateur.RecetteId, Nom=recetteUtilisateur.Nom , ComposantsPrincipaux=recetteUtilisateur.ComposantsPrincipaux, Description=recetteUtilisateur.Description, Image=recetteUtilisateur.Image , Mimetype=recetteUtilisateur.Mimetype, ListeCommentaires=recetteUtilisateur.ListeCommentaires};
            //ViewData["RecetteID"] = new SelectList(_context._recette, "RecetteID", "RecetteID", recetteUtilisateur.RecetteId);
            return View(output);
        }

        // POST: RecetteUtilisateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("RecetteId,Nom,Description,ComposantsPrincipaux,NewImage,Mimetype,Image")] ModeleVueRecetteEdit input)
        {
            byte[]image = null;
            string extension = null;
            if (ModelState.IsValid)
            {
                if (input.NewImage == null)
                {
                   image = input.Image;
                }
                else {
                   

                    using (var memoryStream = new MemoryStream())
                    {
                        // Obtenir le nom d'origine du fichier
                        var fileName = input.NewImage.FileName;

                        // Extraire l'extension
                         extension="image/" + Path.GetExtension(fileName).Remove(0, 1);

                         await input.NewImage.CopyToAsync(memoryStream); // Copie le contenu du fichier dans le MemoryStream
                        image = memoryStream.ToArray(); // Convertit le MemoryStream en tableau de bytes

                    }
                        
                }
                if (input.NewImage != null)
                {
                    Recette recette = new Recette() { RecetteId = input.RecetteId, Nom = input.Nom, Description = input.Description, ComposantsPrincipaux = input.ComposantsPrincipaux, Mimetype = extension, Image = image };
                    _context.Update(recette);
                    await _context.SaveChangesAsync();
                }
                else {
                    Recette recette = new Recette() { RecetteId = input.RecetteId, Nom = input.Nom, Description = input.Description, ComposantsPrincipaux = input.ComposantsPrincipaux, Mimetype = input.Mimetype, Image = image };
                    _context.Update(recette);
                    await _context.SaveChangesAsync();
                }
                
               
                

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction("Index","RecetteUtilisateurs");
        }

        // GET: RecetteUtilisateurs/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            //var recetteUtilisateur = await _context._recetteUtilisateur
            // .Include(r => r.Recette)
            //.FirstOrDefaultAsync(m => m.UtilisateurID == id);
            int recetteId = Convert.ToInt32(id);
            var recetteUtilisateur = await _context._recetteUtilisateur.SingleOrDefaultAsync(p=>p.RecetteId== recetteId);
            var recette = await _context._recette.SingleOrDefaultAsync(p=>p.RecetteId==recetteId);
            if (recetteUtilisateur == null || recette==null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(recetteUtilisateur);
        }

        // POST: RecetteUtilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string idUtilisateurCourant = await RecupererIdUtilisteurCourant();
            var recetteUtilisateur = await _context._recetteUtilisateur.FindAsync(idUtilisateurCourant,id);
            var recette = await _context._recette.FindAsync(id);
            if (recetteUtilisateur != null && recette != null)
            {
                _context._recetteUtilisateur.Remove(recetteUtilisateur);
                _context._recette.Remove(recette);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetteUtilisateurExists(string id)
        {
            return _context._recetteUtilisateur.Any(e => e.UtilisateurID == id);
        }

        private Task<Utilisateur> GetCurrentUserAsync() =>
          _userManager.GetUserAsync(HttpContext.User);
        public async Task<string> RecupererIdUtilisteurCourant()
        {
            Utilisateur utilisateur = await GetCurrentUserAsync();
            return utilisateur?.Id;
        }
    }
}



