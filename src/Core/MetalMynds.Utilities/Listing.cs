using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MetalMynds.Utilities
{
    public abstract class Listing
    {
        protected List<String> BaseResults = new List<string>();
       
        public Listing()
        {
        }

        public virtual void Search(String Path, String Qualifier, Boolean Recursive)
        {
            Clear();
        }

        public virtual void Search(List<String> Paths, String Qualifier, Boolean Recursive)
        {
            Clear();
        }

        public virtual void Clear()
        {
            BaseResults.Clear();
        }

        protected virtual void AddResults(String[] Result)
        {
            BaseResults.AddRange(Result);
        }

        public virtual List<String> Results { get { return BaseResults; } }

    }

    public class FileListing : Listing
    {
        public FileListing()
        {
        }

        public override void Search(String Path, String Wildcards,Boolean Recursive)
        {

            base.Search(Path, Wildcards, Recursive);

            String[] wildcards = Wildcards.Split(';');
            List<String> allFiles = new List<string>();

            foreach (String wildcard in wildcards)
            {
                if (Recursive)
                {
                    allFiles.AddRange(Directory.GetFiles(Path, wildcard, SearchOption.AllDirectories));
                }
                else
                {

                    allFiles.AddRange(Directory.GetFiles(Path, wildcard, SearchOption.TopDirectoryOnly));
                }
            }

            String[] files = allFiles.ToArray();

            Array.Sort(files);

            //foreach (String file in files)
            //{
            //    System.Diagnostics.Debug.WriteLine(file);

            //    if (file.EndsWith("TCT-6211-0001.taft"))
            //    {
            //        System.Diagnostics.Debug.WriteLine("FOUND");
            //    }

            //}

            AddResults(files);

        }

        public void Search(List<String> Paths, String Wildcards, Boolean Recursive)
        {
            base.Search(Paths, Wildcards, Recursive);

            foreach (String path in Paths)
            {

                String[] wildcards = Wildcards.Split(';');
                List<String> allFiles = new List<string>();

                foreach (String wildcard in wildcards)
                {
                    if (Recursive)
                    {
                        allFiles.AddRange(Directory.GetFiles(path, wildcard, SearchOption.AllDirectories));
                    }
                    else
                    {
                        allFiles.AddRange(Directory.GetFiles(path, wildcard, SearchOption.TopDirectoryOnly));
                    }
                }

                String[] files = allFiles.ToArray();
                
                Array.Sort(files);

                AddResults(files);
            }

        }

    }

    public class DirectoryListing : Listing
    {
        public DirectoryListing()
        {
        }

        public override void Search(String Path, String Qualifier, Boolean Recursive)
        {

            base.Search(Path, Qualifier, Recursive);

            String[] directories;

            if (Qualifier == String.Empty)
            {
                Qualifier = "*";
            }

            if (Recursive)
            {
                directories = Directory.GetDirectories(Path, Qualifier, SearchOption.AllDirectories);
            }
            else
            {
                directories = Directory.GetDirectories(Path, Qualifier, SearchOption.TopDirectoryOnly);
            }

            Array.Sort(directories);

            AddResults(directories);

        }

        public void Search(List<String> Paths, String Qualifier, Boolean Recursive)
        {
            base.Search(Paths, Qualifier, Recursive);

            foreach (String path in Paths)
            {
                String[] directories;

                if (Qualifier == String.Empty)
                {
                    Qualifier = "*";
                }

                directories = Directory.GetDirectories(path, Qualifier, SearchOption.AllDirectories);

                Array.Sort(directories);

                AddResults(directories);
            }

        }

    }

}
