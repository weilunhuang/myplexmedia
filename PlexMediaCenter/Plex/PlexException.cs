using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlexMediaCenter.Plex {
    public class PlexException : Exception {

        public Type ErrorSource { get; set; }

        public PlexException(Type sourceClass, string errorMessage, Exception innerException)
            : base(errorMessage, innerException) {
            ErrorSource = sourceClass;
        }
    }
}
