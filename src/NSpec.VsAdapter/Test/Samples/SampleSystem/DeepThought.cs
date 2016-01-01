using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSystem
{
    public class DeepThought
    {
        public int AnswerQuestion()
        {
            return 42;
        }

        public string GetQuestion()
        {
            throw new InvalidTimeZoneException("You can't have both Question and Answer within a single Universe.");
        }

        public object BuildComputerForFindingQuestion()
        {
            return "Earth";
        }
    }
}
