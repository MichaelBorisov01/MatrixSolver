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
            StreamReader sr = File.OpenText(input); //Инициализация переменной считывания(Открытие файла)

            sr.ReadLine(); //Считывание 1 строки

            for (int i = 0; i < row; i++) //Считывание строк
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
            StreamReader sr = File.OpenText(input); //Инициализация переменной считывания(Открытие файла)

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

        static double Mul(double[] rowData, double[] colData) //Операция умножения матриц
        {
            double result = 0;
            for (int i = 0; i < rowData.Length; i++) 
                result += rowData[i] * colData[i];
            return result;
        }

        static async Task<double> MulAsync(string inputLeft, string inputRight, int row, int col) //Асинхронный метод перемножения матриц
        {
            double[] rowData, colData; //Инициализация переменных для строк 1 матрицы и столбцов 2 матрицы

            rowData = await Task.Run(() => ReadRow(inputLeft, row)); //Выполнение асинхронной задачи по считыванию строк 1 матрицы

            colData = await Task.Run(() => ReadCol(inputRight, col)); //Выполнение асинхронной задачи по считыванию столбцов 2 матрицы

            return await Task.Run<double>(() => Mul(rowData, colData)); //Асинхронное возвращение результата умножения матриц 
        }

        public static async void Solve(string inputLeft, string inputRight, string output) 
        {
            StreamReader sr = File.OpenText(inputLeft); //Инициализация переменной sr для записи левой матрицы
            string[] input = sr.ReadLine().Split(' '); //Заполнение массива чисел-числами из матрицы
            int rowsLeft = int.Parse(input[0]); //Нахождение кол-ва строк левой матрицы
            int colsLeft = int.Parse(input[1]); //Нахождение кол-ва столбцов левой матрицы
            sr.Close();

            sr = File.OpenText(inputRight); //Запись правой матрицы в sr
            input = sr.ReadLine().Split(' '); //Заполнение массива чисел-числами из матрицы
            int rowsRight = int.Parse(input[0]);  //Нахождение кол-ва строк правой матрицы
            int colsRight = int.Parse(input[1]);  //Нахождение кол-ва столбцов правой матрицы
            sr.Close();

            if ((rowsLeft != colsRight) || (colsLeft != rowsRight)) //Если количество строк и столбцов не совпадает то вывести ошибку
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

                sw.WriteLine(String.Join(" ", results)); //Запись в файл соединённых символом " " значений
            }
            sw.Close();//Закрытие файла
        }
    }
}
