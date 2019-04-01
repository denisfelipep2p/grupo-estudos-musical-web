﻿

using System;

namespace GrupoEstudosMusical.Models.Entities
{
    public class AvaliacaoTurma:BaseEntity
    {
        public AvaliacaoTurma()
        {
            IdAvaliacaoTurma = Guid.NewGuid();
        }
        public Guid IdAvaliacaoTurma { get; set; }
        public virtual Turma Turma { get; set; }
        public int TurmaID { get; private set; }
        public virtual Avaliacao Avaliacao { get; set; }
        public int AvaliacaoID { get;  set; }
        public DateTime DataPrevista { get;  set; }
        public DateTime DataRealizacao { get;  set; }
    }
}
