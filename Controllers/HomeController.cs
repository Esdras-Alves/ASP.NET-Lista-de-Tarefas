using ListaDeTarefasProjeto.Data;
using ListaDeTarefasProjeto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ListaDeTarefasProjeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string id)
        {
            var filtros = new Filtros(id);

            ViewBag.Filtros = filtros;
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Status = _context.Status.ToList();
            ViewBag.Vencimento = Filtros.VencimentoValoresFiltro;

            IQueryable<Tarefa> consulta = _context.Tarefas.Include(x => x.Categoria).Include(x => x.Status);


            if (filtros.TemCategoria)
            {
                consulta = consulta.Where(x => x.CategoriaId == filtros.CategoriaId);
            }

            if (filtros.TemStatus)
            {
                consulta = consulta.Where(x => x.StatusId == filtros.StatusId);
            }

            if (filtros.TemVencimento)
            {
                var hoje = DateTime.Today;

                if (filtros.EPassado)
                {
                    consulta = consulta.Where(x => x.DataDeVencimento < hoje);
                }

                if (filtros.EFuturo)
                {
                    consulta = consulta.Where(x => x.DataDeVencimento > hoje);
                }

                if (filtros.EHoje)
                {
                    consulta = consulta.Where(x => x.DataDeVencimento == hoje);
                }
            }

            var tarefas = consulta.OrderBy(x => x.DataDeVencimento).ToList();


            return View(tarefas);
        }

        public IActionResult Adicionar()
        {
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Status = _context.Status.ToList();

            var tarefa = new Tarefa { StatusId = "aberto" };

            return View(tarefa);
        }

        [HttpPost]
        public IActionResult Filtrar(string[] filtro)
        {
            string id = string.Join('-', filtro);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult MarcarCompleto([FromRoute] string id, Tarefa tarefaSelecionada)
        {
            tarefaSelecionada = _context.Tarefas.Find(tarefaSelecionada.Id);

            if (tarefaSelecionada != null)
            {
                tarefaSelecionada.StatusId = "completo";
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult Adicionar(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {

                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Categorias = _context.Categorias.ToList();
                ViewBag.Status = _context.Status.ToList();

                return View(tarefa);
            }
        }

        [HttpPost]
        public IActionResult DeletarCompletos(string id)
        {
            var paraDeletar = _context.Tarefas.Where(x => x.StatusId == "completo").ToList();

            foreach (var tarefa in paraDeletar)
            {
                _context.Tarefas.Remove(tarefa);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", new { ID = id });
        }

    }
}