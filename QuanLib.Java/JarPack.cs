﻿using QuanLib.IO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Java
{
    public class JarPack : ZipPack
    {
        public JarPack(string path, Encoding? encoding = null) : base(path, encoding) { }

        public JarPack(Stream stream) : base(stream) { }

        public const string ManifestPath = "META-INF/MANIFEST.MF";

        public NameValueCollection GetManifest()
        {
            if (!ExistsFile(ManifestPath))
                throw new FileNotFoundException("META-INF/MANIFEST.MF 文件不存在");
            return JarUtil.ParseManifest(base[ManifestPath]!.Open());
        }
    }
}
