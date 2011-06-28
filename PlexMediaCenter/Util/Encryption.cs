#region #region Copyright (C) 2005-2011 Team MediaPortal

// 
// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

using System;
using System.Security.Cryptography;
using System.Text;

namespace PlexMediaCenter.Util {
    internal static class Encryption {
        /// <summary>
        ///   Gets the SH a1 hash.
        /// </summary>
        /// <param name = "text">The text.</param>
        /// <returns></returns>
        public static string GetSHA1Hash(string text) {
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();

            string result = null;
            string temp = null;

            byte[] arrayData = Encoding.ASCII.GetBytes(text);
            byte[] arrayResult = SHA1.ComputeHash(arrayData);
            foreach (byte t in arrayResult) {
                temp = Convert.ToString(t, 16);
                if (temp.Length == 1) temp = "0" + temp;
                result += temp;
            }
            return result;
        }
    }
}