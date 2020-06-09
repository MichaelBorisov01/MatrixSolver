using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MatrixSolver
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Введите название файла с 1 матрицей: ");
            string inputLeft = Console.ReadLine();

            Console.WriteLine("Введите название файла со 2 матрицей: ");
            string inputRight = Console.ReadLine();

            Console.WriteLine("Введите название файла результата: ");
            string output = Console.ReadLine();

            MatrixSolver.Solve(inputLeft, inputRight, output);

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
