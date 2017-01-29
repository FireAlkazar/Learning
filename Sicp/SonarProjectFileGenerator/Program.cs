using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarProjectFileGenerator
{
    class Program
    {
        private const string FileTemplate = @"# must be unique in a given SonarQube instance
sonar.projectKey={0}:corefx:dotnet
# this is the name and version displayed in the SonarQube UI. Was mandatory prior to SonarQube 6.1.
sonar.projectName={0}
sonar.projectVersion=1.0
 
# Path is relative to the sonar-project.properties file. Use // on Windows.
# Since SonarQube 4.2, this property is optional if sonar.modules is set. 
# If not set, SonarQube starts looking for source code from the directory containing 
# the sonar-project.properties file.
sonar.sources=.
 
# Encoding of the source code. Default is default system encoding
sonar.sourceEncoding=UTF-8";

        private const string BasePath = @"C:\Github\corefx\src";

        static void Main(string[] args)
        {
            string[] directories = Directory.GetDirectories(BasePath);

            foreach (var directory in directories)
            {
                WriteSonarProjectFile(directory);
                Console.WriteLine($"{directory} - file created");
            }
            Console.ReadLine();
        }

        private static void WriteSonarProjectFile(string directory)
        {
            string filePath = Path.Combine(directory, "sonar-project.properties");

            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(FileTemplate, Path.GetFileName(directory));
            }
        }
    }
}
