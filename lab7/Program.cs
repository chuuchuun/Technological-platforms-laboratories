using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;



    public static class Extensions
    {
        public static FileInfo GetOldestFile(this DirectoryInfo directoryInfo)
        {

            FileSystemInfo[] fileSystemInfo = directoryInfo.GetFileSystemInfos();
            FileInfo oldestFile = null;
        foreach (var info in fileSystemInfo)
        {
            FileInfo newFile = null;
            if (info is FileInfo fileInfo)
            {
                newFile = fileInfo;
            }
            else if (info is DirectoryInfo directoryInfo2)
            {
                newFile = directoryInfo2.GetOldestFile();
            }

            if (oldestFile == null || (newFile != null && newFile.CreationTime < oldestFile.CreationTime))
            {
                oldestFile = newFile;
            }
        }
           
            return oldestFile;
        }

        public static string GetDOSAttributes(this FileSystemInfo fileSystemInfo)
        {
            FileAttributes attributes = fileSystemInfo.Attributes;
            string? result = null;
            if (attributes.HasFlag(FileAttributes.ReadOnly)){
                result += "r";
            }
            else
            {
                result += "-";
            }
            if (attributes.HasFlag(FileAttributes.Archive))
            {
                result += "a";
            }
            else
            {
                result += "-";
            }
            if (attributes.HasFlag(FileAttributes.Hidden))
            {
                result += "h";
            }
            else
            {
                result += "-";
            }
            if (attributes.HasFlag(FileAttributes.System))
            {
                result += "s";
            }
            else
            {
                result += "-";
            }
            return result;
        }
    }

    [Serializable]
    public class StringComparator : IComparer<string>
    {

        public int Compare(string x, string y)
        {
            int difference = x.Length.CompareTo(y.Length);
            if (difference != 0)
            {
                return difference;
            }
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }

    internal class Program
    {

        static void SerializeData(SortedDictionary<string, long> data, string dir)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(dir, FileMode.Create))
            {
                formatter.Serialize(fileStream, data);
            }
        }

        static SortedDictionary<string, long> DeserializeData(string dir)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var fileStream = new FileStream(dir, FileMode.Open))
            {
                return (SortedDictionary<string, long>)formatter.Deserialize(fileStream);
            }
        }

        static void ListAllFilesShifted(string directory, int indent)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            FileSystemInfo[] fileSystemInfos = dirInfo.GetFileSystemInfos();
            foreach (var info in fileSystemInfos)
            {
                if (info is FileInfo fileInfo) Console.WriteLine($"{new string(' ', indent * 2)}{fileInfo.Name} {fileInfo.Length} bajtow {fileInfo.GetDOSAttributes()}");
                else if (info is DirectoryInfo directoryInfo)
                {
                    int directoryCount = directoryInfo.GetFileSystemInfos().Length;
                    Console.WriteLine($"{new string(' ', indent * 2)}{directoryInfo.Name} ({directoryCount}) {directoryInfo.GetDOSAttributes()}");
                    ListAllFilesShifted(directoryInfo.FullName, indent + 1);
                }
            }
        }

        static void LoadDirectoryData(string directory)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            var items = new SortedDictionary<string, long>(new StringComparator());


            foreach (var info in dirInfo.GetDirectories())
            {
                items[info.Name] = info.GetFileSystemInfos().Length;
            }
            foreach (var info in dirInfo.GetFiles())
            {
                items[info.Name] = info.Length;
            }

            SerializeData(items, directory + "test.txt");

            var items2 = DeserializeData(directory + "test.txt");


            Console.WriteLine();

            foreach (var i in items2)
            {
                Console.WriteLine($"{i.Key} -> {i.Value}");
            }

        }

        static void Main(string[] args)
        {

            string directoryPath = args[0];
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            Console.WriteLine(directoryPath);

            try
            {
                ListAllFilesShifted(directoryPath, 0);
                Console.WriteLine("Najstarszy plik: " + Extensions.GetOldestFile(directoryInfo).Name +" " + Extensions.GetOldestFile(directoryInfo).CreationTime);
                LoadDirectoryData(directoryPath);

            }
            catch (Exception e)
            {
                Console.WriteLine("Can not find the folder.");
            }
        }
}