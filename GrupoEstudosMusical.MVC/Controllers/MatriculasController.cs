﻿using AutoMapper;
using GrupoEstudosMusical.Models.Entities;
using GrupoEstudosMusical.Models.Interfaces.Bussines;
using GrupoEstudosMusical.MVC.App_Start;
using GrupoEstudosMusical.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GrupoEstudosMusical.MVC.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly IBussinesTurma _bussinesTurma;
        private readonly IBussinesAluno _bussinesAluno;
        private readonly IBussinesMatricula _bussinesMatricula;

        public MatriculasController(IBussinesTurma bussinesTurma, IBussinesAluno bussinesAluno,
            IBussinesMatricula bussinesMatricula)
        {
            _bussinesTurma = bussinesTurma;
            _bussinesAluno = bussinesAluno;
            _bussinesMatricula = bussinesMatricula;
        }

        public async Task<ActionResult> Index(int idAluno)
        {
            var aluno = await _bussinesAluno.ObterPorIdAsync(idAluno);
            if (aluno == null)
                return HttpNotFound("Aluno não encontrado");

            IList<Matricula> alunosModel = await _bussinesMatricula.ObterMatriculasPorAluno(idAluno);
            var matriculasVM = Mapper.Map<IList<Matricula>, List<MatriculaListaVM>>(alunosModel);            

            return View(matriculasVM);
        }

        [HttpGet]
        [Route("Aluno/Matricular/{idAluno}", Name = "Matricular")]
        public async Task<ActionResult> Novo(int idAluno)
        {
            var matriculaVM = new MatriculaVM();
            var aluno = await _bussinesAluno.ObterPorIdAsync(idAluno);
            if (aluno == null)
                return HttpNotFound("Aluno não encontrado");

            matriculaVM.AlunoId = aluno.Id;
            matriculaVM.Aluno = Mapper.Map<Aluno, AlunoVM>(aluno);
            matriculaVM.Turmas = Mapper.Map<IList<Turma>, List<TurmaVM>>(await _bussinesTurma.ObterTodosAsync());
            return View(matriculaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Aluno/Matricular/{idAluno}")]
        public async Task<ActionResult> Novo(int idAluno, MatriculaVM matriculaVM)
        {
            try
            {
                var matriculaModel = Mapper.Map<MatriculaVM, Matricula>(matriculaVM);
                await _bussinesMatricula.InserirAsync(matriculaModel);
                TempData["Mensagem"] = "Aluno matrículado com sucesso.";
                return RedirectToAction("Index", "Alunos");
            }
            catch (ArgumentException ex)
            {
                ViewData["Mensagem"] = ex.Message;
                return View(matriculaVM);
            }
        }

        public async Task<ActionResult> Detalhes(int id)
        {
            var matriculaModel = await _bussinesMatricula.ObterPorIdAsync(id);
            if (matriculaModel == null)
                return HttpNotFound("Matrícula não encontrada");

            var matriculaVM = Mapper.Map<Matricula, MatriculaVM>(matriculaModel);
            return View(matriculaVM);
        }

        [HttpPost]
        public async Task<ActionResult> AlterarDocumentosApresentados(DocumentosApresentadosVM documentosApresentados)
        {
            try
            {
                var matriculaModel = await _bussinesMatricula.ObterPorIdAsync(documentosApresentados.IdMatricula);
                if (matriculaModel == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                matriculaModel.Cpf = documentosApresentados.Cpf;
                matriculaModel.Rg = documentosApresentados.Rg;
                matriculaModel.ComprovanteResidencia = documentosApresentados.ComprovanteResidencia;
                await _bussinesMatricula.AlterarAsync(matriculaModel);
                return Json(new { mensagem = "Sucesso", pendente = matriculaModel.Pendente }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}