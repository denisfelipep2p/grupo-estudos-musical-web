﻿using GrupoEstudosMusical.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GrupoEstudosMusical.MVC.Models
{
    public class TurmaVM
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        [DisplayName("Data Início")]
        public DateTime DataInicio { get; set; }

        [DisplayName("Término da Aula")]
        public DateTime TerminoAula { get; set; }

        [DisplayName("Período")]
        public int Periodo { get; set; }

        [DisplayName("Nível")]
        public string Nivel { get; set; }

        [DisplayName("Professor")]
        public int ProfessorID { get; set; }

        public string Status { get; set; }
        
        [DisplayName("Módulo")]
        public int ModuloId { get; set; }
        
        public Professor Professor { get; set; }

        public Modulo Modulo { get; set; }

        [DisplayName("Quantidade de Alunos")]
        public int QuantidadeAlunos { get; set; }

        public int Semestre { get; set; }

        public List<Matricula> Matriculas { get; set; }
    }
}