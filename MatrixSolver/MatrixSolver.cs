using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MatrixSolver
{
    class MatrixSolver
    {

        static double[] ReadRow(string input, int row) //Операция считывания строк
        {
            StreamReader sr = File.OpenText(input);

            sr.ReadLine();  

            for (int i = 0; i < row; i++)  
                sr.ReadLine();

            string[] data = sr.ReadLine().Split(' '); //Считывание строк чисел с разделением по пробелу

            sr.Close(); //Закрытие файла

            double[] rowData = new double[data.Length]; //Инициализация массива чисел в строке

            for (int i = 0; i < rowData.Length; i++)

                rowData[i] = double.Parse(data[i]); // Присваивание строке массива считанное значение

            return rowData;
        }

        static double[] ReadCol(string input, int col) //Операция считывания столбцов
        {
            StreamReader sr = File.OpenText(input);  

            string[] header = sr.ReadLine().Split(' ');//Считывание столбцов чисел с разделением по пробелу

            int rows = int.Parse(header[0]); //Присваивание кол-во столбцов = число значений в строке

            string[] data = new string[rows]; //Инициализация массива значений в строке

            for (int i = 0; i < rows; i++)

                data[i] = (sr.ReadLine()).Split(' ')[col]; //Инициализация матрицы row x col 

            sr.Close(); //Закрытие файла

            double[] colData = new double[data.Length]; //Инициализация массива чисел в строке

            for (int i = 0; i < colData.Length; i++)

                colData[i] = double.Parse(data[i]); // Присваивание строке массива считанное значение

            return colData;
        }

        static double Mul(double[] rowData, double[] colData)  
        {
            double result = 0;
            for (int i = 0; i < rowData.Length; i++) 
                result += rowData[i] * colData[i];
            return result;
        }

        static async Task<double> MulAsync(string inputLeft, string inputRight, int row, int col)  
        {
            double[] rowData, colData;  

            rowData = await Task.Run(() => ReadRow(inputLeft, row)); //Выполнение асинхронной задачи по считыванию строк 1 матрицы

            colData = await Task.Run(() => ReadCol(inputRight, col));  

            return await Task.Run<double>(() => Mul(rowData, colData)); //Асинхронное возвращение результата умножения матриц 
        }

        public static async void Solve(string inputLeft, string inputRight, string output) 
        {
            StreamReader sr = File.OpenText(inputLeft); //Инициализация переменной sr для записи левой матрицы
            string[] input = sr.ReadLine().Split(' '); //Заполнение массива чисел-числами из матрицы
            int rowsLeft = int.Parse(input[0]); //Нахождение кол-ва строк левой матрицы
            int colsLeft = int.Parse(input[1]); //Нахождение кол-ва столбцов левой матрицы
            sr.Close();

            sr = File.OpenText(inputRight); 
            input = sr.ReadLine().Split(' ');  
            int rowsRight = int.Parse(input[0]);  
            int colsRight = int.Parse(input[1]);   
            sr.Close();

            if ((rowsLeft != colsRight) || (colsLeft != rowsRight))  
                throw new Exception("Matrix cols and rows does not equal.");

            StreamWriter sw = new StreamWriter(output); // Инициализация файла результата
            for (int i = 0; i < rowsLeft; ++i)
            {
                Task<double>[] mulTasks = new Task<double>[colsRight]; //Инициализация переменной для получения асинхронного значения с размерностью = кол-ву столбцов 2 матрицы
                for (int j = 0; j < colsRight; ++j)
                {
                    mulTasks[j] = MulAsync(inputLeft, inputRight, i, j); //Получение значения умножения матриц из асинхронного метода
                }
                
                double[] results = await Task.WhenAll<double>(mulTasks); //Инициализация массива результатов 

                sw.WriteLine(String.Join(" ", results));  
            }
            sw.Close(); 
        }
    }
}
