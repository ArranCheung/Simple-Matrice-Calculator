using System;
using System.Collections.Generic;

namespace Matrix_Calculator
{
    class Calculate
    {
        public double[,] Add(double[,] Values1, double[,] Values2, int option, bool display = true)
        {
            double[,] Addition = new double[Values1.GetLength(0), Values1.GetLength(1)];
            for (int i = 0; i < Values1.GetLength(0); i++)
            {
                for (int j = 0; j < Values1.GetLength(1); j++)
                {
                    if (option == 1)
                    {
                        Addition[i, j] = Values1[i, j] + Values2[i, j];
                    }
                    else
                    {
                        Addition[i, j] = Values1[i, j] - Values2[i, j];
                    }

                    if (display)
                    {
                        Console.Write($"{Addition[i, j]} ");
                    }
                }
                if (display) Console.WriteLine();
            }

            return Addition;
        }

        public double[,] Multiply(double[,] Values1, double[,] Values2, bool display = true)
        {
            double[,] Multiplied = new double[Values1.GetLength(0), Values2.GetLength(1)];

            for (int i = 0; i < Values1.GetLength(0); i++)
            {
                for (int j = 0; j < Values2.GetLength(1); j++)
                {
                    double value = 0;
                    for (int k = 0; k < Values1.GetLength(0); k++)
                    {
                        value += Values1[i, k] * Values2[k, j];
                    }
                    Multiplied[i, j] = value;
                    if (display) Console.Write($"{value} ");
                }
                if (display) Console.WriteLine();
            }

            return Multiplied;
        }

