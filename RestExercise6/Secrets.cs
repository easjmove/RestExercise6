using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestExercise6
{
    public class Secrets
    {
        //Remember to add this file to the .gitignore file sp you don't accidentally share yoyr azure DB username and password
        public static readonly string ConnectionString = "connections string from an Azure DB";
    }
}
