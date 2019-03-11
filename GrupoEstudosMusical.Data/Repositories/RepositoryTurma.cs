﻿using GrupoEstudosMusical.Models.Entities;
using GrupoEstudosMusical.Models.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace GrupoEstudosMusical.Data.Repositories
{
    public class RepositoryTurma : RepositoryGeneric<Turma>, IRepositoryTurma
    {
        public override async Task<IList<Turma>> ObterTodosAsync() =>
            await DbSet.Include(t => t.Professor)
                .Include(t => t.Modulo)
                .Include(t => t.Matriculas)
                .ToListAsync();

        public override async Task<Turma> ObterPorIdAsync(int id) =>
            await DbSet.Include(t => t.Professor)
                .Include(t => t.Modulo)
                .Include(t => t.Matriculas)
                .FirstOrDefaultAsync( t => t.Id==id);

        public Turma VerificarExistenciaDaTurmaPorNomePeriodoSemestre(string nomeTurma, int periodo, int semestre, int Id) =>
            DbSet.Where(t => t.Nome == nomeTurma & t.Periodo==periodo & t.Semestre==semestre & t.Id !=Id ).FirstOrDefault();

        public List<Turma> ObterTurmasDoAluno(int IdAluno)
        {
            var sql = @"Select * From Turmas
                           Inner Join matriculas
                                   On matriculas.TurmaId = turmas.Id
                           Inner Join alunos
                                   On alunos.Id = matriculas.Id
                         Where alunos.Id = @Id";
           
           
    }
}
