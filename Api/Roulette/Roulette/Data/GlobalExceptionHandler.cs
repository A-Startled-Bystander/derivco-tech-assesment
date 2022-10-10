using System.Xml.Linq;

namespace Roulette.Data
{
    public class InvalidBetException : Exception
    {
        public InvalidBetException() { }

        public InvalidBetException(string exception) : base(String.Format("Error in  placing bet. Error Message: ", exception)) { }
    }

    public class InvalidHistoryException : Exception
    {
        public InvalidHistoryException() { }

        public InvalidHistoryException(string exception) : base("Error in viewing history. Error Message: " + exception) { }

    }


}
