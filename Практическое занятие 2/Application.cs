namespace pz2;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
public class Application
{
	// Исходный текст в текстовом виде
	public static string sourceText = "To_be_or_not_to_be?_This_is_a_question";
	// Исходные данные в битовом виде
	public static byte[] sourceBytes = ASCIIEncoding.ASCII.GetBytes(sourceText);
	// Объект для хеширования функций
    public static MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

    public static void Main(string[] args)
	{
        Console.WriteLine("Исходная строка: " + sourceText);
        Console.WriteLine("Исходная строка в 16-ой системе: " + getHexString(sourceBytes));

        byte[] sourceHash = CSP.ComputeHash(sourceBytes);
        Console.WriteLine("MD5 исходной строки в 16-ой системе: " + getHexString(sourceHash));


        int[] counts = new int[] { 1, 9, 19 };
        int[] diffs;
        byte[] noised;
        byte[] noisedHash;
        foreach (int count in counts)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Ошибки {count} кратности");
            diffs = new int[12];
            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine();
                sourceBytes = ASCIIEncoding.ASCII.GetBytes(sourceText);
                noised = noice(sourceBytes, 1, i);
                noisedHash = CSP.ComputeHash(noised);
                diffs[i] = codeDistance(sourceHash, noisedHash);
                Console.WriteLine("Строка: " + ASCIIEncoding.ASCII.GetString(noised));
                Console.WriteLine("MD5: " + getHexString(noisedHash));
                Console.WriteLine("Кодовое расстояние: " + diffs[i]);
            }
            Console.WriteLine();
            Console.WriteLine("Среднее: " + avg(diffs));
            Console.WriteLine("СКО: " + sco(diffs, avg(diffs)));
            Console.WriteLine("Min: " + min(diffs));
            Console.WriteLine("Max: " + max(diffs));
        }

    }

    // Выводит текст в бинарном формате
	public static string getBinaryString(byte[] bytes)
	{
        int i;
        StringBuilder sOutput = new StringBuilder(bytes.Length);
        for (i = 0; i < bytes.Length; i++)
        {
            sOutput.Append(Convert.ToString(bytes[i], 2));
        }
        return sOutput.ToString();
    }

    // Выводит текст в шестадцатеричном формате
    public static string getHexString(byte[] bytes)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(bytes.Length);
        for (i = 0; i < bytes.Length; i++)
        {
            sOutput.Append(bytes[i].ToString("X2"));
        }
        return sOutput.ToString();
    }

    // Возращает минимальный элемент массива
    public static int min(int[] diffs)
    {
        var res = diffs[0];
        for (int i = 1; i < diffs.Length; i++)
        {
            if (res > diffs[i])
            {
                res = diffs[i];
            }
        }
        return res;
    }

    // Возращает максимальный элемент массива
    public static int max(int[] diffs)
    {
        var res = diffs[0];
        for (int i = 1; i < diffs.Length; i++)
        {
            if (res < diffs[i])
            {
                res = diffs[i];
            }
        }
        return res;
    }

    // Возращает среднее арефметическое массива
    public static double avg(int[] diffs)
    {
        int sum = 0;
        for (int i = 0; i < diffs.Length; i++)
        {
            sum += diffs[i];
        }
        return sum / diffs.Length;
    }

    // Возращает СКО массива
    public static double sco(int[] diffs, double mean)
    {
        double sum = 0;
        for (int i = 0; i < diffs.Length; i++)
        {
            sum += Math.Pow(diffs[i] - mean, 2);
        }
        return Math.Sqrt(sum / (diffs.Length - 1));

    }

    // Возращает кодовое расстояние
    public static int codeDistance(byte[] expect, byte[] input)
    {
        int result = 0;
        for (int i = 0; i < expect.Length; i++)
        {
            for (int bit = 0; bit < 8; bit++)
            {
                int bit1 = expect[i] & (1 << bit);
                int bit2 = input[i] & (1 << bit);
                if (bit1 != bit2)
                {
                    result++;
                }
            }
        }
        return result;
    }

    // Добавляет помехи в сообщение
    public static byte[] noice(byte[] bytes, int count, int offset)
    {
        byte[] result = bytes;
        for (int j = 0; j < count; j++)
        {
            result[offset + j] = (byte)((byte)bytes[offset + j] ^ 1);
        }
        return result;
    }

}