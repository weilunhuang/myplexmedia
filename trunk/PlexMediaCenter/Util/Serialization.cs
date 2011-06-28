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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace PlexMediaCenter.Util {
    public static class Serialization {
        #region FileType enum

        public enum FileType {
            Binary,
            XML
        }

        #endregion

        public static void Serialize(string fileName, object objectToSerialize) {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate)) {
                Serialize(fs, objectToSerialize);
                fs.Close();
            }
        }

        public static void Serialize(string fileName, object objectToSerialize, FileType fileType) {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate)) {
                Serialize(fs, objectToSerialize, fileType);
            }
        }

        public static void Serialize(Stream serializedStream, object objectToSerialize) {
            Serialize(serializedStream, objectToSerialize, FileType.XML);
        }

        public static void Serialize(Stream serializedStream, object objectToSerialize, FileType fileType) {
            switch (fileType) {
                case FileType.Binary:
                    SerializeBinary(serializedStream, objectToSerialize);
                    break;
                case FileType.XML:
                    SerializeXML(serializedStream, objectToSerialize);
                    break;
                default:
                    throw new FormatException();
            }
        }

        public static T DeSerialize<T>(string fileName) {
            using (FileStream fs = new FileStream(fileName, FileMode.Open)) {
                return DeSerialize<T>(fs);
            }
        }

        public static T DeSerialize<T>(string fileName, FileType fileType) {
            using (FileStream fs = new FileStream(fileName, FileMode.Open)) {
                return DeSerialize<T>(fs, fileType);
            }
        }

        public static T DeSerialize<T>(Stream serializedStream) {
            return DeSerialize<T>(serializedStream, FileType.XML);
        }

        public static T DeSerialize<T>(Stream serializedStream, FileType fileType) {
            switch (fileType) {
                case FileType.Binary:
                    return DeSerializeBinary<T>(serializedStream);
                case FileType.XML:
                    return DeSerializeXML<T>(serializedStream);
                default:
                    throw new FormatException();
            }
        }

        private static void SerializeBinary(Stream binaryStream, object objectToSerialize) {
            using (binaryStream) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(binaryStream, objectToSerialize);
            }
        }

        private static T DeSerializeBinary<T>(Stream binaryStream) {
            using (binaryStream) {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T) binaryFormatter.Deserialize(binaryStream);
            }
        }

        private static void SerializeXML(Stream xmlStream, object objectToSerialize) {
            using (xmlStream) {
                XmlSerializer xmlSerializer = new XmlSerializer(objectToSerialize.GetType());
                xmlSerializer.Serialize(xmlStream, objectToSerialize);
                xmlStream.Close();
            }
        }

        private static T DeSerializeXML<T>(Stream xmlStream) {
            using (xmlStream) {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
                XmlReaderSettings settings = new XmlReaderSettings {CheckCharacters = false};
                return (T) xmlSerializer.Deserialize(XmlReader.Create(xmlStream, settings));
            }
        }

        public static T DeSerializeXML<T>(string xmlString) {
            XmlSerializer xs = new XmlSerializer(typeof (T));
            using (StringReader sr = new StringReader(xmlString)) {
                return (T) xs.Deserialize(sr);
            }
        }
    }
}