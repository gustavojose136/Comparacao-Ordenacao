using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ordenacao.Services
{
    public enum FileType
    {
        Text,
        Binary
    }

    public class DataGeneratorService
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Gera uma lista de números aleatórios.
        /// </summary>
        public List<int> GenerateRandomNumbers(int size, int min = 0, int max = 100)
        {
            var numbers = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                numbers.Add(_random.Next(min, max));
            }
            return numbers;
        }

        /// <summary>
        /// Salva a lista de números em um arquivo no formato especificado.
        /// </summary>
        public void SaveNumbersToFile(List<int> numbers, string filePath, FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Text:
                    SaveNumbersAsText(numbers, filePath);
                    break;
                case FileType.Binary:
                    SaveNumbersAsBinary(numbers, filePath);
                    break;
                default:
                    throw new ArgumentException("Tipo de arquivo inválido.");
            }
        }

        private void SaveNumbersAsText(List<int> numbers, string filePath)
        {
            var content = string.Join(",", numbers);
            File.WriteAllText(filePath, content);
        }

        private void SaveNumbersAsBinary(List<int> numbers, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(numbers.Count);
                foreach (var num in numbers)
                {
                    bw.Write(num);
                }
            }
        }
    }
}
