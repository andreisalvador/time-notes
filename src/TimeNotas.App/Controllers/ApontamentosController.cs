using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeNotas.App.Models;

namespace TimeNotas.App.Controllers
{
    public class ApontamentosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private static List<Apontamento> apontamentos = new List<Apontamento>();

        public ApontamentosController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(new Apontamentos(apontamentos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Apontamento apontamento)
        {
            if (ModelState.IsValid)
            {
                //IdentityUser identityUser = await _userManager.GetUserAsync(User);

                //if (identityUser is null) throw new ArgumentException($"Usuário não encontrado na base de dados.");



                Apontamento novoApontamento = new Apontamento()
                {
                    DataHora = apontamento.DataHora,
                    UserId = Guid.NewGuid()//Guid.Parse(identityUser?.Id)
                };

                apontamentos.Add(novoApontamento);
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
            => View(new Apontamento());

        public IActionResult Edit(Guid apontamentoId)
        {
            return View(apontamentos.FirstOrDefault(a => a.Id.Equals(apontamentoId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Apontamento apontamento)
        {
            try
            {
                apontamentos.RemoveAll(a => a.Id.Equals(apontamento.Id));
                apontamentos.Add(apontamento);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApontamentosController/Delete/5
        public ActionResult Delete(Guid apontamentoId)
        {
            return View(apontamentos.FirstOrDefault(a => a.Id.Equals(apontamentoId)));
        }

        // POST: ApontamentosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Apontamento apontamento)
        {
            try
            {
                apontamentos.RemoveAll(a => a.Id.Equals(apontamento.Id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
