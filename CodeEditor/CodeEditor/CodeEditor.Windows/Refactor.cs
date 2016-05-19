using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using DTO;
namespace Refactor
{
    public class Refactor
    {
        static List<Command> list;
        public Refactor()
        {
            list = new List<Command>();
            Command c = new Command() { CommandText = "for", CommandColor = Colors.Blue};
        }
        public Command Refactoring(string str)
        {
            foreach(Command i in list)
            {
                if (str == i.CommandText)
                {
                    return i;
                }
            }
            return new Command() { CommandText = str, CommandColor = Colors.Black };
        }
    }
}
