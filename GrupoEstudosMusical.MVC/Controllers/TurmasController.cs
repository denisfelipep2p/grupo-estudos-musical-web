﻿using AutoMapper;
using GrupoEstudosMusical.Bussines.Exceptions;
using GrupoEstudosMusical.Models.Entities;
using GrupoEstudosMusical.Models.Interfaces.Bussines;
using GrupoEstudosMusical.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GrupoEstudosMusical.MVC.Controllers
{
    public class TurmasController : Controller
    {
        private readonly IBussinesTurma _bussinesTurma;
        private readonly IBussinesModulo _bussinesModulo;
        private readonly IBussinesProfessor _bussinesProfessor;
        private readonly IBussinesAvaliacaoTurma _bussinesAvaliacaoTurma;
        public TurmasController(IBussinesTurma bussinesTurma, IBussinesModulo bussinesModulo, IBussinesProfessor bussinesProfessor,
            IBussinesAvaliacaoTurma bussinesAvaliacaoTurma)
        {
            _bussinesTurma = bussinesTurma;
            _bussinesModulo = bussinesModulo;
            _bussinesProfessor = bussinesProfessor;
            _bussinesAvaliacaoTurma = bussinesAvaliacaoTurma;
        }
        // GET: Turmas
        public async Task<ActionResult> Index()
        {
            var turmasViewModel = Mapper.Map<IList<Turma>, IList<TurmaVM>>(await _bussinesTurma.ObterTodosAsync());
            
            return View(turmasViewModel);

        }

        public async Task<ActionResult> VisaoGeral(int Id)
        {
            var obterDadosDaTurma = Mapper.Map<Turma, TurmaVM>(await _bussinesTurma.ObterPorIdAsync(Id));
            if (obterDadosDaTurma == null)
                return HttpNotFound();
            return View(obterDadosDaTurma);
        }

        public async Task<ActionResult> Novo()
        {
            await InicializarViewBagAsync();
            return View(new TurmaVM());
        }

        public ActionResult AdicionarAvaliacao(int Id)
        {
            return View(new AvaliacaoTurmaVM() { TurmaID = Id});
        }
        public async Task<ActionResult> Editar(int Id)
        {
            await InicializarViewBagAsync();
            var turma = Mapper.Map<Turma, TurmaVM>(await _bussinesTurma.ObterPorIdAsync(Id));

            if (turma == null)
            {
                return HttpNotFound();
            }
            return View(turma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(TurmaVM turma)
        {
            await InicializarViewBagAsync();
            try
            {
                var turmaModel = Mapper.Map<TurmaVM, Turma>(turma);
                await _bussinesTurma.AlterarAsync(turmaModel);
                TempData["Mensagem"] = "Turma alterada com sucessso";
                return RedirectToAction(nameof(Index));
            }catch(CrudTurmaException ex)
            {
                TempData["Mensagem"] = ex.Message;
                return View(turma);
            }

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Novo(TurmaVM entity)
        {
            await InicializarViewBagAsync();
            try
            {
                var turmaModel = Mapper.Map<TurmaVM, Turma>(entity);
                await _bussinesTurma.InserirAsync(turmaModel);
                TempData["Mensagem"] = "Turma cadastrada com sucesso";
                return RedirectToAction(nameof(Index));
            }catch(CrudTurmaException ex)
            {
                TempData["Mensagem"] = ex.Message;
                return View(entity);
            }
            
        }

        public async Task InicializarViewBagAsync()
        {
            var modulosViewModel = Mapper.Map<IList<Modulo>, IList<ModuloVM>>(await _bussinesModulo.ObterTodosAsync());
            var professoresViewModel = Mapper.Map<IList<Professor>, IList<ProfessorVM>>(await _bussinesProfessor.ObterTodosAsync());
            ViewBag.Modulos = modulosViewModel;
            ViewBag.Professores = professoresViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deletar(FormCollection formCollection)
        {
            int.TryParse(formCollection["Id"].ToString(), out var id);
            var turmaModel = await _bussinesTurma.ObterPorIdAsync(id);
            await _bussinesTurma.DeletarAsync(turmaModel);
            TempData["Mensagem"] = "Turma Apagada com Sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult TurmasAtivas(int moduloId, int alunoId)
        {
            try
            {
                var turmas = _bussinesTurma.ObterTurmasAtivasPorModulo(moduloId).ToList();
                var turmasMatricula = new List<TurmasMatriculaVM>();
                turmas.ForEach(turma =>
                {
                    turmasMatricula.Add(new TurmasMatriculaVM
                    {
                        Id = turma.Id,
                        Nome = turma.Nome,
                        QuantidadeAlunos = turma.QuantidadeAlunos,
                        QuantidadeMatriculas = turma.Matriculas.Count,
                        AlunoMatriculado = turma.Matriculas.Any(m => m.AlunoId == alunoId)
                    });
                });
                return Json(turmasMatricula, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

    }
}