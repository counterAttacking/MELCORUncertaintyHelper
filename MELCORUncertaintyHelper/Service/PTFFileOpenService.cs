﻿using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class PTFFileOpenService
    {
        private PTFFile[] files;

        private PTFFileOpenService()
        {

        }

        private static readonly Lazy<PTFFileOpenService> openService = new Lazy<PTFFileOpenService>(() => new PTFFileOpenService());

        public static PTFFileOpenService GetOpenService
        {
            get
            {
                return openService.Value;
            }
        }

        public object GetFiles() => this.files.Clone();

        public void OpenFiles(string[] inputFiles)
        {
            var files = new List<PTFFile>();
            try
            {
                for (var i = 0; i < inputFiles.Length; i++)
                {
                    var file = this.DivideFilePath(inputFiles[i]);
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.files = files.ToArray();
            }
        }

        private PTFFile DivideFilePath(string filePath)
        {
            var file = new PTFFile
            {
                name = Path.GetFileName(filePath),
                path = Path.GetDirectoryName(filePath),
                fullPath = filePath
            };
            return file;
        }
    }
}
