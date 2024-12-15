using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace matricecalc
{
    internal class Program
    {
        static int[,] calc_add(string[,] matrixA, string[,] matrixB)
        {   
            int[,] result = new int[2, 2];
            return result;
        }
        static int[,] calc_subtract(string[,] matrixA, string[,] matrixB)
        {
            int[,] result = new int[2, 2];
            return result;
        }
        static int[,] calc_multiply(string[,] matrixA, string[,] matrixB, int rows_a, int columns_a, int rows_b, int columns_b)
        {
            int[,] result = new int[rows_a, columns_b]; int r_value = 0, c_value = 0;

            for (int i = 0; i < rows_a; i++)
            {
                for (int z = 0; z < columns_b; z++)
                {
                    int value = 0;
                    for (int j = 0; j < rows_b && j < columns_a; j++)
                    {
                        value += int.Parse(matrixA[i, j]) * int.Parse(matrixB[j, z]);
                    }
                    result[r_value, c_value] = value; c_value++;
                }
                r_value++; c_value = 0;
            }
            return result;
        }

        static int[,] calc_inverse(string[,] matrixA, int row_a , int column_a)
        {
            double discrim = 1 / ((int.Parse(matrixA[0, 0]) * int.Parse(matrixA[1, 1])) - (int.Parse(matrixA[0, 1]) * int.Parse(matrixA[1, 0])));
            string temp = matrixA[0,0];
            matrixA[0, 0] = matrixA[1, 1]; matrixA[1, 1] = temp; matrixA[0, 1] = (1 - Convert.ToInt32(matrixA[0, 1])).ToString(); matrixA[1,0] = (1 - Convert.ToInt32(matrixA[0, 1])).ToString();
            for (int j = 0; j < row_a; j++)
            {
                for (int k = 0; k < column_a; k++)
                {
                    matrixA[j, k] = (discrim * Convert.ToDouble(matrixA[j,k])).ToString();
                }
            }

            int[,] result = new int[column_a,row_a];
            for(int j = 0; j < column_a; j++)
            {
                for(int k = 0;k < row_a; k++)
                {
                    result[j, k] = Convert.ToInt32(matrixA[j, k]);
                }
            }
            return result;
        }

        static void print_matrice(int[,] result)
        {
            Console.WriteLine();
            for (int i = 0; i < result.GetLength(0); i++)
            {
                string v = "";
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    v += (Convert.ToString(result[i, j]) + " ");
                }
                Console.WriteLine(v);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Select option:\n1. Add \n2. Subtract\n3. Multiply\n4. Inverse");
            int option = int.Parse(Console.ReadLine());

            bool pass = false; int length = 2; string dimenA = ""; string dimenB = ""; int row_a = 0; int row_b = 0; int column_a = 0; int column_b = 0;
            string[,] matrixA; string[,] matrixB; int[,] result;

            List<int> dimens = new List<int>();

            if (option == 1 || option == 2 || option == 4)
            {
                Console.Write("Enter dimensions of A: "); dimenA = Console.ReadLine(); row_a = int.Parse(dimenA.Substring(0, 1)); column_a = int.Parse(dimenA.Substring(2));
                Console.Write("Enter dimensions of B: "); dimenB = Console.ReadLine(); row_b = int.Parse(dimenB.Substring(0, 1)); column_b = int.Parse(dimenB.Substring(2));

                matrixA = new string[row_a, column_a]; matrixB = new string[row_b, column_b];
                dimens.AddRange(new List<int>() { column_a, row_a, column_b, row_b });
                pass = true;
            }
            else if (option == 3)
            {
                length = 1; pass = true;
                Console.Write("Enter dimensions of A: "); dimenA = Console.ReadLine(); row_a = int.Parse(dimenA.Substring(0, 1)); column_a = int.Parse(dimenA.Substring(2));
                matrixA = new string[row_a, column_a]; matrixB = null;
                dimens.AddRange(new List<int>() { column_a, row_a });
            }
            else
            {
                Console.WriteLine("Invalid selection");
                matrixA = null; matrixB = null;
            }

            if (pass)
            {
                for (int a = 1; a <= length; a++)
                {
                    if (a == 1) { Console.WriteLine("\nMatrice A"); } else { Console.WriteLine("\nMatrice B"); }
                    for (int i = 0; i < dimens[a * 2 - 1]; i++)
                    {
                        Console.Write("row " + (i + 1) + ": ");
                        string row = Console.ReadLine();
                        for (int j = 0; j < dimens[a * 2 - 2]; j++)
                        {
                            if (a == 1) { matrixA[i, j] = row.Split(' ')[j]; } else { matrixB[i, j] = row.Split(' ')[j]; }
                        }
                    }
                }
                switch (option)
                {
                    case 1: // add
                        if (column_a == column_b && row_a == row_b)
                        {
                            result = calc_add(matrixA, matrixB);
                            print_matrice(result);
                            break;
                        }
                        else { break; }
                    case 2: // subtract
                        if (column_a == column_b && row_a == row_b)
                        {
                            result = calc_subtract(matrixA, matrixB);
                            print_matrice(result);
                            break;
                        }
                        else { break; }
                    case 3: // multiply
                        if (column_a == row_b)
                        {
                            result = calc_multiply(matrixA, matrixB, row_a, column_a, row_b, column_b);
                            print_matrice(result);
                            break;
                        }
                        else { break; }
                    case 4: // inverse
                        result = calc_inverse(matrixA, row_a, column_a);
                        print_matrice(result);
                        break;
                }
            }
            else { Console.WriteLine("Error"); }


            Console.ReadKey();
        }
    }
}
