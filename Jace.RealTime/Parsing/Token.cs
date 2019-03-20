namespace Jace.RealTime.Parsing
{
    public struct Token
    {
        public int StartPosition;
        
        public int Length;

        public TokenType TokenType;

        public object Value;
    }
}