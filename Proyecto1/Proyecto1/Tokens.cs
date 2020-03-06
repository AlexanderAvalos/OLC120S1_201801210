using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1
{
    class Tokens
    {
        public string lexema;
        public string lexico;
        public int Fila;
        public int Columna;

        public Tokens(string lexema, string lexico, int fila, int columna)
        {
            this.lexema = lexema;
            this.lexico = lexico;
            Fila = fila;
            Columna = columna;
        }
    }
    class ErrorToken
    {
        public string lexema;
        public string Descripcion;
        public int fila;
        public int columna;

        public ErrorToken(string lexema, string descripcion, int fila, int columna)
        {
            this.lexema = lexema;
            Descripcion = descripcion;
            this.fila = fila;
            this.columna = columna;
        }
    }
}
