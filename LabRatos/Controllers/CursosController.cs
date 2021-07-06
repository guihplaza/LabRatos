using LabRatos.Data;
using LabRatos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabRatos.Controllers
{
    public class CursosController : Controller
    {

        private readonly EscolaContexto _context;
        public CursosController(EscolaContexto context)
        {
            _context = context;
        } 
        // GET: CursoController

        // GET: Estudantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cursos.ToListAsync());
        }

        // GET: CursoController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var estudante = await _context.Estudantes
            //    .SingleOrDefaultAsync(m => m.EstudanteID == id);

            var curso = await _context.Cursos
                .Include(s => s.Matriculas)
                    .ThenInclude(e => e.Curso)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CursoID == id);

            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // GET: CursoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CursoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CursoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var curso = await _context.Cursos.FirstOrDefaultAsync(s => s.CursoID == id);

            return View(curso);
        }

        // POST: CursoController/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var atualizarCurso = await _context.Cursos.SingleOrDefaultAsync(s => s.CursoID == id);
            if (await TryUpdateModelAsync<Curso>(
                atualizarCurso,
                "",
                s => s.Titulo, s => s.Creditos))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Logar o erro (descomente a variável ex e escreva um log
                    ModelState.AddModelError("", "Não foi possível salvar. " +
                        "Tente novamente, e se o problema persistir " +
                        "chame o suporte.");
                }
            }
            return View(atualizarCurso);
        }

        // GET: CursoController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var curso = await _context.Cursos.FirstOrDefaultAsync(s => s.CursoID == id);


            return View(curso);
        }

        // POST: CursoController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CursoID == id);
            if (curso == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Logar o erro
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }
    }
}
