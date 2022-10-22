using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.Generic;
using System.IO.Compression;

namespace HelloApp1
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program1
    {
        public void Readrepos()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                Console.WriteLine($"Тип файловой системы: {drive.DriveFormat}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
            }
            Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
            Console.ReadKey();
        }
    }

    class TextCreat
    {
        public void CreatTextFile()
        {
            string path = @"/users/sl4sh73r/RiderProjects/Solution1/";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();
            using (FileStream fstream = new FileStream($"{path}/note.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }
            using (FileStream fstream = File.OpenRead($"{path}/note.txt"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }
            Console.WriteLine("Файл Создан\nНажмите любую кнопку чтобы продолжить");
            Console.ReadKey();
        }

        public void DeleteTxt()
        {
            string pathFile = @"/users/sl4sh73r/RiderProjects/Solution1/";
            FileInfo fileInf = new FileInfo(pathFile);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
        }
    }

    class JsonText
    {
        public void Json()
        {
            string path_one = @"/users/sl4sh73r/RiderProjects/Solution1/user_json.json";
            Person tom = new Person { Name = "Tom", Age = 35 };
            string json = JsonSerializer.Serialize<Person>(tom);
            File.WriteAllText(path_one, json);
            Console.WriteLine(json);
            Person restoredPerson = JsonSerializer.Deserialize<Person>(json);
            Console.WriteLine(restoredPerson.Name);
            Console.WriteLine("Файл Создан\nНажмите любую кнопку чтобы продолжить");
            Console.ReadKey();
        }

        public void DeleteJson()
        {
            string pathFileJson = @"/users/sl4sh73r/RiderProjects/Solution1/user_json.json";
            FileInfo fileInf = new FileInfo(pathFileJson);
            if (fileInf.Exists)
            {
                File.Delete(pathFileJson);
            }
        }
    }

    class TextXML
    {
        public void xml()
        {
            string path = @"/users/sl4sh73r/RiderProjects/Solution1/";
            using (FileStream fstream = new FileStream($@"{path}/user_xml.xml", FileMode.OpenOrCreate)){}
            Console.WriteLine("Введите имя:");
            string name1 = Console.ReadLine();
            Console.WriteLine("Введите возраст:");
            string age1 = Console.ReadLine();
            XmlDocument FileXML = new XmlDocument();
            XmlElement personElem = FileXML.CreateElement("person");
            XmlElement nameAttr = FileXML.CreateElement("name");
            XmlElement ageElem = FileXML.CreateElement("age");
            XmlText nameText = FileXML.CreateTextNode(name1);
            XmlText ageText = FileXML.CreateTextNode(age1);
            nameAttr.AppendChild(nameText);
            ageElem.AppendChild(ageText);
            personElem.AppendChild(nameAttr);
            personElem.AppendChild(ageElem);
            FileXML.AppendChild(personElem);
            FileXML.Save(@"/users/sl4sh73r/RiderProjects/Solution1/user_xml.xml");
            foreach (XmlNode xnode in FileXML)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "name")
                    {
                        Console.WriteLine($"Имя: {childnode.InnerText}");
                    }
                    if (childnode.Name == "age")
                    {
                        Console.WriteLine($"Возраст: {childnode.InnerText}");
                    }
                }
            }
            Console.WriteLine("Файл Создан\nНажмите любую кнопку чтобы продолжить");
            Console.ReadKey();
        }
        public void delete_xml()
        {
            string pathFileXML = @"/users/sl4sh73r/RiderProjects/Solution1/user_xml.xml";
            FileInfo fileInf = new FileInfo(pathFileXML);
            if (fileInf.Exists)
            {
                File.Delete(pathFileXML);
            }
        }
    }

    class zip
    {
        public void create_zip()
        {
            string path_to_folder = @"/users/sl4sh73r/RiderProjects/Solution1/archive_folder";
            string archive_path = @"/users/sl4sh73r/RiderProjects/Solution1/archive.zip";
            Directory.CreateDirectory(path_to_folder);
            ZipFile.CreateFromDirectory(path_to_folder, archive_path);
            Console.WriteLine("Выберите файл: 1-текстовый, 2-xml, 3-json\n");
            string ans = Console.ReadLine();
        a1: switch (ans)
            {
                case "1":
                    {
                        using (ZipArchive zipArchive = ZipFile.Open(archive_path, ZipArchiveMode.Update))
                        {
                            const string pathFileToAdd = @"/users/sl4sh73r/RiderProjects/Solution1/note.txt";
                            const string nameFileToAdd = "note.txt";
                            zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);
                        }
                    }
                    break;
                case "2":
                    {
                        using (ZipArchive zipArchive = ZipFile.Open(archive_path, ZipArchiveMode.Update))
                        {
                            const string pathFileToAdd = @"/users/sl4sh73r/RiderProjects/Solution1/user_xml.xml";
                            const string nameFileToAdd = "user_xml.xml";
                            zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);
                        }
                    }
                    break;
                case "3":
                    {
                        using (ZipArchive zipArchive = ZipFile.Open(archive_path, ZipArchiveMode.Update))
                        {
                            const string pathFileToAdd = @"/users/sl4sh73r/RiderProjects/Solution1/user_json.json";
                            const string nameFileToAdd = "user_json.json";
                            zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Вы ввели неверное число. 1-текстовый, 2-json, 3-xml");
                    ans = Console.ReadLine();
                    goto a1;
            }
            Console.WriteLine("Файл успешно заархивирован. Нажмите люблую кнопку, чтобы продолжить");
            Console.ReadKey();
            ZipFile.ExtractToDirectory(archive_path, path_to_folder);
            Console.WriteLine(" Файл успешно разархивирован");
            if (ans == "1")
            {
                FileInfo file = new FileInfo(@"/users/sl4sh73r/RiderProjects/Solution1/note.txt");
                Console.WriteLine($"Длина файла: {file.Length}");
            }
            else if (ans == "2")
            {
                FileInfo file = new FileInfo(@"/users/sl4sh73r/RiderProjects/Solution1/user_xml.xml");
                Console.WriteLine($"Длина файла: {file.Length}");
            }
            else if (ans == "3")
            {
                FileInfo file = new FileInfo(@"/users/sl4sh73r/RiderProjects/Solution1/user_json.json");
                Console.WriteLine($"Длина файла: {file.Length}");
            }
            Console.WriteLine("Файл успешно разархивирован. Нажмите люблую кнопку, чтобы продолжить");
            Console.ReadKey();
        }
        public void delete_zip()
        {
            string pathFiletxt = @"/users/sl4sh73r/RiderProjects/Solution1/note.txt";
            string pathFilexml = @"/users/sl4sh73r/RiderProjects/Solution1/user_xml.xml";
            string pathFilejson = @"/users/sl4sh73r/RiderProjects/Solution1/user_json.json";
            string pathFilezip = @"/users/sl4sh73r/RiderProjects/Solution1/archive.zip";
            FileInfo fileInf = new FileInfo(pathFiletxt);
            if (fileInf.Exists)
            {
                File.Delete(pathFiletxt);
            }
            FileInfo fileInfo = new FileInfo(pathFilexml);
            if (fileInfo.Exists)
            {
                File.Delete(pathFilexml);
            }
            FileInfo fileInfq = new FileInfo(pathFilejson);
            if (fileInfq.Exists)
            {
                File.Delete(pathFilejson);
            }
            FileInfo fileInfe = new FileInfo(pathFilezip);
            if (fileInfe.Exists)
            {
                File.Delete(pathFilezip);
            }
            Directory.Delete(@"/users/sl4sh73r/RiderProjects/Solution1/archive_folder", true);
        }
    }

    class Printer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите номер задания:\n1-Вывод информации о дисках\n2-Создание текстового файла\n3-Создание json файла\n4-Создание xml файла\n5-Создание zip архива\n9-удаление всех файлов и папок\n0-Выход из программы");
            string number = Console.ReadLine();
            TextCreat textCreat = new TextCreat();
            JsonText jsonText = new JsonText();
            TextXML textxml = new TextXML();
            zip zip = new zip();
            while (number != "0")
            {
                switch (number)
                {
                    case "1":
                        Program1 program1 = new Program1();
                        program1.Readrepos();
                        break;
                    case "2":
                        textCreat.CreatTextFile();
                        break;
                    case "3":
                        jsonText.Json();
                        break;
                    case "4":
                        textxml.xml();
                        break;
                    case "5":
                        zip.create_zip();
                        break;
                    case "9":
                        textCreat.DeleteTxt();
                        jsonText.DeleteJson();
                        textxml.delete_xml();
                        zip.delete_zip();
                        Console.WriteLine("Файлы успешно удалены. Нажмите люббую кнопку, чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case "0":
                        number = "0";
                        break;
                }
                Console.Clear();
                Console.WriteLine("Введите номер задания:\n1-Вывод информации о дисках\n2-Создание текстового файла\n3-Создание json файла\n4-Создание xml файла\n5-Создание zip архива\n9-удаление всех файлов и папок\n0-Выход из программы");
                number = Console.ReadLine();
            }
        }
    }
}

//TODO сделать вопросы