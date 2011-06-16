using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlexMediaCenter.Plex {
   public class PlexException : Exception{

       Type ErrorSource { get; set; }

        public PlexException(Type sourceClass, string errorMessage, Exception innerException) :base(errorMessage, innerException){
            ErrorSource = sourceClass;
        }

        public override string ToString() {
            return String.Format("Error in {0}: {1}", ErrorSource.GetType().ToString(), base.ToString());
        }
    }
}
