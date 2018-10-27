namespace SyntaxAnalyzer
{
    public class Token
    {
        public Associativity assoc = Associativity.Left;
        public int precedence = 0;
        public int parameterCount = 0;
        public bool value = false;
        public TokenType tokenType = TokenType.Nothing;
        public char symbol = '0';



        public enum TokenType
        {
            Nothing,
            Number,
            Operator,
            LeftBrace,
            RightBrace
        };

        public enum Associativity
        {
            Left,
            Right
        };

        public Token() { }

        //private TokenType GetTokenType => tokenType;
        public void SetValues(char symbol, Associativity assoc, int prec, int paramCount)
        {
            this.symbol = symbol;
            this.assoc = assoc;
            this.precedence = prec;
            this.parameterCount = paramCount;
        }

        public static Token StringToToken(string str)
        {
            Token t = new Token();
            if (str == "0" || str == "1")
            {
                t.tokenType = TokenType.Number;
                t.symbol = str == "0" ? '0' : '1';
                t.value = str == "0" ? false : true;
            }
            else if (str == "(")
            {
                t.tokenType = TokenType.LeftBrace;
            }
            else if (str == ")")
            {
                t.tokenType = TokenType.RightBrace;
            }
            else
            {
                switch (str)
                {

                    case "!":
                        t.tokenType = TokenType.Operator;
                        t.symbol = '!';
                        t.assoc = Associativity.Left;
                        t.precedence = 6;
                        t.parameterCount = 1;
                        break;
                    case "&":
                        t.tokenType = TokenType.Operator;
                        t.symbol = '&';
                        t.assoc = Associativity.Left;
                        t.precedence = 5;
                        t.parameterCount = 2;
                        break;
                    case "V":
                        t.tokenType = TokenType.Operator;
                        t.symbol = 'V';
                        t.assoc = Associativity.Left;
                        t.precedence = 4;
                        t.parameterCount = 2;
                        break;
                    case ">":
                        t.tokenType = TokenType.Operator;
                        t.symbol = '>';
                        t.assoc = Associativity.Left;
                        t.precedence = 3;
                        t.parameterCount = 2;
                        break;
                    case "~":
                        t.tokenType = TokenType.Operator;
                        t.symbol = '~';
                        t.assoc = Associativity.Left;
                        t.precedence = 2;
                        t.parameterCount = 2;
                        break;
                    case "+":
                        t.tokenType = TokenType.Operator;
                        t.symbol = '+';
                        t.assoc = Associativity.Left;
                        t.precedence = 1;
                        t.parameterCount = 2;
                        break;
                    case "|":
                        t.tokenType = TokenType.Operator;
                        t.symbol = '|';
                        t.assoc = Associativity.Left;
                        t.precedence = 0;
                        t.parameterCount = 2;
                        break;
                    default:
                        t.tokenType = TokenType.Nothing;
                        break;
                }
            }
            return t;
        }
    }
}
