using LabRatos.Data;
using LabRatos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LabRatos.Controllers
{
    public class MatriculasController : Controller
    {


        private readonly EscolaContexto _context;
        public MatriculasController(EscolaContexto context)
        {
            _context = context;
        }
        // GET: MatriculaController
        public async Task<IActionResult> Index()
        {
            //AQui você pega as mattriculas cadastradas no banco de dados e joga para uma listagem(.ToList() ou .AsEnumberable() ou .AsQueryable() ou ....)
            var matriculas = _context.Matriculas.Include(s=>s.Curso).Include(w=>w.Estudante).AsEnumerable();

            //Aqui vc passa o que a sua view está esperando, se ela está esperando uma listagem, vc passa uma listagem
            return View(matriculas); 
        }

        // GET: MatriculaController/Details/5
        public ActionResult Details(int id)
        {
            //Aqui você vai pegar o registro que você quer detalhar.
            var matricula = _context.Matriculas.Include(s => s.Curso).Include(w => w.Estudante).Where(w=>w.MatriculaID == id).FirstOrDefaultAsync();

            //se ela espera um unico objeto, vc passa um unico objeto pois sua view espera um unico objeto.
            return View(matricula); //normalmente na primeira linha da sua view, tem o que ela espera. Visualizou agora pouco qdo mostrei o details e o index? sim
        }

        // GET: MatriculaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MatriculaController/Create
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

        // GET: MatriculaController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
   
           ViewBag.CursoID = _context.Cursos.Select(c => new SelectListItem()
            { Text = c.Titulo, Value = c.CursoID.ToString() }).ToList();

            ViewBag.EstudanteID = _context.Estudantes.Select(c => new SelectListItem()
            { Text = c.Nome, Value = c.EstudanteID.ToString() }).ToList();

            var matricula = await _context.Matriculas.SingleOrDefaultAsync(s => s.MatriculaID == id);
             //var matricula = _context.Matriculas.Include(s => s.Curso).Include(w => w.Estudante).Where(w=>w.MatriculaID == id).FirstOrDefaultAsync();

            return View(matricula);
        }



        // POST: MatriculaController/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var atualizarMatricula = await _context.Matriculas.SingleOrDefaultAsync(s => s.MatriculaID == id);
            if (await TryUpdateModelAsync<Matricula>(
                atualizarMatricula,
                "",
                s => s.Nota, s => s.CursoID, s => s.EstudanteID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Não foi possível salvar. " +
                    "Tente novamente, e se o problema persistir " +
                    "chame o suporte.");
                }
              
            }  
            return View(atualizarMatricula);
        }

        // GET: MatriculaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MatriculaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
