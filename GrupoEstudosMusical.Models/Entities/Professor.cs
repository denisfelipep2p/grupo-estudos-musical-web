﻿using System.Collections.Generic;

namespace GrupoEstudosMusical.Models.Entities
{
    public class Professor : BaseEntity
    {
        public Professor()
        {
            Turmas = new List<Turma>();
        }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public List<Turma> Turmas { get; set; }
    }
}
