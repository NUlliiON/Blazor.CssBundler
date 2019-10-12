using System;
using System.Collections.Generic;

namespace Blazor.CssBundler.Models.Bundle
{
    abstract class BundleInfoBase
    {
        public bool Succeed { get; set; }
        public List<Error> Errors { get; set; }
        public TimeSpan BundlingTime { get; set; }
        public int CombinedFilesCount { get; set; }

        public BundleInfoBase()
        {
            Errors = new List<Error>();
        }
    }
}
