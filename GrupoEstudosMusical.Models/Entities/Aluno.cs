﻿using System;
using System.Collections.Generic;

namespace GrupoEstudosMusical.Models.Entities
{
    public class Aluno : BaseEntity
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public List<Matricula> Matriculas { get; set; }
        public List<Ocorrencia> Ocorrencias { get; set; }
    }
}