        public void Display(double[,] Values)
        {
            for (int i = 0; i < Values.GetLength(0); i++)
            {
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    Console.Write($"{Values[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }

    abstract class Matrix
    {
        static Calculate Calc = new Calculate();

        public int rows;
        public int columns;
        public double[,] MatValues { get; set; }
        public string Name { get; set; }

        public Matrix(double[,] values)
        {
            MatValues = values;
            rows = values.GetLength(0);
            columns = values.GetLength(1);
        }

        public double[,] Transpose(double[,] Values, bool display = false)
        {
            double[,] Transposed = new double[Values.GetLength(1), Values.GetLength(0)];

            for (int i = 0; i < Values.GetLength(0); i++)
            {
                for (int j = 0; j < Values.GetLength(0); j++)
                {
                    Transposed[j, i] = Values[i, j];
                }
            }

            return Transposed;
        }

        public void display()
        {
            for (int i = 0; i < MatValues.GetLength(0); i++)
            {
                for (int j = 0; j < MatValues.GetLength(1); j++)
                {
                    Console.Write($"{MatValues[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }

    interface ISquareMat
    {
        double Det(bool display);
        double[,] Inverse(bool display);
    }

    class DMat : Matrix
    {
        public DMat(double[,] values) : base(values) { }
    }

    class mat2x2 : Matrix, ISquareMat
    {
        public mat2x2(double[,] values) : base(values) { }
        public double Det(bool display = false)
        {
            double det;
            det = MatValues[0, 0] * MatValues[1, 1] - MatValues[0, 1] * MatValues[1, 0];

            if (display) { Console.WriteLine(det); }

            return det;
        }
        public double[,] Inverse(bool display = false)
        {
            double[,] values = new double[2, 2];
            double det = Det();

            if (det == 0)
            {
                Console.WriteLine("Cannot invert, singular matrix error");
                return new double[0, 0];
            }

            for (int i = 0; i < MatValues.GetLength(0); i++)
            {
                for (int j = 0; j < MatValues.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        values[i, j] = MatValues[(i + 1) % 2, (j + 1) % 2];
                    }
                    else
                    {
                        values[i, j] = 0 - MatValues[i, j];
                    }
                }
            }

            if (display)
            {
                for (int i = 0; i < MatValues.GetLength(0); i++)
                {
                    for (int j = 0; j < MatValues.GetLength(1); j++)
                    {
                        Console.WriteLine(MatValues[i, j]);
                    }
                }
            }

            return values;
        }
    }

    class mat3x3 : Matrix, ISquareMat
    {
        public mat3x3(double[,] values) : base(values) { }

        private double DetMinorMajor(double[,] values)
        {
            double det;
            det = values[0, 0] * values[1, 1] - values[0, 1] * values[1, 0];

            return det;
        }
        public double Det(bool display = false)
        {
            double det = 0;

            for (int i = 0; i < 3; i++)
            {
                double[,] DetMat = new double[2, 2];
                int x = 0; int y = 0;

                for (int j = 1; j < MatValues.GetLength(0); j++)
                {
                    for (int k = 0; k < MatValues.GetLength(1); k++)
                    {
                        if (k != i)
                        {
                            DetMat[y, x] = MatValues[j, k];
                            x++;
                            if (x >= 2)
                            {
                                x = 0;
                                y++;
                            }
                        }
                    }
                }

                double Mdet = DetMinorMajor(DetMat);
                double multiplier = MatValues[0, i];
                if (i % 2 == 1)
                {
                    multiplier = 0 - multiplier;
                }
                det += multiplier * Mdet;
            }

            if (display) { Console.WriteLine(det); }

            return det;
        }

        public double[,] Inverse(bool display = false)
        {
            double[,] C = new double[3, 3];

            double det = Det();

            if (det == 0)
            {
                Console.WriteLine("Cannot invert, singular matrix error");
                return new double[0, 0];
            }

            int x = 0; int y = 0;
            for (int t = 0; t < 9; t++)
            {
                int CRow = 0; int CCol = 0;
                double[,] SmallMat = new double[2, 2];

                for (int i = 0; i < MatValues.GetLength(0); i++)
                {
                    for (int j = 0; j < MatValues.GetLength(1); j++)
                    {
                        if (i != y && j != x)
                        {
                            SmallMat[CRow, CCol] = MatValues[i, j];
                            CCol++;
                            if (CCol >= 2)
                            {
                                CCol = 0;
                                CRow++;
                            }
                        }
                    }
                }

                double SDet = DetMinorMajor(SmallMat);
                if ((x + y) % 2 == 1)
                {
                    SDet = 0 - SDet;
                }
                C[y, x] = SDet;

                x++;
                if (x == 3)
                {
                    x = 0;
                    y++;

                    if (y == 3)
                    {
                        break;
                    }
                }
            }

            double[,] Ct = Transpose(C);

            for (int i = 0; i < C.GetLength(0); i++)
            {
                for (int j = 0; j < C.GetLength(1); j++)
                {
                    Ct[i, j] = Ct[i, j] * det;
                }
            }

            if (display)
            {
                for (int i = 0; i < Ct.GetLength(0); i++)
                {
                    for (int j = 0; j < Ct.GetLength(1); j++)
                    {
                        Console.Write($"{Ct[i, j]} ");
                    }
                    Console.WriteLine();
                }
            }

            return Ct;
        }
    }

    internal class Program
    {
        static dynamic CreateMat()
        {
            Console.WriteLine("Enter matrix name");
            string name = Console.ReadLine();
            Console.WriteLine();

            List<string> RowValues = EnterValues();
            double[,] MatrixValues = InitialMat(RowValues);
            InsertMat(MatrixValues, RowValues);
            var Mat = DetermineType(MatrixValues);
            Mat.Name = name;

            return Mat;
        }

        static List<string> EnterValues()
        {
            Console.WriteLine("Enter the matrix row by row");
            Console.WriteLine("Type 'end' to end");

            List<string> RowValues = new List<string>();

            // take the rows

            string option;
            do
            {
                Console.Write($"Row {RowValues.Count}: ");
                option = Console.ReadLine();
                if (option.ToLower() != "end")
                {
                    RowValues.Add(option);
                }
            } while (option.ToLower() != "end");

            Console.WriteLine();

            return RowValues;
        }

        static double[,] InitialMat(List<string> RowValues)
        {
            // put into a matrix array
            // determining the size of the array

            int rows = RowValues.Count;
            int cols = 0;

            foreach (string row in RowValues)
            {
                string[] colValues = row.Split(' ');
                if (cols < colValues.Length)
                {
                    cols = colValues.Length;
                }
            }

            double[,] MatrixValues = new double[rows, cols];

            return MatrixValues;
        }

        static void InsertMat(double[,] MatrixValues, List<string> RowValues)
        {
            // adding the values to the matrix

            for (int i = 0; i < MatrixValues.GetLength(0); i++)
            {
                string[] colValues = RowValues[i].Split(' ');

                for (int j = 0; j < MatrixValues.GetLength(1); j++)
                {
                    if (j < colValues.Length)
                    {
                        MatrixValues[i, j] = Convert.ToInt32(colValues[j]);
                    }
                    else
                    {
                        MatrixValues[i, j] = 0;
                    }
                }
            }
        }

        static dynamic DetermineType(double[,] MatrixValues)
        {
            if (MatrixValues.GetLength(0) == MatrixValues.GetLength(1))
            {
                switch (MatrixValues.GetLength(0))
                {
                    case 2:
                        mat2x2 SMat2 = new mat2x2(MatrixValues);
                        return SMat2;
                    case 3:
                        mat3x3 SMat3 = new mat3x3(MatrixValues);
                        return SMat3;
                    default:
                        DMat SMat = new DMat(MatrixValues);
                        return SMat;
                }
            }
            else
            {
                DMat Mat = new DMat(MatrixValues);
                return Mat;
            }
        }

        static void DisplayMats(List<dynamic> Mats, bool display = false)
        {
            foreach (var mat in Mats)
            {
                if (display)
                {
                    Console.Write($"{Mats.IndexOf(mat)}: ");
                }
                Console.WriteLine($"{mat.Name} : {mat.GetType()}");
            }
        }

        static void CalculateMenu(List<dynamic> Mats, dynamic Mat)
        {
            Calculate Calculator = new Calculate();

            Console.WriteLine();
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Subtract");
            Console.WriteLine("3. Multiply");
            if (Mat.rows == Mat.columns)
            {
                Console.WriteLine("4. Inverse");
                Console.WriteLine("5. Get Determinant");
                Console.WriteLine("6. Transpose");
            }

            int option = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (option)
            {
                case 1:
                case 2:
                    foreach (var mat in Mats)
                    {
                        if (mat.rows == Mat.rows && mat.columns == Mat.columns)
                        {
                            Console.WriteLine($"{Mats.IndexOf(mat)}. {mat.Name}");
                        }
                    }

                    int AChoice = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    Calculator.Add(Mats[AChoice].MatValues, Mat.MatValues, option);

                    break;
                case 3:
                    foreach (var mat in Mats)
                    {
                        if (mat.rows == Mat.columns)
                        {
                            Console.WriteLine($"{Mats.IndexOf(mat)}. {mat.Name}");
                        }
                    }

                    int MChoice = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    Calculator.Multiply(Mat.MatValues, Mats[MChoice].MatValues);

                    break;
                case 4:
                    Mat.Inverse(true);
                    break;
                case 5:
                    Mat.Det(true);
                    break;
                case 6:
                    Mat.Transpose(Mat.MatValues);
                    break;
            }
        }

        static void Main(string[] args)
        {
            List<dynamic> Mats = new List<dynamic>();

            string option;
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine();
                Console.WriteLine("1. Create new matrix");
                Console.WriteLine("2. View matrices");
                Console.WriteLine("3. Calculate");
                Console.WriteLine("N. exit program");
                Console.WriteLine();
                Console.ResetColor();

                option = Console.ReadLine().ToUpper();
                switch (option)
                {
                    case "1":
                        var Mat = CreateMat();
                        Mats.Add(Mat);
                        Console.WriteLine($"Mat {Mat.Name} has been created");
                        break;

                    case "2":
                        DisplayMats(Mats);
                        break;

                    case "3":
                        Console.WriteLine("Choose a matrix");
                        DisplayMats(Mats, true);

                        int MatChoice = int.Parse(Console.ReadLine());
                        var Mat1 = Mats[MatChoice];
                        CalculateMenu(Mats, Mat1);

                        break;
                    case "N":
                        Console.WriteLine("Program terminating");
                        break;
                }

            } while (option != "N");

            Console.ReadKey();
        }
    }
}