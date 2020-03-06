using System;
using System.Collections.Generic;

namespace Proyecto1
{

    class Analizador
    {
        private string lexema = "";
        private bool cierre = false;
        private int filaL = 0;
        private int estado = 0;
        private bool continua = false;
        private int columna;
        public List<Tokens> lst_tkn = new List<Tokens>();
        public List<ErrorToken> lst_error = new List<ErrorToken>();
        public void analizadorLexico(string linea, int fila)
        {
            filaL = fila;
            char[] caracter;
            char codCaracter;
            int carac = 0;
            for (columna = 0; columna < linea.Length; columna++)
            {
                caracter = linea.ToCharArray();
                codCaracter = caracter[columna];
                carac = (int)caracter[columna];
                if (estado == 0 && continua == false)
                    estado = Iniciales(codCaracter);
                switch (estado)
                {
                    case 1:
                        if (char.IsLetterOrDigit(codCaracter))
                        {
                            lexema = lexema + codCaracter;
                            estado = 1;
                        }
                        else
                        {
                            if (reservadas(lexema) == true)
                            {
                                lst_tkn.Add(new Tokens(lexema, "Reservada", filaL, columna));
                                lexema = "";
                            }
                            else
                            {
                                lst_tkn.Add(new Tokens(lexema, "Identificador", filaL, columna));
                                lexema = "";
                            }
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 2:
                        if (char.IsDigit(codCaracter))
                        {
                            lexema = lexema + codCaracter;
                            estado = 2;
                        }
                        else
                        {
                            lst_tkn.Add(new Tokens(lexema, "numero", filaL, columna));
                            lexema = "";
                            estado = Iniciales(codCaracter);
                        }
                        break;

                    case 4:
                        if (carac == '"' && cierre == true)
                        {
                            lexema = lexema + codCaracter;
                            estado = 5;
                            continua = false;
                            cierre = false;
                        }
                        else
                        {
                            lexema = lexema + codCaracter;
                            estado = 4;
                            cierre = true;
                            continua = true;
                        }
                        break;
                    case 5:
                        lst_tkn.Add(new Tokens('"' + lexema, "cadena", filaL, columna));
                        lexema = "";
                        columna--;
                        estado = 0;
                        break;
                    case 6:
                        if (carac == '!')
                        {
                            lexema = "<!";
                            estado = 7;
                        }
                        break;
                    case 7:
                        if (carac == '>' && cierre == true)
                        {
                            lexema = lexema + codCaracter;
                            estado = 8;
                            continua = false;
                            cierre = false;
                        }
                        else
                        {
                            lexema = lexema + codCaracter;
                            estado = 7;
                            cierre = true;
                            continua = true;
                        }
                        break;
                    case 8:
                        lst_tkn.Add(new Tokens(lexema, "comentario multilinea", filaL, columna));
                        lexema = "";
                        estado = 0;
                        break;

                    case 10:
                        if (carac == 10 && cierre == true)
                        {
                            lexema = lexema + codCaracter;
                            estado = 11;
                            continua = false;
                            cierre = false;
                        }
                        else
                        {
                            lexema = lexema + codCaracter;
                            estado = 10;
                            cierre = true;
                            continua = true;
                        }
                        break;
                    case 11:
                        lst_tkn.Add(new Tokens(lexema, "Comentario de linea", filaL, columna));
                        lexema = "";
                        columna--;
                        estado = 0;
                        break;
                    case 12:
                        if (carac == '>')
                        {
                            lst_tkn.Add(new Tokens("->" + lexema, "flecha ", filaL, columna));
                            lexema = "";
                            estado = 0;
                        }
                        break;
                    case 13:
                        if (carac == ':')
                        {
                            estado = 14;
                        }
                        break;
                    case 14:
                        if (carac == ']' && cierre == true)
                        {
                            lexema = lexema + codCaracter;
                            estado = 15;
                            continua = false;
                            cierre = false;
                        }
                        else
                        {
                            lexema = lexema + codCaracter;
                            estado = 14;
                            cierre = true;
                            continua = true;
                        }
                        break;
                    case 15:
                        lst_tkn.Add(new Tokens("[:" + lexema, "Todo", filaL, columna));
                        lexema = "";
                        estado = 0;
                        break;
                    case 17:
                        if (carac == 'n')
                        {
                            lst_tkn.Add(new Tokens("salto", "salto de linea", filaL, columna));
                            estado = 0;
                        }
                        else if (carac == '"')
                        {
                            lst_tkn.Add(new Tokens("comilla", "comilla doble", filaL, columna));
                            estado = 0;
                        }
                        else if (carac == 't')
                        {
                            lst_tkn.Add(new Tokens("tabulacion", "tabulacion", filaL, columna));
                            estado = 0;
                        }
                        else if (carac == 39)
                        {
                            lst_tkn.Add(new Tokens("comilla", "comilla simple", filaL, columna));
                            estado = 0;
                        }
                        else
                        {
                            lst_tkn.Add(new Tokens(carac + "", "diagonal invertida", filaL, columna));
                            estado = 0;
                        }
                        break;
                    case 20:
                        if (carac == '"')
                        {
                            estado = 21;
                        }

                        break;
                    case 21:
                        if (carac == '~' || carac == ';')
                        {
                            lst_tkn.Add(new Tokens('"' + lexema, "comilla", filaL, columna));
                            lexema = "";
                            columna--;
                            estado = 0;
                        }
                        else
                        {
                            columna--;
                            estado = 4;
                        }
                        break;
                    case 22:
                        if (carac == '<')
                        {
                            estado = 23;
                        }

                        break;
                    case 23:
                        if (carac == '~' || carac == ';')
                        {
                            lst_tkn.Add(new Tokens('<' + lexema, "menor", filaL, columna));
                            lexema = "";
                            columna--;
                            estado = 0;
                        }
                        else
                        {
                            columna--;
                            estado = 6;
                        }
                        break;

                    case 24:
                        if (carac == '[')
                        {
                            estado = 25;
                        }

                        break;
                    case 25:
                        if (carac == '~' || carac == ';')
                        {
                            lst_tkn.Add(new Tokens('[' + lexema, "corchete", filaL, columna));
                            lexema = "";
                            columna--;
                            estado = 0;
                        }
                        else
                        {
                            columna--;
                            estado = 13;
                        }
                        break;

                    case 100:
                        lst_error.Add(new ErrorToken(lexema, "caracter invalido", filaL, columna));
                        lexema = "";
                        estado = 0;
                        break;
                }
            }
        }

        private int Iniciales(char codCaracter)
        {
            int caracter = (int)codCaracter;
            if (char.IsLetter(codCaracter))
            {
                return 1;
            }
            else if (char.IsDigit(codCaracter))
            {
                return 2;
            }
            else if (codCaracter == '"')
            {
                return 20;
            }
            else if (codCaracter == '<')
            {
                return 22;
            }
            else if (codCaracter == '/')
            {
                return 10;
            }
            else if (codCaracter == '-')
            {
                return 12;
            }
            else if (codCaracter == '[')
            {
                return 24;
            }
            else if (caracter == 92)
            {
                return 17;
            }
            else if (codCaracter == '!')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "simbolo ", filaL, columna));
                return 0;
            }
            else if (codCaracter == ':')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "Dos puntos ", filaL, columna));
                return 0;
            }
            else if (codCaracter == ',')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "coma ", filaL, columna));
                return 0;
            }
            else if (codCaracter == ';')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "punto y coma ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '~')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "simbolo ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '*')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "concatenacion ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '|')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "disyuncion ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '?')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "interrogacion ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '+')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "más ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '{')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "Llave Izquierda ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '}')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "Llave Derecha ", filaL, columna));
                return 0;
            }
            else if (codCaracter == '.')
            {
                lst_tkn.Add(new Tokens(codCaracter + "", "Concatenacion ", filaL, columna));
                return 0;
            }
            else if ((caracter > 32 && caracter <= 47) || (caracter >= 58 && caracter <= 64) || (caracter >= 91 && caracter <= 96) || (caracter >= 123 && caracter <= 125))
            {
                if (caracter == 34 || (caracter >= 42 && caracter <= 47) || (caracter >= 58 && caracter <= 60) || caracter == 63 || (caracter >= 92 && caracter <= 93) ||
                    (caracter >= 123 && caracter <= 125))
                {
                    return 0;
                }
                else
                {
                    lst_tkn.Add(new Tokens(codCaracter + "", "simbolo ", filaL, columna));
                    return 0;
                }
            }
            else if (caracter == 9 || caracter == 13 || caracter == 32 || caracter == 10)
            {
                return 0;
            }
            else
            {
                return 100;
            }
        }
        private bool reservadas(string lexema)
        {
            if (lexema.Equals("CONJ"))
            {
                return true;
            }
            else
                return false;
        }
    }
}