namespace Sicp.Lisp.Tokens
{
    public class Token
    {
        public Token()
        {
        }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; set; }

        public string Value { get; set; }
    }
}